namespace OrderRules.Service.DTO
{
    using System;
    using System.Collections.Generic;
    using OrderRules.Core.Model;
    
    public partial class RulesDTO
    {
        public RulesDTO()
        {
            this.RuleValidationsDTO = new HashSet<RuleValidationsDTO>();
        }
        public int RuleID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool HasDates { get; set; }
        public string Name { get; set; }
        public string TermName { get; set; }
        public string TermContent { get; set; }
        public int RuleStatus { get; set; }
    
        public virtual RuleStatuses RuleStatuses { get; set; }

        public virtual ICollection<RuleValidationsDTO> RuleValidationsDTO { get; set; }
    }
}
