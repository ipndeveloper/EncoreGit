using System.Text.RegularExpressions;
using WatiN.Core;
using NetSteps.Testing.Integration;
using NetSteps.Testing.Integration.PWS.Enroll;

namespace NetSteps.Testing.Integration.PWS.Account
{
    public class PWS_Account_Page : PWS_Base_Page
    {
        private Link _username;
        private Div _account;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _username = _content.GetElement<Link>(new Param("btnEditAccountSettings"));
            _account = _content.GetElement<Div>(new Param("generalAcctBox", AttributeName.ID.ClassName, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return _username.Exists;
        }

         public string AccountType
         {
             get { return _account.GetElement<ListItem>(new Param(0)).GetElement<Span>(new Param(1)).Text.Trim(); }
         }

         public PWS_Account_Settings_Control ClickChangeUserName()
         {
             _username.CustomClick();
             return _content.GetElement<Div>(new Param("editAccountSettings")).As<PWS_Account_Settings_Control>();
         }

         public PWS_Account_ChangePassword_Control ClickChangePassword()
         {
             _content.GetElement<Link>(new Param("btnChangePassword")).CustomClick();
             return _content.GetElement<Div>(new Param("changePasswordModal")).As<PWS_Account_ChangePassword_Control>();
         }

         public PWS_Account_ManageProfiles_Page ClickManageProfiles(int? timeout = null)
        {
            timeout = _content.GetElement<Link>(new Param("editProfilesLink", AttributeName.ID.ClassName, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<PWS_Account_ManageProfiles_Page>(timeout);
        }

         public PWS_Enroll_Page ClickUpgrade(int? timeout = null)
         {
             timeout = _content.GetElement<Link>(new Param("Enroll", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
             return Util.GetPage<PWS_Enroll_Page>();
         }

         public PWS_Account_Parties_Page ClickParties(int? timeout = null)
         {
             timeout = _content.GetElement<Link>(new Param("/HostedParties/", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
             return Util.GetPage<PWS_Account_Parties_Page>();
         }

         public PWS_Account_ProductCredit_Page ClickProductCredit(int? timeout = null)
         {
             timeout = _content.GetElement<Link>(new Param("/Account/ProductCredit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
             return Util.GetPage<PWS_Account_ProductCredit_Page>();
         }

    }
}
