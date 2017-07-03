using WatiN.Core;
using NetSteps.Testing.Integration.DWS.Orders;
using NetSteps.Testing.Integration.DWS.Orders.Party;

namespace NetSteps.Testing.Integration.DWS.Orders
{
    public class DWS_Orders_Order_Control : Control<TableRow>
    {
        private int _orderIndex = 0;

        public DWS_Orders_Order_Control Configure(int orderIndex = 0)
        {
            _orderIndex = orderIndex;
            return this;
        }

        public string Number
        {
            get { return Element.GetElement<TableCell>(new Param(_orderIndex)).CustomGetText(); }
        }

        public decimal Total
        {
            get { return decimal.Parse(Element.GetElement<TableCell>(new Param(_orderIndex + 4)).CustomGetText().Substring(1)); }
        }

        public DWS_Orders_OrderDetail_Page SelectOrder(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<TableCell>(new Param(_orderIndex)).GetElement<Link>().CustomClick(timeout);
            return Util.GetPage<DWS_Orders_OrderDetail_Page>(timeout, pageRequired);
        }

        public TPage SelectPartyOrder<TPage>(int? timeout = null, bool pageRequired = true) where TPage : DWS_Base_Page, new()
        {
            timeout = Element.GetElement<TableCell>(new Param(_orderIndex)).GetElement<Link>().CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
            
    }
}
