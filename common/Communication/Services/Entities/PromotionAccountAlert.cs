using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Communication.Services.Entities
{
    [Table("PromotionAccountAlerts", Schema = "Communication")]
    public class PromotionAccountAlert
    {
        [Key, ForeignKey("AccountAlert")]
        public int AccountAlertId { get; set; }
        public int PromotionId { get; set; }

        public virtual AccountAlert AccountAlert { get; set; }
    }
}
