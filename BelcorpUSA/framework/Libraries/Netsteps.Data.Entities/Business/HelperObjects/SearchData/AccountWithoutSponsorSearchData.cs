using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class AccountWithoutSponsorSearchData
    {

        [Display(AutoGenerateField = false)]
        public int AccountID { get; set; }

        [Display(AutoGenerateField = false)]
        public string Name { get; set; }

        [Display(AutoGenerateField = false)]
        public string AccountStatus { get; set; }

        [Display(AutoGenerateField = false)]
        public string AccountStatusTerm { get; set; }

        [Display(AutoGenerateField = false)]
        public string PeriodID { get; set; }

        [Display(AutoGenerateField = false)]
        public string City { get; set; }

        [Display(AutoGenerateField = false)]
        public string State { get; set; }

        [Display(AutoGenerateField = false)]
        public string Email { get; set; }

        [Display(AutoGenerateField = false)]
        public string Address { get; set; }

        [Display(AutoGenerateField = false)]
        public string Telephone1 { get; set; }

        [Display(AutoGenerateField = false)]
        public string Telephone2 { get; set; }
    }
}
