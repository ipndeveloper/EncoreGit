using System;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Orders
{
    public class DWS_Orders_CartTools_Control : Control<Div>
    {
        private SearchSuggestionBox_Control _search;
        private TextField _quantity;
        private Link _add;

        protected override void InitializeContents()
        {
            _search = Element.GetElement<TextField>(new Param("productLookUp", AttributeName.ID.ClassName  , RegexOptions.None)).As<SearchSuggestionBox_Control>();
            _quantity = Element.GetElement<TextField>(new Param("quantity", AttributeName.ID.ClassName, RegexOptions.None));
            _add = Element.GetElement<Link>(new Param("Add to Cart", AttributeName.ID.Title, RegexOptions.IgnoreCase));
        }

        public DWS_Orders_CartTools_Control SelectProduct(OrderItem orderItem)
        {
            _search.Select(orderItem.Product.SKU);
            if (orderItem.Quantity > 1)
                EnterQuantity(orderItem.Quantity);
            return this;
        }

        public DWS_Orders_CartTools_Control EnterQuantity(int quantity)
        {
            _quantity.CustomSetTextQuicklyHelper(quantity.ToString());
            return this;
        }

        public int? ClickAdd(int? timeout = null)
        {
            timeout = _add.CustomClick(timeout);
            return Element.CustomWaitForSpinners(timeout);

        }
    }
}
