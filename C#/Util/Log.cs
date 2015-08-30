using System;
using System.IO;

namespace Util
{
    public class Log
    {
        public string LogFile { get; set; }

        public void LogMsg(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                LogError("Message passed was null or empty");
                return;
            }
            if(string.IsNullOrEmpty(LogFile))
            {
                LogError("LogFile is not set.");
                return;
            }

            string fileName = LogFile + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
            const string path = @"C:\Users\Public\Log\Send\";
            string fullPath = path + fileName;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var file = new System.IO.StreamWriter(fullPath, true))
            {
                file.WriteLine("INFO:" + message);
            }

        }
        private static void LogError(string message)
        {
            string fileName = "Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            const string path = @"C:\Users\Public\Log\Send\";
            string fullPath = path + fileName;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var file = new System.IO.StreamWriter(fullPath, true))
            {
                file.WriteLine("ERROR:" + message);
            }
        }
    }
}
