using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NetSteps.Web.Extensions;

namespace NetSteps.Web.Base
{
    public class BasicBaseMasterPage : System.Web.UI.MasterPage
    {
        #region Properties
        public SiteUser CurrentUser
        {
            get { return this.GetSessionVar<SiteUser>("CurrentUser", null); }
            set { this.SetSessionVar<SiteUser>("CurrentUser", value); }
        }

        public int LanguageId
        {
            get { return CurrentUser.LanguageID; }
        }

        public Dictionary<string, string> TermTranslations
        {
            get
            {
                if (this.GetSessionVar<Dictionary<string, string>>("TermTranslations", null) == null)
                {
                    NetSteps.Objects.Data.TermTranslationAdapter terms = new NetSteps.Objects.Data.TermTranslationAdapter();
                    this.SetSessionVar<Dictionary<string, string>>("TermTranslations", terms.LoadCollectionByLanguageID_2(this.LanguageId));
                }

                return this.GetSessionVar<Dictionary<string, string>>("TermTranslations", null);
            }
        }
        #endregion

        #region Initialize
        protected override void OnPreRender(EventArgs e)
        {
            TranslateTerms(this.Controls);
            //base.OnPreRender(e);
        }

        public void InitializeBaseMasterPage()
        {

        }
        #endregion

        #region Helper Methods
        public void CompressPage()
        {
            //TODO: Once this is LIVE. We should check performance gains on these websites - JHE
            //http://www.whatsmyip.org/mod_gzip_test/
            //http://www.gidnetwork.com/tools/gzip-test.php

            if (!Request.UserAgent.ToLower().Contains("konqueror"))
            {
                if (Request.Headers["Accept-encoding"] != null && Request.Headers["Accept-encoding"].Contains("gzip"))
                {
                    Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress, true);
                    Response.AppendHeader("Content-encoding", "gzip");
                }
                else if (Request.Headers["Accept-encoding"] != null && Request.Headers["Accept-encoding"].Contains("deflate"))
                {
                    Response.Filter = new DeflateStream(Response.Filter, CompressionMode.Compress, true);
                    Response.AppendHeader("Content-encoding", "deflate");
                }
            }
        }

        /// <summary>
        /// Will set headers appropriately to cache the page in the users Browers when minutes > 0.
        /// Set minutes to 0 to disable all caching on a page and force a new get everytime. - JHE
        /// </summary>
        /// <param name="minutes"></param>
        public void CachePage(int minutes)
        {
            if (minutes > 0)
            {
                Response.Cache.SetExpires(DateTime.Now.AddMinutes(minutes));
                Page.Response.Cache.SetSlidingExpiration(true);
                Response.Cache.SetCacheability(HttpCacheability.Public);
                Response.Cache.SetValidUntilExpires(true);
            }
            else
            {
                Response.Buffer = true;
                Response.ExpiresAbsolute = System.DateTime.Now;
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
            }
        }
        #endregion

        #region Virtual Methods
        /// <summary>
        /// Add global CSS and JavaScripts to the header of the page in this function. - JHE
        /// </summary>
        public virtual void FillHtmlHeader()
        {
        }
        #endregion

