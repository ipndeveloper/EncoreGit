namespace NetSteps.Data.Entities.Dto
{
    using System;

    /// <summary>
    /// Descripcion de la clase
    /// </summary>
    public partial class OrderDto
    {
        /// <summary>
        /// Obtiene o establece OrderID
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public string OrderNumber { get; set; }
        
        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int OrderStatusID { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int OrderTypeID { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int? SiteID { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int? ParentOrderID { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int CurrencyID { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public DateTime? CompleteDateUTC { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public DateTime? CommissionDateUTC { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? HostessRewardsEarned { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? HostessRewardsUsed { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public bool? IsTaxExempt { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? TaxAmountTotal { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal TaxAmountTotalOverride { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal TaxableTotal { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? TaxAmountOrderItems { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? TaxAmountShipping { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? TaxAmount { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? Subtotal { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? DiscountTotal { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? ShippingTotal { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? ShippingTotalOverride { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? HandlingTotal { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? GrandTotal { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? PaymentTotal { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? Balance { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? CommissionableTotal { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int? ReturnTypeID { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public string StepUrl { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int? ModifiedByUserID { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public DateTime DateCreatedUTC { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int? CreatedByUserID { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public byte[] DataVersion { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? DiscountPercent { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? PartyShipmentTotal { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public decimal? PartyHandlingTotal { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public string ETLNaturalKey { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public string ETLHash { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public string ETLPhase { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public DateTime? ETLDate { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public DateTime? DateLastModifiedUTC { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public string IDNationalMail { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int? IDSupportTicket { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int? CreatedPeriodID { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public int? CompletedPeriodID { get; set; }

        #region External Properties
        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public string CustomerFirstName { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        public string CustomerLastName { get; set; }

        /// <summary>
        /// Obtiene o establece Total
        /// </summary>
        public decimal RetailTotal { get; set; }

        /// <summary>
        /// Obtiene o establece ordercustomerAccount
        /// </summary>
        public int OrderCustomer { get; set; }
        #endregion
    }
}