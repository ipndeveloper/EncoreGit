using System.Web;

namespace NetSteps.Web.Mvc.Helpers
{
    public static class BaseContext
    {
        public static string Root
        {
            get
            {
                return VirtualPathUtility.ToAbsolute("~/");
            }
        }

        public static T GetFromApplication<T>(string key)
        {
            object obj = HttpContext.Current.Application[key];
            if (obj == null)
                return default(T);

            return (T)obj;
        }

        public static void SetInApplication<T>(string key, T value)
        {
            if (value == null)
                HttpContext.Current.Application.Remove(key);
            else
                HttpContext.Current.Application[key] = value;
        }

        public static T GetFromSession<T>(string key)
        {
            object obj = HttpContext.Current.Session[key];
            if (obj == null)
            {
                return default(T);
            }
            return (T)obj;
        }

        public static void SetInSession<T>(string key, T value)
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(key);
            }
            else
            {
                HttpContext.Current.Session[key] = value;
            }
        }

        public static string PageUrl
        {
            get
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables == null)
                    return string.Empty;

                return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            }
        }

        public static string PageUrlWithQueryString
        {
            get
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables == null)
                    return string.Empty;

                string queryString = HttpContext.Current.Request.QueryString.ToString();
                if (!string.IsNullOrEmpty(queryString))
                    queryString = "?" + queryString;

                return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString() + queryString;
            }
        }
    }
}
