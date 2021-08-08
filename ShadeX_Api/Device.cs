using System;
using System.Collections.Generic;
using System.Text;

namespace ShadeX_Core
{
    public class DeviceCommand
    {
        public string id;
        public string command_request;
        public string command_response;

        public static DeviceCommand Empty = new DeviceCommand(string.Empty,string.Empty, string.Empty);

        public DeviceCommand(string id,string command_request, string command_response)
        {
            this.id = id;
            this.command_request = command_request;
            this.command_response = command_response;
        }

        public override string ToString()
        {
            return "ID: " + id + " REQUST: " + command_request + " RESPONSE: " + command_response;
        }
    }
}
