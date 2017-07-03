namespace OrderRules.Service.DTO
{
    using System;
    using System.Collections.Generic;
    using OrderRules.Core.Model;
    
    public partial class CustomerPriceTotalDTO
    {
        public CustomerPriceTotalDTO() { }
        public int CustomerPriceTotalID { get; set; }
        public int RuleValidationID { get; set; }
        public int ProductPriceTypeID { get; set; }
        public Nullable<decimal> MinimumAmount { get; set; }
        public Nullable<decimal> MaximumAmount { get; set; }
        public int CurrencyID { get; set; }
    }
}
