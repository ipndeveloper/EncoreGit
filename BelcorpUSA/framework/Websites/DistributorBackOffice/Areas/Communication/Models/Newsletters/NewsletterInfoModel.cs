using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DistributorBackOffice.Areas.Communication.Models.Newsletters
{
    public class NewsletterInfoModel
    {
        public bool ShowContent { get; set; }
        public bool ShowStats { get; set; }

        public int? CampaignActionID { get; set; }
        public string Name { get; set; }
        public TimeSpan? TimeRemaining { get; set; }
        public DateTime? MailingDate { get; set; }

        public int? SelectedLanguageID { get; set; }
        public List<SelectListItem> LanguageSelectList { get; set; }

        public NewsletterContentModel NewsletterContentModel { get; set; }

        public NewsletterInfoModel()
        {
            this.NewsletterContentModel = new NewsletterContentModel();
        }
    }
}