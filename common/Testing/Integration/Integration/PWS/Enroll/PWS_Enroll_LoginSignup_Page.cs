using WatiN.Core;
using WatiN.Core.Extras;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_LoginSignup_Page : PWS_Base_Page
    {
        private TextField _userName, _password, _firstName, _lastName, _email, _newUserName, _newPassword, _comfirm;
        private Link _login, _enroll;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _userName = _content.GetElement<TextField>(new Param("txtExistingUsername"));
            _password = _content.GetElement<TextField>(new Param("txtExistingPassword"));
            _firstName = _content.GetElement<TextField>(new Param("txtFirstName"));
            _lastName = _content.GetElement<TextField>(new Param("txtLastName"));
            _email = _content.GetElement<TextField>(new Param("txtEmail"));
            _newUserName = _content.GetElement<TextField>(new Param("txtNewUsername"));
            _newPassword = _content.GetElement<TextField>(new Param("txtNewPassword"));
            _comfirm = _content.GetElement<TextField>(new Param("txtNewPasswordConfirm"));
            _password.CustomWaitForVisibility();
            _login = _content.GetElement<Link>(new Param("btnExistingLogin"));
            _enroll = _content.GetElement<Link>(new Param("btnSignUp"));
        }

         public override bool IsPageRendered()
        {
            return _login.Exists && _enroll.Exists;
        }

        public PWS_Enroll_LoginSignup_Page EnterExistingUser(RetailCustomer customer)
        {
            _userName.CustomSetTextQuicklyHelper(customer.UserName);
            _password.CustomSetTextQuicklyHelper(customer.Password);
            return this;
        }

        public TPage ClickLogin<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _login.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public PWS_Enroll_LoginSignup_Page EnterNewUser(RetailCustomer customer, bool username = true)
        {
            _firstName.CustomSetTextQuicklyHelper(customer.FirstName);
            _lastName.CustomSetTextQuicklyHelper(customer.LastName);
            _email.CustomSetTextQuicklyHelper(customer.Email);
            if (username)
                _newUserName.CustomSetTextQuicklyHelper(customer.UserName);
            _newPassword.CustomSetTextQuicklyHelper(customer.Password);
            _comfirm.CustomSetTextQuicklyHelper(customer.Password);
            return this;
        }

        public TPage ClickSignUp<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _enroll.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}