using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Imaging;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;

namespace nsDistributor.Controllers
{
    using NetSteps.Web.Mvc.Controls.Models;

    public class AdminController : BaseController
	{
		#region Helpers
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!OwnerLoggedIn)
			{
				if (filterContext.HttpContext.Request.IsAjaxRequest())
					filterContext.Result = new JsonResult()
					{
						Data = new { result = false, message = Translation.GetTerm("MustBeLoggedIn", "You must be logged in.") },
						JsonRequestBehavior = JsonRequestBehavior.AllowGet
					};
				else
				{
					bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
					var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
					var distributor = isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1));
					filterContext.Result = new RedirectResult("~" + distributor + "/Home");
				}
				return;
			}
			base.OnActionExecuting(filterContext);
		}


		private string FormatUrl(string subdomain, string domain)
		{
			if (string.IsNullOrWhiteSpace(subdomain))
			{
				return string.Empty;
			}

			if (string.IsNullOrWhiteSpace(domain))
			{
				return string.Empty;
			}

			return string.Format("http://{0}.{1}", subdomain, domain);
		}

		#endregion

		public virtual ActionResult Index()
		{
			List<HtmlSection> editableSections = new List<HtmlSection>();
			var baseSite = CurrentSite.IsBase ? CurrentSite : SiteCache.GetSiteByID(CurrentSite.BaseSiteID.ToInt());

			foreach (HtmlSection siteSection in baseSite.HtmlSections)
			{
				if (siteSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Choices ||
					siteSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ChoicesPlus ||
					siteSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ConsultantList ||
					siteSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Override)
				{
					editableSections.Add(siteSection);
				}
			}
			foreach (var page in baseSite.Pages)
			{
				foreach (HtmlSection pageSection in page.HtmlSections)
				{
					if (pageSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Choices ||
						pageSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ChoicesPlus ||
						pageSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ConsultantList ||
						pageSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Override)
					{
						editableSections.Add(pageSection);
					}
				}
			}

			ViewBag.DisplayInfo = SiteOwner.AccountPublicContactInfo ?? new AccountPublicContactInfo();
			ViewBag.CurrentSite = CurrentSite;

			return View(editableSections);
		}

		public virtual ActionResult SaveDisplayInfo(bool hideName, string displayName, bool hideEmail, string displayEmail, bool hidePhone, string displayPhone, bool hideAddress, string displayTitle, bool hideTitle)
		{
			try
			{
				var currentSiteOwner = SiteOwner;
				var displayInfo = AccountPublicContactInfo.LoadByAccountID(currentSiteOwner.AccountID);

				if (displayInfo == null)
				{
					displayInfo = new AccountPublicContactInfo();
					displayInfo.AccountID = CoreContext.CurrentAccount.AccountID;
				}

				displayInfo.StartEntityTracking();

				
				displayInfo.HideName = hideName;
				displayInfo.Name = displayName;
				displayInfo.HideEmailAddress = hideEmail;
				displayInfo.EmailAddress = displayEmail;
				displayInfo.HidePhoneNumber = hidePhone;
				displayInfo.PhoneNumber = displayPhone;
				displayInfo.HideAddress = hideAddress;
				displayInfo.Title = displayTitle;
				displayInfo.HideTitle = hideTitle;

				displayInfo.Save();

				currentSiteOwner.AccountPublicContactInfo = displayInfo;

				SiteOwner = currentSiteOwner;

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult EditSection(int id)
		{
			HtmlSection section = HtmlSection.Load(id);

			if (section.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Override)
				return View("OverrideSection", section);
			else
				return View("ChoicesSection", section);
		}

		public virtual ActionResult PreviewContent(int contentId)
		{
			try
			{
				HtmlContent content = HtmlContent.LoadFull(contentId);
				return Json(new { result = true, preview = content.BuildContent() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult EditContent(int sectionId, int? contentId)
		{
			ViewData["ContentId"] = contentId;
		    var model = new HtmlSectionEditModel { Section = HtmlSection.Load(sectionId) };
            var allowedEditTypes = new List<int>()
			{
				(int)Constants.HtmlSectionEditType.ChoicesPlus, 
				(int)Constants.HtmlSectionEditType.ConsultantList, 
				(int)Constants.HtmlSectionEditType.Override
			};
			if (allowedEditTypes.Contains(model.Section.HtmlSectionEditTypeID))
				return View(model);
			else
				return RedirectToAction("Index");
		}

		[ValidateInput(false)]
		public virtual ActionResult SaveContent(int sectionId, int? contentId, string name, string body)
		{
			try
			{
				HtmlSection htmlSection = HtmlSection.Load(sectionId);
				string choices = null;

				if (htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Override)
				{
					var contentSite = SiteCache.GetSiteByID(CurrentSite.SiteID);
					var sectionContent = contentSite.HtmlSectionContents.OrderByDescending(hsc => hsc.HtmlContent.PublishDateUTC).FirstOrDefault(hsc => hsc.HtmlSectionID == sectionId && hsc.HtmlContent.LanguageID == NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID);
					if (sectionContent != default(HtmlSectionContent))
					{
						HtmlContent htmlContent = sectionContent.HtmlContent;
						if (htmlContent != null)
						{
							// Don't save if no changes were made - JHE
							if (htmlContent.GetBody() == body && htmlContent.Name == name)
								return Json(new { result = true });

							htmlContent.StartEntityTracking();

							htmlContent.Name = name;
							htmlContent.SetBody(body);
							htmlContent.PublishDate = DateTime.Now;
							htmlContent.Save();
						}
					}
					else
					{
						// Create new content  - JHE
						var htmlContent = new HtmlContent();
						htmlContent.StartEntityTracking();
						htmlContent.Name = name;
						htmlContent.SetBody(body);
						htmlContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Production;
						htmlContent.PublishDate = DateTime.Now;
						htmlContent.LanguageID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID;

						HtmlSectionContent htmlSectionContent = new HtmlSectionContent();
						htmlSectionContent.StartEntityTracking();
						htmlSectionContent.HtmlSectionID = htmlSection.HtmlSectionID;
						htmlSectionContent.HtmlContent = htmlContent;
						htmlSectionContent.SiteID = CurrentSite.SiteID;
						htmlSectionContent.Save();

						contentId = htmlContent.HtmlContentID;
					}
				}
				else
				{
					HtmlContent content;
					if (contentId.HasValue && contentId.Value > 0)
						content = HtmlContent.LoadFull(contentId.Value);
					else
					{
						content = new HtmlContent();
						content.StartEntityTracking();
						content.HtmlContentStatusID = Constants.HtmlContentStatus.Production.ToInt();
					}
					content.Name = name;
					content.SetBody(body);
					content.PublishDate = DateTime.Now;
					content.LanguageID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID;

					if (!contentId.HasValue)
					{
						HtmlSectionChoice choice = new HtmlSectionChoice();
						choice.StartEntityTracking();
						choice.HtmlSectionID = htmlSection.HtmlSectionID;
						choice.HtmlContent = content;
						choice.SiteID = CurrentSite.SiteID;
						choice.SortIndex = CurrentSite.GetMaxChoiceIndex(htmlSection.HtmlSectionID) + 1;
						choice.Save();

						contentId = content.HtmlContentID;
					}
					else
						content.Save();
				}

				// Re-set Content cache - JHE
				// This may not be optimal for frequent updates. - JHE
				SiteCache.ExpireSite(CurrentSite.SiteID);

				Site.UpdateSiteLastEditDate(CurrentSite.SiteID);

				if (htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ChoicesPlus ||
					htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ConsultantList)
				{
					var distributorChoices = new StringBuilder();

					foreach (var distributorChoice in CurrentSite.GetActiveChoices(sectionId))
					{
						TagBuilder builder = new TagBuilder("div");
						builder.MergeAttribute("id", "content" + distributorChoice.HtmlContentID);
						builder.AddCssClass("item");
						if (distributorChoice.HtmlContentID == htmlSection.ProductionContentForDisplay(CurrentSite).HtmlContentID)
							builder.AddCssClass("selected");

						TagBuilder title = new TagBuilder("span");
						title.AddCssClass("titlecontent");
						title.InnerHtml = distributorChoice.Name;
						builder.InnerHtml = title.ToString();

						distributorChoices.Append(builder.ToString());
					}

					choices = distributorChoices.ToString();
				}

				return Json(new { result = true, choices = choices, contentId = contentId });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult SavePhoto(int sectionId, int? contentId, string image, string coordinates)
		{
			try
			{
				HtmlSection htmlSection = HtmlSection.Load(sectionId);
				string choices = null;

				image = ImageUtilities.CropWebUpload(coordinates, htmlSection.Width.ToInt(), htmlSection.Height.ToInt(), image).AddFileUploadPathToken();

				if (htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Override)
				{
					HtmlContent htmlContent = htmlSection.ProductionContent(CurrentSite);
					if (htmlContent != null)
					{
						// Don't save if no changes were made - JHE
						if (htmlContent.GetImage().GetHtmlAttributeValue("src") == image)
							return Json(new { result = true });

						htmlContent.StartEntityTracking();

						htmlContent.SetImage(image);
						htmlContent.PublishDate = DateTime.Now;
						htmlContent.Save();
					}
					else
					{
						// Create new content  - JHE
						htmlContent = new HtmlContent();
						htmlContent.StartEntityTracking();
						htmlContent.SetImage(image);
						htmlContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Production;
						htmlContent.PublishDate = DateTime.Now;
						htmlContent.LanguageID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID;

						HtmlSectionContent htmlSectionContent = new HtmlSectionContent();
						htmlSectionContent.StartEntityTracking();
						htmlSectionContent.HtmlSectionID = htmlSection.HtmlSectionID;
						htmlSectionContent.HtmlContent = htmlContent;
						htmlSectionContent.SiteID = CurrentSite.SiteID;
						htmlSectionContent.Save();

						contentId = htmlContent.HtmlContentID;
					}
				}
				else
				{
					HtmlContent content;
					if (contentId.HasValue && contentId.Value > 0)
						content = HtmlContent.LoadFull(contentId.Value);
					else
					{
						content = new HtmlContent();
						content.StartEntityTracking();
					}
					content.SetImage(image);
					content.HtmlContentStatusID = Constants.HtmlContentStatus.Production.ToInt();
					content.PublishDate = DateTime.Now;
					content.LanguageID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID;

					if (!contentId.HasValue)
					{
						HtmlSectionChoice choice = new HtmlSectionChoice();
						choice.StartEntityTracking();
						choice.HtmlSectionID = htmlSection.HtmlSectionID;
						choice.HtmlContent = content;
						choice.SiteID = CurrentSite.SiteID;
						choice.SortIndex = CurrentSite.GetMaxChoiceIndex(htmlSection.HtmlSectionID) + 1;
						choice.Save();

						contentId = content.HtmlContentID;
					}
					else
						content.Save();
				}

				// Re-set Content cache - JHE
				// This may not be optimal for frequent updates. - JHE
				SiteCache.ExpireSite(CurrentSite.SiteID);

				Site.UpdateSiteLastEditDate(CurrentSite.SiteID);

				if (htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ChoicesPlus ||
					htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ConsultantList)
				{
					var distributorChoices = new StringBuilder();

					foreach (var distributorChoice in CurrentSite.GetActiveChoices(sectionId))
					{
						TagBuilder builder = new TagBuilder("div");
						builder.MergeAttribute("id", "content" + distributorChoice.HtmlContentID);
						builder.AddCssClass("item");
						if (distributorChoice.HtmlContentID == htmlSection.ProductionContentForDisplay(CurrentSite).HtmlContentID)
							builder.AddCssClass("selected");

						builder.InnerHtml = distributorChoice.GetImage().SetHtmlAttributeValue("width", "75");

						distributorChoices.Append(builder.ToString());
					}

					choices = distributorChoices.ToString();
				}

				return Json(new { result = true, choices = choices, contentId = contentId });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult DeleteContent(int contentId)
		{
			try
			{
				HtmlContent.Delete(contentId);

				SiteCache.ExpireSite(CurrentSite.SiteID);

				Site.UpdateSiteLastEditDate(CurrentSite.SiteID);

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult SaveSelection(int sectionId, int contentId)
		{
			try
			{
				var contentSite = SiteCache.GetSiteByID(CurrentSite.SiteID);
				var sectionContent = contentSite.HtmlSectionContents.FirstOrDefault(hsc => hsc.HtmlSectionID == sectionId && hsc.HtmlContent.LanguageID == NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID);

				if (sectionContent == default(HtmlSectionContent))
				{
					sectionContent = new HtmlSectionContent();
					sectionContent.StartEntityTracking();
					sectionContent.SiteID = CurrentSite.SiteID;
					sectionContent.HtmlSectionID = sectionId;
				}
				else
				{
					sectionContent = HtmlSectionContent.Load(sectionContent.HtmlSectionContentID);
				}
				sectionContent.HtmlContentID = contentId;
				sectionContent.Save();

				SiteCache.ExpireSite(CurrentSite.SiteID);

				Site.UpdateSiteLastEditDate(CurrentSite.SiteID);

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult UseCorporateContent(int sectionId)
		{
			try
			{
				Site contentSite = SiteCache.GetSiteByID(CurrentSite.SiteID);
				var myContentCollection = contentSite.HtmlSectionContents.OrderByDescending(hsc => hsc.HtmlContent.PublishDateUTC).Where(hsc => hsc.HtmlSectionID == sectionId && hsc.HtmlContent.LanguageID == NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID).ToArray();

				foreach (var content in myContentCollection)
				{
					content.HtmlContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Archive;
					content.HtmlContent.Save();
				}

				if (myContentCollection.Count() > 0)
				{
					SiteCache.ExpireSite(CurrentSite.SiteID);

					Site.UpdateSiteLastEditDate(CurrentSite.SiteID);
				}

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult UseMyContent(int sectionId)
		{
			try
			{
				Site contentSite = SiteCache.GetSiteByID(CurrentSite.SiteID);
				var myContentCollection = contentSite.HtmlSectionContents.OrderByDescending(hsc => hsc.HtmlContent.PublishDateUTC).Where(hsc => hsc.HtmlSectionID == sectionId && hsc.HtmlContent.LanguageID == NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID).ToArray();

				foreach (var content in myContentCollection)
				{
					content.HtmlContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Production;
					content.HtmlContent.Save();
				}

				if (myContentCollection.Count() > 0)
				{
					SiteCache.ExpireSite(CurrentSite.SiteID);

					Site.UpdateSiteLastEditDate(CurrentSite.SiteID);
				}

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult ViewMySiteName()
		{
			if (CurrentSite.PrimaryUrl != null)
			{
				ViewBag.SiteName = CurrentSite.PrimaryUrl.GetSubDomain();
				ViewBag.Domain = CurrentSite.PrimaryUrl.GetDomain();
			}
			else if(CurrentSite.SiteUrls != null && CurrentSite.SiteUrls.Count > 0)
			{
				ViewBag.SiteName = CurrentSite.SiteUrls.FirstOrDefault().GetSubDomain();
				ViewBag.Domain = CurrentSite.SiteUrls.FirstOrDefault().GetDomain();
			}
			return View(CurrentSite);
		}
		
		public virtual ActionResult CheckDomainAvailability(string siteName)
		{
			siteName = siteName.Trim();
			var domainRegex = @"^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,6}$";
			var isValidSiteName = !System.Text.RegularExpressions.Regex.IsMatch(siteName, domainRegex);
			var domain = CurrentSite.PrimaryUrl.GetDomain();
			
			bool isAvailable = isValidSiteName ? SiteUrl.IsAvailable(CurrentSite.SiteID, FormatUrl(siteName, domain)) : false;

			return CreateCheckDomainAvailabilityJsonResult(isValidSiteName, isAvailable);
		}

		public virtual ActionResult ChangeSiteName(string newSiteName)
		{
			if (!OwnerLoggedIn) return CreateChangeSiteNameJsonResult(false, "/Home");

			string siteUrl = String.Format("http://{0}.{1}/", newSiteName, CurrentSite.PrimaryUrl.GetDomain());

            SiteUrl siteUrlEntity = CurrentSite.PrimaryUrl; 
			siteUrlEntity.StartEntityTracking();
			siteUrlEntity.IsPrimaryUrl = true;
			siteUrlEntity.Url = siteUrl;
			siteUrlEntity.Save();
			siteUrlEntity.StopEntityTracking();
			SiteCache.ExpireSite(CurrentSite.SiteID);
			string successUrl = CreateSsoUrlToSite(siteUrl);
			return CreateChangeSiteNameJsonResult(true, successUrl);
		}

		protected JsonResult CreateCheckDomainAvailabilityJsonResult(bool isValidSiteName, bool isAvailable)
		{
			return Json(new
						{
							available = isAvailable,
							valid = isValidSiteName
						});
		}

		protected JsonResult CreateChangeSiteNameJsonResult(bool wasSuccessful, string successOrFailUrl)
		{
			return Json(new
			{
				success = wasSuccessful,
				successUrl = wasSuccessful ? successOrFailUrl : null,
				failUrl = !wasSuccessful ? successOrFailUrl : null
			});
		}

		protected string CreateSsoUrlToSite(string siteUrl)
		{
			var token = Account.GetSingleSignOnToken(SiteOwner.AccountID);
			var editModeUrl = System.Uri.EscapeUriString("/Admin");
			siteUrl = siteUrl.EldEncode();
			var successUrl = string.Format("{0}{1}Login?token={2}&returnUrl={3}", siteUrl, siteUrl.EndsWith("/") ? "" : "/", token, editModeUrl);
			return successUrl;
		}
	}
}
