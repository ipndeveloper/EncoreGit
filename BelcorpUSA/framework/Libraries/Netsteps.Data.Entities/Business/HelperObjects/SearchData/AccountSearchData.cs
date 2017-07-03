using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class AccountSearchData
    {
        /// <summary>
        /// Gets or sets AccountID.
        /// </summary>
        [TermName("ID")]
        [Display(AutoGenerateField = false)]
        public int AccountID { get; set; }

        /// <summary>
        /// Gets or sets AccountNumber.
        /// </summary>
        [TermName("AccountNumber", "Account Number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        [TermName("FirstName", "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets LastName.
        /// </summary>
        [TermName("LastName", "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        [TermName("Name")]
        [Display(AutoGenerateField = false)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets AccountTypeID.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public short AccountTypeID { get; set; }

        /// <summary>
        /// Gets or sets AccountType.
        /// </summary>
		[Sortable(false)]
        [TermName("Type")]
        [Display(Name = "Type")]
        public string AccountType { get; set; }

        /// <summary>
        /// Gets or sets AccountStatusID.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public short AccountStatusID { get; set; }

        /// <summary>
        /// Gets or sets AccountStatus.
        /// </summary>
		[Sortable(false)]
        [TermName("Status")]
        [Display(Name = "Status")]
        public string AccountStatus { get; set; }

        /// <summary>
        /// Gets or sets DateEnrolled.
        /// </summary>
        [TermName("DateEnrolled", "Date Enrolled")]
        [PropertyName("EnrollmentDateUTC")]
        public DateTime DateEnrolled { get; set; }

        /// <summary>
        /// Gets or sets DateCreated.
        /// </summary>
        [TermName("DateCreated", "Date Created")]
        [PropertyName("DateCreatedUTC")]
        [Display(AutoGenerateField = false)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets EmailAddress.
        /// </summary>
        [TermName("Email")]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets CoApplicant.
        /// </summary>
        [TermName("CoApplicant")]
        [Display(Name = "Co-Applicant")]
        public string CoApplicant { get; set; }

        /// <summary>
        /// Gets or sets Sponsor.
        /// </summary>
        [TermName("Sponsor")]
        public string Sponsor { get; set; }

        /// <summary>
        /// Gets or sets SponsorAccountNumber.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string SponsorAccountNumber { get; set; }

        /// <summary>
        /// Gets or sets Location.
        /// </summary>
        [TermName("Location")]
		public string Location { get; set; }

        // Used for Distributor locator - JHE

        /// <summary>
        /// Gets or sets MilesAway.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public decimal? MilesAway { get; set; }

        // Used to set the contact source

        /// <summary>
        /// Gets or sets Source.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsOptedOut.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public bool IsOptedOut { get; set; }

		/// <summary>
		/// Gets or sets a value indicating the MarketID
		/// </summary>
		[Display(AutoGenerateField = false)]
		public int MarketID { get; set; }

        //CAMBIO ENCORE-4
        [TermName("AccountBrowseGender")]
        public string Gender { get; set; }

    }
}
