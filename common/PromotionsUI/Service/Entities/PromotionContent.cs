using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.UI.Service.Entities
{
    [Table("PromotionContent", Schema = "Promo")]
    public class PromotionContent
    {
        public int PromotionContentID { get; set; }
        public int PromotionID { get; set; }
        public int LanguageID { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        [MaxLength(255)]
        public string ActionText { get; set; }
        public string Description { get; set; }
        [MaxLength(512)]
        public string ImagePath { get; set; }
        [MaxLength(255)]
        public string AlertTitle { get; set; }
    }
}
