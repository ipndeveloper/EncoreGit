using System.Web;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;

namespace NetSteps.Web
{
    public static class WebContext
    {
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
        public static bool IsLocalHost
        {
            get
            {
                try
                {
                    if (!IsServerVariablesAvailable())
                        return false;

                    if (HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString().IndexOf("::1") >= 0 ||
                        HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString().IndexOf("127.0.0.1") >= 0 ||
                        HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString().ToUpper().IndexOf("LOCALHOST") >= 0 ||
                        HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString().ToUpper().Replace("WWW.", string.Empty) == "LOCALHOST")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
        public static bool IsUsingSsl
        {
            get
            {
                if (!IsServerVariablesAvailable())
                    return false;

                return (HttpContext.Current.Request.ServerVariables["HTTPS"].ToLower() == "on");
            }
        }

        public static string RemoteIpAddress
        {
            get
            {
                if (!IsServerVariablesAvailable())
                    return string.Empty;

                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
        }
        public static string ServerIpAddress
        {
            get
            {
                if (!IsServerVariablesAvailable())
                    return string.Empty;

                return HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString();
            }
        }

        public static string PageUrl
        {
            get
            {
                if (!IsServerVariablesAvailable())
                    return string.Empty;

                return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            }
        }
        public static string PageUrlWithQueryString
        {
            get
            {
                if (!IsServerVariablesAvailable())
                    return string.Empty;

                string queryString = HttpContext.Current.Request.QueryString.ToString();
                if (!string.IsNullOrEmpty(queryString))
                    queryString = "?" + queryString;

                return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString() + queryString;
            }
        }
        public static string PageTitleFromUrl
        {
            get
            {
                if (!IsServerVariablesAvailable())
                    return string.Empty;

                string pageUrl = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
                int startPoint = pageUrl.LastIndexOf('/') + 1;
                int endPoint = pageUrl.LastIndexOf('.');
                string returnValue = pageUrl.Substring(startPoint, pageUrl.Length - startPoint - (pageUrl.Length - endPoint)).PascalToSpaced();
                return returnValue;
            }
        }
        public static string QueryString
        {
            get
            {
                if (HttpContext.Current == null ||
                    HttpContext.Current.Request == null ||
                    HttpContext.Current.Request.QueryString == null)
                    return string.Empty;

                return HttpContext.Current.Request.QueryString.ToString();
            }
        }
        public static string Browser
        {
            get
            {
                if (!IsHttpContextRequestAvailable())
                    return string.Empty;

                if (HttpContext.Current.Request.Browser.AOL)
                    return "AOL";
                else
                    return HttpContext.Current.Request.Browser.Browser;
            }
        }

        public static string ServerName
        {
            get
            {
                try
                {
                    string httpHost = GetServerVariable("HTTP_HOST");
                    if (!httpHost.IsNullOrEmpty())
                    {
                        string serverName = httpHost.ToUpper().Replace("WWW.", string.Empty);
                        int startChar = serverName.IndexOf(":");
                        if (startChar != -1)
                            serverName = serverName.Substring(0, startChar);

                        return serverName;
                    }
                    else
                        return string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public static string WebRoot
        {
            get
            {
                try
                {
                    if (!IsServerVariablesAvailable() || !IsHttpContextRequestAvailable())
                        return string.Empty;

                    string webRoot = string.Format("/{0}/", HttpContext.Current.Request.ApplicationPath.Trim().Replace("/", string.Empty));

                    if (!string.IsNullOrEmpty(webRoot) || webRoot == "//")
                        webRoot = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/";

                    return webRoot;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public static string ReferalUrl
        {
            get
            {
                if (!IsHttpContextRequestAvailable())
                    return string.Empty;

                return HttpContext.Current.Request.UrlReferrer == null ? string.Empty : HttpContext.Current.Request.UrlReferrer.ToString();
            }
        }


        public static string PageUrlWithDomain
        {
            get
            {
                if (!IsHttpContextRequestAvailable())
                    return string.Empty;

                return (WebRoot + PageUrl).ToCleanString().Replace("//", "/");
            }
        }

        #region Webconfig Values
        public static Configuration WebConfigSettings
        {
            get
            {
                Configuration settings = null;
                if (HttpContext.Current.Cache["NetStepsWebModuleConfig"] == null)
                {
                    settings = (Configuration)System.Configuration.ConfigurationManager.GetSection("NetStepsWeb/Settings");
                    if (settings != null)
                        HttpContext.Current.Cache["NetStepsWebModuleConfig"] = settings;
                    else
                    {
                        settings = new Configuration();
                        HttpContext.Current.Cache["NetStepsWebModuleConfig"] = settings;
                    }
                }
                else
                    settings = (Configuration)HttpContext.Current.Cache["NetStepsWebModuleConfig"];
                return settings;
            }
        }

        public static class WebConfig
        {
            public static string PathToSAN
            {
                get
                {
                    string pathToSAN = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.PathToSAN);
                    if (!string.IsNullOrEmpty(pathToSAN))
                        return pathToSAN;
                    else
                        return @"\\Production";
                }
            }

            public static string FileUploadAbsolutePath
            {
                get
                {
                    string imagesAbsolutePath = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadAbsolutePath);
                    if (!string.IsNullOrEmpty(imagesAbsolutePath))
                        return imagesAbsolutePath;
                    else
                        return @"";
                }
            }

            //public static string ImagesWebPath
            //{
            //    get
            //    {
            //        if (!string.IsNullOrEmpty(CustomConfigurationHandler.Config.FilePaths.ImagesWebPath))
            //            return CustomConfigurationHandler.Config.FilePaths.ImagesWebPath;
            //        else
            //            return @"";
            //    }
            //}

            //public static string EmailDomainName
            //{
            //    get
            //    {
            //        if (!string.IsNullOrEmpty(CustomConfigurationHandler.Config.Mail.EmailDomainName))
            //            return CustomConfigurationHandler.Config.Mail.EmailDomainName;
            //        else
            //            return string.Empty;
            //    }
            //}

            //public static int SubscriptionItemsCatalogID
            //{
            //    get
            //    {
            //        return CustomConfigurationHandler.Config.IDs.SubscriptionItemsCatalogID;
            //    }
            //}

            //public static string ProxyUrlFormatString
            //{
            //    get
            //    {
            //        if (!string.IsNullOrEmpty(CustomConfigurationHandler.Config.Urls.SiteLoginUrl))
            //            return CustomConfigurationHandler.Config.Urls.SiteLoginUrl;
            //        else
            //            return string.Empty;
            //    }
            //}

            //public static int EnglishLanguageId
            //{
            //    get
            //    {
            //        return CustomConfigurationHandler.Config.IDs.EnglishLanguageId;
            //    }
            //}

            //public static int AccountDefaultFilterStatusId
            //{
            //    get
            //    {
            //        return CustomConfigurationHandler.Config.IDs.AccountDefaultFilterStatusId;
            //    }
            //}

            public static bool CacheHandlerImages
            {
                get
                {
                    return ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.HandlerImages);
                }
            }
            public static bool CacheTinyHandlerImages
            {
                get
                {
                    return ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.TinyHandlerImages);
                }
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Allows safe gets of server variables; in case the server variables are not available - JHE
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetServerVariable(string name)
        {
            if (HttpContext.Current == null ||
                HttpContext.Current.Request == null ||
                HttpContext.Current.Request.ServerVariables == null)
                return string.Empty;

            string value = HttpContext.Current.Request.ServerVariables[name];
            if (!string.IsNullOrEmpty(value))
                value = value.Trim();

            return value;
        }

        /// <summary>
        /// Allows safe gets of server variables; to check that server variables are available - JHE
        /// </summary>
        /// <returns></returns>
        public static bool IsServerVariablesAvailable()
        {
            if (HttpContext.Current != null &&
                HttpContext.Current.Request != null &&
                HttpContext.Current.Request.ServerVariables != null)
                return true;
            else
                return false;
        }

        public static bool IsHttpContextRequestAvailable()
        {
            if (HttpContext.Current != null &&
                HttpContext.Current.Request != null)
                return true;
            else
                return false;
        }

        public static bool IsSessionAvailable()
        {
            return HttpContext.Current != null
                && HttpContext.Current.Session != null;
        }
        #endregion
    }
}