using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ParcelPortIntegrationService
{
    static class ConfigurationHelper
    {
        public static String GetParcelPortContextConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ParcelPortIntegrationService.Properties.Settings.ItWorksCoreConnectionString"].ConnectionString;
        }

        public static String GetParcelPortOwner()
        {
            return ConfigurationManager.AppSettings["ParcelPortOwner"];
        }

        public static String GetParcelPortURL()
        {
            return ConfigurationManager.AppSettings["ParcelPortURL"];
        }

        public static String GetParcelPortPostAttributute()
        {
            return ConfigurationManager.AppSettings["ParcelPortPostAttribute"];
        }

        public static String GetItemPrefix()
        {
            return ConfigurationManager.AppSettings["ParcelPortItemPrefix"];
        }

        public static String GetAPIKey()
        {
            return ConfigurationManager.AppSettings["ParcelPortAPIKey"];
        }

        public static int CommandTimeoutPeriod()
        {
            string commandTimeout = ConfigurationManager.AppSettings["CommandTimeoutPeriod"];

            int result;

            if (!String.IsNullOrEmpty(commandTimeout) && int.TryParse(commandTimeout, out result))
                return result;

            return 300; // seconds
        }
    }
}
