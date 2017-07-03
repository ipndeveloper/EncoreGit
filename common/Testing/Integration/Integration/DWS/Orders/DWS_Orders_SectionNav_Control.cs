using WatiN.Core;
using NetSteps.Testing.Integration.DWS.Orders.Party;

namespace NetSteps.Testing.Integration.DWS.Orders
{
    public class DWS_Orders_SectionNav_Control : Control<UnorderedList>
    {
        /// <summary>
        /// Click on Start a personal order link.
        /// </summary>
        /// <returns>Order Entry page.</returns>
        public DWS_Orders_OrderEntry_Page ClickStartPersonalOrder(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param(0)).CustomClick(timeout);
            return Util.GetPage<DWS_Orders_OrderEntry_Page>(timeout, pageRequired);
        }

        /// <summary>
        /// Click on Schedule a party link.
        /// </summary>
        /// <returns>Order Entry page.</returns>
        public DWS_Orders_Party_SetUp_Page ClickScheduleParty(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param(1)).CustomClick(timeout);
            return Util.GetPage<DWS_Orders_Party_SetUp_Page>(timeout, pageRequired);
        }

        /// <summary>
        /// Click Manage My Parties link.
        /// </summary>
        /// <returns>Orders page.</returns>
        public DWS_Orders_Page ClickManageMyParties(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param(2)).CustomClick(timeout);
            return Util.GetPage<DWS_Orders_Page>(timeout, pageRequired);
        }
    }
}