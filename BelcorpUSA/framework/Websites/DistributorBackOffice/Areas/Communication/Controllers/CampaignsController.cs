
namespace DistributorBackOffice.Areas.Communication.Controllers
{
	public class CampaignsController : BaseCommunicationController
	{
		//public virtual ActionResult Edit(int id)
		//{
		//    var campaign = Campaign.LoadFull(id);
		//    var emailTemplateId = campaign.CampaignEmails.First().EmailTemplateID;
		//    ViewBag.EmailTemplateID = emailTemplateId;
		//    ViewBag.Accounts = Account.LoadBatchSlim(campaign.CampaignSubscribers.Where(s => s.AddedByAccountID == CurrentAccount.AccountID).Select(s => s.AccountID));
		//    ViewBag.EmailTemplate = EmailTemplate.LoadFull(emailTemplateId);
		//    var emailTemplateTokens = EmailTemplateToken.GetAllCustomTokensForAccount(emailTemplateId, CurrentAccount.AccountID);
		//    ViewBag.DistributorContent = emailTemplateTokens.Any(ett => ett.Token == "DistributorContent") ? emailTemplateTokens.First(ett => ett.Token == "DistributorContent").Value : "";

		//    return View(campaign);
		//}

		//public virtual ActionResult EmailTemplatePreview(int id)
		//{
		//    var emailTemplate = EmailTemplate.Load(id);
		//    var emailTemplateTokens = EmailTemplateToken.GetAllCustomTokensForAccount(id, CurrentAccount.AccountID);
		//    var distributorContent = emailTemplateTokens.Any(ett => ett.Token == "DistributorContent") ? emailTemplateTokens.First(ett => ett.Token == "DistributorContent").Value : "";

		//    List<EmailTemplateToken> tokens = emailTemplate.GetPreviewTokens(null, CurrentAccount);

		//    List<EmailTemplateToken> overrideTokens = new List<EmailTemplateToken>();
		//    overrideTokens.Add(new EmailTemplateToken() { Token = "DistributorContent", Value = string.Format("<textarea id=\"distributorContent\" rows=\"\" cols=\"\" style=\"width:100%;\">{0}</textarea>", distributorContent), AccountID = CurrentAccount.AccountID });

		//    tokens = EmailTemplateToken.CombineTokens(tokens, overrideTokens);

		//    ViewBag.Tokens = tokens;

		//    return View(emailTemplate);
		//}

		//public virtual ActionResult AddSubscriber(int campaignId, int accountId)
		//{
		//    try
		//    {
		//        var addedByAccountId = CampaignSubscriber.IsSubscriberAdded(campaignId, accountId);
		//        if (addedByAccountId.HasValue)
		//            return Json(new { result = false, message = Translation.GetTerm("SubscriberAlreadyAdded", "That subscriber was already added by {0}", Account.LoadSlim(addedByAccountId.Value).FullName) });
		//        else
		//        {
		//            var newSubscriber = new CampaignSubscriber();
		//            newSubscriber.AccountID = accountId;
		//            newSubscriber.AddedByAccountID = CurrentAccount.AccountID;
		//            newSubscriber.CampaignID = campaignId;
		//            newSubscriber.DateAdded = DateTime.Now;
		//            newSubscriber.Save();
		//        }
		//        return Json(new { result = true });
		//    }
		//    catch (Exception ex)
		//    {
		//        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
		//        return Json(new { result = false, message = exception.PublicMessage });
		//    }
		//}

		//public virtual ActionResult Save(int emailTemplateId, string content)
		//{
		//    try
		//    {
		//        var emailTemplateTokens = EmailTemplateToken.GetAllCustomTokensForAccount(emailTemplateId, CurrentAccount.AccountID);
		//        if (emailTemplateTokens.Any(ett => ett.Token == "DistributorContent"))
		//        {
		//            var emailTemplateToken = emailTemplateTokens.First(ett => ett.Token == "DistributorContent");
		//            emailTemplateToken.Value = content;
		//            emailTemplateToken.Save();
		//        }
		//        else
		//        {
		//            var emailTemplateToken = new EmailTemplateToken();
		//            emailTemplateToken.EmailTemplateID = emailTemplateId;
		//            emailTemplateToken.AccountID = CurrentAccount.AccountID;
		//            emailTemplateToken.Token = "DistributorContent";
		//            emailTemplateToken.Value = content;
		//            emailTemplateToken.Save();
		//        }
		//        return Json(new { result = true });
		//    }
		//    catch (Exception ex)
		//    {
		//        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
		//        return Json(new { result = false, message = exception.PublicMessage });
		//    }
		//}
	}
}
