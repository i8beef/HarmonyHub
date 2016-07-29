# HarmonyHub

[![Build status](https://ci.appveyor.com/api/projects/status/e346wmks920k8ik7/branch/release?svg=true)](https://ci.appveyor.com/project/i8beef/harmonyhub/branch/release)
[![Build status](https://ci.appveyor.com/api/projects/status/e346wmks920k8ik7/branch/master?svg=true)](https://ci.appveyor.com/project/i8beef/harmonyhub/branch/master)

HarmonyHub is an event based .NET library for interacting with a Logitech Harmony Hub. 

### Where to get it

Run the following command in the NuGet Package Manager console to install the library:

    PM> Install-Package HarmonyHub

### Architecture

The Logitech Harmony Hub exposes an XMPP server on port 5222 that this library makes use of. While there are several XMPP libraries available for .NET,
due to various issues with all of them, we chose to create our own internal implementation based on [Sharp.Xmpp](https://github.com/pgstath/Sharp.Xmpp).

The library leverages an event based approach as it felt more natural for a library that needed to listen for changes published from the hub itself. 

This library purposefully avoids any dependencies on third party libraries.

### Usage & Examples

See wiki.

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