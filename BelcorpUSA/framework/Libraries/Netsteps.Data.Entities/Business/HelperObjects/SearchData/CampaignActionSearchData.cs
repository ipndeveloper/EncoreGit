using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class CampaignActionSearchData
    {
        [Display(AutoGenerateField = false)]
        public int CampaignActionID { get; set; }

        [Display(AutoGenerateField = false)]
        public int? EmailTemplateID { get; set; }

        [TermName("Name")]
        public string Name { get; set; }

        [TermName("Active")]
        public bool Active { get; set; }

        [Display(Name = "NextCampaignActionScheduledDate")]
        [TermName("NextCampaignActionScheduledDate")]
        public DateTime? NextRunDateUTC { get; set; }

        [Display(AutoGenerateField = false)]
        public int? AlertTemplateID { get; set; }

    }
}
