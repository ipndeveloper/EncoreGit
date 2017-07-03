
namespace NetSteps.Data.Entities.Business
{
	public class CampaignActionTokenValueSearchParameters
	{
		public int? TokenID { get; set; }

        public int? AccountID { get; set; }

        public int? LanguageID { get; set; }

        public int? CampaignActionID { get; set; }
    
        /// <summary>
        /// If null and AccountID is not specified, we assume true. If null and AccountID is specified, we assume false.
        /// </summary>
        public bool? IncludeDefaults { get; set; }
    }
}
