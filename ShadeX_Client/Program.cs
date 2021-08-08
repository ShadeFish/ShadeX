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
            connectionHandler.GetConnection += Api_GetConnection;
            connectionHandler.LoseConnection += Api_LoseConnection;

            api = new ShadeApi(@"http://localhost/");

            config = new ConfigFile("config.cfg");

            

        }

        private static void Api_LoseConnection()
        {
            Console.WriteLine("Disconnected!");
        }

        private static void Api_GetConnection()
        {
            if(device_id == null)
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
                else
                {
                    Console.WriteLine("Connected: " + device_id);
                }
            }
            api.UpdateOnlineStatusTime(device_id);
        }
    }
}
