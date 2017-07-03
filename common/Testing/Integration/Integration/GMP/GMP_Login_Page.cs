using WatiN.Core;
using NetSteps.Testing.Integration.GMP.Accounts;

namespace NetSteps.Testing.Integration.GMP
{
    public class GMP_Login_Page : NS_Page
    {
        private TextField txtUserName;
        private TextField txtPassword;
        //private TextField txtConPassword;
        private Link btnSubmit;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            this.txtUserName = Document.GetElement<TextField>(new Param("username"));
            this.txtPassword = Document.GetElement<TextField>(new Param("password"));
            this.btnSubmit = Document.GetElement<Link>(new Param("btnLogin"));
        }

        public void EnterUserName(string userName)
        {
            txtUserName.CustomSetTextQuicklyHelper(userName);
            txtUserName.CustomRunScript(Util.strKeyUp);
        }

        /// <summary>
        /// Set password.
        /// </summary>
        /// <param name="password">Password.</param>
        public void EnterPassword(string password)
        {
            txtPassword.Focus();
            txtPassword.CustomSetTextHelper(password, null, false);

            var confirm = Document.GetElement<TextField>(new Param("password", AttributeName.ID.Name).And(new Param(1)));
            if (confirm.Exists)
            {
                confirm.Focus();
                confirm.CustomSetTextHelper(password, null, false);
            }

            //txtPassword.CustomRunScript(Util.strKeyUp);
        }

        /// <summary>
        /// Click sign in button.
        /// </summary>
        public bool ClickSignInLink(int? timeout = null)
        {
            this.btnSubmit.CustomClick(timeout);
            btnSubmit.CustomWaitForSpinner();
            return !Document.GetElement<Div>(new Param("error_message", AttributeName.ID.ClassName)).Exists;
        }

        /// <summary>
        /// Login to GMP.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        public GMP_Accounts_Page LoginToGMP(string userName, string password, int? timeout = null)
        {
            this.EnterUserName(userName);
            this.EnterPassword(password);
            ClickSignInLink(timeout);
            return Page.CreatePage<GMP_Accounts_Page>(Util.Browser);
        }

        /// <summary>
        ///  Verify page loads properly.
        /// </summary>
         public override bool IsPageRendered()
        {
            return (txtUserName.Exists && txtPassword.Exists && btnSubmit.Exists);
        }
    }    
}
