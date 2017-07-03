using System.Collections.Concurrent;
using System.Globalization;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.Globalization
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Class to server up cached instances of CultureInfo 'lazy-load initialized'.
    /// Created: 01-13-2011
    /// </summary>
    public class CultureInfoCache
    {
        private static readonly ConcurrentDictionary<string, CultureInfo> _cachedCultures = new ConcurrentDictionary<string, CultureInfo>();

        public static CultureInfo GetCultureInfo(string countryCultureInfoCode)
        {
            if (!countryCultureInfoCode.IsNullOrEmpty())
                return _cachedCultures.GetOrAdd(countryCultureInfoCode, key => new CultureInfo(key));
            else
            {
                return GetCultureInfo("en-US");
            }
        }

        public static void ExpireCache()
        {
            _cachedCultures.Clear();
        }
    }
}
