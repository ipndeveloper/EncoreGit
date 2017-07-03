using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Orders
{
    public class GMP_Orders_SubNav_Control : Control<UnorderedList>
    {

        public GMP_Orders_Browse_Page ClickBrowseOrders(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Orders/Browse", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Orders_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Orders_ShipOrder_Page ClickShipOrders(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Orders/Shipping", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Orders_ShipOrder_Page>(timeout, pageRequired);
        }

        public GMP_Orders_AutoshipRunOverview_Page ClickAutoshipRunOverview(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Orders/Autoships", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Orders_AutoshipRunOverview_Page>(timeout, pageRequired);
        }

        public GMP_Orders_NewOrder_Page ClickStartANewOrder(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Orders/OrderEntry/NewOrder", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Orders_NewOrder_Page>(timeout, pageRequired);
        }
    }
}
