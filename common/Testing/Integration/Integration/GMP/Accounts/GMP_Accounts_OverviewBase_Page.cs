using WatiN.Core;
using System.Text.RegularExpressions;
using NetSteps.Testing.Integration.GMP.Orders;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public abstract class GMP_Accounts_OverviewBase_Page : GMP_Accounts_Section_Page
    {
        public GMP_Accounts_Overview_Page ClickOverview(int? timeout = null, bool pageRequired = true)
        {
            timeout = _secHeader.GetElement<Link>(new Param("/Accounts/Overview", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_Overview_Page>(timeout, pageRequired);
        }

        public GMP_Orders_Entry_Page ClickPlaceANewOrder(int? timeout = null, bool pageRequired = true)
        {
            timeout = _secHeader.GetElement<Link>(new Param("/Orders/OrderEntry", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Orders_Entry_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_PoliciesChangeHistory_Page ClickPoliciesChangeHistory(int? timeout = null, bool pageRequired = true)
        {
            timeout = _secHeader.GetElement<Link>(new Param("/Accounts/Overview/PoliciesChangeHistory", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_PoliciesChangeHistory_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_AuditHistory_Page ClickAuditHistory(int? timeout = null, bool pageRequired = true)
        {
            timeout = _secHeader.GetElement<Link>(new Param("/Accounts/Overview/AuditHistory", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_AuditHistory_Page>(timeout, pageRequired);
        }
    }
}
