using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Orders
{
    public class DWS_Orders_OrderDetails_Control : Control<TableRow>
    {
        private TableCell _sku, _name, _unitPrice, _qty, _totalPrice;
        private Span _originalUnitPrice, _discountUnitPrice, _originalTotalPrice, _discountTotalPrice;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _sku = Element.GetElement<TableCell>(new Param(0));
            _name = Element.GetElement<TableCell>(new Param(1));
            _unitPrice = Element.GetElement<TableCell>(new Param(2));
            _originalUnitPrice = _unitPrice.GetElement<Span>(new Param(0));
            _discountUnitPrice = _unitPrice.GetElement<Span>(new Param(1));
            _qty = Element.GetElement<TableCell>(new Param(3));
            _totalPrice = Element.GetElement<TableCell>(new Param(5));
            _originalTotalPrice = _totalPrice.GetElement<Span>(new Param(0));
            _discountTotalPrice = _totalPrice.GetElement<Span>(new Param(1));
        }

        public string SKU
        {
            get { return _sku.Text.Trim(); }
        }

        public string Name
        {
            get { return _name.Text.Trim(); }
        }

        public decimal UnitPrice
        {
            get
            {
                string price;
                if (_originalUnitPrice.Exists)
                    price = _originalUnitPrice.Text;
                else
                    price = _unitPrice.Text;
                return decimal.Parse(price.Substring(1));
            }
        }

        public decimal DiscountUnitPrice
        {
            get { return decimal.Parse(_discountUnitPrice.Text.Substring(1)); }
        }

        public ushort Quantity
        {
            get { return ushort.Parse(_qty.Text); }
        }

        public decimal TotalPrice
        {
            get
            {
                string price;
                if (_originalTotalPrice.Exists)
                    price = _originalTotalPrice.Text;
                else
                    price = _totalPrice.Text;
                return decimal.Parse(price.Substring(1));
            }
        }

        public decimal DiscountTotalPrice
        {
            get { return decimal.Parse(_discountTotalPrice.Text.Substring(1)); }
        }

        public OrderItem GetOrderItem()
        {
            return new OrderItem(new Product(SKU, Name, UnitPrice), Quantity);
        }

        public bool ValidateOrderItem(OrderItem item)
        {
            bool valid = Compare.CustomCompare<string>(item.Product.SKU, CompareID.Equal, SKU, "SKU");
            if (item.Product.Name != null && !Compare.CustomCompare<string>(Name, CompareID.Contains, item.Product.Name, "Name"))
                valid = false;
            if (!Compare.CustomCompare<ushort>(item.Quantity, CompareID.Equal, Quantity, "Quantity"))
                valid = false;
            if (item.Product.Price.HasValue)
            {
                if (!Compare.CustomCompare<decimal>((decimal)item.Product.Price, CompareID.Equal, UnitPrice, "Unit Price"))
                    valid = false;
                if (!Compare.CustomCompare<decimal>((decimal)item.Product.Price * item.Quantity, CompareID.Equal, TotalPrice, "Total Price"))
                    valid = false;
            }
            if (item.Product.DiscountPrice.HasValue)
            {
                if (!Compare.CustomCompare<decimal>((decimal)item.Product.DiscountPrice, CompareID.Equal, DiscountUnitPrice, "Discount Unit Price"))
                    valid = false;
                if (!Compare.CustomCompare<decimal>((decimal)item.Product.DiscountPrice * item.Quantity, CompareID.Equal, DiscountTotalPrice, "Discount Unit Price"))
                    valid = false;
            }
            return valid;
        }
    }
}
