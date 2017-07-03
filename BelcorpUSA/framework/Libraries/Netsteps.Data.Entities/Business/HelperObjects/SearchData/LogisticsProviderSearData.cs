using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
   public class LogisticsProviderSearData
    {
        [TermName("LogisticProviderCode", "Logistic Provider Code")] 
        [Display(Name = "Logistics Provider Code")]
        public int LogisticsProviderID { get; set; }

        [TermName("Name", "Name")] 
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Display(AutoGenerateField = false)]
        public int AddressID { get; set; }

        [Display(AutoGenerateField = false)]
        public string PhoneNumber { get; set; }

        [Display(AutoGenerateField = false)]
        public string FaxNumber { get; set; }

        [Display(AutoGenerateField = false)]
        public string EmailAddress { get; set; }

        [Display(AutoGenerateField = false)]
        public string TermName { get; set; }

        [Display(AutoGenerateField = false)]
        public string Description { get; set; }

        [Display(AutoGenerateField = false)]
        public bool Active { get; set; }

        [Display(AutoGenerateField = false)]
        public int MarketID { get; set; }

        [Display(AutoGenerateField = false)]
        public string ExternalCode { get; set; }

        [Display(AutoGenerateField = false)]
        public bool WorkInSaturdays { get; set; }

        [Display(AutoGenerateField = false)]
        public bool WorkInSundays { get; set; }

        [Display(AutoGenerateField = false)]
        public bool WorkInHolidays { get; set; }

        [Display(AutoGenerateField = false)]
        public string ExternalTrakingURL { get; set; }

    }
}
