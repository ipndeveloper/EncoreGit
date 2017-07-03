using System.Collections.Generic;
using NetSteps.Data.Entities;

namespace nsCore.Areas.Communication.Models
{
    public class EmailTemplateModel
    {

        public EmailTemplate EmailTemplate { get; set; }

        public TrackableCollection<Token> Tokens { get; set; }

        public Dictionary<int, string> Languages { get; set; }

        public bool IsNewsLetterType { get; set; }

        public bool IsNewEmailTemplate { get; set; }

        public int LanguageCount
        {
            get
            {
                return Languages == null ? 0 : Languages.Count;
            }
        }

        public int? CampaignActionID { get; set; }
        public int? CampaignID { get; set; }
        public short? CampaignTypeID { get; set; }
        public string CancelURL { get; set; }
    }
}