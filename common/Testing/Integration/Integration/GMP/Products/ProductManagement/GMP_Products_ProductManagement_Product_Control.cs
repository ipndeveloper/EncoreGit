using WatiN.Core;
using NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement
{
	public class GMP_Products_ProductManagement_Product_Control : Control<TableRow>
	{
        private Link lnkSKU;
        private string _sku, _productName;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            lnkSKU = Element.GetElement<TableCell>(new Param(1)).GetElement<Link>();
            _sku = lnkSKU.CustomGetText();
            _productName = Element.GetElement<TableCell>(new Param(2)).CustomGetText();

        }

        public string SKU
        {
            get { return _sku; }
            set { _sku = value; }
        }

        public string ProductName
        {
            get { return _productName; }
        }

        public GMP_Products_ProductManagement_Product_Overview_Page SelectProduct(int? timeout = null, bool pageRequired = true)
        {
            timeout = lnkSKU.CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_Overview_Page>(timeout, pageRequired);
        }


	}
}
