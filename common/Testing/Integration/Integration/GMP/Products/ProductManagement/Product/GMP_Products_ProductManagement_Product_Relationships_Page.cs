using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_Relationships_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
        private TextField _search;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _search = Document.GetElement<TextField>(new Param("txtProductRelationshipSearch"));
        }

         public override bool IsPageRendered()
        {
            return _search.Exists;
        }
    }
}
