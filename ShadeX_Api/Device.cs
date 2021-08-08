using System;
using System.Collections.Generic;
using System.Text;

namespace ShadeX_Core
{
    public partial class ShadeApi
    {
        public struct DeviceCommand
        {
            public string command_request;
            public string command_response;
            public DeviceCommand(string command_request,string command_response)
            {
                this.command_request = command_request;
                this.command_response = command_response;
            }
        }
    }
}
