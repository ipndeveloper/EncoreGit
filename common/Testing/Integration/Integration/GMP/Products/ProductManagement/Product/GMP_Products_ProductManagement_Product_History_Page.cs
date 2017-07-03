using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_History_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
        private Link _overview;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _overview = _secHeader.GetElement<Link>((new Param("/Products/Products/Overview/", AttributeName.ID.Href, RegexOptions.None)));
        }

        public GMP_Products_ProductManagement_Product_Overview_Page ClickOverview(int? timeout = null, bool pageRequired = true)
        {
            timeout = _overview.CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_Overview_Page>(timeout, pageRequired);
        }

         public override bool IsPageRendered()
        {
            return _overview.Exists;
        }
    }
}
