using WatiN.Core;
using NetSteps.Testing.Integration.GMP.Products.ProductManagement;
using System.Text;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_ShoppingBag_Control : Control<Div>
    {
        private SearchSuggestionBox_Control _search;
        private TextField _quantity;
        private Link _add, _directShip;
        string _order;
        private Span _subTotal, _taxTotal, _shippingTotal, _grandTotal;
        private Div _bag;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _directShip = Element.GetElement<Link>(new Param("directShip", AttributeName.ID.ClassName));
            _bag = (Div)Element.NextSibling;
            _search = _bag.GetElement<TextField>(new Param("item")).As<SearchSuggestionBox_Control>();
            _quantity = _bag.GetElement<TextField>(new Param("quantity"));
            _add = _bag.GetElement<Link>(new Param("Add to Cart", AttributeName.ID.Title, RegexOptions.IgnoreCase));
            _order = _bag.GetElement<TextField>(new Param("orderCustomerId", AttributeName.ID.ClassName)).GetAttributeValue("value");
            _subTotal = _bag.GetElement<Span>(new Param("subtotal Total", AttributeName.ID.ClassName));
            _taxTotal = _bag.GetElement<Span>(new Param("taxTotal Total", AttributeName.ID.ClassName));
            _shippingTotal = _bag.GetElement<Span>(new Param("shippingAndHandlingTotal Total", AttributeName.ID.ClassName));
            _grandTotal = _bag.GetElement<Span>(new Param("grandTotal Total", AttributeName.ID.ClassName));
        }

        public void SelectProduct(OrderItem item, int? timeout = null)
        {
            _search.Select(item.Product.SKU.Substring(0, 3), item.Product.SKU, false, timeout, 0, string.Format("$('#orderCustomerBody{0} input').first().keyup()", _order));
            EnterQuantity(item.Quantity);
            ClickAdd();
        }

        public DWS_Orders_Party_ShoppingBag_Control EnterQuantity(int quantity)
        {            
            _quantity.CustomSetTextQuicklyHelper(quantity.ToString());
            return this;
        }

        public decimal SubTotal
        {
            get { return decimal.Parse(_subTotal.CustomGetText().Substring(1)); }
        }

        public decimal Tax
        {
            get { return decimal.Parse(_taxTotal.CustomGetText().Substring(1)); }
        }

        public decimal Shipping
        {
            get { return  decimal.Parse(_shippingTotal.CustomGetText().Substring(1)); }
        }

        public decimal Balance
        {
            get { return decimal.Parse(_grandTotal.CustomGetText().Substring(1)); }
        }

        public void ClickAdd(int? timeout = null)
        {
            timeout = _add.CustomClick(timeout);
            _bag.CustomWaitForSpinners(timeout);

        }

        public DWS_Orders_Party_AddDirectShip_Page ClickDirectShip(int? timeout = null, bool pageRequired = true)
        {
            timeout = _directShip.CustomClick(timeout);
            return Util.GetPage<DWS_Orders_Party_AddDirectShip_Page>(timeout, pageRequired);
        }
    }
}
