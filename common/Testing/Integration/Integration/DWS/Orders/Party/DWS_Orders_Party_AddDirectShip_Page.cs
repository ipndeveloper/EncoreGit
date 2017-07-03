using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_AddDirectShip_Page : DWS_Base_Page
    {
        private Address_Control _address;
        private Link _save;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _save = Document.GetElement<Link>(new Param("btnSave"));
            _address = Document.GetElement<Div>(new Param("FormContainer", AttributeName.ID.ClassName)).As<Address_Control>();
        }

        public override bool IsPageRendered()
        {
            return _save.Exists;
        }

        public Address_Control Address
        {
            get { return _address; }
        }

        public DWS_Orders_Party_Cart_Page ClickSave(int? timeout = null, bool pageRequired = true)
        {
            timeout = _save.CustomClick(timeout);
            return Util.GetPage<DWS_Orders_Party_Cart_Page>(timeout, pageRequired);
        }
    }
}
