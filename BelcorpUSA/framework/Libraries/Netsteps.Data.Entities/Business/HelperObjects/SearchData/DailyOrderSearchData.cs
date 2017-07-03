using NetSteps.Common.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class DailyOrderSearchData
    {
        [Sortable(false)]
        [TermName("OrderNumber")]
        public string OrderNumber { get; set; }

        [Sortable(false)]
        [TermName("RepNumber")]
        public string RepNumber { get; set; }

        [Sortable(false)]
        [TermName("RepName")]
        public string RepName { get; set; }

        [Sortable(false)]
        [TermName("CustomerNumber")]
        public string CustomerNumber { get; set; }

        [Sortable(false)]
        [TermName("CustomerName")]
        public string CustomerName { get; set; }

        [Sortable(false)]
        [TermName("CustomerType")]
        public string CustomerType { get; set; }

        [Sortable(false)]
        [TermName("Sponsor")]
        public string Sponsor { get; set; }

        [Sortable(false)]
        [TermName("OrderShipmentID")]
        public string OrderShipmentID { get; set; }

        [Sortable(false)]
        [TermName("ShipmentDate")]
        public string DateShippedUTC { get; set; }

        [Sortable(false)]
        [TermName("CompleteDate")]
        public string CompleteDateUTC { get; set; }

        [Sortable(false)]
        [TermName("DateCreated")]
        public string DateCreatedUTC { get; set; }

        [Sortable(false)]
        [TermName("OrderType")]
        public string OrderType { get; set; }

        [Sortable(false)]
        [TermName("OrderStatus")]
        public string OrderStatus { get; set; }

        [Sortable(false)]
        [TermName("QV")]
        public string QV { get; set; }

        [Sortable(false)]
        [TermName("CV")]
        public string CV { get; set; }

        [Sortable(false)]
        [TermName("Price")]
        public string Price { get; set; }

        [Sortable(false)]
        [TermName("State")]
        public string State { get; set; }

        [Sortable(false)]
        [TermName("SubTotal")]
        public string SubTotal { get; set; }

        [Sortable(false)]
        [TermName("Shipping")]
        public string ShippingTotal { get; set; }

        [Sortable(false)]
        [TermName("Handling")]
        public string HandlingTotal { get; set; }

        [Sortable(false)]
        [TermName("Tax")]
        public string TaxAmountTotal { get; set; }

        [Sortable(false)]
        [TermName("Total")]
        public string GrandTotal { get; set; }

        [Sortable(false)]
        [TermName("HasStarterKit")]
        public string HasStarterKit { get; set; }

        [Sortable(false)]
        [TermName("StarterKitPrice")]
        public string StarterKitPrice { get; set; }

        [Sortable(false)]
        [TermName("City")]
        public string City { get; set; }

        //[Sortable(false)]
        //[TermName("AccountNumber")]
        //public string  AccountNumber{ get; set; }

        //[Sortable(false)]
        //[TermName("Name")]
        //public string  Name{ get; set; }
        
        //[TermName("AccountType")]
        //public string AccountType { get; set; }

        //[TermName("Sponsor")]
        //public string Sponsor { get; set; }
    }
}
