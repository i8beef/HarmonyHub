# HarmonyHub

HarmonyHub is an event based .NET library for interacting with a Logitech Harmony Hub. 

### Where to get it

Right now this library is in beta and is undergoing active development. It will be published as a NuGet package under the name "HarmonyHub" once
a stable state has been reached.

### Architecture

The Logitech Harmony Hub exposes an XMPP server on port 5222 that this library makes use of. While there are several XMPP libraries available for .NET,
due to various issues with all of them, we chose to create our own internal implementation based on [Sharp.Xmpp](https://github.com/pgstath/Sharp.Xmpp).

The library leverages an event based approach as it felt more natural for a library that needed to listen for changes published from the hub itself. 

This library purposefully avoids any dependencies on third party libraries.

### Usage & Examples

To use this library, add a reference to it and follow the examples set below. There is also a test application in the repository that can be referenced 
for the basics.

Always Dispose() the client, or use it in a "using" statement as below. You can subscribe to any of the following events.

+ ConfigUpdated - Published when the internal Config is updated
+ CurrentActivityUpdated - Published when the current Activity is updated
+ Error - Published on certain errors
+ MessageReceived - General debugging event published when a message is received from the Harmony Hub
+ MessageSent - General debugging event published when a message is sent to the Harmony Hub

The following is just an example. The actual names of your devices, control groups, functions, etc. will differ, and the only way to really find the 
right names is to examine the Config object you get from the Harmony Hub.

    using (var client = new HarmonyHub.Client(ip, username, password))
    {
        // Setup event handlers
        client.MessageSent += (o, e) => { Console.WriteLine(e.Message); };
        client.MessageReceived += (o, e) => { Console.WriteLine(e.Message); };
        client.ConfigUpdated += (o, e) => {
            /* 
             * Once the config has been loaded from the Harmony, it contains all of the available "activities",
             * "devices", and "sequences" that can be used through this library. Devices are nice enough to 
             * expose "controlGroups", like "Volume" or "Power", which expose "functions" like "VolumeUp", "VolumeDown", etc.
             * You can think of these "functions" as the buttons on your remote, which you "press" by sending a command.
             *
             * These "functions" will contains an "action" string which is a JSON object that is word-for-word sent to the
             * Harmony as the command to "press" it.
             *
             * Obviously, this can be simplified, an is just an example of how to walk the Config tree.
             */
            var pvr = client.Config.Device.FirstOrDefault(x => x.Label == "Pace DVR");
            var channelControlGroup = pvr.ControlGroup.FirstOrDefault(x => x.Name == "Channel");
            var channelUpFunction = channelControlGroup.Function.FirstOrDefault(x => x.Name == "ChannelUp");
            var channelUpAction = channelUpFunction.Action

            // The actual command is sent to 
            client.SendCommand(channelUpAction);
        };

        /*
         * Requests that the Harmony Hub send its current configuration. There is no guarentee that when this call returns,
         * the Config object will be available / updated. In fact, its more of a guarentee that it WON'T be available immediately.
         * Subscribe to the ConfigUpdated event, as above.
         */
        client.RequestConfig();

        /*
         * Requests that the Harmony Hub send the current active Activity. This is basically the only real "state" that the
         * Harmony Hub has. This call is dependent on the Config object being populated, as the Harmony Hub only sends back 
         * an ID that needs to be looked up in the Config to get the actual Name of the activity, etc.
         *
         * As such, given the above warning about the config object not being populated immediately after the RequestConfig() call,
         * if you actually try this example it will fail. This call is more appropriate (a) in a ConfigUpdated event handler, 
         * (b) Made from a caller that already has verified that the Config object is present.
         */
        client.RequestCurrentActivity();
    }

### Credits
Lots of inspiration taken from various places.

+ [Sharp.Xmpp](https://github.com/pgstath/Sharp.Xmpp) for much of the groundwork of the Xmpp implementation used in this library
+ [PyHarmony](https://github.com/jterrace/pyharmony) for an excellent blueprint of the authorization process (https://github.com/jterrace/pyharmony/blob/master/PROTOCOL.md)
+ [hdurdle's Harmony project](https://github.com/hdurdle/harmony) for inspiration of a baseline implementation
+ [ctorx's JSON serializer helper](http://stackoverflow.com/questions/9573119/how-to-parse-json-without-json-net-library) to eliminate any reliance or dependency on JSON.Net, etc.

### License

This library is released under the [MIT license](https://github.com/i8beef/HarmonyHub/blob/master/LICENSE).

### Bug reports

Please create a new issue on the GitHub project homepage.