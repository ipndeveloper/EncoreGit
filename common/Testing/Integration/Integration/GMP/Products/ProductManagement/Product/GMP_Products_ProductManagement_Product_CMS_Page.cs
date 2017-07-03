using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_CMS_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
        private Link _save;

        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
             _save = Document.GetElement<Link>(new Param("btnSaveDescriptions"));
        }

         public override bool IsPageRendered()
        {
            return _save.Exists;
        }
    }
}
