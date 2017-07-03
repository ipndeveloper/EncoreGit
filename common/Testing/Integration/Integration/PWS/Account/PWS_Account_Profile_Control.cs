using WatiN.Core;

namespace NetSteps.Testing.Integration.PWS.Account
{
    public class PWS_Account_Profile_Control : Control<Div>
    {
        private Div _address;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _address = Element.GetElement<Div>(new Param("FLabel", AttributeName.ID.ClassName));
        }

        public string Address
        {
            get { return _address.CustomGetText(); }
        }

        public PWS_Account_Profile_Page ClickEdit(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>().CustomClick(timeout);
            return Util.GetPage<PWS_Account_Profile_Page>(timeout, pageRequired);
        }
    }
}
