using System;
using System.Globalization;
using System.Reflection;
using System.Web.UI;
using NetSteps.Objects.Business;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Base
{
    public class BaseUserControl : System.Web.UI.UserControl, ICallbackEventHandler
    {

        protected NetSteps.Common.Constants.ViewingMode PageMode
        {
            get
            {
                return Utility.PageMode;
            }
        }

        public NetSteps.Objects.Business.Site CurrentSite
        {
            get
            {
                return Utility.CurrentSite;
            }
        }

        public SitePage CurrentPage
        {
            get
            {
                return Utility.CurrentPage;
            }
        }

        public void FillContent(ContentControl control, HtmlSection section)
        {
            Utility.FillContent(control, section, false);
        }

        public Account CurrentAccount
        {
            get
            {
                return Utility.CurrentAccount;
            }
        }

        public ShoppingCart CurrentCart
        {
            get
            {
                return ((BasePage)Page).CurrentCart;
            }
        }

        public CultureInfo CurrentCulture
        {
            get
            {
                return ((BasePage)Page).CurrentCulture;
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
