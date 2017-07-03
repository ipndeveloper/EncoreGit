using WatiN.Core;

namespace NetSteps.Testing.Integration.PWS.Account
{
    public class PWS_Account_Profile_Page : PWS_Base_Page
    {
        private Address_Control _address;

        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
             _address = _content.GetElement<Div>(new Param("FormContainer", AttributeName.ID.ClassName)).As<Address_Control>();
        }

        public Address_Control Address
        {
            get { return _address; }
        }

        public override bool IsPageRendered()
        {
            return _address.Element.Exists;
        }

        public PWS_Account_ManageProfiles_Page ClickSaveProfile(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("btnSaveAddress")).CustomClick(timeout);
            return Util.GetPage<PWS_Account_ManageProfiles_Page>(timeout, pageRequired);
        }

        public PWS_Account_ManageProfiles_Page ClickCancel(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("jqmClose", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<PWS_Account_ManageProfiles_Page>(timeout, pageRequired);
        }

        public PWS_Account_ManageProfiles_Page ClickDelete(int? timeout = null, bool pageRequired = true)
        {
            Util.HandleConfirmDialog(Document.GetElement<Link>(new Param("btnDelete")));
            timeout = Util.Browser.CustomWaitForComplete(timeout);
            return Util.GetPage<PWS_Account_ManageProfiles_Page>(timeout, pageRequired);
        }
    }
}