        #region Viewstate Variable Methods
        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetViewstateVar<T>(string name)
        {
            return ((BasicBasePage)Page).GetViewstateVar<T>(name, ViewState, default(T));
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetViewstateVar<T>(string name, T defaultValue)
        {
            return ((BasicBasePage)Page).GetViewstateVar<T>(name, ViewState, defaultValue);
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetViewstateVar<T>(string name, T value)
        {
            ((BasicBasePage)Page).SetViewstateVar<T>(name, ViewState, value);
        }
        #endregion

        #region Translation Methods
        public void TranslateTerms(ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is WebControl)
                {
                    WebControl webcontrol = control as WebControl;
                    //for list controls
                    if (webcontrol is ListControl)
                    {
                        foreach (ListItem item in (webcontrol as ListControl).Items)
                        {
                            string term = GetTranslatedTerm(item);
                            if (!String.IsNullOrEmpty(term))
                            {
                                item.Text = term;
                            }
                        }
                    }

                    //for text boxes
                    else if (webcontrol is TextBox)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as TextBox).Text = term;
                        }
                    }
                    //for checkboxes
                    else if (webcontrol is CheckBox)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as CheckBox).Text = term;
                        }
                    }
                    //for radio buttons
                    else if (webcontrol is RadioButton)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as RadioButton).Text = term;
                        }
                    }
                    //for link buttons
                    else if (webcontrol is LinkButton)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as LinkButton).Text = term;
                        }
                    }
                    //for buttons
                    else if (webcontrol is Button)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as Button).Text = term;
                        }
                    }
                    //for validators
                    else if (webcontrol is RequiredFieldValidator)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as RequiredFieldValidator).ErrorMessage = term;
                        }
                    }
                    //for compare validarors
                    else if (webcontrol is CompareValidator)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as CompareValidator).ErrorMessage = term;
                        }
                    }
                    //for custom validators
                    else if (webcontrol is CustomValidator)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as CustomValidator).ErrorMessage = term;
                        }
                    }
                    //for Validation summary
                    else if (webcontrol is ValidationSummary)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as ValidationSummary).HeaderText = term;
                        }
                    }
                    //for regularExpressionValidators 
                    else if (webcontrol is RegularExpressionValidator)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as RegularExpressionValidator).ErrorMessage = term;
                        }
                    }
                    else if (webcontrol is ValidatedTextBox)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as ValidatedTextBox).IsRequiredErrorMessage = term;
                        }
                    }
                    else if (webcontrol is ValidatedDropDownList)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!string.IsNullOrEmpty(term))
                        {
                            (webcontrol as ValidatedDropDownList).IsRequiredErrorMessage = term;
                        }
                    }
                    //for TemplateColumnTranslated 
                    else if (webcontrol is GridView)
                    {
                        if (((GridView)webcontrol).DataSource != null)
                        {
                            bool hasColumnTranslated = false;
                            foreach (DataControlField col in ((GridView)webcontrol).Columns)
                            {
                                if (col is BoundFieldTranslated || col is TemplateFieldTranslated || col is CheckBoxFieldTranslated
                                    || col is HyperLinkFieldTranslated)
                                {
                                    PropertyInfo info = col.GetType().GetProperty("TermName");
                                    string term = GetTranslatedTerm(info.GetValue(col, null).ToString());
                                    hasColumnTranslated = hasColumnTranslated || (!String.IsNullOrEmpty(term));

                                    if (!String.IsNullOrEmpty(term))
                                    {
                                        if (col is HyperLinkFieldTranslated)
                                            (col as HyperLinkFieldTranslated).Text = term;
                                        else
                                            col.HeaderText = term;
                                    }
                                }
                            }

                            if (hasColumnTranslated)
                                webcontrol.DataBind();
                        }
                    }
                    else if (webcontrol is HyperLink)
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (webcontrol as HyperLink).Text = term;
                        }
                    }
                    //for labels
                    else
                    {
                        string term = GetTranslatedTerm(webcontrol);
                        if (!String.IsNullOrEmpty(term))
                        {
                            if (webcontrol is Label)
                            {
                                Label label = webcontrol as Label;
                                label.Text = term;
                            }
                        }
                    }
                }
                else if (control is HtmlControl)
                {
                    //for Selec html control
                    if (control is HtmlSelect)
                    {
                        {
                            foreach (ListItem items in (control as HtmlSelect).Items)
                            {
                                string term = GetTranslatedTerm(items);
                                if (!String.IsNullOrEmpty(term))
                                {
                                    items.Text = term;
                                }
                            }
                        }
                    }
                    //finds <a> tags
                    else if (control is HtmlAnchor)
                    {
                        string term = GetTranslatedTerm(control as HtmlAnchor);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (control as HtmlAnchor).InnerText = term;
                        }
                    }
                    else if (control is HtmlTableCell)
                    {
                        // The translation of this is cause problems in postbacks - JHE
                        //string term = GetTranslatedTerm(control as HtmlTableCell);
                        //if (!String.IsNullOrEmpty(term))
                        //{
                        //    (control as HtmlTableCell).InnerText = term;
                        //}
                    }
                    else if (control is HtmlGenericControl)
                    {
                        string term = GetTranslatedTerm(control as HtmlGenericControl);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (control as HtmlGenericControl).InnerText = term;
                        }
                    }
                }

                if (control.Controls.Count > 0)
                    TranslateTerms(control.Controls);
            }
        }

        private string GetTranslatedTerm(WebControl control)
        {
            if (control.Attributes["TermName"] != null)
            {
                return GetTranslatedTerm(control.Attributes["TermName"]);
            }
            else
            {
                return String.Empty;
            }
        }

        private string GetTranslatedTerm(HtmlControl control)
        {
            if (control.Attributes["TermName"] != null)
            {
                return GetTranslatedTerm(control.Attributes["TermName"]);
            }
            else
            {
                return String.Empty;
            }
        }

        private string GetTranslatedTerm(ListItem control)
        {
            if (control.Attributes["TermName"] != null)
            {
                return GetTranslatedTerm(control.Attributes["TermName"]);
            }
            else
            {
                return String.Empty;
            }
        }

        public string GetTranslatedTerm(string termName)
        {
            if (!string.IsNullOrEmpty(termName))
            {
                try
                {
                    return TermTranslations[termName];
                }
                catch
                {
                    return string.Empty;
                }
            }
            else
            {
                return String.Empty;
            }
        }

        public string GetTranslatedTerm(string termName, string defaultText)
        {
            string term = GetTranslatedTerm(termName);
            if (!string.IsNullOrEmpty(term))
                return term;
            else
                return defaultText;
        }
        #endregion
    }
}
