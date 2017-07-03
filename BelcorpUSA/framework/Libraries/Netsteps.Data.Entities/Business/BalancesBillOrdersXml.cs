using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class BalancesBillOrdersXml
    {
        public string OrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceSerie { get; set; }
        public DateTime DateCreatedUTC { get; set; }
        public DateTime DateInvoice { get; set; }
        public string InvoiceStatus { get; set; }
        public string InvoiceType { get; set; }
        public int DistributionCenter { get; set; }
        public string SortLine { get; set; }
        public string ChaveNFe { get; set; }
        public int Boxes { get; set; }
        public int Weight { get; set; }
        public int Quantity { get; set; }
        public string Material { get; set; }
        public int QuantityOnHand { get; set; }
        public int ICMS { get; set; }
        public int ICMS_ST { get; set; }
        public int IPI { get; set; }
        public int PIS { get; set; }
        public int COFINS { get; set; }
        public string InvoicePath { get; set; }
        public decimal InvoiceUnitValue { get; set; }

    }
}
