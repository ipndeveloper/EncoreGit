using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;


namespace NetSteps.Data.Entities.Business
{
    public class LogisticProviderSuppliedIDs
    {
        
        [Display(Name = "Document Type")]
        public string Name { get; set; }

        [Display(Name = " Document Number")]
        public string IDValue { get; set; }

        [Display(Name = "Expedition Date")]
        public DateTime IDExpeditionDate { get; set; }

        [Display(Name = "Expidition Entity")]
        public string ExpeditionEntity { get; set; }

        [Display(Name = "Is Primary Document")]
        public bool IsPrimaryID { get; set; }

        [Display(AutoGenerateField = false)]
        public int LogisticsProviderID { get; set; }

        [Display(AutoGenerateField = false)]
        public int IDTypeID { get; set; }
    }
}
