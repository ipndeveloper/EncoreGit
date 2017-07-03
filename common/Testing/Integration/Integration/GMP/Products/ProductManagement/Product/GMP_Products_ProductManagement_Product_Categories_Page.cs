using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_Categories_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
        private Div _container;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _container = Document.GetElement<Div>(new Param("categoryTreeContainer"));
            _container.CustomWaitForExist();
        }

         public override bool IsPageRendered()
        {
            return _container.Exists;
        }
    }
}
