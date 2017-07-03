using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NetSteps.Objects.Business;

namespace NetSteps.Web.Base
{
    public class SiteNotFoundByUrlException : Exception
    {
        public SiteNotFoundByUrlException(string message)
            : base(message)
        { }
    }

    public class BaseMasterPage : System.Web.UI.MasterPage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Add Keyword META tag and set Page Title
            if (Page.Header != null)
            {
                if (CurrentPage != null)
                {
                    //Page.Header.Title = CurrentPage.PageTitle;
                    Page.Header.Controls.Add(new LiteralControl(String.Format("<meta name=\"keywords\" content=\"{0}\"></meta>", CurrentPage.Keywords)));
                }
            }

            //TODO: Only translate if they have multiple languages
            TranslateTerms(this.Controls, true);
        }

        #region --- Properties that use the Utility class ---
        protected override object SaveViewState()
        {
            // -- So terms can be translated because binding that is happening in OnPreRender (Neways - bk)
            TranslateTerms(this.Controls, false);
            return base.SaveViewState();
        }

        public NetSteps.Common.Constants.ViewingMode PageMode
        {
            get { return Utility.PageMode; }
                        //Session["PageMode"] = NetSteps.Common.Constants.ViewingMode.Staging;
                        //Session["PageMode"] = NetSteps.Common.Constants.ViewingMode.Archive;
                        //Session["PageMode"] = NetSteps.Common.Constants.ViewingMode.Edit;
        }

        public NetSteps.Objects.Business.Site CurrentSite
        {
            get { return Utility.CurrentSite; }
            set { Utility.CurrentSite = value; }
        }

        public SitePage CurrentPage
        {
            get { return Utility.CurrentPage; }
        }

        public SiteUser CurrentUser
        {
            get { return Utility.CurrentUser; }
        }

        public void FillContent(ContentControl control, HtmlSection section, bool useNewEditWrapper)
        {
            Utility.FillContent(control, section, useNewEditWrapper);
        }

        public void FillContent(ContentControl control, HtmlSection section)
        {
            FillContent(control, section, false);
        }

        public int ReviewSectionId
        {
            get
            {
                return Utility.ReviewSectionId;
            }
        }

        public int ReviewContentId
        {
            get
            {
                return Utility.ReviewContentId;
            }
        }

        public Account CurrentAccount
        {
            get
            {
                return Utility.CurrentAccount;
            }
            set { Utility.CurrentAccount = value; }
        }

        #endregion

        public CultureInfo CurrentCulture
        {
            get
            {
                if (Session["CurrentCulture"] == null)
                {
                    CultureInfo currentCulture = new CultureInfo("en-US", false);
                    Session["CurrentCulture"] = currentCulture;
                }

                return (CultureInfo)Session["CurrentCulture"];
            }
        }

        protected string CurrentModeText
        {
            get
            {
                switch (PageMode)
                {
                    case NetSteps.Common.Constants.ViewingMode.Edit:
                    case NetSteps.Common.Constants.ViewingMode.ConsultantEdit:
                        return "Edit";
                    case NetSteps.Common.Constants.ViewingMode.Staging:
                    case NetSteps.Common.Constants.ViewingMode.Archive:
                        return "Preview";
                    default:
                        return "";
                }
            }
        }

        #region --- Term Translations ---

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="OnlyBindControls">Only process controls that require binding</param>
        private void TranslateTerms(ControlCollection controls, bool OnlyBindControls)
        {
            // added OnlyBindControls becuase some reason we are re binding controls just to translate them
            // NOTE even though you should not have to have the over head of rebinding every time, ex. DATAGRIDS with 1000s of items
            int termId = -1;
            foreach (Control control in controls)
            {
                if (control is WebControl)
                {
                    WebControl webcontrol = control as WebControl;
                    if (OnlyBindControls)
                    {
                        //for TemplateColumnTranslated 
                        if (webcontrol is DataGrid)
                        {
                            if (((DataGrid)webcontrol).DataSource != null)
                            {
                                bool hasColumnTranslated = false;
                                foreach (DataGridColumn col in ((DataGrid)webcontrol).Columns)
                                {
                                    if (col is TemplateColumnTranslated || col is BoundColumnTranslated)
                                    {
                                        PropertyInfo info = col.GetType().GetProperty("TermID");
                                        string term = GetTranslatedTerm(info.GetValue(col, null).ToString());
                                        hasColumnTranslated = hasColumnTranslated || (!String.IsNullOrEmpty(term));
                                        if (!String.IsNullOrEmpty(term))
                                            col.HeaderText = term;
                                    }
                                }
                                if (hasColumnTranslated)
                                    webcontrol.DataBind();
                            }
                        }
                    }
                    else
                    {
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
                        //else if (webcontrol is HiddenField  && !OnlyBindControls)
                        //{
                        //    string term = GetTranslatedTerm(webcontrol);
                        //    if (!String.IsNullOrEmpty(term))
                        //    {
                        //        (webcontrol as HiddenField).Value = term;
                        //    }
                        //}

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
                }
                else if (control is HtmlControl)
                {
                    if (control is HtmlGenericControl)
                    {
                        string term = GetTranslatedTerm(control as HtmlGenericControl);
                        if (!String.IsNullOrEmpty(term))
                        {
                            (control as HtmlGenericControl).InnerText = term;
                        }
                    }
                }
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
                if (control is HtmlAnchor)
                {
                    string term = GetTranslatedTerm(control as HtmlAnchor);
                    if (!String.IsNullOrEmpty(term))
                    {
                        (control as HtmlAnchor).InnerText = term;
                    }
                }

                if (control.Controls.Count > 0)
                    TranslateTerms(control.Controls, OnlyBindControls);
            }

        }

        #region GetTranslatedTerm Overloads
        private string GetTranslatedTerm(WebControl control)
        {
            if (control.Attributes["termid"] != null)
            {
                return GetTranslatedTerm(control.Attributes["termid"]);
            }
            return String.Empty;
        }

        private string GetTranslatedTerm(HtmlControl control)
        {
            if (control.Attributes["termid"] != null)
            {
                return GetTranslatedTerm(control.Attributes["termid"]);
            }
            return String.Empty;
        }

        private string GetTranslatedTerm(ListItem control)
        {
            if (control.Attributes["termid"] != null)
            {
                return GetTranslatedTerm(control.Attributes["termid"]);
            }
            return String.Empty;
        }

        //Returns English Error message if the term is empty
        public string GetTranslatedTerm(string termid, string englishDefault)
        {
            int tempTermId;
            if (Int32.TryParse(termid, out tempTermId))
            {
                try
                {
                    return CurrentSite.TermTranslations[tempTermId];
                }
                catch
                {
                    return englishDefault;
                }
            }
            return englishDefault;
        }


        public string GetTranslatedTerm(string termid)
        {
            int tempTermId;
            if (Int32.TryParse(termid, out tempTermId))
            {
                try
                {
                    return CurrentSite.TermTranslations[tempTermId];
                }
                catch
                {
                    return string.Empty;
                }
            }
            return String.Empty;
        }
        #endregion

        #endregion

        public Hashtable GetFemaleTitles(Hashtable femaleTitles, Account acct, AccountAddress addr)
        {
            if (Session["cachedF_Titles"] == null)
            {
                femaleTitles = getTitles(acct, addr);
            }
            else
            {
                femaleTitles = (Hashtable)Session["cachedF_Titles"];
            }

            return femaleTitles;
        }

        public Hashtable GetMaleTitles(Hashtable maleTitles, Account acct, AccountAddress addr)
        {
            if (Session["cachedM_Titles"] == null)
            {
                maleTitles = getTitles(acct, addr);

            }
            else
            {
                maleTitles = (Hashtable)Session["cachedM_Titles"];
            }

            return maleTitles;
        }

        //Get Cached Titles
        private Hashtable getTitles(Account acct, AccountAddress addr)
        {

            Hashtable maleTitles = new Hashtable();
            Hashtable femaleTitles = new Hashtable();
            //ArrayList sortedTitles = new ArrayList();
            Hashtable sortedHash = new Hashtable();
            addr.TitleLookUpData(maleTitles, femaleTitles);

            if (femaleTitles.Count > 0)
            {
                //sortedHash = new ArrayList(femaleTitles.Values);
                //sortedHash.Sort();
                sortedHash = femaleTitles;
                Session["cachedF_Titles"] = femaleTitles;
            }

            if (maleTitles.Count > 0)
            {
                //sortedHash = new ArrayList(maleTitles.Values);
                //sortedHash.Sort();
                //sortedTitles = sortedHash;
                sortedHash = maleTitles;
                Session["cachedM_Titles"] = maleTitles;
            }

            return sortedHash;

        }


        public CultureInfo CultureInfo
        {
            get
            {
                return CultureInformation.CurrentCultureInfo(CurrentSite);
            }
        }

        public ShoppingCart CurrentCart
        {
            get
            {
                if (Session["CurrentCart"] == null)
                {
                    ShoppingCart cart = new ShoppingCart(CurrentSite);
                    cart.DistributorId = CurrentAccount.AccountId;
                    Session["CurrentCart"] = cart;
                }

                return (ShoppingCart)Session["CurrentCart"];
            }
            set { Session["CurrentCart"] = value; }
        }

        #region --- Static Methods ---
        public static string GetWebPageSource(string url)
        {
            try
            {
                string content = "";
                if (!string.IsNullOrEmpty(url))
                {
                    System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);
                    System.Net.WebResponse webResponse = webRequest.GetResponse();
                    System.IO.StreamReader sr = new System.IO.StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                    content = sr.ReadToEnd();

                    #region File saving example
                    //save to file
                    //StreamWriter sw = new StreamWriter(Server.MapPath("webcontent.txt"));
                    //sw.Write(content);
                    //sw.Flush(); 
                    #endregion
                }

                return content;
            }
            catch { return string.Empty; }
        }
        #endregion
    }
}
