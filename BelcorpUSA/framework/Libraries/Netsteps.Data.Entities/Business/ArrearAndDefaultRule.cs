using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business
{
    [Table("ArrearsAndDefaultsRules")]
    public class ArrearAndDefaultRule
    {
        [Column("ArrearsAndDefaultsRulesID"), Key]
        public int ArrearsAndDefaultsRulesID { get; set; }
        
        [Column("MinimumTotalAmount")]
        public int MinimumTotalAmount { get; set; }
        [Column("RestrictedAmount")]
        
        public int RestrictedAmount { get; set; }
        [Column("RestrictedAmountPercentage")]
        public double RestrictedAmountPercentage { get; set; }
        
        [Column("MonthsPreviousToReport")]
        public int MonthsPreviousToReport { get; set; }
    }
}
