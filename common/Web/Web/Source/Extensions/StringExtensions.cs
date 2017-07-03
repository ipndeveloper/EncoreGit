using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using NetSteps.Common;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: String Extensions
    /// Created: 03-18-2009
    /// </summary>
    public static class StringExtensions
    {
        #region Enums
        public enum HtmlTag
        {
            None,
            H1,
            H2,
            H3,
            H4,
            H5,
            H6,
            B,
            Div,
            Span,
            Ul,
            Li,
            Center,
            JavaScriptBlock,
            JavaScriptStartupBlock,
            CssStyleBlock
        }

        public enum HeaderHtmlTag
        {
            None,
            H1,
            H2,
            H3,
            H4,
            H5,
            H6
        }
        #endregion

        #region Conversion Methods
        public static System.Drawing.Color ToColor(this string value)
        {
            try
            {
                return System.Drawing.ColorTranslator.FromHtml("#" + value.Replace("#", string.Empty));
            }
            catch
            {
                try
                {
                    return System.Drawing.Color.FromName(value);
                }
                catch
                {
                    return System.Drawing.Color.Transparent;
                }
            }
        }

        public static string ToHtmlBreaks(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Replace("\r", "<br/>").Replace("\n", "<br/>").Replace("<br/><br/>", "<br/>");
            else
                return string.Empty;
        }
        #endregion

        #region Remove/Replace Methods
        /// <summary>
        /// Remove any characters that are unfriendly in the URL - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CleanForUrl(this string value)
        {
            return Regex.Replace(value.Trim().Replace(" ", "_"), @"[\?#&\./:]", string.Empty);
        }
        #endregion

        public static string AppendQueryStringValue(this string url, string key, string value)
        {
            if (!Regex.IsMatch(url, "(\\?|&)" + key + "=", RegexOptions.IgnoreCase))
            {
                string fragment = string.Empty, u = url;
                if (url.Contains("#"))
                {
                    fragment = url.Substring(url.LastIndexOf("#"));
                    u = url.Substring(0, url.LastIndexOf("#"));

                }
                return new StringBuilder(u).Append(url.Contains("?") ? "&" : "?").Append(key).Append("=").Append(value).Append(fragment).ToString();
            }
            return url;
        }

        public static string AppendOrReplaceQueryStringValue(this string url, string key, string value)
        {
            if (!Regex.IsMatch(url, "(\\?|&)" + key + "=", RegexOptions.IgnoreCase))
            {
                string fragment = string.Empty, u = url;
                if (url.Contains("#"))
                {
                    fragment = url.Substring(url.LastIndexOf("#"));
                    u = url.Substring(0, url.LastIndexOf("#"));

                }
                return new StringBuilder(u).Append(url.Contains("?") ? "&" : "?").Append(key).Append("=").Append(value).Append(fragment).ToString();
            }
            else
            {
                url = Regex.Replace(url, "((?:\\?|&)" + key + "=)[^&#]*", "$1" + value, RegexOptions.IgnoreCase);
            }
            return url;
        }

        public static string RemoveQueryStringValue(this string url, string key)
        {
            var queryGroup = Regex.Match(url, RegularExpressions.URL).Groups["query"];
            if (queryGroup.Success)
            {
                string queryString = queryGroup.Value;
                if (queryString.ContainsIgnoreCase("?" + key + "="))
                    queryString = Regex.Replace(url, "\\?" + key + "=[^&#]*[&#]", "?", RegexOptions.IgnoreCase);
                queryString = Regex.Replace(url, "&" + key + "=[^&#]*[&#]", string.Empty, RegexOptions.IgnoreCase);
                return Regex.Replace(url, @"\?[^#]*", queryString, RegexOptions.IgnoreCase);
            }
            return url;
        }

        internal static string GetAttributeValueTag(string attribute, int value)
        {
            return GetAttributeValueTag(attribute, (value != 0) ? value.ToString() : string.Empty);
        }
        internal static string GetAttributeValueTag(string attribute, string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Trim();

            return (!string.IsNullOrEmpty(value)) ? string.Format(" {0}=\"{1}\"", attribute, value) : string.Empty;
        }

        #region WrapInTag Methods
        public static string WrapInTag(this string value, HtmlTag htmlTag)
        {
            return WrapInTag(value, value, htmlTag, string.Empty);
        }
        public static string WrapInTag(this string value, HtmlTag htmlTag, string cssClass)
        {
            return WrapInTag(value, value, htmlTag, cssClass);
        }
        public static string WrapInTag(this string value, string text, HtmlTag htmlTag, string cssClass)
        {
            if (string.IsNullOrEmpty(text) || htmlTag == HtmlTag.None)
                return value;
            else
            {
                if (htmlTag == HtmlTag.JavaScriptBlock)
                    return string.Format("<script type=\"text/javascript\"> {0} </script>", text);
                if (htmlTag == HtmlTag.JavaScriptStartupBlock)
                    return string.Format("\n<script type=\"text/javascript\">\n $(function() {0} ); \n</script>", "{" + text + "}");
                else if (htmlTag == HtmlTag.CssStyleBlock)
                    return string.Format("\n<style type=\"text/css\">\n <!--\n {0} \n--> \n</style>", text);
                else
                    return value.ReplaceIgnoreCase(text.Trim(), string.Format("<{1}{2}>{0}</{1}>", text.Trim(), htmlTag.ToString().ToLower(), (!string.IsNullOrEmpty(cssClass)) ? string.Format(" class=\"{0}\"", cssClass) : string.Empty));
            }
        }


        public static string WrapInLinkTag(this string value, string hrefLink)
        {
            return WrapInLinkTag(value, hrefLink, string.Empty);
        }
        public static string WrapInLinkTag(this string value, string hrefLink, string cssClass)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return string.Format("<a href=\"{1}\"{2}>{0}</a>", value, (!string.IsNullOrEmpty(hrefLink)) ? hrefLink : "#", (!string.IsNullOrEmpty(cssClass)) ? string.Format(" class=\"{0}\"", cssClass) : string.Empty);
        }


        public static string WrapInImageTag(this string value)
        {
            return WrapInImageTag(value, string.Empty, System.Web.UI.WebControls.ImageAlign.AbsMiddle, 0, 0);
        }
        public static string WrapInImageTag(this string value, int height, int width)
        {
            return WrapInImageTag(value, string.Empty, System.Web.UI.WebControls.ImageAlign.AbsMiddle, height, width);
        }
        public static string WrapInImageTag(this string value, System.Web.UI.WebControls.ImageAlign imageAlign)
        {
            return WrapInImageTag(value, string.Empty, imageAlign, 0, 0);
        }
        public static string WrapInImageTag(this string value, System.Web.UI.WebControls.ImageAlign imageAlign, int height, int width)
        {
            return WrapInImageTag(value, string.Empty, imageAlign, height, width);
        }
        public static string WrapInImageTag(this string value, string toolTip, System.Web.UI.WebControls.ImageAlign imageAlign)
        {
            return WrapInImageTag(value, toolTip, imageAlign, 0, 0);
        }
        public static string WrapInImageTag(this string value, string toolTip, System.Web.UI.WebControls.ImageAlign imageAlign, int height, int width)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            else
                return string.Format("<img src=\"{0}\"{3}{4} alt=\"{1}\"{2}/>", value, toolTip,
                    (imageAlign != ImageAlign.NotSet) ? string.Format(" align=\"{0}\"", imageAlign.ToString()) : string.Empty,
                    GetAttributeValueTag("height", height),
                    GetAttributeValueTag("width", width));
        }
        #endregion

        //public static string ResolveUrl(this string url)
        //{
        //    if (url == null)
        //        return null;

        //    if (url.IndexOf("://") != -1)
        //        return url;

        //    if (url.StartsWith("~"))
        //    {
        //        if (System.Web.HttpContext.Current != null)
        //        {
        //            return System.Web.HttpContext.Current.Request.ApplicationPath + (System.Web.HttpContext.Current.Request.ApplicationPath.EndsWith("/") ? string.Empty : "/") + url.Substring(2);
        //        }
        //        else
        //            throw new ArgumentException("Invalid URL: Relative URL not allowed.");
        //    }
        //    return url;
        //}

        public static string SetHtmlAttributeValue(this string value, string attribute, string attributeValue)
        {
            string currentAttributeValue = string.Empty;
            string returnValue = string.Empty;

            string attribTag = string.Format("{0}=", attribute);

            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value.ContainsIgnoreCase(attribTag))
                {
                    var match = Regex.Match(value, string.Format("{0}=(?<quote>[\\\"']?)", attribute));
                    var quoteMark = match.Groups["quote"].Success ? match.Groups["quote"].Value : string.Empty;

                    returnValue = Regex.Replace(value, string.Format("{0}={1}([^{2}]*){1}", attribute, quoteMark, string.IsNullOrEmpty(quoteMark) ? @"\s" : quoteMark), string.Format("{0}=\"{1}\"", attribute, attributeValue), RegexOptions.IgnoreCase);
                }
                else
                {
                    returnValue = value.Insert(value.IndexOf(" "), string.Format(" {0}=\"{1}\" ", attribute, attributeValue));
                }
            }

            returnValue = returnValue.Replace("  ", " ");
            return returnValue;
        }

        public static string GetHtmlAttributeValue(this string value, string attribute)
        {
            var quoteMatch = Regex.Match(value, string.Format("{0}=(?<quote>[\\\"']?)", attribute));
            var quoteMark = quoteMatch.Groups["quote"].Success ? quoteMatch.Groups["quote"].Value : string.Empty;
            var match = Regex.Match(value, string.Format("{0}={1}(?<value>[^{2}]*){1}", attribute, quoteMark, string.IsNullOrEmpty(quoteMark) ? @"\s" : quoteMark));
            return match.Groups["value"].Success ? match.Groups["value"].Value : "";
        }

        // TODO: All other symbols to this list later - JHE
        // http://www.utexas.edu/learn/html/spchar.html
        public static string ToHtmlSpecialCharacters(this string value)
        {
            return value.Replace("<", "&lt;").Replace(">", "&gt;");
        }
        public static string FromHtmlSpecialCharacters(this string value)
        {
            return value.Replace("&lt;", "<").Replace("&gt;", ">");
        }

        public static string HtmlEncode(this string value)
        {
            return System.Web.HttpUtility.HtmlEncode(value);
        }
    }
}
