using WatiN.Core;

namespace NetSteps.Testing.Integration
{
    public enum OrderTotal
    {
        Subtotal,
        Shipping,
        Handling,
        Tax
    }

    public class OrderTotal_Control : Control<TableRow>
    {
        private TableCell _totals;
        int _subtotal, _tax, _shipping, _handling;

        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
            _totals = Element.GetElement<TableCell>(new Param (1));
        }

        public OrderTotal_Control Configure(int subtotal = 0, int tax = 1, int shipping = 2, int handling = 3)
        {
            _subtotal = subtotal;
            _tax = tax;
            _shipping = shipping;
            _handling = handling;
            return this;
        }

        public decimal GetTotalValue(OrderTotal orderTotal)
        {
            int index;

            switch (orderTotal)
            {
                case OrderTotal.Subtotal:
                    index = _subtotal;
                    break;
                case OrderTotal.Shipping:
                    index = _shipping;
                    break;
                case OrderTotal.Handling:
                    index = _handling;
                    break;
                default: //OrderTotal.Tax:
                    index = _tax;
                    break;
            }

            return decimal.Parse(_totals.GetElement<Span>(new Param(index)).Text.Substring(1));
        }

        public bool ValidateTotals(ShoppingBag shoppingBag)
        {
            return ValidateTotals(shoppingBag.Balance, shoppingBag.Shipping, shoppingBag.Handling, shoppingBag.Tax);
        }

        public bool ValidateTotals(decimal subtotal, decimal shipping, decimal handling, decimal tax)
        {
            bool valid = Compare.CustomCompare<decimal>(subtotal, CompareID.Equal, GetTotalValue(OrderTotal.Subtotal), "Subtotal");
            if (!Compare.CustomCompare<decimal>(tax, CompareID.Equal, GetTotalValue(OrderTotal.Tax), "Tax"))
                valid = false;
            if (!Compare.CustomCompare<decimal>(shipping, CompareID.Equal, GetTotalValue(OrderTotal.Shipping), "Shipping"))
                valid = false;
            if (!Compare.CustomCompare<decimal>(handling, CompareID.Equal, GetTotalValue(OrderTotal.Handling), "Handling"))
                valid = false;
            return valid;
        }
    }
}
