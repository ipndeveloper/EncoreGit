
namespace DistributorBackOffice.Areas.Communication.Models.Newsletters
{
    public class NewsletterContentModel
    {
        public int? CampaignActionID { get; set; }
        public int? LanguageID { get; set; }

        public string DistributorContent { get; set; }
        public string DistributorImageUrl { get; set; }
    }
}