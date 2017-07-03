using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class AccountSlimSearchData
    {
        [TermName("ID")]
        [Display(AutoGenerateField = false)]
        public int AccountID { get; set; }

		public int MarketID { get; set; }

        [TermName("AccountNumber", "Account Number")]
        public string AccountNumber { get; set; }

        public short AccountTypeID { get; set; }

        public int? SponsorID { get; set; }

        [TermName("FirstName", "First Name")]
        public string FirstName { get; set; }

        [TermName("LastName", "Last Name")]
        public string LastName { get; set; }

        [TermName("Email")]
        public string EmailAddress { get; set; }

        [TermName("TaxNumber")]
        public string DecryptedTaxNumber { get; set; }

        [TermName("Name")]
        [Display(AutoGenerateField = false)]
        public string FullName { get; set; }

        [TermName("IsTaxExempt")]
        [Display(AutoGenerateField = false)]
        public bool? IsTaxExempt { get; set; }

        [TermName("TaxGeocode")]
        [Display(AutoGenerateField = false)]
        public string TaxGeocode { get; set; }
    }
}
