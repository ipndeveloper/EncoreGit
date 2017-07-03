using WatiN.Core;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_Cart_Page : DWS_Base_Page
    {
        private ElementCollection<Div> _shoppingBags;
        private Link _addGuest;
        private DWS_Orders_Party_Summary_Control _summary;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            Document.GetElement<Div>(new Param("UI-bg UI-header brdrYYNN pad5 GuestTag customerCartHeader", AttributeName.ID.ClassName)).CustomWaitForExist();
            Thread.Sleep(1000);
            _shoppingBags = Document.GetElements<Div>(new Param("UI-bg UI-header brdrYYNN pad5 GuestTag customerCartHeader", AttributeName.ID.ClassName));
            _addGuest = Document.GetElement<Link>(new Param("/Orders/Party/AddGuests", AttributeName.ID.Href, RegexOptions.IgnoreCase));
            _summary = Document.GetElement<Div>(new Param("SideModule")).As<DWS_Orders_Party_Summary_Control>();
        }

         public override bool IsPageRendered()
        {
            Document.CustomWaitForSpinners();
            return _shoppingBags.Count > 0;
        }

        public List<DWS_Orders_Party_ShoppingBag_Control> GetShoppingBags()
         {
            List<DWS_Orders_Party_ShoppingBag_Control> shoppingBags = new List<DWS_Orders_Party_ShoppingBag_Control>(_shoppingBags.As<DWS_Orders_Party_ShoppingBag_Control>());
            return shoppingBags;
        }

        public DWS_Orders_Party_ShoppingBag_Control GetShoppingBag(int index)
        {
            return _shoppingBags[index].As<DWS_Orders_Party_ShoppingBag_Control>();
        }

        public DWS_Orders_Party_Summary_Control Summary
        {
            get { return _summary; }
        }

        [System.Obsolete("Use ClickContinue<DWS_Orders_Party_HostRewards_Page>()", true)]
        public DWS_Orders_Party_HostRewards_Page ClickContinue(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>("btnContinue").CustomClick(timeout);
            return Util.GetPage<DWS_Orders_Party_HostRewards_Page>(timeout, pageRequired);
        }

        public TPage ClickContinue<TPage>(int? timeout = null, bool pageRequired = true) where TPage: DWS_Base_Page, new()
        {
            timeout = Document.GetElement<Link>(new Param("btnContinue")).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public DWS_Orders_Party_AddGuest_Page ClickAddGuest(int? timeout = null, bool pageRequired = true)
        {
            _addGuest.CustomClick(timeout);
            return Util.GetPage<DWS_Orders_Party_AddGuest_Page>(timeout, pageRequired);
        }
    }
}
