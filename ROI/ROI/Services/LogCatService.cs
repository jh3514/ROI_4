using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROI.Services
{
    public class LogCatService
    {
        private static string Path = "./Log.tctd";
        private static bool IsCreated = false;

        private static bool Init()
        {
            try
            {
                // Check Log file Path & File Exists
                if (!File.Exists(Path))
                {
                    File.Create(Path);
                }
                IsCreated = true;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static void Write(string msg)
        {
            if (!Init())
                return;
            try
            {
                StreamWriter writer = File.AppendText(Path);
                writer.WriteLine(msg);
                writer.Close();
            }
            catch
            {
                //Exception Process
            }
        }
    }
}
