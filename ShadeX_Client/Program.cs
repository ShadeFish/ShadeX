using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShadeX_Core;

namespace ShadeX_Client
{
    class Program
    {
        private static ShadeApi api;
        private static ConnectionHandler connectionHandler;
        private static ConfigFile config;
        private static string device_id;
        static void Main(string[] args)
        {
            connectionHandler = new ConnectionHandler(@"http://localhost/");
            connectionHandler.GetConnection += connectionHandler_GetConnection;
            connectionHandler.LoseConnection += connectionHandler_LoseConnection;

            api = new ShadeApi(connectionHandler);
            config = new ConfigFile("config.cfg");

        }

        private static void connectionHandler_LoseConnection()
        {
            Console.WriteLine("Disconnected!");
        }

        private static void connectionHandler_GetConnection()
        {
            Console.WriteLine("Connected to server. ");

            if (device_id == null)
            {
                device_id = config.GetValue("device_id");
                if (device_id == string.Empty)
                {
                    /* CREATE NEW DEVICE */
                    Random rand = new Random();
                    for (int i = 0; i < 9; i++) { device_id += rand.Next(9); }
                    while (!api.CreateNewDevice(device_id)) { Thread.Sleep(1000); }
                    config.SetValue("device_id", device_id);
                    Console.WriteLine("New Device Created: " + device_id);
                }
            }
            api.UpdateOnlineStatusTime(device_id);

            api.ResponseCommand("9", device_id, "dzialaj_kasdfasdfurwa");

            Console.ReadLine();

            new Thread(delegate() {
                while (connectionHandler.connected)
                {
                    DeviceCommand command = api.GetCommandToExecute(device_id);

                    if(command != DeviceCommand.Empty)
                    {
                        Console.WriteLine("Next COmmand : " + command.command_request);
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }
    }
}
