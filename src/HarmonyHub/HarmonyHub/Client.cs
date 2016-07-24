using HarmonyHub.Config;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace HarmonyHub
{
    public class Client : IDisposable
    {
        private bool _disposed;
        private readonly object _writeLock = new object();

        private string _ip;
        private string _username;
        private string _password;

        private TcpClient _client;
        private NetworkStream _stream;
        private StreamParser _parser;

        private string _sessionToken;

        /// <summary>
        /// A FIFO of stanzas waiting to be processed.
        /// </summary>
        //private BlockingCollection<Stanza> stanzaQueue = new BlockingCollection<Stanza>();

        /// <summary>
        /// Connected.
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Current config cache.
        /// </summary>
        public HarmonyConfig Config { get; set; }

        /// <summary>
        /// Current active activity.
        /// </summary>
        public string CurrentActivity { get; set; }

        /// <summary>
        /// The event that is raised when an unrecoverable error condition occurs.
        /// </summary>
        public event EventHandler<ErrorEventArgs> Error;

        /// <summary>
        /// The event that is raised when the Config is updated.
        /// </summary>
        //public event EventHandler<ConfigUpdatedEventArgs> ConfigUpdated;

        /// <summary>
        /// The event that is raised when CurrentActivity is updated.
        /// </summary>
        //public event EventHandler<CurrentActivityEventArgs> CurrentActivityUpdated;

        /// <summary>
        /// The event that is raised when a Presence stanza has been received.
        /// </summary>
        //public event EventHandler<PresenceEventArgs> Presence;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Client(string ip, string username, string password)
        {
            _ip = ip;
            _username = username;
            _password = password;

            // Authenticate to Logitech
            var authToken = LoginToLogitech(username, password);
            if (string.IsNullOrEmpty(authToken))
            {
                throw new Exception("Could not get token from Logitech server.");
            }

            // Establish TCP connection
            _client = new TcpClient(ip, 5222);
            _stream = _client.GetStream();
            InitiateStream();

            // Get session token
            _sessionToken = GetSessionToken(ip, authToken);
            if (string.IsNullOrEmpty(_sessionToken))
            {
                throw new Exception("Could not get session token on Harmony Hub.");
            }

            Close();
            _client = new TcpClient(ip, 5222);
            _stream = _client.GetStream();
            InitiateStream();

            // Login with session tken
            LoginToHarmony(ip, _sessionToken);

            Connected = true;

            // Set up the listener and dispatcher tasks.
            Task.Factory.StartNew(ReadXmlStream, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// Send message to HarmonyHub to request Configuration.
        /// </summary>
        public void RequestConfig()
        {
            var xml = Xml.Element("iq")
                .Attr("type", "get")
                .Attr("id", "1")
                .Child(Xml.Element("oa", "connect.logitech.com")
                    .Attr("mime", MimeTypes.Config));
            Send(xml);
        }

        /// <summary>
        /// Send message to HarmonyHub to request current activity.
        /// </summary>
        public void RequestCurrentActivity()
        {
            var xml = Xml.Element("iq")
                .Attr("type", "get")
                .Attr("id", "1")
                .Child(Xml.Element("oa", "connect.logitech.com")
                    .Attr("mime", MimeTypes.CurrentActivity));
            Send(xml);
        }

        /// <summary>
        /// Send message to HarmonyHub to start an activity.
        /// </summary>
        public void StartActivity(string activityId)
        {
            var xml = Xml.Element("iq")
                .Attr("type", "get")
                .Attr("id", "1")
                .Child(Xml.Element("oa", "connect.logitech.com")
                    .Attr("mime", MimeTypes.StartActivity)
                    .Text("activityId=" + activityId + ":timestamp=0"));
            Send(xml);
        }

        /// <summary>
        /// Send command to the HarmonyHub.
        /// </summary>
        public void SendCommand(string command)
        {
            var xml = Xml.Element("iq")
                .Attr("type", "get")
                .Attr("id", "1")
                .Child(Xml.Element("oa", "connect.logitech.com")
                    .Attr("mime", MimeTypes.DeviceCommand)
                    .Text("action=" + command.Replace(":", "::") + ":status=press"));
            Send(xml);
        }

        /// <summary>
        /// Initiates the stream.
        /// </summary>
        private void InitiateStream()
        {
            // Stream initialization request
            var xml = Xml.Element("stream:stream", "jabber:client")
                .Attr("to", "connect.logitech.com")
                .Attr("version", "1.0")
                .Attr("xmlns:stream", "http://etherx.jabber.org/streams")
                .Attr("xml:lang", CultureInfo.CurrentCulture.Name);
            Send(xml.ToXmlString(xmlDeclaration: true, leaveOpen: true));

            // Create a new parser instance.
            if (_parser != null)
                _parser.Close();

            _parser = new StreamParser(_stream, true);

            // The first element of the stream must be <stream:features>.
            var features = _parser.NextElement("stream:features");
        }

        /// <summary>
        /// Initiates the stream.
        /// </summary>
        private void ReInitiateStream()
        {
            // Stream initialization request
            var xml = Xml.Element("stream:stream", "jabber:client")
                .Attr("to", "connect.logitech.com")
                .Attr("version", "1.0")
                .Attr("xmlns:stream", "http://etherx.jabber.org/streams")
                .Attr("xml:lang", CultureInfo.CurrentCulture.Name);
            Send(xml.ToXmlString(xmlDeclaration: false, leaveOpen: true));

            // Create a new parser instance.
            if (_parser != null)
                _parser.Close();

            _parser = new StreamParser(_stream, true);

            // The first element of the stream must be <stream:features>.
            var features = _parser.NextElement("stream:features");
        }

        /// <summary>
        /// Gets the session token for the supplied Logitech auth token.
        /// </summary>
        /// <param name="ip">IP address of the Harmony Hub.</param>
        /// <param name="authToken">Logitech auth token.</param>
        /// <returns>Harmony session token.</returns>
        private string GetSessionToken(string ip, string authToken)
        {
            // <auth xmlns='urn:ietf:params:xml:ns:xmpp-sasl' mechanism='PLAIN'>AGd1ZXN0QHguY29tAGd1ZXN0</auth>
            var saslXml = Xml.Element("auth", "urn:ietf:params:xml:ns:xmpp-sasl")
                .Attr("mechanism", "PLAIN")
                .Text(Convert.ToBase64String(Encoding.UTF8.GetBytes("\0" + "guest@x.com" + "\0" + "guest")));
            Send(saslXml);

            // Handle response
            while (true)
            {
                XmlElement ret = _parser.NextElement("challenge", "success", "failure");

                if (ret.Name == "failure")
                    throw new Exception("SASL authentication failed.");

                if (ret.Name == "success")
                    break;
            }

            /*
             * <iq type="get" id="3174962747" from="guest">
             *   <oa xmlns="connect.logitech.com" mime="vnd.logitech.connect/vnd.logitech.pair">
             *     token=y6jZtSuYYOoQ2XXiU9cYovqtT+cCbcyjhWqGbhQsLV/mWi4dJVglFEBGpm08OjCW:name=SOMEID#iOS6.0.1#iPhone
             *   </oa>
             * </iq>
             */
            var authXml = Xml.Element("iq")
                .Attr("type", "get")
                .Attr("id", "1")
                .Child(Xml.Element("oa", "connect.logitech.com")
                    .Attr("mime", "vnd.logitech.connect/vnd.logitech.pair")
                    .Text("token=" + authToken + ":name=foo#iOS8.3.0#iPhone"));
            Send(authXml);

            // Handle response
            while (true)
            {
                XmlElement ret = _parser.NextElement("iq");
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
                                return match.Groups[1].ToString();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Logs into Harmony device with the supplied session token.
        /// </summary>
        /// <param name="ip">IP address of the Harmony Hub.</param>
        /// <param name="sessionToken">HarmonyHub session token.</param>
        /// <returns>Harmony session token.</returns>
        private void LoginToHarmony(string ip, string sessionToken)
        {
            //ReInitiateStream();

            // <auth xmlns='urn:ietf:params:xml:ns:xmpp-sasl' mechanism='PLAIN'>AGd1ZXN0QHguY29tAGd1ZXN0</auth>
            var saslXml = Xml.Element("auth", "urn:ietf:params:xml:ns:xmpp-sasl")
                .Attr("mechanism", "PLAIN")
                .Text(Convert.ToBase64String(Encoding.UTF8.GetBytes("\0" + sessionToken + "\0" + sessionToken)));
            Send(saslXml);

            // Handle response
            while (true)
            {
                XmlElement ret = _parser.NextElement("challenge", "success", "failure");

                if (ret.Name == "failure")
                    throw new Exception("SASL authentication failed.");

                if (ret.Name == "success")
                    break;
            }

            ReInitiateStream();
        }

        /// <summary>
        /// Logs into Logitech endpoint and retrieves an "auth token"
        /// </summary>
        /// <param name="username">Username to authenticate with.</param>
        /// <param name="password">Password to authenticate with.</param>
        /// <returns>Logitech auth token.</returns>
        private static string LoginToLogitech(string username, string password)
        {
            const string logitechAuthUrl = "https://svcs.myharmony.com/CompositeSecurityServices/Security.svc/json/GetUserAuthToken";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(logitechAuthUrl);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    email = username,
                    password
                });

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            var responseStream = httpResponse.GetResponseStream();
            if (responseStream == null) return null;

            using (var streamReader = new StreamReader(responseStream))
            {
                var result = streamReader.ReadToEnd();
                var harmonyData = Newtonsoft.Json.JsonConvert.DeserializeObject<GetUserAuthTokenResultRootObject>(result);
                return harmonyData.GetUserAuthTokenResult.UserAuthToken;
            }
        }

        /// <summary>
        /// Serializes and sends the specified XML element to the server.
        /// </summary>
        /// <param name="element">The XML element to send.</param>
        /// <exception cref="ArgumentNullException">The element parameter
        /// is null.</exception>
        /// <exception cref="IOException">There was a failure while writing
        /// to the network.</exception>
        private void Send(XmlElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            Send(element.ToXmlString());
        }

        /// <summary>
        /// Sends the specified string to the server.
        /// </summary>
        /// <param name="xml">The string to send.</param>
        /// <exception cref="ArgumentNullException">The xml parameter is null.</exception>
        /// <exception cref="IOException">There was a failure while writing to
        /// the network.</exception>
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
                }
                catch (IOException e)
                {
                    Connected = false;
                    throw;
                }
            }
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
                while (true)
                {
                    XmlElement elem = _parser.NextElement("iq", "message", "presence");

                    Console.WriteLine(elem.InnerText);

                    // Parse element and dispatch.
                    switch (elem.Name)
                    {
                        case "iq":
                            if (elem.FirstChild != null)
                            {
                                if (elem.FirstChild.Name == "oa")
                                {
                                    switch(elem.FirstChild.Attributes["mime"].Value)
                                    {
                                        case MimeTypes.Config:
                                            Config = Newtonsoft.Json.JsonConvert.DeserializeObject<HarmonyConfig>(elem.FirstChild.FirstChild.Value);
                                            break;
                                        case MimeTypes.CurrentActivity:
                                            CurrentActivity = Config.Activity.First(x => x.Id == elem.FirstChild.FirstChild.Value.Split('=')[1]).Label;
                                            break;
                                    }
                                }
                            }
                            //stanzaQueue.Add(new Message(elem));
                            break;

                        case "message":
                            //stanzaQueue.Add(new Message(elem));
                            break;

                        case "presence":
                            //stanzaQueue.Add(new Presence(elem));
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                //Add the failed connection
                if (e is IOException)
                {
                    Connected = false;
                }
                
                // Raise the error event.
                if (!_disposed)
                {
                    Error?.Invoke(this, new ErrorEventArgs(e));
                }
            }
        }

        /// <summary>
		/// Closes the connection with the XMPP server. This automatically disposes
		/// of the object.
		/// </summary>
		/// <exception cref="ObjectDisposedException">The XmppIm object has been
		/// disposed.</exception>
		public void Close()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            Dispose();
        }

        /// <summary>
		/// Releases all resources used by the current instance of the XmppIm class.
		/// </summary>
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
                    if (_client != null)
                        _client.Close();
                    _client = null;
                }
            }
        }
    }
}
