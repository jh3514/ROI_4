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
    public class APIProtocolService
    {
        //Variant
        private string API_Server = "127.0.0.1";
        private Ping ping;

        //Function
        public bool Init()
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
            PingReply reply = ping.Send(IPAddress.Parse(API_Server), timeout, buffer, options);

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
    }
}
