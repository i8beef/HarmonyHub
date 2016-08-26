using System;
using System.Configuration;
using System.Linq;

namespace HarmonyHub.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];
            string ip = ConfigurationManager.AppSettings["ip"];

            using (var client = new Client(ip, username, password))
            {
                // Setup event handlers
                client.MessageSent += (o, e) => { Console.WriteLine(e.Message); };
                client.MessageReceived += (o, e) => { Console.WriteLine(e.Message); };
                client.Error += (o, e) => { Console.WriteLine(e.GetException().Message); };

                client.Connect();

                var config = client.GetConfigAsync().Result;

                var pvr = config.Device.FirstOrDefault(x => x.Label == "Pace DVR");
                var channelAction = pvr.ControlGroup.FirstOrDefault(x => x.Name == "Channel");
                Console.ReadLine();
            }
        }
    }
}
