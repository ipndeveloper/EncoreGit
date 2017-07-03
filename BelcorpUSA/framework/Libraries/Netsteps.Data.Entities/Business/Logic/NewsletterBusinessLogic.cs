
namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class NewsletterBusinessLogic
	{
        //protected const string DISTRIBUTOR_PWSURL = "DistributorPWSUrl";
        //protected const string DISTRIBUTOR_IMAGE = "DistributorImage";

        //public virtual List<string> GetEmailTemplateTokenNames()
        //{
        //    List<string> result = new List<string>(EmailTemplateToken.GetTokenNames());
        //    result.Add(DISTRIBUTOR_PWSURL);
        //    result.Add(DISTRIBUTOR_IMAGE);

        //    return result;
        //}

        //public virtual string GeneratePreviewHtml(Newsletter newsletter)
        //{
        //    List<EmailTemplateToken> tokens = newsletter.EmailTemplate.GetPreviewTokens(null, null);
        //    return newsletter.EmailTemplate.GeneratePreviewHtml(tokens);
        //}

        //public virtual string GeneratePreviewHtml(Newsletter newsletter, Account consultantAccount)
        //{
        //    List<EmailTemplateToken> tokens = newsletter.EmailTemplate.GetPreviewTokens(null, consultantAccount);
        //    return newsletter.EmailTemplate.GeneratePreviewHtml(tokens);
        //}

        //public virtual string GeneratePreviewHtml(Newsletter newsletter, Account consultantAccount, Account contactAccount)
        //{
        //    List<EmailTemplateToken> tokens = newsletter.EmailTemplate.GetPreviewTokens(contactAccount, consultantAccount);
        //    return newsletter.EmailTemplate.GeneratePreviewHtml(tokens);
        //}

        //public virtual List<Newsletter> LoadHistoricalNewslettersForConsultant(INewsletterRepository repository, Account account)
        //{
        //    var obj = repository.LoadHistoricalNewslettersForConsultant(account.AccountID);
        //    foreach (var item in obj)
        //    {
        //        item.StartEntityTracking();
        //        item.IsLazyLoadingEnabled = true;
        //    }
        //    return obj;
        //}
	}
}
