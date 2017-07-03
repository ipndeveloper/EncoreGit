using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class AccountSponsorLogSearchData
    {

        /// <summary>
        /// Gets or sets SponsorID.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public int OldSponsorID { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string OldSponsorFirstName { get; set; }

        /// <summary>
        /// Gets or sets LastName.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string OldSponsorLastName { get; set; }

        /// <summary>
        /// Gets or sets SponsorID.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public int NewSponsorID { get; set; }
        
        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string NewSponsorFirstName { get; set; }

        /// <summary>
        /// Gets or sets LastName.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string NewSponsorLastName { get; set; }

        /// <summary>
        /// Gets or sets CampainStart.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string CampainStart { get; set; }

        /// <summary>
        /// Gets or sets UpdateDate.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets UpdateUser.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string UpdateUser { get; set; }
    
    }
}
