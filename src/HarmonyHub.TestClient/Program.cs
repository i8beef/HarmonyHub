using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HarmonyHub.TestClient
{
    class Program
    {
        /// <summary>
        /// Simple test program.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Task.Run(async () => await MainAsync(args)).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];
            string ip = ConfigurationManager.AppSettings["ip"];

            using (var client = new Client(ip, username, password))
            {
                // Setup event handlers
                client.MessageSent += (o, e) => { Debug.WriteLine(e.Message); };
                client.MessageReceived += (o, e) => { Debug.WriteLine(e.Message); };
                client.Error += (o, e) => { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(e.GetException().Message); Console.ResetColor(); };

                client.Connect();
                var config = await client.GetConfigAsync();

                char input = 'l';
                while (input != 'q')
                {
                    Console.ResetColor();
                    Console.Clear();
                    foreach (var device in config.Device)
                    {
                        Console.WriteLine($"Device: {device.Label} ({device.Manufacturer} {device.Model}) - {device.Id}");
                    }

                    Console.WriteLine();

                    switch (input)
                    {
                        case 'a':
                            var activityId = await client.GetCurrentActivityIdAsync();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Current activity: ");
                            Console.ResetColor();
                            Console.WriteLine($"{config.Activity.First(x => x.Id == activityId.ToString()).Label} ({activityId})");
                            break;
                        case 'c':
                            Console.WriteLine("EXECUTE COMMAND");
                            Console.WriteLine("===============");

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Device name: ");
                            Console.ResetColor();

                            var targetDeviceName = Console.ReadLine();

                            if (!config.Device.Any(x => x.Label == targetDeviceName))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Unknown device: {targetDeviceName}");
                                Console.ResetColor();
                                break;
                            }

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Available control groups: ");
                            Console.ResetColor();
                            foreach (var controlGroup in config.Device.First(x => x.Label == targetDeviceName).ControlGroup)
                            {
                                Console.Write($"{controlGroup.Name}, ");
                            }
                            Console.WriteLine();

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Control group: ");
                            Console.ResetColor();
                            var targetControlGroup = Console.ReadLine();

                            if (!config.Device.First(x => x.Label == targetDeviceName).ControlGroup.Any(x => x.Name == targetControlGroup))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Unknown control group: {targetControlGroup}");
                                Console.ResetColor();
                                break;
                            }

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Available control functions: ");
                            Console.ResetColor();
                            foreach (var controlFunction in config.Device.First(x => x.Label == targetDeviceName).ControlGroup.First(x => x.Name == targetControlGroup).Function)
                            {
                                Console.Write($"{controlFunction.Label}, ");
                            }
                            Console.WriteLine();

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Control function: ");
                            Console.ResetColor();
                            var targetControlFunction = Console.ReadLine();

                            if (!config.Device.First(x => x.Label == targetDeviceName).ControlGroup.First(x => x.Name == targetControlGroup).Function.Any(x => x.Label == targetControlFunction))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Unknown function: {targetControlFunction}");
                                Console.ResetColor();
                                break;
                            }

                            var commandAction = config.Device.First(x => x.Label == targetDeviceName).
                                ControlGroup.First(x => x.Name == targetControlGroup).
                                Function.First(x => x.Label == targetControlFunction).Action;
                            await client.SendCommandAsync(commandAction);
                            break;
                    }

                    Console.WriteLine();
                    Console.WriteLine("=======================================================");
                    Console.WriteLine("Enter choice: get current (a)ctivity, (c)ommand, (q)uit");
                    input = Console.ReadKey().KeyChar;
                }
            }
        }
    }
}
