using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Mail;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.Web.Mvc.Extensions;
using nsDistributor.Models;
using Belcorp.Policies.Service;
using Belcorp.Policies.Service.DTO;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Generated;

namespace nsDistributor.Controllers
{
	public class StaticController : BaseController
	{
		public virtual ActionResult Home()
		{
			var currentSite = CurrentSite;
            if (Convert.ToInt32(Session["CategoryId"]) > 0)
            {
                //Shop/Category/36
                int Id = Convert.ToInt32(Session["CategoryId"]);
                return Redirect("~/Shop/Category/" + Id);
            }
			var homePage = currentSite.GetPageByUrl("/Home");

			if (!homePage.Active)
			{
				return RedirectToAction("Index", "Shop");
			}
            else
                return RedirectToAction("Index", "");

            ///*CS.12JUN.2016.Inicio*/
            //CurrentLanguageID = currentSite.DefaultLanguageID;/*ITG C0671(JCT)*/
            //if (IsLoggedIn)
            //{
            //    var account = CoreContext.CurrentAccount;
            //    CurrentLanguageID = account.DefaultLanguageID;/*ITG C0671(JCT)*/
            //    var policiesService = Create.New<IPoliciesService>();
            //    AccountPolicyDetailsDTO modelPolicy = policiesService.AccountPolicyDetail(account.AccountID, (int)ConstantsGenerated.AccountType.Distributor, account.DefaultLanguageID);/*R2908 - HUNDRED(JAUF)*/
            //    if (modelPolicy.IsApplicableAccount && !modelPolicy.IsAcceptedPolicy)
            //    {
            //        return RedirectToAction("Index", "Welcome");
            //    }
            //}
            ///*CS.12JUN.2016.Fin*/

            if (BaseController.IsLoggedIn)
            {
                int AccountID = CoreContext.CurrentAccount.AccountID;

                if (AccountID > 0)
                {
                    SecurityController sc = new SecurityController();
                    Account account = new NetSteps.Data.Entities.Account();

                    AccountController ac = new AccountController();
                    account = ac.GetCurrentAccount();
                    
                    //account.AccountStatus = CoreContext.CurrentAccount.AccountStatus;
                    //account.EnrollmentDateUTC = CoreContext.CurrentAccount.EnrollmentDateUTC;

                    if (account != null && account.AccountStatusID == (short)NetSteps.Data.Entities.Constants.AccountStatus.BegunEnrollment && account.EnrollmentDateUTC == null)
                    {
                        Tuple<int, int, int> result = new OrderBusinessLogic().CheckOrdersByAccountID(account.AccountID);
                        var ordersPending = result.Item1;
                        var otherOrders = result.Item2;
                        var direccionaPantalla2 = result.Item3;
                        if (otherOrders == 0)
                        {
                            if (direccionaPantalla2 == 1)
                                return RedirectToAction("EnrollmentItems", "Products","Enroll");
                            else
                                return RedirectToAction("EnrollmentVariantKits", "Products", "Enroll");
                        }
                    }
                }
            }

			var baseSite = currentSite.IsBase
				? currentSite
				: SiteCache.GetSiteByID(currentSite.BaseSiteID.Value);

			ViewBag.News = baseSite.News.Where(n => DateTime.Now.IsBetween(n.StartDate, n.EndDate) && n.Active && n.IsFeatured).OrderByDescending("StartDate");

			SetViewBagOpenParties();

			var layout = SmallCollectionCache.Instance.Layouts.GetById(homePage.LayoutID);

			return View(layout.ViewName, homePage);
		}

		public virtual ActionResult DesignCenter()
		{
			return View();
		}

		#region Contact Me Page (PWS)
		public virtual ActionResult ContactMe()
		{
			var baseSite = CurrentSite.IsBase ? CurrentSite : SiteCache.GetSiteByID(CurrentSite.BaseSiteID.ToInt());
			ViewBag.MyPhoto = baseSite.HtmlSections.GetBySectionName("MyPhoto");
			ViewBag.MyStory = baseSite.HtmlSections.GetBySectionName("MyStory");

			SetViewBagOpenParties();

			return View();
		}

