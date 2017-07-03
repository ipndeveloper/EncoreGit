namespace OrderRules.Service.DTO
{
    using System;
    using System.Collections.Generic;
    using OrderRules.Core.Model;
    
    public partial class RuleValidationsDTO
    {
        public RuleValidationsDTO()
        {
            this.CustomerPriceTotalDTO = new HashSet<CustomerPriceTotalDTO>();
            this.CustomerPriceSubTotalDTO = new HashSet<CustomerPriceSubTotalDTO>();
        }
    
        public int RuleValidationID { get; set; }
        public int RuleID { get; set; }

        public ICollection<int> ProductTypeIDs { get; set; }
        public ICollection<int> ProductIDs { get; set; }
        public ICollection<int> StoreFrontIDs { get; set; }
        public ICollection<int> AccountIDs { get; set; }
        public ICollection<short> AccountTypeIDs { get; set; }
        public ICollection<short> OrderTypeIDs { get; set; }

        public virtual ICollection<CustomerPriceTotalDTO> CustomerPriceTotalDTO { get; set; }
        public virtual ICollection<CustomerPriceSubTotalDTO> CustomerPriceSubTotalDTO { get; set; }

    }
}
