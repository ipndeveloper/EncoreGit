namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;
    using System;

    /// <summary>
    /// OrderCustomers Table on BelcorpCommissions
    /// </summary>
    [Table("OrderCustomers")]
    public class OrderCustomerTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("OrderCustomerID"), Key]
        public int OrderCustomerID { get; set; }

        [Column("OrderCustomerTypeID")]
        public int OrderCustomerTypeID { get; set; }

        [Column("OrderID")]
        public int OrderID { get; set; }

        [Column("AccountID")]
        public int AccountID { get; set; }

        [Column("ShippingTotal")]
        public decimal ShippingTotal { get; set; }

        [Column("HandlingTotal")]
        public decimal HandlingTotal { get; set; }

        [Column("DiscountAmount")]
        public decimal DiscountAmount { get; set; }

        [Column("Subtotal")]
        public decimal Subtotal { get; set; }

        [Column("PaymentTotal")]
        public decimal PaymentTotal { get; set; }

        [Column("CommissionableTotal")]
        public decimal CommissionableTotal { get; set; }

        [Column("Balance")]
        public decimal Balance { get; set; }

        [Column("Total")]
        public decimal Total { get; set; }

        [Column("FutureBookingDateUTC")]
        public DateTime FutureBookingDateUTC { get; set; }

        [Column("IsTaxExempt")]
        public bool IsTaxExempt { get; set; }

        [Column("TaxAmountTotal")]
        public decimal TaxAmountTotal { get; set; }

        [Column("TaxAmountCity")]
        public decimal TaxAmountCity { get; set; }

        [Column("TaxAmountState")]
        public decimal TaxAmountState { get; set; }

        [Column("TaxAmountCounty")]
        public decimal TaxAmountCounty { get; set; }

        [Column("TaxAmountDistrict")]
        public decimal TaxAmountDistrict { get; set; }

        [Column("TaxAmountOrderItems")]
        public decimal? TaxAmountOrderItems { get; set; }

        [Column("TaxAmountShipping")]
        public decimal? TaxAmountShipping { get; set; }

        [Column("TaxableTotal")]
        public decimal? TaxableTotal { get; set; }

        [Column("TaxAmount")]
        public decimal? TaxAmount { get; set; }

        [Column("DataVersion")]
        public DateTime DataVersion { get; set; }

        [Column("ModifiedByUserID")]
        public int? ModifiedByUserID { get; set; }

        [Column("TaxAmountCountry")]
        public decimal? TaxAmountCountry { get; set; }

        [Column("IsBookingCredit")]
        public bool IsBookingCredit { get; set; }

        [Column("TaxGeocode")]
        public string TaxGeocode { get; set; }

        [Column("SalesTaxTransactionNumber")]
        public string SalesTaxTransactionNumber { get; set; }

        [Column("UseTaxTransactionNumber")]
        public string UseTaxTransactionNumber { get; set; }

        [Column("ETLNaturalKey")]
        public string ETLNaturalKey { get; set; }

        [Column("ETLHash")]
        public string ETLHash { get; set; }

        [Column("ETLPhase")]
        public string ETLPhase { get; set; }

        [Column("ETLDate")]
        public DateTime? ETLDate { get; set; }

        [Column("DateCreatedUTC")]
        public DateTime DateCreatedUTC { get; set; }

        [Column("DateLastModifiedUTC")]
        public DateTime DateLastModifiedUTC { get; set; }

        [Column("WarehouseID")]
        public int WarehouseID { get; set; }
    }
}