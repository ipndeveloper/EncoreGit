using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Mail;
using NetSteps.Data.Entities.TokenValueProviders;

namespace NetSteps.Web.Mvc.Controls.Models
{
    public class EmailTemplateContentModel
    {
        #region Properties
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }

        public int? CampaignActionID { get; set; }
        public short? EmailTemplateTypeID { get; set; }
        public int? LanguageID { get; set; }

        public string DefaultDistributorContent { get; set; }
        public string DefaultDistributorImageUrl { get; set; }
        #endregion

        public static MailMessage GetPreviewMailMessage(EmailTemplateContentModel model, EmailTemplateTranslation tempTranslation)
        {
            ITokenValueProvider previewTokenValueProvider;

            if (!string.IsNullOrWhiteSpace(model.DefaultDistributorContent) || !string.IsNullOrWhiteSpace(model.DefaultDistributorImageUrl))
            {
                Dictionary<string, string> defaultDistributorContentTokenValues = new Dictionary<string, string>();

                if (model.DefaultDistributorContent != null)
                {
                    defaultDistributorContentTokenValues.Add(NetSteps.Data.Entities.Constants.Token.DistributorContent.ToPlaceholder(), model.DefaultDistributorContent);
                }

                if (model.DefaultDistributorImageUrl != null)
                {
                    defaultDistributorContentTokenValues.Add(NetSteps.Data.Entities.Constants.Token.DistributorImage.ToPlaceholder(), model.DefaultDistributorImageUrl);
                }

                previewTokenValueProvider = new CompositeTokenValueProvider(new[]
                {
                    new TokenValueProvider(defaultDistributorContentTokenValues),
                    GetPreviewTokenValueProvider(model)
                });
            }
            else
            {
                previewTokenValueProvider = GetPreviewTokenValueProvider(model);
            }

            return tempTranslation.GetTokenReplacedMailMessage(previewTokenValueProvider);
        }
        
        static ITokenValueProvider GetPreviewTokenValueProvider(EmailTemplateContentModel model)
        {
            IEnumerable<string> tokens = model.EmailTemplateTypeID.ToShort() > 0 ? EmailTemplateType.LoadFull(model.EmailTemplateTypeID.ToShort()).Tokens.Select(x => x.Placeholder) : Token.LoadAll().Select(x => x.Placeholder);

            return new MockTokenValueProvider(tokens, model.LanguageID.ToInt());
        }
    }
}
