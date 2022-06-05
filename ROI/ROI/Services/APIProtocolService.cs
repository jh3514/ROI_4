using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
        private static string Server_Addr = "127.0.0.1";
        private static int Server_Port = 3000;
        private static Ping ping;
        private static Socket client;
        private static byte[] data = new byte[1024];
        private static int data_size = 1024;
        private static string recv_data;
        public static bool recv_Flag = false;

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
            PingReply reply = ping.Send(IPAddress.Parse(Server_Addr), timeout, buffer, options);

            if (reply.Status == IPStatus.Success)
            {
                //Next Init
            }
            else
            {
                LogCatService.Write($"{DateTime.Now.ToString("MMddHHmmss")} :: Server is not Live");
                return false;
            }

            LogCatService.Write($"{DateTime.Now.ToString("MMddHHmmss")} :: Server is Live");
            return true;
        }

        /// <summary>
        /// QueryAPI Function is Only can use API Type Server
        /// </summary>
        public static string QueryAPI()
        {
            string word = "/INFO";
            try
            {
                using (WebClient client = new WebClient())
                {
                    var json = new WebClient().DownloadString("http://" + Server_Addr + ":" + Server_Port + word);
                    return json.ToString();
                }
            }
            catch(Exception e)
            {
                string ee = e.ToString();
                return "Error";
            }
        }

        /// <summary>
        /// StartSession Function is Only use TCP Server
        /// </summary>
        public static void StartSession()
        {
            try
            {
                Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(Server_Addr), Server_Port);
                LogCatService.Write($"{DateTime.Now.ToString("MMddHHmmss")} :: Client try connect to server");
                sck.BeginConnect(iep, new AsyncCallback(Connected), sck);
            }
            catch
            {

            }
        }

        private static void Connected(IAsyncResult iar)
        {
            client = (Socket)iar.AsyncState;

            try
            {
                client.EndConnect(iar);
                LogCatService.Write($"{DateTime.Now.ToString("MMddHHmmss")} :: Client is connected to server");
                client.BeginReceive(data, 0, data_size, SocketFlags.None, new AsyncCallback(ReceviedData), client);
            }
            catch
            {
                LogCatService.Write($"{DateTime.Now.ToString("MMddHHmmss")} :: Client is connected failed");
            }
        }

        private static void ReceviedData(IAsyncResult iar)
        {
            Socket remote = (Socket)iar.AsyncState;
            int recv = remote.EndReceive(iar);
            recv_data = Encoding.UTF8.GetString(data, 0, recv);

            // if received data type is emergency, flag is true
            if (recv_data.IndexOf("emergency") != -1)
                recv_Flag = true;
        }

        public static string getSessionData()
        {
            return recv_data;
        }

        /// <summary>
        /// StopSession Function is Only use TCP Server
        /// </summary>
        public static void StopSession()
        {
            client.Close();
        }
    }
}
