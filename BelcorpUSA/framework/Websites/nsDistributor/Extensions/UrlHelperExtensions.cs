using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Configuration;

namespace nsDistributor.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string Resolve(this UrlHelper helper, string path, params object[] pathParts)
        {
            bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
            if (path.StartsWith("~") && !isSubdomain)
            {
                var localPath = helper.RequestContext.HttpContext.Request.Url.LocalPath;
                path = "~" + localPath.Substring(0, localPath.IndexOf('/', 1));
            }

            path = helper.Content(path);
            path = HttpUtility.UrlPathEncode(path);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (object obj in pathParts)
            {
                Type type = obj.GetType();
                if (type.GetInterfaces().Length > 0)
                {
                    string str = Convert.ToString(obj, (IFormatProvider)CultureInfo.InvariantCulture);
                    path = path + "/" + HttpUtility.UrlPathEncode(str);
                }
                else
                {
                    foreach (PropertyInfo propertyInfo in type.GetProperties())
                    {
                        if (stringBuilder.Length == 0)
                            stringBuilder.Append('?');
                        else
                            stringBuilder.Append('&');
                        string str = Convert.ToString(propertyInfo.GetValue(obj, (object[])null), (IFormatProvider)CultureInfo.InvariantCulture);
                        stringBuilder.Append(HttpUtility.UrlEncode(propertyInfo.Name)).Append('=').Append(HttpUtility.UrlEncode(str));
                    }
                }
            }
            return path + (object)stringBuilder;
        }
    }
}