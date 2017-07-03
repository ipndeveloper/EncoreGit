using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class DailyPaymentSearchData
    {
        [TermName("OrderNumber")]
        public string OrderNumber { get; set; }

        [TermName("OrderShipmentID")]
        public string OrderShipmentID { get; set; }

        [TermName("CompleteDate")]
        public string CompleteDate { get; set; }

        [TermName("OrderType")]
        public string OrderType { get; set; }

        [TermName("OrderStatus")]
        public string OrderStatus { get; set; }

        [TermName("GrandTotal")]
        public string GrandTotal { get; set; }

        [TermName("PaymentTotal")]
        public string PaymentTotal { get; set; }

        [TermName("Balance")]
        public string Balance { get; set; }

        [TermName("DateCreated")]
        public string DateCreated { get; set; }

        [TermName("PaymentType")]
        public string PaymentType { get; set; }

        [TermName("Amount")]
        public string Amount { get; set; }
    }
}
