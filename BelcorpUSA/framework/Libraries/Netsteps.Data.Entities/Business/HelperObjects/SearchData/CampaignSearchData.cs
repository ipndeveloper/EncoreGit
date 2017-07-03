using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class CampaignSearchData
    {
        [Display(AutoGenerateField = false)]
        public int CampaignID { get; set; }

        [TermName("Name")]
        public string Name { get; set; }

        [TermName("NoOfEmails", "No. of Emails")]
        public int NumberOfEmails { get; set; }

        [TermName("StartDate", "Start Date")]
        public DateTime? StartDate { get; set; }

        [TermName("EndDate", "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Status")]
        [TermName("Status")]
        public bool Active { get; set; }

        [Display(AutoGenerateField = false)]
        public int MarketID { get; set; }

        [Display(Name = "NextCampaignActionScheduledDate")]
        [TermName("NextCampaignActionScheduledDate")]
        public DateTime? NextCampaignActionScheduledDate { get; set; }
    }
}