		[HttpPost]
		public virtual ActionResult ContactMe(ContactMeModel model)
		{
			if (String.IsNullOrWhiteSpace(model.FirstName))
				ModelState.AddModelError("FirstName", Translation.GetTerm("YourFirstNameIsRequired", "Your first name is required."));
			if (String.IsNullOrWhiteSpace(model.LastName))
				ModelState.AddModelError("LastName", Translation.GetTerm("YourLastNameIsRequired", "Your last name is required."));
			if (String.IsNullOrWhiteSpace(model.Email))
				ModelState.AddModelError("Email", Translation.GetTerm("YourEmailIsRequired", "Your email is required."));
            if (String.IsNullOrWhiteSpace(model.State))
                ModelState.AddModelError("State", Translation.GetTerm("StateRequired", "State is required."));
            if (String.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError("Phone", Translation.GetTerm("PhoneRequired", "Phone Number is required."));

			var baseSite = CurrentSite.IsBase ? CurrentSite : SiteCache.GetSiteByID(CurrentSite.BaseSiteID.ToInt());
			ViewBag.MyPhoto = baseSite.HtmlSections.GetBySectionName("MyPhoto");
			ViewBag.MyStory = baseSite.HtmlSections.GetBySectionName("MyStory");

			try
			{
				if (ModelState.IsValid && (BaseController.SiteOwner != null || BaseController.CurrentSite.IsBase))
				{
					if (CurrentSite.AccountID.HasValue)
					{
						Account parentAccount = Account.Load(CurrentSite.AccountID.Value);
						SaveContactToProspect(model, parentAccount);
					}

                    var corporateMailAccount = MailAccount.GetCorporateMailAccount();
					var template = EmailTemplate.GetFirstTemplateByTemplateTypeID(NetSteps.Data.Entities.Constants.EmailTemplateType.ContactMe.ToShort());
					if (template != null)
					{
						var translation = template.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);
						if (translation != null)
						{
							var message = translation
								.GetTokenReplacedMailMessage(new ContactMeTokenValueProvider(model.FullName,
									model.Email,
									model.Phone,
									model.Comments,
                                    model.State));

							message.To.Add(new MailMessageRecipient(BaseController.CurrentSite.IsBase ? corporateMailAccount.Account.EmailAddress : BaseController.SiteOwner.EmailAddress));
							message.Send(corporateMailAccount, CurrentSite.SiteID);

							ViewBag.SuccessfullySubmitted = true;

							return View();
						}
						else
						{
							throw new InvalidOperationException(String.Format("There is no 'ContactMe' email template translation for LanguageId:{0} defined.", ApplicationContext.Instance.CurrentLanguageID));
						}
					}
					else
					{
						throw new InvalidOperationException("There is no 'ContactMe' email template defined.");
					}
				}
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
				ViewBag.Error = exception.PublicMessage;
			}

			//There was a problem, let them fix it
			return View(model);
		}

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult SearchState(string query)
        {
            try
            {
                return Json(SmallCollectionCache.Instance.StateProvinces
                            .Where(x => x.Name.ToUpper().Contains(query.ToUpper()) && x.CountryID == 1)
                                .Select(x => x.Name).ToDictionary(x => x).ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = "Error" });
            }
        }

		public static void SaveContactToProspect(ContactMeModel model, Account parentAccount)
		{
			Account newAccount = new Account();
			newAccount.SponsorID = parentAccount.AccountID;
			newAccount.EnrollerID = parentAccount.AccountID;
			newAccount.MarketID = parentAccount.MarketID;
			//set account status to active and set enrollment date to now
			newAccount.Activate();
			newAccount.DefaultLanguageID = parentAccount.DefaultLanguageID;
			newAccount.AccountTypeID = (int)NetSteps.Data.Entities.Constants.AccountType.Prospect;
			model.ApplyTo(newAccount);
			newAccount.Save();
		}

		#endregion

		#region News
		public virtual ActionResult News(int? category)
		{
			var baseSite = CurrentSite.IsBase ? CurrentSite : SiteCache.GetSiteByID(CurrentSite.BaseSiteID.ToInt());
			ViewBag.News = baseSite.News.Where(n => DateTime.Now.IsBetween(n.StartDate, n.EndDate) && n.Active && n.IsFeatured).OrderByDescending("StartDate");

			ViewBag.Category = category;

			ViewData["Categories"] = CurrentSite.News.Where(n => n.Active && DateTime.Now.ApplicationNow().IsBetween(n.StartDate, n.EndDate)).Select(n => n.NewsTypeID).Distinct();
			return View();
		}

