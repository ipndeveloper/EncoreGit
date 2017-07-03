using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_Inventory_Page : GMP_Products_ProductManagement_Product_Base_Page
    { 
        private TextField _quantity;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _quantity = _content.GetElement<TextField>(new Param("QuantityOnHand numeric", AttributeName.ID.ClassName));
        }

         public override bool IsPageRendered()
        {
            return _quantity.Exists;
        }
    }
}
