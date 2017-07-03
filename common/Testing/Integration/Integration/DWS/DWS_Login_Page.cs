using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using NetSteps.Testing.Integration.DWS;

namespace NetSteps.Testing.Integration.DWS
{
    /// <summary>
    /// Controls and Operations on DWS Login page.
    /// </summary>
    public class DWS_Login_Page : NS_Page
    {
        #region Controls.

        private TextField _txtUserName;
        private TextField _txtPassword;
        private TextField _txtConPassword;
        private Link _lnkLogin;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            this._txtUserName = Document.GetElement<TextField>(new Param("username"));
            this._txtPassword = Document.GetElement<TextField>(new Param("password"));
            this._txtConPassword = Document.GetElement<TextField>(new Param("password", AttributeName.ID.Value));
            this._lnkLogin = Document.GetElement<Link>(new Param("btnLogin"));
        }

        #endregion

        #region Methods

        public DWS_Home_Page Login(Distributor distributor, int? timeout = null, bool pageRequired = true)
        {
            return Login(distributor.UserName, distributor.Password, timeout, pageRequired);
        }

        public DWS_Home_Page Login(string userName, string password, int? timeout = null, bool pageRequired = true)
        {
            if (!timeout.HasValue)
                timeout = Settings.WaitForCompleteTimeOut;
            timeout = this._txtUserName.CustomSetTextQuicklyHelper(userName, timeout);
            timeout = this._txtPassword.CustomSetTextQuicklyHelper(password, timeout, false);
            this._txtConPassword.CustomSetTextQuicklyHelper(password, timeout, true);
            timeout = this._lnkLogin.CustomClick(timeout);
            return Util.GetPage<DWS_Home_Page>(timeout, pageRequired);
        }

        public override bool IsPageRendered()
        {
            return this._txtUserName.Exists;
        }

        #endregion
    }
}
