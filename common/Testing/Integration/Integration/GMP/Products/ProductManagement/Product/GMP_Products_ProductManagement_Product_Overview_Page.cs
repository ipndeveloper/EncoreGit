using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_Overview_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
        private Link _auditHistory;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _auditHistory = _secHeader.GetElement<Link>(new Param("auditHistory"));
        }

         public override bool IsPageRendered()
        {
            return _auditHistory.Exists;
        }

        public GMP_Products_ProductManagement_Product_History_Page ClickAuditHistory(int? timeout=null, int? delay=null, bool pageRequired = true)
        {
            timeout = _auditHistory.CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_History_Page>(timeout, pageRequired);
        }
    }
}
