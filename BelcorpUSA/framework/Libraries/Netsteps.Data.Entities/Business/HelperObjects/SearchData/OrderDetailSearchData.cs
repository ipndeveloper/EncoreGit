using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class OrderDetailSearchData
    {
        [TermName("OrderNumber")]
        public string OrderNumber { get; set; }

        [TermName("AccountNumber")]
        public string AccountNumber { get; set; }

        [TermName("AccountType")]
        public String AccountType { get; set; }

        [TermName("OrderType")]
        public String OrderType { get; set; }

        [TermName("OrderStatus")]
        public String OrderStatus { get; set; }

        [TermName("BAState")]
        public String BAState { get; set; }

        [TermName("ShipmentState")]
        public String ShipmentState { get; set; }

        [TermName("OrderDate")]
        public String OrderDate { get; set; }

        [TermName("ParentCode")]
        public String ParentCode { get; set; }

        [TermName("CUV")]
        public String CUV { get; set; }

        [TermName("SAP")]
        public String SAP { get; set; }
        
        [TermName("BPCS")]
        public String BPCS { get; set; }

        [TermName("ProductDescription")]
        public String ProductDescription { get; set; }

        [TermName("Quantity")]
        public String Quantity { get; set; }

        [TermName("Price")]
        public decimal Price { get; set; }

        [TermName("CV")]
        public String CV { get; set; }

        [TermName("QV")]
        public String QV { get; set; }

        [TermName("Net")]
        public String Net { get; set; }

        [TermName("Subtotal")]
        public String Subtotal { get; set; }
        
        [TermName("TaxAmount")]
        public String TaxAmount { get; set; }

        [TermName("GrandTotal")]
        public String GrandTotal { get; set; }

    }
}
