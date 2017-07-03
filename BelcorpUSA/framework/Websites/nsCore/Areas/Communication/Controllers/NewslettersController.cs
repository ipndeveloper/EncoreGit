using nsCore.Controllers;

namespace nsCore.Areas.Communication.Controllers
{
    public class NewslettersController : BaseController
    {
        //[FunctionFilter("Communication", "~/Accounts")]
        //public virtual ActionResult Index(int marketId = -1, int languageId = -1)
        //{
        //    IQueryable<Newsletter> newsletters = Newsletter.LoadAll().AsQueryable();

        //    if (marketId > 0)
        //        newsletters = newsletters.Where(x => x.MarketID == marketId);
        //    if (languageId > 0)
        //        newsletters = newsletters.Where(x => x.LanguageID == languageId);

        //    return View(newsletters.ToList());
        //}
        
        //[FunctionFilter("Communication", "~/Accounts")]
        //public virtual ActionResult Create()
        //{
        //    Newsletter newsletter = new Newsletter { NewsletterID = -1 };
        //    newsletter.EmailTemplate = new EmailTemplate() { EmailTemplateID = -1 };
        //    return View("Edit", newsletter);
        //}
        
        //[FunctionFilter("Communication", "~/Accounts")]
        //public virtual ActionResult Edit(int id = 0)
        //{
        //    return View(Newsletter.LoadFull(id));
        //}

        //[ValidateInput(false)]
        //[HttpPost]
        //[FunctionFilter("Communication", "~/Accounts")]
        //public virtual ActionResult Save(int newsletterId = 0, string name = "", string subject = "", string body = "", string defaultImage = "", string defaultMessage = "",
        //                                   DateTime? scheduledSendDate = null, DateTime? consultantEditableDate = null, bool active = false, int marketId = 0, int languageId = 0)
        //{
        //    if (scheduledSendDate == null)
        //        return Json(new { success = false, message = "Scheduled Send Date is invalid" });

        //    if (consultantEditableDate == null)
        //        return Json(new { success = false, message = "Consultant Editable Date is invalid" });

        //    Newsletter newsletter;

        //    if (newsletterId > 0)
        //    {
        //        newsletter = Newsletter.LoadFull(newsletterId);
        //        if (newsletter.IsSent || newsletter.IsProcessing)
        //            return Json(new { success = false, message = "Newsletter cannot be edited" });
        //    }
        //    else
        //    {
        //        newsletter = new Newsletter();
        //        newsletter.EmailTemplate = new EmailTemplate();
        //    }

        //    newsletter.EmailTemplate.Body = body;
        //    newsletter.EmailTemplate.Subject = subject;
        //    newsletter.EmailTemplate.EmailTemplateTypeID = (int)Constants.EmailTemplateType.Newsletter;

        //    EmailTemplateToken defaultImageToken = newsletter.EmailTemplate.EmailTemplateTokens.FirstOrDefault(x => x.Token == "DistributorImage");
        //    if (defaultImageToken == null)
        //        newsletter.EmailTemplate.EmailTemplateTokens.Add(new EmailTemplateToken() { Token = "DistributorImage", Value = defaultImage });
        //    else
        //        defaultImageToken.Value = defaultImage;

        //    EmailTemplateToken defaultMessageToken = newsletter.EmailTemplate.EmailTemplateTokens.FirstOrDefault(x => x.Token == "DistributorContent");
        //    if (defaultMessageToken == null)
        //        newsletter.EmailTemplate.EmailTemplateTokens.Add(new EmailTemplateToken() { Token = "DistributorContent", Value = defaultMessage });
        //    else
        //        defaultMessageToken.Value = defaultMessage;

        //    newsletter.Name = name;
        //    newsletter.ScheduledSendDateUTC = (DateTime)scheduledSendDate;
        //    newsletter.ConsultantEditableUTC = consultantEditableDate;
        //    newsletter.Active = active;
        //    newsletter.MarketID = marketId;
        //    newsletter.LanguageID = languageId;
        //    //newsletter.DateSentUTC = DateTime.Parse("1/1/1900"); // change to nullable datetime

        //    List<string> invalidTokens = newsletter.EmailTemplate.FindInvalidTokens(newsletter.GetEmailTemplateTokenNames());

        //    if (invalidTokens.Count > 0)
        //        return Json(new { success = false, message = "The following tokens are invalid:\n" + invalidTokens.Aggregate("", (current, token) => current + token + "\n") });

        //    foreach (EmailTemplateToken token in newsletter.EmailTemplate.EmailTemplateTokens)
        //        token.Save();
        //    newsletter.EmailTemplate.Save();
        //    newsletter.Save();
        //    return Json(new { success = true, message = "Newsletter successfully saved" });
        //}

        //public virtual ActionResult Preview(int id = -1)
        //{
        //    if (id <= 0)
        //        return Content("<html><body>Invalid id</body></html>", "text/html");

        //    Newsletter newsletter = Newsletter.LoadFull(id);
        //    return Content(newsletter.Preview(), "text/html");
        //}

        //[HttpGet]
        //public virtual ContentResult PreviewUnsaved()
        //{
        //    return Content(TempData["NewsletterPreviewUnsaved"] as string, "text/html");
        //}

        //[ValidateInput(false)]
        //[HttpPost]
        //public virtual JsonResult PreviewUnsaved(string html = "", string defaultImage = "", string defaultMessage = "")
        //{
        //    EmailTemplate template = new EmailTemplate();
        //    template.Body = html;

        //    List<EmailTemplateToken> tokens = EmailTemplateToken.GetFakeTokensForPreview();
        //    List<EmailTemplateToken> overrideTokens = new List<EmailTemplateToken>();
        //    overrideTokens.Add(new EmailTemplateToken() { Token = "DistributorImage", Value = defaultImage });
        //    overrideTokens.Add(new EmailTemplateToken() { Token = "DistributorContent", Value = defaultMessage });
        //    tokens = EmailTemplateToken.CombineTokens(tokens, overrideTokens);

        //    TempData["NewsletterPreviewUnsaved"] = template.GeneratePreviewHtml(tokens);

        //    return Json(new { success = true });
        //}

        //    [HttpPost]
        //[FunctionFilter("Communication", "~/Accounts")]
        //    public virtual ActionResult Send(int id = 0)
        //    {
        //        Newsletter newsletter = Newsletter.Load(id);

        //        if (!newsletter.Active)
        //           return Json(new { success = false, message = "Newsletter is not active" });

        //        if (newsletter.IsProcessing || newsletter.IsSent)
        //           return Json(new { success = false, message = "Newsletter is already sending" });

        //        return Json(new { success = true });
        //    }

        //    [HttpPost]
        //[FunctionFilter("Communication", "~/Accounts")]
        //    public virtual ActionResult SendTest(int id = 0)
        //    {
        //        //grab list of emails from web.config & hardcode consultant info?
        //        int[] testIds = getNewsletterTestAccounts();
        //        Newsletter newsletter = Newsletter.Load(id);
        //    }
        
        //[FunctionFilter("Communication", "~/Accounts")]
        //    public virtual ActionResult Stats(int id)
        //    {
        //        return View();
        //    }
    }
}
