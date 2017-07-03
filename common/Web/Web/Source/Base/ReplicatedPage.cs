using System;
using System.Web;
using System.Web.UI;
using NetSteps.Objects.Business;
//using AjaxControlToolkit;

namespace NetSteps.Web.Base
{
    public class ReplicatedPage : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Page_PreInit(sender, e);
            Page.Theme = GetTheme(Page.Theme);
        }

        /// <summary>
        /// Gets the site's desired theme setting based off the custom config theme section
        /// </summary>
        /// <param name="defaultTheme"></param>
        /// <returns></returns>
        public string GetTheme(string defaultTheme)
        {
            try
            {
                //get theme config collection
                ThemeCollection list = ThemeConfigurationHandler.Config.Themes;
                if (list.Count == 0)
                    return defaultTheme;

                // see if we should load theme fresh from DB
                if (HttpContext.Current.Request.QueryString["ReloadTheme"] != null)
                    CurrentSite.Settings = null;

                SiteSettingValue setting = CurrentSite.Settings["ThemeID"];
                if (setting != null && setting.Value != null)
                {
                    int themeID = Convert.ToInt32(setting.Value);
                    Theme objTheme = list.GetByID(themeID);
                    if (objTheme == null)
                        return defaultTheme;
                    else
                        return objTheme.ThemeName;
                }
                else
                    return defaultTheme;
            }
            catch { return defaultTheme; }
        }
    }
}
