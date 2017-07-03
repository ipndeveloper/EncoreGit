using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

//@01 20150717 BR-CC-003 G&S LIB: Se crea la clase con sus respectivos métodos

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{

    [Serializable]
    public class RenegotiationSearchData
    {

        [Display(Name = "RenegotiationConfigurationID")]
        public int RenegotiationConfigurationID { get; set; }

        [Display(Name = "DescriptionRenegotiation")]
        public string DescriptionRenegotiation { get; set; }

        [Display(Name = "SharesNumber")]
        public int SharesNumber { get; set; }

        [Display(Name = "Site")]        
        public string Site { get; set; }


        [Display(Name = "FineAndInterestRules")]
        public string FineAndInterestRules { get; set; }

        [Display(Name = "Day Validate")]
        public int DayValidate { get; set; }

        //[Display(Name = "Day Expiration")]
        //public int DayExpiration { get; set; } 

        //[Display(Name = "FinePercentage")]
        //public string RegFinePercentage { get; set; }

        //[Display(Name = "InteresPercentage")]
        //public string RegInteresPercentage { get; set; }

        //[Display(AutoGenerateField = false)]
        //public int RegMinimumAmountForFine { get; set; }

        [Display(Name = "FirstSharesday")]
        public int FirstSharesday { get; set; }

        [Display(Name = "SkillfulCalendarFirst")]
        public string SkillfulCalendarFirst { get; set; }

        [Display(Name = "SharesInterval")]
        public int SharesInterval { get; set; }

        [Display(Name = "SkillfulRemainingCalendar")]
        public string SkillfulRemainingCalendar { get; set; }

        [Display(Name = "ModifiesDates")]     
        public string ModifiesDates { get; set; }

        [Display(Name = "ModifiesValues")]       
        public string ModifiesValues { get; set; }

        [Display(AutoGenerateField = false)]
        public string Observation { get; set; }

        [Display(AutoGenerateField = false)]
        public bool DisabledFineAndInterestRules { get; set; }

        [Display(AutoGenerateField = false)]
        public string FineAndInterestRuleID { get; set; }
    }
}
