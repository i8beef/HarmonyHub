using System;
using System.Configuration;
using System.Linq;
using System.Threading;

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
                client.RequestConfig();
                client.RequestCurrentActivity();

                Thread.Sleep(2000);

                var pvr = client.Config.Device.FirstOrDefault(x => x.Label == "Pace DVR");
                var channelAction = pvr.ControlGroup.FirstOrDefault(x => x.Name == "Channel");
                client.SendCommand(channelAction.Function.FirstOrDefault().Action);
                Console.ReadLine();
            }
        }
    }
}
