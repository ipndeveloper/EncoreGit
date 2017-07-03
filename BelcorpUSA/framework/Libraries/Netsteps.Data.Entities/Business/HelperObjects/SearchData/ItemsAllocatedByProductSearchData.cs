using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class ItemsAllocatedByProductSearchData
    {
        [TermName("CUV")]
        public string CUV { get; set; }

        [TermName("SAPcode")]
        public string SAPcode { get; set; }

        [TermName("ProductDescription")]
        public string ProductDescription { get; set; }

        [TermName("OrderNumber")]
        public string OrderNumber { get; set; }

        [TermName("AccountNumber")]
        public string AccountNumber { get; set; }

        [TermName("AccountType")]
        public string AccountType { get; set; }

        [TermName("OrderType")]
        public string OrderType { get; set; }

        [TermName("OrderStatus")]
        public string OrderStatus { get; set; }

        [TermName("OrderDate")]
        public string OrderDate { get; set; }

        [TermName("ParentCode")]
        public string ParentCode { get; set; }

        [TermName("BPCSCode")]
        public string BPCSCode { get; set; }

        [TermName("Quantity")]
        public string Quantity { get; set; }

        [TermName("OrderShipment")]
        public string OrderShipment { get; set; }

        [TermName("Price")]
        public string Price { get; set; }

        [TermName("CV")]
        public string CV { get; set; }

        [TermName("QV")]
        public string QV { get; set; }

        [TermName("NetSale")]
        public string NetSale { get; set; }

        [TermName("Subtotal")]
        public string Subtotal { get; set; }

        [TermName("TaxAmount")]
        public string TaxAmount { get; set; }

        [TermName("GrandTotal")]
        public string GrandTotal { get; set; }

    }
}
