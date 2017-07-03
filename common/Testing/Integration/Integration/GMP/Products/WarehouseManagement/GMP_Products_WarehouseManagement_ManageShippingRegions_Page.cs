using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.WarehouseManagement
{
    public class GMP_Products_WarehouseManagement_ManageShippingRegions_Page : GMP_Products_WarehouseManagement_Base_Page
    {
        private Link _browse;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _browse = Document.GetElement<Link>(new Param("/Products/ShippingRegions", AttributeName.ID.Href, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return Document.GetElement<SelectList>(new Param("sRegion")).Exists;
        }
    }
}
