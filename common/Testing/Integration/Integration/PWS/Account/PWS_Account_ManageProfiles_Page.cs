using WatiN.Core;
using System.Collections.Generic;

namespace NetSteps.Testing.Integration.PWS.Account
{
    public class PWS_Account_ManageProfiles_Page : PWS_Base_Page
    {
        private ElementCollection<Div> _shippingProfiles;
        private Link _addProfile; 

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _addProfile = Document.GetElement<Link>(new Param("btnAddShippingProfile"));
            _shippingProfiles = Document.GetElement<Div>(new Param("FL splitCol shippingCol", AttributeName.ID.ClassName)).GetElements<Div>(new Param("FRow", AttributeName.ID.ClassName));
        }

        public override bool IsPageRendered()
        {
            return _addProfile.Exists;
        }

        public PWS_Account_Profile_Page ClickAddShippingProfile(int? timeout = null, bool pageRequired = true)
        {
            timeout = _addProfile.CustomClick(timeout);
            return Util.GetPage<PWS_Account_Profile_Page>(timeout, pageRequired);
        }

        public List<PWS_Account_Profile_Control> GetShippingProfiles()
        {
            return new List<PWS_Account_Profile_Control>(_shippingProfiles.As<PWS_Account_Profile_Control>());
        }

        public PWS_Account_Profile_Control GetShippingProfile(int index)
        {
            return _shippingProfiles[index].As<PWS_Account_Profile_Control>();
        }
    }
}
