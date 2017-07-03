using System;
using System.IO;
using System.Reflection;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.Utility
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Helper class to get GoogleAnalyticsScript
    /// Created: 03-25-2011
    /// </summary>
    public class GoogleAnalyticsHelper
    {
        private static bool _cacheResources = true; // False for testing - JHE
        private static object _lock = new object();
        private static string resourcePath = "NetSteps.Common.Resources.{0}";

        private static string _googleAnalyticsSnippet = string.Empty;
        private static string GoogleAnalyticsSnippet
        {
            get
            {
                if (!_cacheResources)
                    _googleAnalyticsSnippet = null;

                if (string.IsNullOrEmpty(_googleAnalyticsSnippet))
                {
                    lock (_lock)
                    {
                        if (string.IsNullOrEmpty(_googleAnalyticsSnippet))
                        {
                            try
                            {
                                using (StreamReader textStreamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format(resourcePath, "GoogleAnalyticsSnippet.js"))))
                                {
                                    while (!textStreamReader.EndOfStream)
                                        _googleAnalyticsSnippet = textStreamReader.ReadToEnd();
                                }
                            }
                            catch
                            {
                                throw new Exception("Error loading resource: GoogleAnalyticsSnippet.js");
                            }
                        }
                    }
                }
                return _googleAnalyticsSnippet;
            }
        }

        public static string GetGoogleAnalyticsScript(string trackerID, string domainName)
        {
            if (ApplicationContextCommon.Instance.IsLocalHost || trackerID.IsNullOrEmpty())
                return string.Empty;
            else
                return GoogleAnalyticsSnippet.Replace("{{TrackerID}}", trackerID).Replace("{{DomainName}}", domainName);
        }

    }
}
