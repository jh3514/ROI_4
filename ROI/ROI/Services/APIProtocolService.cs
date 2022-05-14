using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace ROI.Services
{
    class APIProtocolService_Version
    {
        public string ServiceName = "APIProtocolService";
        public string ServiceVersion = "0.0.1";

    }
    public static class APIProtocolService
    {
        //Variant
        private static string APIServer_Addr = "127.0.0.1";
        private static int APIServer_Port = 3000;
        private static Ping ping;

        //Function
        public static bool Init()
        {
            //
            // Check Server State
            //

            ping = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string queryData = "live?";
            byte[] buffer = Encoding.ASCII.GetBytes(queryData);
            int timeout = 120;  //2 Min
            PingReply reply = ping.Send(IPAddress.Parse(APIServer_Addr), timeout, buffer, options);

            if (reply.Status == IPStatus.Success)
            {
                //Next Init
            }
            else
            {
                return false;
            }

            return true;
        }

        public static string QueryAPI()
        {
            string word = "/INFO";
            try
            {
                using (WebClient client = new WebClient())
                {
                    var json = new WebClient().DownloadString("http://" + APIServer_Addr + ":" + APIServer_Port + word);
                    return json.ToString();
                }
            }
            catch(Exception e)
            {
                string ee = e.ToString();
                return "Error";
            }
        }
    }
}
