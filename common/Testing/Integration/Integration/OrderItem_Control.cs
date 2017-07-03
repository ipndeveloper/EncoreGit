using System;
using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration
{
    public class OrderItem_Control : Control<TableRow>
    {
        private TableCell _sku, _name, _unitPrice, _totalPrice;
        private TextField _qty;
        private Link _remove;
        private Span _originalUnitPrice, _discountUnitPrice, _originalTotalPrice, _discountTotalPrice;

        public OrderItem_Control Configure(int skuIndex = 1)
        {
            _sku = Element.GetElement<TableCell>(new Param(skuIndex));
            _name = Element.GetElement<TableCell>(new Param(skuIndex + 1));
            _unitPrice = Element.GetElement<TableCell>(new Param(skuIndex + 2));
            _originalUnitPrice = _unitPrice.GetElement<Span>(new Param(0));
            _discountUnitPrice = _unitPrice.GetElement<Span>(new Param(1));
            if (skuIndex == 0)
            {
                _qty = Element.GetElement<TableCell>(new Param(skuIndex + 3)).GetElement<TextField>(new Param("value: Quantity", "data-bind").Or(new Param("quantity", AttributeName.ID.ClassName)));
            }
            else
            {
                _remove = Element.GetElement<TableCell>(new Param(0)).GetElement<Link>();
                _qty = Element.GetElement<TableCell>(new Param(skuIndex + 4)).GetElement<TextField>();
                _totalPrice = Element.GetElement<TableCell>(new Param(skuIndex + 5));
                _originalTotalPrice = _totalPrice.GetElement<Span>(new Param(0));
                _discountTotalPrice = _totalPrice.GetElement<Span>(new Param(1));
            }
            return this;
        }

        protected override void InitializeContents()
        {
            base.InitializeContents();
        }

        public string Sku
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
            set
            {
                _qty.CustomSetTextHelper(value.ToString());
                _qty.CustomRunScript("change");
            }
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
            return new OrderItem(new Product(Sku, Name, UnitPrice), Quantity);
        }

        public bool ValidateOrderItem(OrderItem item)
        {
            bool valid = Compare.CustomCompare<string>(item.Product.SKU, CompareID.Equal, Sku, "SKU");
            if (item.Product.Name != null && !Compare.CustomCompare<string>(item.Product.Name, CompareID.Equal, Name, "Name"))
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
        public void RemoveProduct()
        {
            Element.GetElement<Link>(new Param("Remove", AttributeName.ID.Title, RegexOptions.None)).CustomClick();
            Element.CustomWaitForSpinner();
        }
    }
}
