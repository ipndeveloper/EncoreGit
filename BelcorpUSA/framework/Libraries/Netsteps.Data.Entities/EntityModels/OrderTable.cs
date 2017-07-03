namespace NetSteps.Data.Entities.EntityModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [Table("Orders")]
    public class OrderTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("OrderID"), Key]
        public int OrderID { get; set; }

        [Column("OrderNumber")]
        public string OrderNumber { get; set; }

        [Column("OrderStatusID")]
        public int OrderStatusID { get; set; }

        [Column("OrderTypeID")]
        public int OrderTypeID { get; set; }

        [Column("AccountID")]
        public int AccountID { get; set; }

        [Column("SiteID")]
        public int? SiteID { get; set; }

        [Column("ParentOrderID")]
        public int? ParentOrderID { get; set; }

        [Column("CurrencyID")]
        public int CurrencyID { get; set; }

        [Column("CompleteDateUTC")]
        public DateTime? CompleteDateUTC { get; set; }

        [Column("CommissionDateUTC")]
        public DateTime? CommissionDateUTC { get; set; }

        [Column("HostessRewardsEarned")]
        public decimal? HostessRewardsEarned { get; set; }

        [Column("HostessRewardsUsed")]
        public decimal? HostessRewardsUsed { get; set; }

        [Column("IsTaxExempt")]
        public bool? IsTaxExempt { get; set; }

        [Column("TaxAmountTotal")]
        public decimal? TaxAmountTotal { get; set; }

        [Column("TaxAmountTotalOverride")]
        public decimal TaxAmountTotalOverride { get; set; }

        [Column("TaxableTotal")]
        public decimal TaxableTotal { get; set; }

        [Column("TaxAmountOrderItems")]
        public decimal? TaxAmountOrderItems { get; set; }

        [Column("TaxAmountShipping")]
        public decimal? TaxAmountShipping { get; set; }

        [Column("TaxAmount")]
        public decimal? TaxAmount { get; set; }

        [Column("Subtotal")]
        public decimal? Subtotal { get; set; }

        [Column("DiscountTotal")]
        public decimal? DiscountTotal { get; set; }

        [Column("ShippingTotal")]
        public decimal? ShippingTotal { get; set; }

        [Column("ShippingTotalOverride")]
        public decimal? ShippingTotalOverride { get; set; }

        [Column("HandlingTotal")]
        public decimal? HandlingTotal { get; set; }

        [Column("GrandTotal")]
        public decimal? GrandTotal { get; set; }

        [Column("PaymentTotal")]
        public decimal? PaymentTotal { get; set; }

        [Column("Balance")]
        public decimal? Balance { get; set; }

        [Column("CommissionableTotal")]
        public decimal? CommissionableTotal { get; set; }

        [Column("ReturnTypeID")]
        public int? ReturnTypeID { get; set; }

        [Column("StepUrl")]
        public string StepUrl { get; set; }

        [Column("ModifiedByUserID")]
        public int? ModifiedByUserID { get; set; }

        [Column("DateCreatedUTC")]
        public DateTime DateCreatedUTC { get; set; }

        [Column("CreatedByUserID")]
        public int? CreatedByUserID { get; set; }
        
        [Column("DataVersion")]
        [Timestamp]
        public byte[] DataVersion { get; set; }

        [Column("DiscountPercent")]
        public decimal? DiscountPercent { get; set; }

        [Column("PartyShipmentTotal")]
        public decimal? PartyShipmentTotal { get; set; }

        [Column("PartyHandlingTotal")]
        public decimal? PartyHandlingTotal { get; set; }

        [Column("ETLNaturalKey")]
        public string ETLNaturalKey { get; set; }

        [Column("ETLHash")]
        public string ETLHash { get; set; }

        [Column("ETLPhase")]
        public string ETLPhase { get; set; }

        [Column("ETLDate")]
        public DateTime? ETLDate { get; set; }

        [Column("DateLastModifiedUTC")]
        public DateTime? DateLastModifiedUTC { get; set; }

        [Column("IDNationalMail")]
        public string IDNationalMail { get; set; }

        [Column("IDSupportTicket")]
        public int? IDSupportTicket { get; set; }

        [Column("CreatedPeriodID")]
        public int? CreatedPeriodID { get; set; }

        [Column("CompletedPeriodID")]
        public int? CompletedPeriodID { get; set; }
    }
}
