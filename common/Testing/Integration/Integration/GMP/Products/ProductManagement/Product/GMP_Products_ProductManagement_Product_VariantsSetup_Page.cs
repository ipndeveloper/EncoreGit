using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_VariantsSetup_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
        private RadioButton _master;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _master = Document.RadioButton("existingGroup");
        }

         public override bool IsPageRendered()
        {
            return _master.Exists;
        }

         public GMP_Products_ProductManagement_Product_VariantsManage_Page ClickManageVariantSKUs(int? timeout = null, bool pageRequired = true)
        {
            timeout = _secHeader.GetElement<Link>(new Param("/Products/Products/VariantSKUS", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_VariantsManage_Page>(timeout, pageRequired);
        }
    }
}
