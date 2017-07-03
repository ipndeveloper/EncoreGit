using WatiN.Core;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_Summary_Control : Control<Div>
    {
        private ElementCollection<TableRow> _guests;
        private TableCell _partyTotal, _partyShipping, _directShipping, _partyTax, _partySubTotal;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _guests = Element.GetElement<Div>(new Param("ParySumGuestWrap")).GetElement<TableBody>().GetElements<TableRow>();
            _partyTotal = Element.GetElement<TableCell>(new Param("partyGrandTotal"));
            _partyShipping = Element.GetElement<TableCell>(new Param("partyShippingAndHandling"));
            _directShipping = Element.GetElement<TableCell>(new Param("directShippingAndHandling"));
            _partyTax = Element.GetElement<TableCell>(new Param("partyTax"));
            _partySubTotal = Element.GetElement<TableCell>(new Param("partySubtotal"));
        }

        public decimal PartyTotal
        {
            get { return decimal.Parse(_partyTotal.CustomGetText().Substring(1)); }
        }

        public decimal PartySubTotal
        {
            get { return decimal.Parse(_partySubTotal.CustomGetText().Substring(1)); }
        }

        public decimal PartyShipping
        {
            get { return decimal.Parse(_partyShipping.CustomGetText().Substring(1)); }
        }

        public decimal DirectShipping
        {
            get { return decimal.Parse(_directShipping.CustomGetText().Substring(1)); }
        }

        public decimal Tax
        {
            get { return decimal.Parse(_partyTax.CustomGetText().Substring(1)); }
        }

        public DWS_Orders_Party_GuestSummary GetGuest(int index)
        {
            TableRow summary = _guests[index];
            return new DWS_Orders_Party_GuestSummary(summary.GetElement<Link>().CustomGetText(), decimal.Parse(summary.GetElement<Span>(new Param("Total", AttributeName.ID.ClassName, RegexOptions.None)).CustomGetText().Substring(1))); 
        }

        public List<DWS_Orders_Party_GuestSummary> GetGuests()
        {
            List<DWS_Orders_Party_GuestSummary> summaries = new List<DWS_Orders_Party_GuestSummary>();
            for (int index = 0; index < _guests.Count; index++)
            {
                summaries.Add(GetGuest(index));
            }
            return summaries;
        }
    }
}
