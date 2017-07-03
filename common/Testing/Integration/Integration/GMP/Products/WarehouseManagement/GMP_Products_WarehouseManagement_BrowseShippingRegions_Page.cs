using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.WarehouseManagement
{
    public class GMP_Products_WarehouseManagement_BrowseShippingRegions_Page : GMP_Products_WarehouseManagement_Base_Page
    {
        private Link _manageRegions;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _manageRegions = Document.GetElement<Link>(new Param("/Products/ShippingRegions/Manage", AttributeName.ID.Href, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return _manageRegions.Exists;
        }

         public GMP_Products_WarehouseManagement_ManageShippingRegions_Page ClickManageShippingRegions(int? timeout = null, bool pageRequired = true)
        {
            timeout = _manageRegions.CustomClick(timeout);
            return Util.GetPage<GMP_Products_WarehouseManagement_ManageShippingRegions_Page>(timeout, pageRequired);
        }
    }
}
