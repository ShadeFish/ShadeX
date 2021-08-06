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
        static ShadeApi api;
        static ConfigFile config;
        static void Main(string[] args)
        {
            api = new ShadeApi(@"http://localhost/");
            config = new ConfigFile("config.cfg");

            api.GetConnection += Api_GetConnection;
            api.LoseConnection += Api_LoseConnection;

            Console.Read();
        }

        private static void Api_LoseConnection()
        {
            Console.WriteLine("Disconnected!");
        }

        private static void Api_GetConnection()
        {
            Console.WriteLine("Connected!");
            string device_id = config.GetValue("device_id");

            if(device_id == string.Empty)
            {
                /* CREATE NEW DEVICE */
                Random rand = new Random();
                for (int i = 0; i < 9; i++) { device_id += rand.Next(9); }
                while (!api.CreateNewDevice(device_id)) { Thread.Sleep(1000); }
                config.SetValue("device_id", device_id);
                Console.WriteLine("New Device Created!");
            }
            else
            {
                api.UpdateOnlineStatusTime(config.GetValue("device_id"));
            }

            
        }

       
    }
}
