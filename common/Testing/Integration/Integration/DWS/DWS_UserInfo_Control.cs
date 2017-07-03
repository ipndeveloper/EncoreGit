using WatiN.Core;
using NetSteps.Testing.Integration.DWS.MyAccount;

namespace NetSteps.Testing.Integration.DWS
{
    public class DWS_UserInfo_Control : Control<Div>
    {
        private Span spanLoggedInUser;
        private Link lnkLogOut;

        protected override void  InitializeContents()
        {
            base.InitializeContents();
            this.spanLoggedInUser = Element.GetElement<Span>(new Param("UserID", AttributeName.ID.ClassName));
            this.lnkLogOut = Element.GetElement<Link>(new Param("logOutLink", AttributeName.ID.ClassName)); 
        }

        public DWS_MyAccount_Page ClickMyAccount(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("myAcctManagement", AttributeName.ID.ClassName)).CustomClick(timeout, false);
            return Util.GetPage<DWS_MyAccount_Page>(timeout, pageRequired);

        }

        public DWS_Login_Page ClickLogout(int? timeout = null, bool pageRequired = true)
        {
            timeout = this.lnkLogOut.CustomClick(timeout, false);
            return Util.GetPage<DWS_Login_Page>(timeout, pageRequired);
        }



        public string User
        {
            get { return spanLoggedInUser.CustomGetText(); }
        }
    }
}
