using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using NetSteps.Common;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Page Extensions
    /// Created: 03-18-2009
    /// </summary>
    public static class PageExtensions
    {
        public enum BrowserVersion
        {
            IE6,
            IE7,
            IE8
        }

        /// <summary>
        /// This method guarantees that only one instance of the css get included in the page header. - JHE
        /// </summary>
        public static void AddFavicon(this System.Web.UI.Page value, string faviconPath)
        {
            if (value.Header == null)
                throw new Exception("Missing runat=server attribute on <head> tag.");

            string key = IO.GetFileName(faviconPath).Replace(".", "_");
            if (value.Header.FindControl(key) == null)
            {
                HtmlLink link = NewAspPageFavIcon(faviconPath);
                link.ID = key;
                value.Header.Controls.Add(link);
            }
        }

        public static void RemoveAllCssStyleSheets(this System.Web.UI.Page value)
        {
            if (value.Header == null)
                throw new Exception("Missing runat=server attribute on <head> tag.");

            for (int i = value.Header.Controls.Count - 1; i >= 0; i--)
            {
                if (value.Header.Controls[i] is HtmlLink)
                {
                    HtmlLink link = (value.Header.Controls[i] as HtmlLink);
                    if (link.Attributes["type"] == "text/css" && link.Attributes["rel"] == "stylesheet")
                        value.Header.Controls.Remove(value.Header.Controls[i]);
                }
            }
        }

        /// <summary>
        /// This method guarantees that only one instance of the css get included in the page header. - JHE
        /// </summary>
        public static void AddCss(this System.Web.UI.Page value, string cssPath)
        {
            if (value.Header == null)
                throw new Exception("Missing runat=server attribute on <head> tag.");

            string key = IO.GetFileName(cssPath).Replace(".", "_");
            if (value.Header.FindControl(key) == null)
            {
                HtmlLink link = NewAspPageCss(cssPath);
                link.ID = key;
                value.Header.Controls.Add(link);
            }
        }

        public static void AddCssIfBrowserIs(this System.Web.UI.Page value, string cssPath, BrowserVersion browserVersion)
        {
            if (value.Header == null)
                throw new Exception("Missing runat=server attribute on <head> tag.");

            string key = IO.GetFileName(cssPath).Replace(".", "_");
            if (value.Header.FindControl(key) == null)
            {
                HtmlLink link = NewAspPageCss(cssPath);
                link.ID = key;

                LiteralControl literal = new LiteralControl(string.Format("<!--[if {0}]>{1}<![endif]-->",
                    (browserVersion == BrowserVersion.IE7) ? "IE 7" : (browserVersion == BrowserVersion.IE6) ? "IE 6" : "LT IE 8",
                    link.ToHtml()));
                literal.ID = key;
                value.Form.Controls.AddAt(1, literal);
            }
        }

        public static void RemoveAllCssBlocks(this System.Web.UI.Page value)
        {
            if (value.Header == null)
                throw new Exception("Missing runat=server attribute on <head> tag.");

            for (int i = value.Header.Controls.Count - 1; i >= 0; i--)
            {
                if (value.Header.Controls[i] is LiteralControl)
                {
                    LiteralControl style = (value.Header.Controls[i] as LiteralControl);
                    if (style.Text.ContainsIgnoreCase("<style type=\"text/css\">"))
                        value.Header.Controls.Remove(value.Header.Controls[i]);
                }
            }
        }

        public static void AddCssBlock(this System.Web.UI.Page value, string cssBlock, bool wrapInStyleBlock)
        {
            if (value.Header == null)
                throw new Exception("Missing runat=server attribute on <head> tag.");

            string style = cssBlock;
            if (wrapInStyleBlock)
                style = string.Format("<style type=\"text/css\">{0}</style>", "body{font: 11px/1.5em Geneva, Arial, Helvetica, sans-serif;background: #ffffff;}");
            value.Header.Controls.Add(new LiteralControl() { Text = style });
        }

        /// <summary>
        /// This method guarantees that only one instance of the script get included in the page header. - JHE
        /// </summary>
        public static void AddJavascript(this System.Web.UI.Page value, string key, string javascriptPath)
        {
            if (value.Header == null)
                throw new Exception("Missing runat=server attribute on <head> tag.");

            if (!string.IsNullOrEmpty(javascriptPath) && !javascriptPath.ToUpper().StartsWith("HTTP:"))
                javascriptPath = VirtualPathUtility.ToAbsolute(javascriptPath);

            if (value.Header.FindControl(key) == null)
            {
                HtmlGenericControl script = NewAspPageJavascript(javascriptPath);
                script.ID = key;
                value.Header.Controls.Add(script);
            }
        }

        /// <summary>
        /// This method guarantees that only one instance of the script get included in the page header. - JHE
        /// </summary>
        public static void AddJavascriptCode(this System.Web.UI.Page value, string key, string javascriptCode)
        {
            if (value.Header == null)
                throw new Exception("Missing runat=server attribute on <head> tag.");

            if (value.Header.FindControl(key) == null)
            {
                string script = "\n<script type=\"text/javascript\">//<![CDATA[{0}//]]></script>";
                LiteralControl literal = new LiteralControl(String.Format(script, "\n" + javascriptCode + "\n") + "\n");
                literal.ID = key;
                value.Controls.Add(literal);
            }
        }

        /// <summary>
        /// This method registers a script to run after page loads and places the script at the bottom of the page. - JHE
        /// </summary>
        public static void RegisterStartupScript(this System.Web.UI.Page page, string script, bool addScriptTags)
        {
            string uniqueKey = Guid.NewGuid().ToString();
            if (page != null)
                if (!ScriptManager.GetCurrent(page).IsInAsyncPostBack)
                    ScriptManager.RegisterStartupScript(page, typeof(System.Web.UI.Page), uniqueKey, script, addScriptTags);
        }

        #region Helper Methods
        public static HtmlLink NewAspPageCss(string cssPath)
        {
            HtmlLink link = new HtmlLink();
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");
            link.Attributes.Add("href", cssPath);
            return link;
        }

        public static HtmlGenericControl NewAspPageJavascript(string javascriptPath)
        {
            HtmlGenericControl Include = new HtmlGenericControl("script");
            Include.Attributes.Add("type", "text/javascript");
            Include.Attributes.Add("src", javascriptPath);
            return Include;
        }

        public static HtmlLink NewAspPageFavIcon(string favIconPath)
        {
            HtmlLink link = new HtmlLink();
            link.Attributes.Add("type", "image/x-icon");
            link.Attributes.Add("rel", "shortcut icon");
            link.Attributes.Add("href", favIconPath);
            return link;
        }
        #endregion
    }
}
