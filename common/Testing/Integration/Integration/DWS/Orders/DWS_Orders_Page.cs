using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WatiN.Core;
using System.Threading;

namespace NetSteps.Testing.Integration.DWS.Orders
{
    /// <summary>
    /// Class related to controls and ops of DWS orders page.
    /// </summary>
    public class DWS_Orders_Page : DWS_Orders_Base_Page
    {
        protected Link _orderHistory, _myParties;
        //private UnorderedList _orderBox;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _orderHistory = Document.GetElement<ListItem>(new Param("li_History_TabContent")).GetElement<Link>();
            _myParties = Document.GetElement<ListItem>(new Param("li_Pending_TabContent")).GetElement<Link>();
        }

         public override bool IsPageRendered()
        {
             return _orderHistory.Exists;
        }

         public DWS_Orders_OrderHistory_Control ClickOrderHistory(int? timeout = null)
        {
            Util.Browser.CustomWaitForSpinners();
            _orderHistory.CustomClick(timeout);
            return _content.GetElement<Div>(new Param("History_TabContent")).As<DWS_Orders_OrderHistory_Control>();
        }

         public DWS_Orders_Parties_Control ClickMyParties(int? timeout = null)
        {
            this._myParties.CustomClick(timeout);
            return _content.GetElement<Div>(new Param("Parties_TabContent")).As<DWS_Orders_Parties_Control>();
        }

        [Obsolete("Use 'SectionNav.ClickStartPersonalOrder()'", true)]
        public DWS_Orders_OrderEntry_Page StartPersonalOrder(int? timeout = null, bool pageRequired = true)
        {
            //var order = _orderBox.GetElement<Link>(new Param("/Orders/OrderEntry", AttributeName.ID.Href, RegexOptions.None));
            //timeout = order.CustomClick();
            return Util.GetPage<DWS_Orders_OrderEntry_Page>(timeout, pageRequired);
        }
    }
}
