namespace OrderRules.Service.DTO
{
    using System;
    using System.Collections.Generic;
    using OrderRules.Core.Model;
    
    public partial class CustomerPriceSubTotalDTO
    {
        public CustomerPriceSubTotalDTO() { }
        public int CustomerPriceSubTotalID { get; set; }
        public int RuleValidationID { get; set; }
        public Nullable<decimal> MinimumAmount { get; set; }
        public Nullable<decimal> MaximumAmount { get; set; }
        public int CurrencyID { get; set; }
    }
}
