using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;

namespace NetSteps.Silverlight
{
    public class ApplicationContext
    {
        public static Uri ContainingHtmlPageUri
        {
            get
            {
                return HtmlPage.Document.DocumentUri;
            }
        }

        private static Uri _xapUri = null;
        public static Uri XapUri
        {
            get
            {
                if (_xapUri == null)
                    _xapUri = Application.Current.Host.Source;
                return _xapUri;
            }
        }
        public static Uri ResolveUrl(Uri baseUri, string relativeUrl)
        {
            try
            {
                //rather than check for all valid absolute Uri's [mailTo, http, etc], we'll let an exception catch it
                return new Uri(relativeUrl);
            }
            catch
            {
                //must be relative Uri
            }

            if (relativeUrl.StartsWith("~"))
                relativeUrl = relativeUrl.Substring(1);

            string directory = baseUri.LocalPath;
            int lastSlash = directory.LastIndexOf("/");
            if (lastSlash != -1)
                directory = directory.Substring(0, lastSlash);

            if (relativeUrl.Length >= directory.Length && relativeUrl.Substring(0, directory.Length).ToLower() == directory.ToLower())
                relativeUrl = relativeUrl.Substring(directory.Length);

            Uri uri = new Uri(baseUri, directory + ("/" + relativeUrl).Replace("//", "/"));
            return uri;
        }

        public static Uri ResolveAbsoluteURL(Uri baseUri, string relativeUrl)
        {
            try
            {
                //rather than check for all valid absolute Uri's [mailTo, http, etc], we'll let an exception catch it
                return new Uri(relativeUrl);
            }
            catch
            {
                //must be relative Uri
            }

            if (relativeUrl.StartsWith("~"))
                relativeUrl = relativeUrl.Substring(1);

            string directory = baseUri.LocalPath;
            int lastSlash = directory.LastIndexOf("/");
            if (lastSlash != -1)
                directory = directory.Substring(0, lastSlash);

            if (relativeUrl.Length >= directory.Length && relativeUrl.Substring(0, directory.Length).ToLower() == directory.ToLower())
                relativeUrl = relativeUrl.Substring(directory.Length);

            Uri uri = new Uri(baseUri, ("/" + relativeUrl).Replace("//", "/"));
            return uri;
        }

        public static Uri ResolveAbsoluteURL(string relativeUrl)
        {
            return ResolveAbsoluteURL(ContainingHtmlPageUri, relativeUrl);
        }

        public static Uri ResolveUrlWithHtmlPageAsBase(string relativeUrl)
        {
            return ResolveUrl(ContainingHtmlPageUri, relativeUrl);
        }

        public static Uri ResolveUrlWithXapAsBase(string relativeUrl)
        {
            return ResolveUrl(XapUri, relativeUrl);
        }

        public static Uri ResolveAbsoluteUrl(string absoluteUrl)
        {
            return new Uri(absoluteUrl);
        }

        public static bool IsInDesignMode
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(new Button());
            }
        }

        public static bool IsInRuntimeMode
        {
            get { return !IsInDesignMode; }
        }

        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsLocalhost
        {
            get
            {
                if (Application.Current.Host.Source.ToString().ToUpper().Contains("LOCALHOST"))
                    return true;
                else
                    return false;
            }
        }

        public static int _siteId = 0;
        public static int SiteId
        {
            get
            {
                return _siteId;
            }
            set
            {
                _siteId = value;
            }
        }

        public static string _securityId = string.Empty;
        public static string SecurityId
        {
            get
            {
                return _securityId;
            }
            set
            {
                _securityId = value;
            }
        }

        public static string _sso = string.Empty;
        public static string Sso
        {
            get
            {
                return _sso;
            }
            set
            {
                _sso = value;
            }
        }

        public static int _languageId = 0;
        public static int LanguageId
        {
            get
            {
                return _languageId;
            }
            set
            {
                _languageId = value;
            }
        }

        public static int _marketId = 0;
        public static int MarketId
        {
            get
            {
                return _marketId;
            }
            set
            {
                _marketId = value;
            }
        }

        public static Uri _applicationHostSource = null;
        public static Uri ApplicationHostSource
        {
            get
            {
                return _applicationHostSource;
            }
            set
            {
                _applicationHostSource = value;
            }
        }

        private static string _serviceDomain = string.Empty;
        public static string ServiceDomain
        {
            get
            {
                return _serviceDomain;
            }
            set
            {
                _serviceDomain = value;
            }
        }

        private static string _pwsLink = string.Empty;
        public static string PwsLink
        {
            get
            {
                return _pwsLink;
            }
            set
            {
                _pwsLink = value;
            }
        }

        private static bool _useBinaryMessageEncoding = false;
        public static bool UseBinaryMessageEncoding
        {
            get
            {
                return _useBinaryMessageEncoding;
            }
            set
            {
                _useBinaryMessageEncoding = value;
            }
        }

        private static bool _EnableIMAP = true;
        public static bool EnableIMAP
        {
            get
            {
                return _EnableIMAP;
            }
            set
            {
                _EnableIMAP = value;
            }
        }

        private static string _UserName = string.Empty;
        public static string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }

        private static string _Password = string.Empty;
        public static string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }


        public static bool _useIsolatedStorageCache = false;
        public static bool UseIsolatedStorageCache
        {
            get
            {
                return _useIsolatedStorageCache;
            }
            set
            {
                _useIsolatedStorageCache = value;
            }
        }
    }
}
