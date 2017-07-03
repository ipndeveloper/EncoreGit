using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class AccountSponsorSearchData
    {

        /// <summary>
        /// Gets or sets AccountID.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public int AccountID { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets LastName.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets AccoutnStatusID.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public int AccountStatusID { get; set; }

        /// <summary>
        /// Gets or sets AccoutnStatus.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string AccountStatus { get; set; }

        /// <summary>
        /// Gets or sets TitleID.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public int TitleID { get; set; }

        /// <summary>
        /// Gets or sets TitleName.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string TitleName { get; set; }

        /// <summary>
        /// Gets or sets TitleID.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public int SponsorID { get; set; }

        /// <summary>
        /// Gets or sets TerminatedSponsorID.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public int TerminatedSponsorID { get; set; }

        [Display(AutoGenerateField = false)]
        public string TerminatedSponsor { get; set; } 
    }
}
