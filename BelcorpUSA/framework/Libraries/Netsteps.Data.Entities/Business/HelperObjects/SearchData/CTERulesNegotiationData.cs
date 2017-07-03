using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business
{
    public class CTERulesNegotiationData
    {
        [Display(AutoGenerateField = false)]
        public int FineAndInterestRulesID { get; set; }

        [Display(Name = "Negotiation")]
        public string Negotiation { get; set; }

        [Display(Name = "OpeningDay")]
        public int OpeningDay { get; set; }

        [Display(Name = "FinalDay")]
        public int FinalDay { get; set; }

        [Display(Name = "FinePercentage")]
        public string FinePercentage { get; set; }

        [Display(Name = "AppliedValue")]
        public string AppliedValue { get; set; }

        [Display(Name = "Minimum Debt")]
        public double MinimumDebt { get; set; }

        [Display(Name = "InterestPercentage")]
        public string InterestPercentage { get; set; }

        [Display(Name = "Applied Value")]
        public string Interest { get; set; }

        [Display(Name = "Discount")]
        public string Discount { get; set; }

        [Display(Name = "Applied Value")]
        public string FineBaseAmountIDReg { get; set; }

        //SAVE
        [Display(AutoGenerateField = false)]
        public int FineAndInterestRulesPerNegotiationLevelID { get; set; }

        [Display(AutoGenerateField = false)]
        public int NegotiationLevelID { get; set; }

        [Display(AutoGenerateField = false)]
        public int StartDay { get; set; }

        [Display(AutoGenerateField = false)]
        public int EndDay { get; set; }

        [Display(AutoGenerateField = false)]
        public int FineBaseAmountID { get; set; }

        [Display(AutoGenerateField = false)]
        public string Name { get; set; }

        [Display(AutoGenerateField = false)]
        public int InterestBaseAmountID { get; set; }

        [Display(AutoGenerateField = false)]
        public string MinimumAmountForFine { get; set; }

        [Display(AutoGenerateField = false)]
        public string NegotiationLevel { get; set; }
        [Display(AutoGenerateField = false)]
        public string FineBaseAmount { get; set; }
        [Display(AutoGenerateField = false)]
        public string InterestBaseAmount { get; set; }
        //[Display(AutoGenerateField = false)]
        //public string Discount { get; set; }
        //[Display(AutoGenerateField = false)]
        //public string FineBaseAmountIDReg { get; set; }

    }
}
