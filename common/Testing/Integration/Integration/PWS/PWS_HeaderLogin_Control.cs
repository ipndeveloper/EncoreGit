using WatiN.Core;
using System.Threading;
using NetSteps.Testing.Integration;
using NetSteps.Testing.Integration.PWS.Enroll;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS
{
    public class PWS_HeaderLogin_Control : Control<Div>
    {
        private TextField _txtUsername, _txtPassword;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _txtUsername = Element.GetElement<TextField>(new Param("txtUsername"));
            _txtPassword = Element.GetElement<TextField>(new Param("txtPassword"));
        }

        public PWS_HeaderLogin_Control EnterExistingUser(RetailCustomer customer)
        {
            return EnterExistingUser(customer.UserName, customer.Password);
        }

        public PWS_HeaderLogin_Control EnterExistingUser(string userName, string password)
        {
            this._txtUsername.CustomSetTextQuicklyHelper(userName);
            this._txtPassword.CustomSetTextQuicklyHelper(password);
            return this;

        }

        public TPage ClickLogin<TPage>(int? timeout = null, int? delay = 3, bool pageRequired = true) where TPage : NS_Page, new()
        {
            Element.GetElement<Link>(new Param("Login", AttributeName.ID.Id, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public PWS_Enroll_LoginSignup_Page ClickSignUp(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Login", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<PWS_Enroll_LoginSignup_Page>(timeout, pageRequired);
        }
    }
}