		public virtual ActionResult Article(int id)
		{
			var baseSite = CurrentSite.IsBase ? CurrentSite : SiteCache.GetSiteByID(CurrentSite.BaseSiteID.ToInt());
			ViewData["Categories"] = baseSite.News.Where(n => n.Active && DateTime.Now.ApplicationNow().IsBetween(n.StartDate, n.EndDate)).Select(n => n.NewsTypeID).Distinct();
			var newsItem = baseSite.News.FirstOrDefault(n => n.NewsID == id);
			return View(newsItem);
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult GetNews(int? category, string query, int page, int? pageSize)
		{
			try
			{
				query = query ?? "";

				var baseSite = CurrentSite.IsBase ? CurrentSite : SiteCache.GetSiteByID(CurrentSite.BaseSiteID.ToInt());
				IEnumerable<News> news = baseSite.News.Where(n =>
				{
					var content = n.HtmlSection.ProductionContent(CurrentSite);
					return n.Active && DateTime.Now.ApplicationNow().IsBetween(n.StartDate, n.EndDate) && (!category.HasValue || category.Value == n.NewsTypeID) && content != null
						&& ((content.Title() != null && content.Title().ToLower().Contains(query)) || (content.Caption() != null && content.Caption().ToLower().Contains(query)) || (content.Body() != null && content.Body().ToLower().Contains(query)));
				}).OrderByDescending(n => n.StartDate);
				var count = news.Count();
				var builder = new StringBuilder();

				if (pageSize.HasValue)
					news = news.Skip(page * pageSize.Value).Take(pageSize.Value);

				bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
				var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
				var distributor = isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1));

				foreach (var n in news)
				{
					var content = n.HtmlSection.ProductionContent(CurrentSite);
					builder.Append("<div class=\"news\"><div class=\"newsDate\"><span class=\"newsMonth\">").Append(n.StartDate.ToString("MMM"))
						.Append("</span>&nbsp;<span class=\"newsDay\">").Append(n.StartDate.Day).Append(",</span>&nbsp;<span class=\"newsYear\">").Append(n.StartDate.Year)
						.Append("</span></div><div class=\"newsInfo\"><h2 class=\"title\"><a href=\"").Append("~/News/Article/".ResolveUrl()).Append(n.NewsID).Append("\">")
						.Append(content.GetTitle()).Append("</a></h2><p>").Append(content.GetCaption()).Append("<a class=\"readMore block\" href=\"")
						.Append(("~" + distributor + "/News/Article/").ResolveUrl()).Append(n.NewsID).Append("\">").Append(Translation.GetTerm("ReadMore", "Read more"))
						.Append("</a></p>").Append("</div><span class=\"clr\"></span></div>");
				}

				return Json(new { totalPages = pageSize.HasValue ? Math.Ceiling(count / (double)pageSize.Value) : count > 0 ? 1 : 0, page = builder.ToString() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		public virtual ActionResult TrackClick(string url, int mailMessageId, int newsletterId)
		{
			NewsletterTrackingStatistic tracking = new NewsletterTrackingStatistic();
			tracking.MailMessageID = mailMessageId;
			tracking.NewsletterID = newsletterId;
			tracking.Date = DateTime.Now;
			tracking.Save();

			return Redirect(url);
		}


		public virtual ActionResult Optout(string emailAddress)
		{
			try
			{
				if (!String.IsNullOrWhiteSpace(emailAddress))
				{
					if (emailAddress.IsValidEmail())
					{
						OptOut optOut = OptOut.Search(emailAddress);
						if (optOut == null)
						{
							OptOut optOutEmailAddress = new OptOut();
							optOutEmailAddress.EmailAddress = emailAddress;
							optOutEmailAddress.OptOutTypeID = NetSteps.Data.Entities.Constants.OptOutType.EndUser.ToShort();
							optOutEmailAddress.Save();
						}

						return View("OptOutAdded");
					}
				}
				return View();
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { success = false, error = exception.PublicMessage });
			}
		}

		public virtual ActionResult LoggedIn()
		{
			return PartialView("LoggedIn");
		}

		private void SetViewBagOpenParties()
		{
			if (OrdersSection.Instance.IsPartyOrderClient)
			{
				if (SiteOwner != null)
				{
					ViewBag.OpenParties = Party.GetOpenParties(SiteOwner.AccountID);
				}
			}
		}

	}
}
