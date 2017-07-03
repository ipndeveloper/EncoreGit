using System;
using System.Globalization;
using System.Reflection;
using System.Web.UI;
using NetSteps.Objects.Business;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Base
{
    public class BasePage : System.Web.UI.Page, ICallbackEventHandler
    {
        protected NetSteps.Common.Constants.ViewingMode PageMode
        {
            get { return Utility.PageMode; }
        }

        public NetSteps.Objects.Business.Site CurrentSite
        {
            get { return Utility.CurrentSite; }
        }
        
        public virtual SitePage CurrentPage
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
            Utility.FillContent(control, section, false);
        }

        public virtual Account CurrentAccount
        {
            get
            {
                return Utility.CurrentAccount;
            }
            set { Utility.CurrentAccount = value; }
        }

        public CultureInfo CultureInfo
        {
            get
            {
                return ((BaseMasterPage)Master).CultureInfo;
            }
        }

        public ShoppingCart CurrentCart
        {
            get
            {
                return ((BaseMasterPage)Master).CurrentCart;
            }
        }

        public CultureInfo CurrentCulture
        {
            get
            {
                return ((BaseMasterPage)Master).CurrentCulture;
            }
        }

        #region ICallbackEventHandler Members

        private string _CallbackResult;

        protected virtual void CreateServerFunction()
        {
            ClientScriptManager cm = Page.ClientScript;
            string cbReference = cm.GetCallbackEventReference(this, "arg", "ReceiveServerAck", "context", "ReceiveServerError", false);
            string callbackScript = "function ServerFunction(functionName, context, args) { var arg = functionName; for (var i = 2; i < ServerFunction.arguments.length; i++) { arg =  arg + '|' + ServerFunction.arguments[i]; } " + cbReference + "; }";
            cm.RegisterClientScriptBlock(this.GetType(), "AjaxServerFunction", callbackScript, true);
        }

        public virtual string GetCallbackResult()
        {
            return _CallbackResult;
        }

        public string GetCurrentUrl(string urlPath)
        {
            int countLast = urlPath.LastIndexOf("/");
            string currentUrl = urlPath.Substring(countLast);
            if (currentUrl.Contains("?"))
            {
                int countChar = currentUrl.LastIndexOf("?");
                currentUrl = currentUrl.Remove(countChar);
            }

            return currentUrl;

        }


        public virtual void RaiseCallbackEvent(string eventArgument)
        {
            try
            {
                string[] args = eventArgument.Split('|');

                Type MethodDeclaringType = this.GetType();
                string MethodName = args[0];
                string[] paramArgs = null;

                // create paramArgs array if necessary
                if (args.Length > 1)
                {
                    paramArgs = new string[args.Length - 1];
                    Array.Copy(args, 1, paramArgs, 0, paramArgs.Length);
                }

                // check of fully qualified class is provided
                if (MethodName.Contains("."))
                {
                    MethodDeclaringType = MethodName.Substring(0, MethodName.LastIndexOf('.')).LoadType();
                    MethodName = MethodName.Substring(MethodName.LastIndexOf('.') + 1);
                }

                // find function with name at argsArray[0]
                MethodInfo mi = MethodDeclaringType.GetMethod(MethodName);
                if (mi == null)
                {
                    // alert function wasn't found (go back to args[0] so fully qualified name shows)
                    _CallbackResult = "Alert|Function " + args[0] + " was not found.";
                }
                else
                {
                    // call function
                    _CallbackResult = mi.Invoke(this, paramArgs).ToStringSafe();
                }

                //prepend functon name if necessary
                if (!_CallbackResult.Contains("|"))
                {
                    _CallbackResult = MethodName + "|" + _CallbackResult;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                _CallbackResult = "Alert|" + ex.Message;
            }
        }

        #endregion ICallbackEventHandler Members
    }
}
