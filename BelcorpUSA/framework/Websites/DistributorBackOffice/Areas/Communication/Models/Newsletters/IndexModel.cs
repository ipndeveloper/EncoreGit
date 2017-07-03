using System.Collections.Generic;
using System.Web.Mvc;

namespace DistributorBackOffice.Areas.Communication.Models.Newsletters
{
    public class IndexModel
    {
        public bool ShowNewsletters { get; set; }

        public int? SelectedCampaignID { get; set; }
        public List<SelectListItem> CampaignSelectList { get; set; }

        public int? SelectedCampaignActionID { get; set; }
        public List<SelectListItem> CampaignActionSelectList { get; set; }

        public NewsletterInfoModel NewsletterInfoModel { get; set; }

        public IndexModel()
        {
            this.NewsletterInfoModel = new NewsletterInfoModel();
        }
    }
}