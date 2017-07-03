using WatiN.Core;
using System.Text.RegularExpressions;
using NetSteps.Testing.Integration;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_VariantsManage_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
        private Link _setup;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _setup = Document.GetElement<Link>(new Param("/Products/Products/Variants", AttributeName.ID.Href, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return _setup.Exists;
        }

         public GMP_Products_ProductManagement_Product_VariantsSetup_Page ClickSetUpNewVariant(int? timeout = null, bool pageRequired = true)
        {
            timeout = _setup.CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_VariantsSetup_Page>(timeout, pageRequired);
        }
    }
}
