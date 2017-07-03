using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace NetSteps.Hurricane.Plugin
{
    public static class Log
    {
        public static void Debug(string msg)
        {
            Logger.Write(msg, "Debug");
        }
        public static void Debug(string format, params object[] args)
        {
            string s = string.Format(format, args);
            Logger.Write(s, "Debug");
        }
        public static void Info(string msg)
        {
            Logger.Write(msg, "Info");
        }
        public static void Info(string format, params object[] args)
        {
            string s = string.Format(format, args);
            Logger.Write(s, "Info");
        }
        public static void Error(string msg)
        {
            Logger.Write(msg, "Error");
        }
        public static void Error(string format, params object[] args)
        {
            string s = string.Format(format, args);
            Logger.Write(s, "Error");
        }
    }
}
