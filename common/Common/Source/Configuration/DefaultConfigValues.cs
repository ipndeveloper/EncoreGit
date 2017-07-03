using System;
using System.Collections.Generic;

namespace NetSteps.Common.Configuration
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Class to provide default values for config settings.
    /// TODO: Make this also have default values for the various enviroments. - JHE
    /// Created: 04-19-2010
    /// </summary>
    public class DefaultConfigValues
    {
        private static Lazy<Dictionary<string, string>> _defaultValues = new Lazy<Dictionary<string, string>>(() =>
            {
                Dictionary<string, string> defaultValues = new Dictionary<string, string>();
                AddDefaultValues(defaultValues);
                return defaultValues;
            }, true);

        private static void AddDefaultValues(Dictionary<string, string> defaultValues)
        {
            defaultValues.Add("HandlerImages", "false");
            defaultValues.Add("TinyHandlerImages", "false");
            defaultValues.Add("CacheNavigation", "true");
            defaultValues.Add("CacheTextImages", "true");
            defaultValues.Add("TaxCachingDurationDays", "1");


            defaultValues.Add("ViewstateCompression", "CompressionType.GZip");

            defaultValues.Add("AlertEmailAddresses", "johne@netsteps.com");
            defaultValues.Add("LogErrors", "true");

            defaultValues.Add("IsPaymentLiveMode", "false");
            defaultValues.Add("IsPaymentTestTransaction", "true");
            defaultValues.Add("EnableFailSafeCreditCardCode", "false");

        }

        public static string GetDefaultConfigValue(string key)
        {
            if (_defaultValues.Value.ContainsKey(key))
                return _defaultValues.Value[key];
            else
                return null;
        }
    }
}
