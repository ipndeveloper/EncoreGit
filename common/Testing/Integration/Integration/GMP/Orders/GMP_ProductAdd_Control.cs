using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Orders
{
    public class ProductAdd_Control : Control<Para>
    {
        private SearchSuggestionBox_Control _search;
        private TextField txtproductQuantity;
        private Link lnkAddToOrder;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _search = Element.GetElement<TextField>(new Param("txtQuickAddSearch")).As<SearchSuggestionBox_Control>();
            this.txtproductQuantity = Element.GetElement<TextField>(new Param("txtQuickAddQuantity"));
            this.lnkAddToOrder = Element.GetElement<Link>(new Param("btnQuickAdd"));
        }

        public void SearchAndSelectProduct(OrderItem item, int? timeout = null)
        {
            _search.Select(item.Product.SKU);
            if (item.Quantity > 1)
                txtproductQuantity.CustomSetTextQuicklyHelper(item.Quantity.ToString());
            this.lnkAddToOrder.CustomClick(timeout);
            Element.DomContainer.CustomWaitForSpinners();
        }
    }
}
