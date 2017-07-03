using WatiN.Core;
using NetSteps.Testing.Integration.GMP.Sites;
using NetSteps.Testing.Integration.GMP.Accounts;
using NetSteps.Testing.Integration.GMP.Orders;
using NetSteps.Testing.Integration.GMP.Commissions;
using NetSteps.Testing.Integration.GMP.Communication;
using NetSteps.Testing.Integration.GMP.Support;
using NetSteps.Testing.Integration.GMP.Reports;
using NetSteps.Testing.Integration.GMP.Admin;
using NetSteps.Testing.Integration.GMP.Products.CatalogManagement;
using System.Text.RegularExpressions;

using WatiN.Core.Constraints;

namespace NetSteps.Testing.Integration.GMP
{
    public class GMP_GlobalNav_Control : Control<UnorderedList>
    {
        public GMP_Sites_Page ClickSitesTab(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Sites", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Sites_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_Page ClickAccountsTab(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Accounts", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_Page>(timeout, pageRequired);
        }

        public GMP_Orders_Page ClickOrdersTab(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Orders", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Orders_Page>(timeout, pageRequired);
        }

        public GMP_Products_CatalogManagement_Browse_Page ClickProductsTab(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_CatalogManagement_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Commissions_Admin_Page ClickCommissionsTab(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Commissions", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Commissions_Admin_Page>(timeout, pageRequired);
        }

        public GMP_Communication_Campaigns_Page ClickCommunicationTab(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Communication", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Communication_Campaigns_Page>(timeout, pageRequired);
        }

        public GMP_Support_BrowseTickets_Page ClickSupportTab(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Support", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Support_BrowseTickets_Page>(timeout, pageRequired);
        }

        public GMP_Reports_Page ClickReportsTab(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Reports", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Reports_Page>(timeout, pageRequired);
        }

        public GMP_Admin_Users_Page ClickAdminTab(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Admin", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_Users_Page>(timeout, pageRequired);
        }
    }
}
