using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using HarmonyHub.Config;
using HarmonyHub.Events;
using HarmonyHub.Exceptions;
using HarmonyHub.LogitechDataContracts;

namespace HarmonyHub
{
    /// <inheritdoc />
    public class Client : IClient
    {
        private readonly object _writeLock = new object();

        // A lookup to correlate request and responses
        private readonly IDictionary<string, TaskCompletionSource<XmlElement>> _resultTaskCompletionSources = new ConcurrentDictionary<string, TaskCompletionSource<XmlElement>>();

        private bool _disposed;

        private string _ip;
        private string _username;
        private string _password;
        private bool _bypassLogitech;

        private TcpClient _client;
        private NetworkStream _stream;
        private StreamParser _parser;
        private System.Timers.Timer _heartbeat;

        private string _sessionToken;
        private string _clientId;
        private int _messageId = 1;
        private int _commandTimeout;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="ip">IP address of the Harmony.</param>
        public Client(string ip)
            : this(ip, null, null, 2000, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="ip">IP address of the Harmony.</param>
        /// <param name="username">Logitech username.</param>
        /// <param name="password">Logitech password.</param>
        /// <param name="commandTimeout">Timeout value for commands.</param>
        /// <param name="bypassLogitech">if <c>false</c> use Logitech auth, otherwise bypass Logitech auth.</param>
        public Client(string ip, string username, string password, int commandTimeout = 2000, bool bypassLogitech = false)
        {
            _ip = ip;
            _username = username;
            _password = password;
            _commandTimeout = commandTimeout;
            _bypassLogitech = bypassLogitech;
        }

        /// <inheritdoc />
        public event EventHandler<ActivityUpdatedEventArgs> CurrentActivityUpdated;

        /// <inheritdoc />
        public event EventHandler<ErrorEventArgs> Error;

        /// <inheritdoc />
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <inheritdoc />
        public event EventHandler<MessageSentEventArgs> MessageSent;

        /// <inheritdoc />
        public bool Connected { get; private set; }

        /// <inheritdoc />
        public bool Authenticated { get; private set; }

        #region Requests

        /// <inheritdoc />
        public async Task<HarmonyConfig> GetConfigAsync()
        {
            var xml = Xml.Element("iq")
                .Attr("type", "get")
                .Attr("id", _clientId + "_" + _messageId.ToString())
                .Child(Xml.Element("oa", "connect.logitech.com")
                    .Attr("mime", HarmonyMimeTypes.Config));

            var result = await RequestResponseAsync(xml, _commandTimeout).ConfigureAwait(false);

            // Validate
            if (result.Name == "iq" && result.HasChildNodes && result.FirstChild.Name == "oa" &&
                result.FirstChild.Attributes["mime"] != null && result.FirstChild.Attributes["mime"].Value == HarmonyMimeTypes.Config)
            {
                return JsonSerializer<HarmonyConfig>.Deserialize(result.FirstChild.InnerText);
            }

            throw new UnrecognizedMessageException("Response message was not in the correct format");
        }

        /// <inheritdoc />
        public async Task<int> GetCurrentActivityIdAsync()
        {
            var xml = Xml.Element("iq")
                .Attr("type", "get")
                .Attr("id", _clientId + "_" + _messageId.ToString())
                .Child(Xml.Element("oa", "connect.logitech.com")
                    .Attr("mime", HarmonyMimeTypes.CurrentActivity));

            var result = await RequestResponseAsync(xml, _commandTimeout).ConfigureAwait(false);

            // Validate
            if (result.Name == "iq" && result.HasChildNodes && result.FirstChild.Name == "oa" &&
                result.FirstChild.Attributes["mime"] != null && result.FirstChild.Attributes["mime"].Value == HarmonyMimeTypes.CurrentActivity)
            {
                var currentActivityParts = result.FirstChild.InnerText.Split('=');
                if (currentActivityParts.Length == 2)
                {
                    int id;
                    if (int.TryParse(currentActivityParts[1], out id))
                    {
                        return id;
                    }
                }
            }

            throw new UnrecognizedMessageException("Response message was not in the correct format");
        }

        /// <inheritdoc />
        public async Task SendCommandAsync(string command, bool press = true, int? timestamp = null)
        {
            var text = new StringBuilder();
            text.Append("action=").Append(command.Replace(":", "::"));
            text.Append(":").Append("status=").Append(press ? "press" : "release");
            if (timestamp.HasValue)
                text.Append(":").Append("timestamp=").Append(timestamp.Value);

            var xml = Xml.Element("iq")
                .Attr("type", "get")
                .Attr("id", _clientId + "_" + _messageId.ToString())
                .Child(Xml.Element("oa", "connect.logitech.com")
                    .Attr("mime", HarmonyMimeTypes.DeviceCommand)
                    .Text(text.ToString()));

            await FireAndForgetAsync(xml).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task SendKeyPressAsync(string command, int timespan = 100)
        {
            var now = (int)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            await SendCommandAsync(command, true, now - timespan);
            await SendCommandAsync(command, false, timespan);
        }

        /// <inheritdoc />
        public async Task StartActivityAsync(int activityId)
        {
            var xml = Xml.Element("iq")
                .Attr("type", "get")
                .Attr("id", _clientId + "_" + _messageId.ToString())
                .Child(Xml.Element("oa", "connect.logitech.com")
                    .Attr("mime", HarmonyMimeTypes.StartActivity)
                    .Text(string.Format("activityId={0}:timestamp={1}", activityId, DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds)));

            await RequestResponseAsync(xml, _commandTimeout).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task SendPingAsync()
        {
            var xml = Xml.Element("iq")
                .Attr("type", "get")
                .Attr("id", _clientId + "_" + _messageId.ToString())
                .Child(Xml.Element("oa", "connect.logitech.com")
                    .Attr("mime", HarmonyMimeTypes.Ping));

            await RequestResponseAsync(xml, _commandTimeout).ConfigureAwait(false);
        }

        /// <summary>
        /// Listens for incoming XML stanzas and raises the appropriate events.
        /// </summary>
        /// <remarks>This runs in the context of a separate thread. In case of an
        /// exception, the Error event is raised and the thread is shutdown.</remarks>
        private void ReadXmlStream()
        {
            try
            {
                while (_parser != null && !_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    XmlElement elem = _parser.NextElement("iq", "message", "presence");
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(elem.OuterXml));

                    if (elem.Name == "iq" && elem.HasChildNodes)
                    {
                        var messageId = elem.Attributes["id"].Value;
                        TaskCompletionSource<XmlElement> resultTaskCompletionSource;
                        if (messageId != null && _resultTaskCompletionSources.TryGetValue(messageId, out resultTaskCompletionSource))
                        {
                            var oaNode = elem.FirstChild;
                            if (oaNode != null && oaNode.Name == "oa")
                            {
                                switch (oaNode.Attributes["errorcode"].Value)
                                {
                                    case "200":
                                        resultTaskCompletionSource.TrySetResult(elem);
                                        _resultTaskCompletionSources.Remove(messageId);
                                        break;
                                    case "100":
                                        // Ignore continuation messages
                                        break;
                                    default:
                                        var errorMessage = oaNode.Attributes["errorstring"].Value;
                                        resultTaskCompletionSource.TrySetException(new Exception(errorMessage));
                                        _resultTaskCompletionSources.Remove(messageId);
                                        break;
                                }
                            }
                        }
                    }
                    else if (elem.Name == "message" && elem.HasChildNodes)
                    {
                        var eventNode = elem.FirstChild;
                        if (eventNode != null && eventNode.Name == "event" && eventNode.Attributes["type"] != null)
                        {
                            switch (eventNode.Attributes["type"].Value)
                            {
                                case HarmonyEventTypes.StartActivityFinished:
                                    var messageParams = eventNode.InnerText.ToParamDictionary();

                                    // Find the activityId
                                    int activityId;
                                    if (int.TryParse(messageParams["activityId"], out activityId))
                                    {
                                        CurrentActivityUpdated?.Invoke(this, new ActivityUpdatedEventArgs(activityId));
                                    }

                                    break;
                                case HarmonyEventTypes.StateDigest:
                                    // TODO: What to do with this?
                                    // It seems that these are purely informational, currently comments out to avoid the serialization hit
                                    // but left in, for the case that this becomes useful later on
                                    // var notify = JsonSerializer<HarmonyNotify>.Deserialize(eventNode.InnerText);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Stream has dropped, this happens naturally on Dispose
                if (e is IOException)
                {
                    Authenticated = false;
                    Connected = false;
                }

                // An exception outside of Dispose should be raised to the caller to handle
                if (!_disposed)
                {
                    Error?.Invoke(this, new ErrorEventArgs(e));
                }
            }
        }

        /// <summary>
        /// Send a message without waiting for response.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="timeout">Time to wait for an error response.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        private async Task FireAndForgetAsync(XmlElement message, int timeout = 50)
        {
            // Heartbeat check
            if (!_heartbeat.Enabled)
                throw new ConnectionException("Connection already closed or not authenticated");

            var messageId = message.Attributes["id"].Value;

            // Prepare the TaskCompletionSource, which is used to await the result
            var resultTaskCompletionSource = new TaskCompletionSource<XmlElement>();
            _resultTaskCompletionSources[messageId] = resultTaskCompletionSource;

            // Start the sending
            Send(message);

            // Await, to make sure there wasn't an error
            var task = await Task.WhenAny(resultTaskCompletionSource.Task, Task.Delay(timeout)).ConfigureAwait(false);

            // Remove the result task, as we no longer need it.
            _resultTaskCompletionSources.Remove(messageId);

            // This makes sure the exception, if there was one, is unwrapped
            await task;
        }

        /// <summary>
        /// Sends a request response message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="timeout">Timeout for the response, exception thrown on expiration.</param>
        /// <returns>Response message.</returns>
        private async Task<XmlElement> RequestResponseAsync(XmlElement message, int timeout = 2000)
        {
            // Heartbeat check
            if (!_heartbeat.Enabled)
                throw new ConnectionException("Connection already closed or not authenticated");

            var messageId = message.Attributes["id"].Value;

            // Prepate the TaskCompletionSource, which is used to await the result
            var resultTaskCompletionSource = new TaskCompletionSource<XmlElement>();
            _resultTaskCompletionSources[messageId] = resultTaskCompletionSource;

            // Create the action which is called when a timeout occurs
            Action timeoutAction = () =>
            {
                _resultTaskCompletionSources.Remove(messageId);
                resultTaskCompletionSource.TrySetException(new TimeoutException($"Timeout while waiting on response {messageId} after {timeout}"));
            };

            Send(message);

            // Handle timeout
            var cancellationTokenSource = new CancellationTokenSource(timeout);
            using (cancellationTokenSource.Token.Register(timeoutAction))
            {
                // Await until response or timeout
                return await resultTaskCompletionSource.Task.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Serializes and sends the specified XML element to the server.
        /// </summary>
        /// <param name="element">The XML element to send.</param>
        /// <exception cref="ArgumentNullException">The element parameter is null.</exception>
        /// <exception cref="IOException">There was a failure while writing to the network.</exception>
        private void Send(XmlElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            Send(element.ToXmlString());
        }

        /// <summary>
        /// Sends the specified string to the server.
        /// </summary>
        /// <remarks>
        /// Do not use this method directly except for stream initialization and closing.
        /// </remarks>
        /// <param name="xml">The string to send.</param>
        /// <exception cref="ArgumentNullException">The xml parameter is null.</exception>
        /// <exception cref="IOException">There was a failure while writing to the network.</exception>
        private void Send(string xml)
        {
            if (xml == null)
                throw new ArgumentNullException("xml");

            // XMPP is guaranteed to be UTF-8.
            byte[] buf = Encoding.UTF8.GetBytes(xml);
            lock (_writeLock)
            {
                try
                {
                    _stream.Write(buf, 0, buf.Length);
                    _messageId++;
                    MessageSent?.Invoke(this, new MessageSentEventArgs(xml));
                }
                catch
                {
                    Authenticated = false;
                    Connected = false;

                    throw;
                }
            }
        }

        #endregion

        #region Client Init and Close

        /// <inheritdoc />
        public void Connect()
        {
            // Establish a connection
            if (_client == null)
                _client = new TcpClient(_ip, 5222);

            // Get stream handle
            if (_stream == null)
                _stream = _client.GetStream();

            // Open stream
            OpenStream();

            // Authenticate the stream
            Authenticate();

            // Set up the listener task after authentication has been handled
            Task.Factory.StartNew(ReadXmlStream, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// Initiates the stream.
        /// </summary>
        private void OpenStream()
        {
            // Generate new client and message ids
            _clientId = Guid.NewGuid().ToString();
            _messageId = 1;

            // Stream initialization request
            var xml = Xml.Element("stream:stream", "jabber:client")
                .Attr("to", "connect.logitech.com")
                .Attr("version", "1.0")
                .Attr("xmlns:stream", "http://etherx.jabber.org/streams")
                .Attr("xml:lang", CultureInfo.CurrentCulture.Name);
            Send(xml.ToXmlString(xmlDeclaration: true, leaveOpen: true));

            // Create a new parser instance.
            if (_parser != null)
                _parser.Dispose();

            _parser = new StreamParser(_stream, true);

            // The first element of the stream must be <stream:features>.
            var features = _parser.NextElement("stream:features");
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(features.OuterXml));

            // Enable keepalive
            if (_heartbeat != null)
                _heartbeat.Dispose();

            _heartbeat = new System.Timers.Timer();
            _heartbeat.Elapsed += HeatbeatAsync;
            _heartbeat.Interval = 30000;
            _heartbeat.Start();

            Connected = true;
        }

        /// <summary>
        /// Heartbeat ping. Failure will result in the heartbeat being stopped, which will
        /// make any future calls throw an exception as the heartbeat indicator will be disabled.
        /// </summary>
        /// <param name="sender">Sender of event.</param>
        /// <param name="e">Event parameters.</param>
        private async void HeatbeatAsync(object sender, ElapsedEventArgs e)
        {
            try
            {
                await SendPingAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, new ErrorEventArgs(ex));
                System.Timers.Timer timer = (System.Timers.Timer)sender;
                timer.Stop();
            }
        }

        /// <summary>
        /// Opens an authenticated stream.
        /// </summary>
        private void Authenticate()
        {
            // Get session token
            if (string.IsNullOrEmpty(_sessionToken))
                _sessionToken = GetSessionToken(_ip, _username, _password, _bypassLogitech);

            if (string.IsNullOrEmpty(_sessionToken))
            {
                throw new SessionTokenException("Could not get session token on Harmony Hub.");
            }

            // Login with session token
            LoginToHarmony(_ip, _sessionToken);
            Authenticated = true;
        }

        /// <summary>
        /// Closes the connection with the XMPP server.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The XmppIm object has been disposed.</exception>
        private void CloseStream()
        {
            if (Connected)
            {
                // Kill heartbeat
                if (_heartbeat != null)
                {
                    _heartbeat.Stop();
                    _heartbeat.Dispose();
                }

                // Close the XML stream.
                Send("</stream:stream>");

                // Kill the stream parser
                if (_parser != null)
                    _parser.Dispose();

                Authenticated = false;
                Connected = false;
            }
        }

        /// <inheritdoc />
        public void Close()
        {
            // Close the Harmony stream
            CloseStream();

            // Stop parsing incoming feed
            _cancellationTokenSource.Cancel();

            // Dispose of stream
            if (_stream != null)
                _stream.Dispose();

            // Disconnect socket
            if (_client != null)
            {
                _client.Close();
                _client = null;
            }
        }

        #endregion

        #region Authentication and Authorization

        /// <summary>
        /// Gets the session token for the supplied Logitech username and password.
        /// </summary>
        /// <param name="ip">IP address of the Harmony Hub.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Passwprd.</param>
        /// <param name="bypassLogitech">if <c>false</c> use Logitech auth, otherwise bypass Logitech auth.</param>
        /// <returns>Harmony session token.</returns>
        private string GetSessionToken(string ip, string username, string password, bool bypassLogitech)
        {
            var sessionToken = string.Empty;

            var authToken = string.Empty;
            if (!bypassLogitech)
            {
                // Authenticate to Logitech
                authToken = LoginToLogitech(username, password);
                if (string.IsNullOrEmpty(authToken))
                    throw new AuthTokenException("Could not get token from Logitech server.");
            }

            // Swap auth token for a session token
            using (var authClient = new TcpClient(ip, 5222))
            {
                using (var authStream = authClient.GetStream())
                {
                    // Initialize stream
                    var streamXml = Xml.Element("stream:stream", "jabber:client")
                        .Attr("to", "connect.logitech.com")
                        .Attr("version", "1.0")
                        .Attr("xmlns:stream", "http://etherx.jabber.org/streams")
                        .Attr("xml:lang", CultureInfo.CurrentCulture.Name);
                    byte[] streamBuffer = Encoding.UTF8.GetBytes(streamXml.ToXmlString(xmlDeclaration: true, leaveOpen: true));
                    authStream.Write(streamBuffer, 0, streamBuffer.Length);

                    // Begin reading auth stream
                    using (var authParser = new StreamParser(authStream, true))
                    {
                        // The first element of the stream must be <stream:features>.
                        var features = authParser.NextElement("stream:features");
                        MessageReceived?.Invoke(this, new MessageReceivedEventArgs(features.OuterXml));

                        // Auth as guest
                        // <auth xmlns='urn:ietf:params:xml:ns:xmpp-sasl' mechanism='PLAIN'>AGd1ZXN0QHguY29tAGd1ZXN0</auth>
                        var saslXml = Xml.Element("auth", "urn:ietf:params:xml:ns:xmpp-sasl")
                            .Attr("mechanism", "PLAIN")
                            .Text(Convert.ToBase64String(Encoding.UTF8.GetBytes("\0" + "guest@x.com" + "\0" + "guest")));
                        byte[] saslBuffer = Encoding.UTF8.GetBytes(saslXml.ToXmlString());
                        authStream.Write(saslBuffer, 0, saslBuffer.Length);

                        // Handle response
                        while (true)
                        {
                            var response = authParser.NextElement("challenge", "success", "failure");
                            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(response.OuterXml));
                            if (response.Name != "success")
                                throw new SaslAuthenticationException("SASL authentication failed.");

                            break;
                        }

                        // Swap the authToken for a sessionToken on the Harmony Hub
                        /*
                         * <iq type="get" id="3174962747" from="guest">
                         *   <oa xmlns="connect.logitech.com" mime="vnd.logitech.connect/vnd.logitech.pair">
                         *     token=y6jZtSuYYOoQ2XXiU9cYovqtT+cCbcyjhWqGbhQsLV/mWi4dJVglFEBGpm08OjCW:name=SOMEID#iOS6.0.1#iPhone
                         *   </oa>
                         * </iq>
                         */
                        var methodOrToken = bypassLogitech ? "method=pair" : "token=" + authToken;
                        var swapXml = Xml.Element("iq")
                            .Attr("type", "get")
                            .Attr("id", _clientId + "_" + _messageId.ToString())
                            .Child(Xml.Element("oa", "connect.logitech.com")
                                .Attr("mime", HarmonyMimeTypes.Pair)
                                .Text(methodOrToken + ":name=foo#iOS8.3.0#iPhone"));
                        byte[] swapBuffer = Encoding.UTF8.GetBytes(swapXml.ToXmlString());
                        authStream.Write(swapBuffer, 0, swapBuffer.Length);

                        // Handle response
                        while (true)
                        {
                            XmlElement ret = authParser.NextElement("iq");
                            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(ret.OuterXml));
                            if (ret.Name == "iq")
                            {
                                if (ret.FirstChild != null && ret.FirstChild.Name == "oa")
                                {
                                    if (ret.FirstChild.InnerXml.Contains("status=succeeded"))
                                    {
                                        const string identityRegEx = "identity=([A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}):status";
                                        var regex = new Regex(identityRegEx, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                        var match = regex.Match(ret.FirstChild.InnerXml);
                                        if (match.Success)
                                        {
                                            sessionToken = match.Groups[1].ToString();
                                            break;
                                        }
                                        else
                                        {
                                            throw new AuthTokenException("AuthToken swap failed.");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Kill stream
                    byte[] killBuffer = Encoding.UTF8.GetBytes("</stream:stream>");
                    authStream.Write(killBuffer, 0, killBuffer.Length);
                }
            }

            return sessionToken;
        }

        /// <summary>
        /// Logs into Harmony device with the supplied session token.
        /// </summary>
        /// <param name="ip">IP address of the Harmony Hub.</param>
        /// <param name="sessionToken">HarmonyHub session token.</param>
        private void LoginToHarmony(string ip, string sessionToken)
        {
            // <auth xmlns='urn:ietf:params:xml:ns:xmpp-sasl' mechanism='PLAIN'>Base64EncodedValue</auth>
            var saslXml = Xml.Element("auth", "urn:ietf:params:xml:ns:xmpp-sasl")
                .Attr("mechanism", "PLAIN")
                .Text(Convert.ToBase64String(Encoding.UTF8.GetBytes("\0" + sessionToken + "\0" + sessionToken)));
            Send(saslXml);

            // Handle response
            while (true)
            {
                var response = _parser.NextElement("challenge", "success", "failure");

                MessageReceived?.Invoke(this, new MessageReceivedEventArgs(response.OuterXml));
                if (response.Name != "success")
                    throw new SaslAuthenticationException("SASL authentication failed.");

                break;
            }
        }

        /// <summary>
        /// Logs into Logitech endpoint and retrieves an "auth token"
        /// </summary>
        /// <param name="username">Username to authenticate with.</param>
        /// <param name="password">Password to authenticate with.</param>
        /// <returns>Logitech auth token.</returns>
        private string LoginToLogitech(string username, string password)
        {
            const string logitechAuthUrl = "https://svcs.myharmony.com/CompositeSecurityServices/Security.svc/json/GetUserAuthToken";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(logitechAuthUrl);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = JsonSerializer<GetUserAuthTokenRequest>.Serialize(new GetUserAuthTokenRequest { Email = username, Password = password });

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            var responseStream = httpResponse.GetResponseStream();
            if (responseStream == null)
                return null;

            using (var streamReader = new StreamReader(responseStream))
            {
                var result = streamReader.ReadToEnd();
                var harmonyData = JsonSerializer<GetUserAuthTokenResultRootObject>.Deserialize(result);
                return harmonyData.GetUserAuthTokenResult?.UserAuthToken;
            }
        }

        #endregion

        #region IDisposable implementation

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the XmppIm
        /// class, optionally disposing of managed resource.
        /// </summary>
        /// <param name="disposing">true to dispose of managed resources, otherwise
        /// false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Indicate that the instance has been disposed.
                _disposed = true;

                // Get rid of managed resources.
                if (disposing)
                {
                    Close();
                    _cancellationTokenSource.Dispose();
                }
            }
        }

        #endregion
    }
}
