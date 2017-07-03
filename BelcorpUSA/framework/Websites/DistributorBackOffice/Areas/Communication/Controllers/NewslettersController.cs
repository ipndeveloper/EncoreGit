using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Communication.Models.Newsletters;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Imaging;
using NetSteps.Common.Interfaces;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using Constants = NetSteps.Data.Entities.Constants;

namespace DistributorBackOffice.Areas.Communication.Controllers
{
	public class NewslettersController : BaseCommunicationController
	{
		#region Actions
		[FunctionFilter("Communications-Newsletters", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Index()
		{
			var model = new IndexModel();
			var emailCampaignActions = EmailCampaignAction.LoadFullAllDistributorVisible(Constants.CampaignType.Newsletters);
			model.Load(emailCampaignActions);

			return View(model);
		}

		[HttpPost]
		[FunctionFilter("Communications-Newsletters", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual JsonResult GetNewsletterCampaignActions(int campaignID)
		{
			try
			{
				var allNewsletterCampaignActions = EmailCampaignAction.LoadAllDistributorVisible(Constants.CampaignType.Newsletters, campaignID);
				var selectList = (from x in allNewsletterCampaignActions
								  where x.CampaignAction.CampaignID == campaignID
								  select new SelectListItem
								  {
									  Text = x.CampaignAction.Name,
									  Value = x.CampaignActionID.ToString()
								  }).ToList();

				return Json(new { result = true, selectList = selectList });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpPost]
		[FunctionFilter("Communications-Newsletters", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual PartialViewResult NewsletterInfo(int campaignActionID)
		{
			var model = new NewsletterInfoModel();
			var emailCampaignAction = CampaignAction.LoadFull(campaignActionID).EmailCampaignActions.FirstOrDefault();
			model.Load(emailCampaignAction);

			return PartialView(model);
		}

		[HttpPost]
		[FunctionFilter("Communications-Newsletters", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual PartialViewResult NewsletterContent(int campaignActionID, int languageID)
		{
			var model = new NewsletterContentModel();
			model.Load(campaignActionID, languageID);

			return PartialView(model);
		}

		[HttpPost]
		[ValidateInput(false)]
		[FunctionFilter("Communications-Newsletters", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult SaveDistributorContent(NewsletterContentModel model)
		{
			try
			{
				if (model.CampaignActionID == null
						|| model.LanguageID == null)
				{
					throw new ArgumentNullException();
				}

				if (string.IsNullOrWhiteSpace(model.DistributorContent))
				{
					DeleteTokenValue(Constants.Token.DistributorContent, model.CampaignActionID.Value, model.LanguageID.Value);
				}
				else
				{
					SaveTokenValue(Constants.Token.DistributorContent, model.CampaignActionID.Value, model.LanguageID.Value, Server.HtmlEncode(model.DistributorContent));
				}

				if (string.IsNullOrWhiteSpace(model.DistributorImageUrl))
				{
					DeleteTokenValue(Constants.Token.DistributorImage, model.CampaignActionID.Value, model.LanguageID.Value);
				}
				else
				{
					SaveTokenValue(
							Constants.Token.DistributorImage,
							model.CampaignActionID.Value,
							model.LanguageID.Value,
							model.DistributorImageUrl.RemoveWebUploadPath(Constants.Token.DistributorImage.ToString(), CurrentAccount.AccountID.ToString()));
				}

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpPost]
		[ValidateInput(false)]
		[FunctionFilter("Communications-Newsletters", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult SetupPreview(NewsletterContentModel model)
		{
			Session["NewsletterContentModel"] = model;

			return Json(new { result = true });
		}

		[FunctionFilter("Communications-Newsletters", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Preview()
		{
			NewsletterContentModel model = Session["NewsletterContentModel"] as NewsletterContentModel;
			if (model == null)
				return Content("<html><body>No preview data</body></html>", "text/html");

			if (model.CampaignActionID == null)
				return Content("<html><body>Invalid CampaignActionID</body></html>", "text/html");
			if (model.LanguageID == null)
				return Content("<html><body>Invalid LanguageID</body></html>", "text/html");

			CampaignAction campaignAction;
			try
			{
				campaignAction = CampaignAction.LoadFull(model.CampaignActionID.Value);
			}
			catch
			{
				return Content("<html><body>No campaign action</body></html>", "text/html");
			}

			EmailCampaignAction emailCampaignAction = campaignAction.EmailCampaignActions.FirstOrDefault();
			if (emailCampaignAction == null)
				return Content("<html><body>No email campaign action</body></html>", "text/html");

			var emailTemplate = EmailTemplate.LoadFull(emailCampaignAction.EmailTemplateID);
			var emailTemplateTranslation = emailTemplate.EmailTemplateTranslations.GetByLanguageID(model.LanguageID.Value);

			var tokenValueObjects = GetTokenValues(model.CampaignActionID.Value, model.LanguageID.Value);

			// There may be other distributor values in the db so get all except the ones passed in via the model
			var distributorValues = tokenValueObjects
					.Where(x =>
							x.AccountID == CurrentAccount.AccountID
							&& x.TokenID != (int)Constants.Token.DistributorContent
							&& x.TokenID != (int)Constants.Token.DistributorImage)
					.ToDictionary(x => x.Token.Placeholder, x => x.PlaceholderValue);
			// Add values passed in via the model
			if (!string.IsNullOrWhiteSpace(model.DistributorContent))
				SetTokenValue(distributorValues, Constants.Token.DistributorContent, model.DistributorContent);
			if (!string.IsNullOrWhiteSpace(model.DistributorImageUrl))
				SetTokenValue(distributorValues, Constants.Token.DistributorImage, model.DistributorImageUrl.RemoveWebUploadPath(Constants.Token.DistributorImage.ToString(), CurrentAccount.AccountID.ToString()));

			// Get default values
			var defaultValues = tokenValueObjects
					.Where(x => x.AccountID == null)
					.ToDictionary(x => x.Token.Placeholder, x => x.PlaceholderValue);

			IEnumerable<string> tokens = EmailTemplateType.LoadFull(Constants.EmailTemplateType.Newsletter.ToShort()).Tokens.Select(x => x.Placeholder);

			// Composite TVP
			var tokenValueProvider = new CompositeTokenValueProvider(new ITokenValueProvider[]
						{
								// Distributor comes first
								new DistributorCampaignActionTokenValueProvider(distributorValues, CurrentAccount.AccountID.ToString()),
								// Default comes second
								new DistributorCampaignActionTokenValueProvider(defaultValues, "Default"),
								// Fake CampaignSubscriber comes third
								new MockTokenValueProvider(tokens, model.LanguageID.ToShort())
						});

			var mailMessage = emailTemplateTranslation.GetTokenReplacedMailMessage(tokenValueProvider);
			return Content(mailMessage.HTMLBody ?? mailMessage.Body, "text/html");
		}

		[FunctionFilter("Communications-Newsletters", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult UploadPhotoControl()
		{
			PhotoCropperModel model = new PhotoCropperModel()
			{
				Content = null,
				Folder = string.Format("{0}/{1}", Constants.Token.DistributorImage, CurrentAccount.AccountID),
				Mode = "newsletter",
				OriginalHeight = 0,
				OriginalWidth = 0,
				TargetWidth = 130,
				TargetHeight = 175
			};

			return PartialView("_UploadPhoto", model);
		}

		[HttpPost]
		[FunctionFilter("Communications-Newsletters", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult SavePhoto(string coords, string imagePath)
		{
			try
			{
				var imageUrl = ImageUtilities.CropWebUpload(coords, 130, 175, imagePath).AbsoluteUploadPathToWebUploadPath();

				return Json(new { result = true, imagePath = imageUrl });
			}
			catch (Exception ex)
			{
				NetStepsException exception;
				//divide by 0 attempted
				if (ex is NetStepsApplicationException)
					exception = ex as NetStepsApplicationException;
				else
					exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region Private
		protected Dictionary<Tuple<int, int>, List<CampaignActionTokenValue>> _tokenValuesCache
				= new Dictionary<Tuple<int, int>, List<CampaignActionTokenValue>>();

		protected virtual List<CampaignActionTokenValue> GetTokenValues(int campaignActionID, int languageID)
		{
			var cacheKey = new Tuple<int, int>(campaignActionID, languageID);
			List<CampaignActionTokenValue> tokenValues;

			if (!_tokenValuesCache.ContainsKey(cacheKey))
			{
				tokenValues = CampaignActionTokenValue.LoadAllFull(new CampaignActionTokenValueSearchParameters
				{
					AccountID = CurrentAccount.AccountID,
					IncludeDefaults = true,
					CampaignActionID = campaignActionID,
					LanguageID = languageID,
				});
			}
			else
			{
				tokenValues = _tokenValuesCache[cacheKey];
			}
			return tokenValues;
		}

		protected virtual void SetTokenValue(Dictionary<string, string> tokenValues, Constants.Token token, string value)
		{
			string key = SmallCollectionCache.Instance.Tokens.GetById((int)token).Placeholder;

			if (tokenValues.ContainsKey(key))
			{
				tokenValues[key] = value;
			}
			else
			{
				tokenValues.Add(key, value);
			}
		}

		protected virtual void DeleteTokenValue(Constants.Token token, int campaignActionID, int languageID)
		{
			var value = CampaignActionTokenValue.LoadByUniqueKey(
									token,
									languageID,
									campaignActionID,
									CurrentAccount.AccountID);
			if (value != null)
				CampaignActionTokenValue.Delete(value.CampaignActionTokenValueID);
		}

		protected virtual void SaveTokenValue(Constants.Token token, int campaignActionID, int languageID, string value)
		{
			CampaignActionTokenValue.SaveTokenValue(token, languageID, campaignActionID, value, CurrentAccount.AccountID);
		}
		#endregion
	}
}
