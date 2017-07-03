using WatiN.Core;
using System;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS
{
    public class PWS_GlobalHeader_Control : Header_Control
    {
        private PWS_UserMenu_Control _userMenu;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _userMenu = Element.GetElement<Div>(new Param("userLinks", AttributeName.ID.ClassName, RegexOptions.None)).As<PWS_UserMenu_Control>();
        }

        public PWS_HeaderLogin_Control ClickLogin(int? timeout = 240)
        {
            Element.GetElement<Link>(new Param("btnShowLogin")).CustomClick(timeout);
            return Element.GetElement<Div>(new Param("loginContainer")).As<PWS_HeaderLogin_Control>();
        }

        public TPage ClickLogin<TPage>(int? timeout = null, bool pageRequired = true) where TPage : PWS_Base_Page, new()
        {
            timeout = Element.GetElement<Link>(new Param("/Login", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        [Obsolete("Use 'UserMenu.ClickLogout<TPage>()'", true)]
        public TPage ClickLogout<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = Element.GetElement<Link>(new Param("/Logout", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout, false);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public PWS_UserMenu_Control UserMenu
        {
            get { return _userMenu; }
        }

        public bool ValidateLogin(RetailCustomer customer, int? timeout = null)
        {
            Span user = Element.GetElement<Span>(new Param("loggedInName"));
            timeout = user.CustomWaitForVisibility(true, timeout);
            return user.CustomGetText().Contains(string.Format("{0} {1}", customer.FirstName, customer.LastName));
        }

        public bool ValidateLogOut()
        {
            return Element.GetElement<Span>(new Param("loggedInName")).CustomGetText(null, false) == null;
        }

        [Obsolete("Use 'ValidateLogin(RetailCustomer, int?)'")]
        public bool ValidateLogin(string customer, int? timeout = null)
        {
           Span user = Element.GetElement<Span>(new Param("loggedInName"));
            timeout = user.CustomWaitForVisibility();
            return user.CustomGetText().Contains(customer);
        }
    }
}
