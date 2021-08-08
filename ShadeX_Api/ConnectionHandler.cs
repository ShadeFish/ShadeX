using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading;

namespace ShadeX_Core
{
    public class ConnectionHandler : ShadeApi
    {
        private string adress;
        public bool connected;

        /*             CONNECTION EVENTS              */
        public delegate void DelegateConnected();
        public event DelegateConnected GetConnection;

        public delegate void DelegateLoseConnection();
        public event DelegateLoseConnection LoseConnection;

        public ConnectionHandler(string adress) : base(adress)
        {

            /* Connection Handler */
            new Thread(delegate () {
                while (true)
                {
                    if (Request(adress, new NameValueCollection() { { "action", "test" } }) == "True")
                    {
                        if (!connected)
                        {
                            GetConnection();
                            connected = true;
                        }
                    }
                    else
                    {
                        if (connected)
                        {
                            LoseConnection();
                            connected = false;
                        }
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }
    }
}
