using System;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business
{
    public class OrderShippingSearchData
    {
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
        public short OrderTypeID { get; set; }
        public short OrderStatusID { get; set; }
        public int ConsultantID { get; set; }
        public string ConsultantAccountNumber { get; set; }
        public string ConsultantFirstName { get; set; }
        public string ConsultantLastName { get; set; }
        public DateTime? CompleteDateUTC { get; set; }
        public int CurrencyID { get; set; }
        public IList<OrderShipmentSearchData> OrderShipments { get; set; }
        
        public class OrderShipmentSearchData
        {
            public int OrderShipmentID { get; set; }
            public int OrderShipmentIndex { get; set; }
            public short OrderShipmentStatusID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public bool IsWillCall { get; set; }
            public IList<OrderShipmentPackageSearchData> OrderShipmentPackages { get; set; }
        }

        public class OrderShipmentPackageSearchData
        {
            public int OrderShipmentPackageID { get; set; }
            public int OrderShipmentPackageIndex { get; set; }
            public string TrackingNumber { get; set; }
            public DateTime DateShippedUTC { get; set; }
        }
    }

    //public class OrderShipmentSearchData
    //{
    //    // Order shipment properties
    //    public int OrderShipmentID { get; set; }
    //    public int OrderShipmentIndex { get; set; }
    //    public short OrderShipmentStatusID { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public bool IsWillCall { get; set; }
    //    public int OrderID { get; set; }

    //    // Parent order properties
    //    public string OrderNumber { get; set; }
    //    public short OrderTypeID { get; set; }
    //    public short OrderStatusID { get; set; }
    //    public int ConsultantID { get; set; }
    //    public string ConsultantAccountNumber { get; set; }
    //    public string ConsultantFirstName { get; set; }
    //    public string ConsultantLastName { get; set; }
    //    public DateTime? CompleteDateUTC { get; set; }
    //    public int CurrencyID { get; set; }
    //    public int OrderShipmentCount { get; set; }

    //    // Packages
    //    public IEnumerable<OrderShipmentPackageSearchData> OrderShipmentPackages { get; set; }
    //    public class OrderShipmentPackageSearchData
    //    {
    //        public int OrderShipmentPackageID { get; set; }
    //        public int OrderShipmentPackageIndex { get; set; }
    //        public string TrackingNumber { get; set; }
    //        public DateTime DateShippedUTC { get; set; }
    //    }

    //    //public IEnumerable<OrderCustomerSearchData> OrderCustomers { get; set; }

    //    //public class OrderCustomerSearchData
    //    //{
    //    //    public int OrderCustomerID { get; set; }
    //    //    public short OrderCustomerTypeID { get; set; }
    //    //    public string OrderCustomerType { get; set; }
    //    //    public int AccountID { get; set; }
    //    //    public string AccountNumber { get; set; }
    //    //    public string FirstName { get; set; }
    //    //    public string LastName { get; set; }
    //    //    public decimal Total { get; set; }
    //    //    public IEnumerable<OrderItemSearchData> OrderItems { get; set; }
    //    //}

    //    //public class OrderItemSearchData
    //    //{
    //    //    public int OrderItemID { get; set; }
    //    //    public short OrderItemTypeID { get; set; }
    //    //    public string OrderItemType { get; set; }
    //    //    public int? ParentOrderItemID { get; set; }
    //    //    public int? ProductID { get; set; }
    //    //    public string ProductName { get; set; }
    //    //    public string SKU { get; set; }
    //    //    public int Quantity { get; set; }
    //    //}

    //    //public class OrderShipmentPackageItemSearchData
    //    //{
    //    //    public int OrderShipmentPackageItemID { get; set; }
    //    //    public int OrderItemID { get; set; }
    //    //    public int Quantity { get; set; }
    //    //}
    //}
}
