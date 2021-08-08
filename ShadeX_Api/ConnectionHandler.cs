using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;

namespace ShadeX_Core
{
    public class ConnectionHandler
    {
        public string adress;
        public bool connected;

        /*             CONNECTION EVENTS              */
        public delegate void DelegateConnected();
        public event DelegateConnected GetConnection;

        public delegate void DelegateLoseConnection();
        public event DelegateLoseConnection LoseConnection;

        public ConnectionHandler(string adress) : base()
        {
            this.adress = adress;

            /* Connection Handler */
            new Thread(delegate () {
                while (true)
                {
                    if (Request(adress, new NameValueCollection() { { "action", "test" } }) == "True")
                    {
                        if(!connected)
                        {
                            connected = true;
                            GetConnection();
                        }
                    }
                    else
                    {
                        if(connected)
                        {
                            connected = false;
                            LoseConnection();
                        } 
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }
        public string Request(string adress, NameValueCollection args)
        {
            using (WebClient client = new WebClient())
                {
                    try
                    {
                        byte[] response_ = client.UploadValues(adress, "POST", args);
                        string text_response = Encoding.UTF8.GetString(response_);
                         Console.WriteLine("request response " + text_response);
                        return text_response;
                    }
                    catch (WebException E)
                    {

                    Console.WriteLine("REQUEST ERROR " + E.Message);
                        return null;
                    }
                }
            
        }

    }
}
