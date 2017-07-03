using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Base;
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
using NetSteps.Web.Mvc.Controls.Models;

namespace NetSteps.Web.Mvc.Controls.Controllers
{
	using NetSteps.Data.Entities.Generated;
	using NetSteps.Web.Validation;

	public class EditController : Controller
	{
		private const int x = 0, y = 1, x2 = 2, y2 = 3, w = 4, h = 5;

		#region Helpers
		public new JsonResult Json(object data)
		{
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		protected string RenderPartialToString(string viewName, object model = null)
		{
			if (string.IsNullOrEmpty(viewName))
				viewName = ControllerContext.RouteData.GetRequiredString("action");

			ViewData.Model = model;

			using (StringWriter sw = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
				ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
				viewResult.View.Render(viewContext, sw);

				return sw.GetStringBuilder().ToString();
			}
		}

		private static Action<Controller> _setViewData = null;
		public static Action<Controller> SetViewData
		{
			get { return _setViewData; }
			set { _setViewData = value; }
		}

		private static Func<bool> _loggedIn = null;
		public static Func<bool> LoggedIn
		{
			get { return _loggedIn; }
			set { _loggedIn = value; }
		}

		private static Func<HttpContextBase, ActionResult> _unauthorizedAction = null;
		public static Func<HttpContextBase, ActionResult> UnauthorizedAction
		{
			get { return _unauthorizedAction; }
			set { _unauthorizedAction = value; }
		}

		internal class EditAuthorizeAttribute : AuthorizeAttribute
		{
			protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
			{
				if (LoggedIn != null)
					return LoggedIn();
				return base.AuthorizeCore(httpContext);
			}

			public override void OnAuthorization(AuthorizationContext filterContext)
			{
				base.OnAuthorization(filterContext);
				if (filterContext.Result is HttpUnauthorizedResult && UnauthorizedAction != null)
				{
					filterContext.Result = UnauthorizedAction(filterContext.RequestContext.HttpContext);
				}
			}
		}

		private static Func<bool> _corporateLoggedIn = null;
		public static Func<bool> CorporateLoggedIn
		{
			get { return _corporateLoggedIn; }
			set { _corporateLoggedIn = value; }
		}

		private static Func<HttpContextBase, ActionResult> _corporateUnauthorizedAction = null;
		public static Func<HttpContextBase, ActionResult> CorporateUnauthorizedAction
		{
			get { return _corporateUnauthorizedAction; }
			set { _corporateUnauthorizedAction = value; }
		}

		internal class CorporateEditAuthorizeAttribute : AuthorizeAttribute
		{
			protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
			{
				if (CorporateLoggedIn != null)
					return CorporateLoggedIn();
				return base.AuthorizeCore(httpContext);
			}

			public override void OnAuthorization(AuthorizationContext filterContext)
			{
				base.OnAuthorization(filterContext);
				if (filterContext.Result is HttpUnauthorizedResult && CorporateUnauthorizedAction != null)
				{
					filterContext.Result = CorporateUnauthorizedAction(filterContext.RequestContext.HttpContext);
				}
			}
		}

		private static Func<string, ActionResult> _verifyEditModeAction = null;
		public static Func<string, ActionResult> VerifyEditModeAction
		{
			get { return _verifyEditModeAction; }
			set { _verifyEditModeAction = value; }
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.Result is ViewResult && _setViewData != null)
			{
				_setViewData(this);
			}
			base.OnActionExecuted(filterContext);
		}

		public static IEnumerable<CMSMessage> GetMessages()
		{
			List<CMSMessage> messages = new List<CMSMessage>();

			var baseSite = CurrentSite.IsBase ? CurrentSite : SiteCache.GetSiteByID(CurrentSite.BaseSiteID.ToInt());
			var baseSiteContent = CurrentSite.IsBase ? SiteCache.GetSiteByID(CurrentSite.SiteID) : SiteCache.GetSiteByID(CurrentSite.BaseSiteID.ToInt());

			if (NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUser.HasFunction("CMS-Content Approving"))
			{
				foreach (var htmlSectionContent in baseSiteContent.HtmlSectionContents.Where(hsc => hsc.HtmlContent.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Submitted))
				{
					HtmlSection section = null;
					Page page = null;
					if (baseSite.HtmlSections.Any(hs => hs.HtmlSectionID == htmlSectionContent.HtmlSectionID))
						section = baseSite.HtmlSections.First(hs => hs.HtmlSectionID == htmlSectionContent.HtmlSectionID);
					else
					{
						foreach (var p in baseSite.Pages)
						{
							if (p.HtmlSections.Any(hs => hs.HtmlSectionID == htmlSectionContent.HtmlSectionID))
							{
								page = p;
								section = p.HtmlSections.First(hs => hs.HtmlSectionID == htmlSectionContent.HtmlSectionID);
								break;
							}
						}
					}
					messages.Add(new CMSMessage()
					{
						FromUserID = htmlSectionContent.HtmlContent.CreatedByUserID,
						Message = "There is new content awaiting approval" + (section != null ? " for section \"" + section.SectionName + "\"" : ""),
						Date = htmlSectionContent.HtmlContent.PublishDate,
						HtmlSectionID = htmlSectionContent.HtmlSectionID,
						Environment = CMSEnvironment.PendingApproval,
						ReturnUrl = page == null ? (System.Web.HttpContext.Current.Request.UrlReferrer ?? System.Web.HttpContext.Current.Request.Url).PathAndQuery : page.Url.ResolveUrl()
					});
				}
			}

			foreach (var message in HtmlContentHistory.GetUnseenHistoryForUser(CurrentSite.SiteID, NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUserID))
			{
				messages.Add(new CMSMessage()
				{
					FromUserID = message.UserID,
					Message = message.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Pushed ? "Content pushed from site " + message.Comments : (message.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Disapproved ? "Your content was disapproved because: " + message.Comments : message.Comments),
					Date = message.HistoryDate,
					HtmlSectionID = baseSiteContent.HtmlSectionContents.FirstOrDefault(hsc => hsc.HtmlContentID == message.HtmlContentID).HtmlSectionID,
					Environment = message.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Pushed ? CMSEnvironment.Pushed : CMSEnvironment.WorkingDraft,
					ReturnUrl = (System.Web.HttpContext.Current.Request.UrlReferrer ?? System.Web.HttpContext.Current.Request.Url).PathAndQuery,
					HtmlContentHistoryID = message.HtmlContentHistoryID
				});
			}

			return messages.OrderByDescending(m => m.Date);
		}
		#endregion

		public static Site CurrentSite
		{
			get
			{
				string currentSiteUrl = "http://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath;

				var site = SiteCache.GetSiteByUrl(currentSiteUrl.EldDecode());

				return site;
			}
		}

		#region Designer Methods
		public void LoadDesigner()
		{
			Session["InDesignerMode"] = true;
		}

		public void ChangeSkin(string skin)
		{
			Session["SelectedSkinName"] = skin;
			Session["DesignerModeSkin"] = "<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ("~/Content/Styles/Skin-" + skin + ".css").ResolveUrl() + "\" />";
		}

		public void ChangeLayout(string layout)
		{
			Session["DesignerModeLayout"] = layout;
		}

		public void SaveTheme(string layout)
		{
			//If they chose a different skin, save it
			if (Session["SelectedSkinName"] != null)
			{
				CurrentSite.SaveSiteSetting("SelectedSkinName", Session["SelectedSkinName"].ToString());
			}

			//Save the layout

			CurrentSite.SaveSiteSetting("Layout", layout);

			CancelTheme();
		}

		public void CancelTheme()
		{
			Session["SelectedSkinName"] = null;
			Session["DesignerModeSkin"] = null;
			Session["DesignerModeLayout"] = null;
			Session["InDesignerMode"] = null;
		}
		#endregion

		#region Content Approval Message Methods
		public ActionResult GetCMSMessages(int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection)
		{
			try
			{
				var messages = GetMessages();
				var builder = new StringBuilder();
				int i = 0;

				foreach (var message in (orderByDirection == Common.Constants.SortDirection.Ascending ? messages.OrderBy(m => m.Date) : messages).Skip(page * pageSize).Take(pageSize))
				{
					builder.Append("<tr>")
						.AppendCell(message.FromUserID.HasValue ? CachedData.GetUser(message.FromUserID.ToInt()).Username : Translation.GetTerm("N/A"))
						.AppendCell(message.Date.ToStringDisplay(SmallCollectionCache.Instance.Languages.GetById(NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID).Culture))
						.AppendCell(message.Message)
						.AppendLinkCell("~/Edit/CorporateEdit?sectionId=" + message.HtmlSectionID + "&returnUrl=" + Server.UrlEncode(message.ReturnUrl) + (message.HtmlContentHistoryID.HasValue ? "&historyId=" + message.HtmlContentHistoryID : "") + "&environment=" + message.Environment, Translation.GetTerm("View"))
						.AppendCell("</tr>");
					++i;
				}
				return Json(new { result = true, totalPages = Math.Ceiling(messages.Count() / (double)pageSize), page = builder.ToString() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public ActionResult ReadMessage(int id)
		{
			throw new NotImplementedException("Not finished porting yet."); // from Scentsy
			//HtmlContentHistory.ReadMessage(id);
			//return RedirectToAction("GetUnseenMessages");
		}
		#endregion

		#region Photo Methods
		public ActionResult UploadPhoto(string folder)
		{
			folder = folder.Replace("_", " ");
			string cleanedFileName = string.Empty;
			string fileName = string.Empty;

			bool nonHtml5Browser = Request.Files.Count > 0;

			if (nonHtml5Browser)
				fileName = Request.Files[0].FileName;
			else if (Request.ContentType.Equals("application/octet-stream", StringComparison.InvariantCultureIgnoreCase))
				fileName = Request.Params["qqfile"];
			else
				return Json(new { success = false, error = "No files uploaded." });

			//Save the file on the server
			cleanedFileName = Path.GetFileName(fileName);
			cleanedFileName = System.Text.RegularExpressions.Regex.Replace(cleanedFileName, @"[^a-zA-Z0-9\s_\.]", "");
			string fullPath = ConfigurationManager.GetAbsoluteFolder(folder) + cleanedFileName;

			if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
				Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

			if (nonHtml5Browser)
				Request.Files[0].SaveAs(fullPath);
			else if (Request.ContentType.Equals("application/octet-stream", StringComparison.InvariantCultureIgnoreCase))
				Request.SaveAs(fullPath, false);

			using (var image = System.Drawing.Image.FromFile(fullPath))
			{
				var json = new { success = true, fileName = ConfigurationManager.GetWebFolder(folder) + Path.GetFileName(cleanedFileName), originalWidth = image.Width, originalHeight = image.Height };
				if (nonHtml5Browser)
					return Content(json.ToJSON(), "text/html");
				return this.Json(json);
			}
		}

		public string CropPhoto(string selectedCoords, int width, int height, string imagePath)
		{
			imagePath = imagePath.WebUploadPathToAbsoluteUploadPath();

			string[] coords = selectedCoords.Split(new char[] { ',' });
			var image = System.Drawing.Image.FromFile(imagePath);

			// Resize
			decimal rx, ry;
			int originalImageWidth, originalImageHeight;
			Image resizedImage;
			try
			{
				rx = width / (coords[w].ToDecimal() * (width / (decimal)image.Width));
				ry = height / (coords[h].ToDecimal() * (height / (decimal)image.Height));
				Size newSize = new Size((int)Math.Round((rx * width), 0), (int)Math.Round((ry * height), 0));
				resizedImage = ImageUtilities.ResizeImage(image, newSize);
				originalImageWidth = image.Width;
				originalImageHeight = image.Height;
			}
			finally
			{
				image.Dispose();
			}

			// Crop
			Image final;
			try
			{
				Rectangle cropArea = new Rectangle((int)Math.Round(rx * (width / (decimal)originalImageWidth * coords[x].ToDecimal()), 0),
					(int)Math.Round(ry * ((height / (decimal)originalImageHeight) * coords[y].ToDecimal()), 0),
					width, height);
				final = ImageUtilities.CropImage(resizedImage, cropArea);
			}
			finally
			{
				resizedImage.Dispose();
			}

			string newImagePath;
			try
			{
				newImagePath = Regex.IsMatch(imagePath, @"_\w{32}") ? Regex.Replace(imagePath, @"_\w{32}", "_" + Guid.NewGuid().ToString("N")) : imagePath.Insert(imagePath.LastIndexOf('.'), "_" + Guid.NewGuid().ToString("N"));
				final.Save(newImagePath);
			}
			finally
			{
				final.Dispose();
			}

			try
			{
				// Clean up the file after we've finished with it to save server space
				System.IO.File.Delete(imagePath);
			}
			catch { }

			return newImagePath;
		}
		#endregion

		#region Corporate Content Editing Methods
		[CorporateEditAuthorize]
		public ActionResult CorporateEdit(int sectionId, int? pageId, string returnUrl, int? historyId)
		{
			HtmlSection htmlSection = HtmlSection.GetById(sectionId);
			ViewBag.SiteDesignContent = CurrentSite.GetHtmlSectionByName("SiteDesignContent");
			ViewBag.MessageCount = GetMessagesCount();

			if (historyId.HasValue)
			{
				HtmlContentHistory history = HtmlContentHistory.Load(historyId.Value);
				history.MessageSeen = true;
				history.Save();
			}

			if (_verifyEditModeAction != null)
			{
				var result = _verifyEditModeAction(returnUrl);
				if (result != null)
					return result;
			}

			var model = new HtmlSectionEditModel();
			model.LoadModel(htmlSection, ApplicationContext.Instance.CurrentUser, CurrentSite, pageId);

			if (htmlSection.HtmlSectionEditTypeID == (int)ConstantsGenerated.HtmlSectionEditType.Choices
				|| htmlSection.HtmlSectionEditTypeID == (int)ConstantsGenerated.HtmlSectionEditType.ChoicesPlus)
			{
				return View("Choices", model);
			}

			return this.View("Content", model);
		}

		[CorporateEditAuthorize]
		[ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post)]
		[OutputCache(CacheProfile = "DontCache")]
		public ActionResult SaveCorporateContent(int sectionId, int? contentId, Constants.HtmlContentStatus status, string name, string body,
			DateTime? publishDate = null, DateTime? publishTime = null, bool defaultChoice = false)
		{
			try
			{
				IEnumerable<IHTMLParseError> errors = null;
				IHTMLValidator validator = new HTMLValidator();
				var isValidHtml = validator.IsValid(body, out errors);
				if (!isValidHtml)
				{
					StringBuilder validatorErrors = new StringBuilder();
					foreach (var error in errors)
					{
						validatorErrors.AppendLine(error.Reason);
					}

					throw new HTMLValidationException(validatorErrors.ToString());
				}

				HtmlSection htmlSection = HtmlSection.GetById(sectionId);
				string choices = null;

				if (htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.CorporateOnly ||
					htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Override)
				{
					HtmlContent htmlContent = htmlSection.GetContentUncached(CurrentSite, status, ApplicationContext.Instance.CurrentLanguageID);
					if (htmlContent != null && status != Constants.HtmlContentStatus.Submitted)
					{
						DateTime? newPublishDate = DateTime.Now;
						if (publishDate != null)
							newPublishDate = publishDate.AddTime(publishTime);

						// Don't save if no changes were made - JHE
						if (htmlContent.GetBody() == body && htmlContent.Name == name && publishDate == null)
							return Json(new { result = true });

						htmlContent.StartEntityTracking();

						var reloadedProductionContent = HtmlContent.Load(htmlContent.HtmlContentID);
						if (status == Constants.HtmlContentStatus.Production)
						{
							htmlContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Archive; // Archive the old content - JHE
							reloadedProductionContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Archive;
						}
						else
						{
							htmlContent.Name = name;
							htmlContent.SetBody(body);
							htmlContent.PublishDate = newPublishDate;
							reloadedProductionContent.Name = name;
							reloadedProductionContent.SetBody(body);
							reloadedProductionContent.PublishDate = newPublishDate;
						}
						reloadedProductionContent.Save();

						if (status != Constants.HtmlContentStatus.Production)
							contentId = htmlContent.HtmlContentID;
					}

					if (htmlContent == null || status == Constants.HtmlContentStatus.Production || status == Constants.HtmlContentStatus.Submitted)
					{
						// Create new content  - JHE
						htmlContent = new HtmlContent();
						htmlContent.StartEntityTracking();
						htmlContent.Name = name;
						htmlContent.SetBody(body);
						htmlContent.HtmlContentStatusID = status.ToInt();
						htmlContent.PublishDate = publishDate.AddTime(publishTime) ?? DateTime.Now;
						htmlContent.LanguageID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID;

						if (status == Constants.HtmlContentStatus.Submitted)
							htmlContent.CreatedByUserID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUserID;

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
					{
						content = HtmlContent.LoadFull(contentId.Value);
					}
					else
					{
						content = new HtmlContent();
						content.StartEntityTracking();

						if (status == Constants.HtmlContentStatus.Submitted)
							content.CreatedByUserID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUserID;
					}
					if (htmlSection.SectionName == "SiteDesignContent")
					{
						content.Name = name;
						content.SetHead(body);
						content.HtmlContentStatusID = Constants.HtmlContentStatus.Production.ToInt();
						content.PublishDate = publishDate.AddTime(publishTime) ?? DateTime.Now;
						content.LanguageID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID;
					}
					else
					{
						content.Name = name;
						content.SetBody(body);
						content.HtmlContentStatusID = Constants.HtmlContentStatus.Production.ToInt();
						content.PublishDate = publishDate.AddTime(publishTime) ?? DateTime.Now;
						content.LanguageID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID;
					}

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

					var allChoices = CurrentSite.GetAllChoices(htmlSection.HtmlSectionID);
					if (allChoices == null || allChoices.Count == 0 || defaultChoice)
					{
						HtmlSectionContent htmlSectionContent =
							HtmlSectionContent.LoadByHtmlSectionIdAndHtmlContentId(htmlSection.HtmlSectionID,
																				   content.HtmlContentID);
						if (htmlSectionContent == null)
							htmlSectionContent = new HtmlSectionContent();
						htmlSectionContent.StartEntityTracking();
						htmlSectionContent.HtmlSectionID = htmlSection.HtmlSectionID;
						htmlSectionContent.HtmlContentID = content.HtmlContentID;
						htmlSectionContent.SiteID = CurrentSite.SiteID;
						htmlSectionContent.Save();
					}
				}

				// Re-set Content cache - JHE
				// This may not be optimal for frequent updates. - JHE
				SiteCache.ExpireSite(CurrentSite.SiteID);

				Site.UpdateSiteLastEditDate(CurrentSite.SiteID);

				if (htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Choices ||
					htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ChoicesPlus)
				{
					choices = Regex.Replace(RenderPartialToString("ChoicesList", htmlSection), "[\\r\\n\\t]", string.Empty);
				}

				return Json(new { result = true, choices = choices, contentId = contentId });
			}
			catch (HTMLValidationException validationException)
			{
				return Json(new { result = false, message = validationException.Message.HtmlEncode() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[CorporateEditAuthorize]
		public ActionResult SaveCorporatePhoto(int sectionId, int? contentId, Constants.HtmlContentStatus status, string image, string coordinates,
			DateTime? publishDate = null, DateTime? publishTime = null, bool defaultChoice = false)
		{
			try
			{
				HtmlSection htmlSection = HtmlSection.Load(sectionId);
				string choices = null;

				image = (string.IsNullOrEmpty(coordinates) ? image : CropPhoto(coordinates, htmlSection.Width.ToInt(), htmlSection.Height.ToInt(), image)).AddFileUploadPathToken();

				if (htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.CorporateOnly ||
					htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Override)
				{
					HtmlContent htmlContent = htmlSection.ContentByStatus(CurrentSite, status);
					if (htmlContent != null)
					{
						DateTime? newPublishDate = DateTime.Now;
						if (publishDate != null)
							newPublishDate = publishDate.AddTime(publishTime);

						// Don't save if no changes were made - JHE
						if (htmlContent.GetImage().GetHtmlAttributeValue("src") == image && htmlContent.PublishDate == newPublishDate)
							return Json(new { result = true });

						htmlContent.StartEntityTracking();

						if (status == NetSteps.Data.Entities.Generated.ConstantsGenerated.HtmlContentStatus.Production)
						{
							htmlContent.HtmlContentStatusID = Constants.HtmlContentStatus.Archive.ToInt(); // Archive the old content - JHE
						}
						else
						{
							htmlContent.SetImage(image);
							htmlContent.PublishDate = newPublishDate;
						}
						htmlContent.Save();
					}

					if (htmlContent == null || status == NetSteps.Data.Entities.Generated.ConstantsGenerated.HtmlContentStatus.Production)
					{
						// Create new content  - JHE
						htmlContent = new HtmlContent();
						htmlContent.StartEntityTracking();
						htmlContent.SetImage(image);
						htmlContent.HtmlContentStatusID = status.ToInt();
						htmlContent.PublishDate = publishDate.AddTime(publishTime) ?? DateTime.Now;
						htmlContent.LanguageID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID;

						if (status == Constants.HtmlContentStatus.Submitted)
							htmlContent.CreatedByUserID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUserID;

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

						if (status == Constants.HtmlContentStatus.Submitted)
							content.CreatedByUserID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUserID;
					}
					content.SetImage(image);
					content.HtmlContentStatusID = Constants.HtmlContentStatus.Production.ToInt();
					content.PublishDate = publishDate.AddTime(publishTime) ?? DateTime.Now;
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

						contentId = choice.HtmlContent.HtmlContentID;
					}
					else
						content.Save();

					var allChoices = CurrentSite.GetAllChoices(htmlSection.HtmlSectionID);
					if (allChoices == null || allChoices.Count == 0 || defaultChoice)
					{
						HtmlSectionContent htmlSectionContent = SiteCache.GetSiteByID(CurrentSite.SiteID).HtmlSectionContents.FirstOrDefault(hsc => hsc.HtmlSectionID == sectionId && hsc.HtmlContent.LanguageID == NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID);
						if (htmlSectionContent == null)
							htmlSectionContent = new HtmlSectionContent();
						htmlSectionContent.StartEntityTracking();
						htmlSectionContent.HtmlSectionID = htmlSection.HtmlSectionID;
						htmlSectionContent.HtmlContentID = content.HtmlContentID;
						htmlSectionContent.SiteID = CurrentSite.SiteID;
						htmlSectionContent.Save();
					}
				}

				// Re-set Content cache - JHE
				// This may not be optimal for frequent updates. - JHE
				SiteCache.ExpireSite(CurrentSite.SiteID);

				Site.UpdateSiteLastEditDate(CurrentSite.SiteID);

				if (htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Choices ||
					htmlSection.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ChoicesPlus)
				{
					choices = Regex.Replace(RenderPartialToString("ChoicesList", htmlSection), "[\\r\\n\\t]", string.Empty);
				}

				return Json(new { result = true, choices = choices, contentId = contentId, croppedFilePath = image.ReplaceFileUploadPathToken() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		#region Archives
		[CorporateEditAuthorize]
		[OutputCache(CacheProfile = "DontCache")]
		public ActionResult GetPreviousContent(int sectionId, int currentContentId, Constants.HtmlContentStatus status)
		{
			try
			{
				var contentHistory = new OrderedList<HtmlContent>(CurrentSite.AllContentByStatus(sectionId, status).OrderBy(hc => hc.PublishDateUTC));
				contentHistory.CurrentItem = contentHistory.FirstOrDefault(c => c.HtmlContentID == currentContentId);
				if (contentHistory.HasPreviousItem)
				{
					contentHistory.CurrentItem = contentHistory.PreviousItem;

					return Json(new
					{
						result = true,
						contentId = contentHistory.CurrentItem.HtmlContentID,
						preview = contentHistory.CurrentItem.BuildContent(),
						hasPrevious = contentHistory.HasPreviousItem,
						hasNext = contentHistory.HasNextItem,
						publishedOn = contentHistory.CurrentItem.PublishDate.ToStringDisplay(SmallCollectionCache.Instance.Languages.GetById(NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID).Culture)
					});
				}
				return Json(new { result = false, message = Translation.GetTerm("NoPreviousContent", "There is no previous content.") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[CorporateEditAuthorize]
		[OutputCache(CacheProfile = "DontCache")]
		public ActionResult GetNextContent(int sectionId, int currentContentId, Constants.HtmlContentStatus status)
		{
			try
			{
				var contentHistory = new OrderedList<HtmlContent>(CurrentSite.AllContentByStatus(sectionId, status).OrderBy(hc => hc.PublishDateUTC));
				contentHistory.CurrentItem = contentHistory.FirstOrDefault(c => c.HtmlContentID == currentContentId);
				if (contentHistory.HasNextItem)
				{
					contentHistory.CurrentItem = contentHistory.NextItem;

					return Json(new
					{
						result = true,
						contentId = contentHistory.CurrentItem.HtmlContentID,
						preview = contentHistory.CurrentItem.BuildContent(),
						hasPrevious = contentHistory.HasPreviousItem,
						hasNext = contentHistory.HasNextItem,
						publishedOn = contentHistory.CurrentItem.PublishDate.ToStringDisplay(SmallCollectionCache.Instance.Languages.GetById(NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID).Culture)
					});
				}
				return Json(new { result = false, message = Translation.GetTerm("NoNewerContent", "There is no newer content.") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[CorporateEditAuthorize]
		[OutputCache(CacheProfile = "DontCache")]
		public ActionResult GetContentPreview(int sectionId, int contentId, Constants.HtmlContentStatus status)
		{
			try
			{
				var contentHistory = new OrderedList<HtmlContent>(CurrentSite.AllContentByStatus(sectionId, status).OrderBy(hc => hc.PublishDateUTC));
				contentHistory.CurrentItem = contentHistory.FirstOrDefault(c => c.HtmlContentID == contentId);

				var culture = SmallCollectionCache.Instance.Languages.GetById(NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID).Culture;

				string title = "";

				switch (status)
				{
					case Constants.HtmlContentStatus.Archive:
						title = contentHistory.CurrentItem.PublishDate.ToStringDisplay(culture);
						break;
					case Constants.HtmlContentStatus.Pushed:
						var history = HtmlContentHistory.GetHistoryForContent(contentId);
						if (history.Count > 0)
							title = history.First().Comments + " - " + history.First().HistoryDate.ToStringDisplay(culture);
						else
							title = contentHistory.CurrentItem.PublishDate.ToStringDisplay(culture);
						break;
					case Constants.HtmlContentStatus.Submitted:

						string username = Translation.GetTerm("N/A");

						if (contentHistory.CurrentItem.CreatedByUserID.HasValue && contentHistory.CurrentItem.CreatedByUserID.Value > 0)
						{
							username = CachedData.GetUser(contentHistory.CurrentItem.CreatedByUserID.ToInt()).Username;
						}

						title = username + " - " + contentHistory.CurrentItem.PublishDate.ToStringDisplay(culture);
						break;
				}

				return Json(new
				{
					result = true,
					contentId = contentHistory.CurrentItem.HtmlContentID,
					preview = contentHistory.CurrentItem.BuildContent(),
					hasPrevious = contentHistory.HasPreviousItem,
					hasNext = contentHistory.HasNextItem,
					title = title
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[CorporateEditAuthorize]
		public ActionResult CopyContent(int htmlSectionId, int contentId, Constants.HtmlContentStatus status, DateTime? publishDate, DateTime? publishTime)
		{
			try
			{
				HtmlContent htmlContent = CurrentSite.ContentByStatus(htmlSectionId, status);
				if (htmlContent != null)
				{
					htmlContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Archive;
					htmlContent.Save();
				}

				var content = HtmlContent.Load(contentId);
				bool isWorkingDraft = content.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Submitted;

				content.HtmlContentStatusID = (int)status;
				content.PublishDate = publishDate.AddTime(publishTime) ?? DateTime.Now;
				//Set this as this person's working draft - DES
				if (status == Constants.HtmlContentStatus.Submitted)
					content.CreatedByUserID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUserID;
				content.Save();

				//Tell the user who submitted this that it was approved - DES
				if (isWorkingDraft)
				{
					HtmlContentHistory history = new HtmlContentHistory();
					history.UserID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUserID;
					history.HistoryDate = DateTime.Now;
					history.Comments = Translation.GetTerm("YourWorkingDraftApproved", "Your working draft has been approved.");
					history.HtmlContentStatusID = (int)status;
					history.HtmlContentID = content.HtmlContentID;
					history.MessageSeen = false;
					history.Save();
				}

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

		[CorporateEditAuthorize]
		public ActionResult DenyContent(int contentId, string reason)
		{
			try
			{
				var content = HtmlContent.Load(contentId);
				content.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Disapproved;
				content.Save();

				//Tell the user who submitted this that it was denied and why - DES
				HtmlContentHistory history = new HtmlContentHistory();
				history.HistoryDate = DateTime.Now;
				history.HtmlContentID = contentId;
				history.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Disapproved;
				history.UserID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUserID;
				history.Comments = reason;
				history.MessageSeen = false;
				history.Save();

				// Re-set Content cache - JHE
				// This may not be optimal for frequent updates. - JHE
				SiteCache.ExpireSite(CurrentSite.SiteID);

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region Choices
		public ActionResult GetContent(int contentId)
		{
			try
			{
				HtmlContent content = HtmlContent.LoadFull(contentId);
				string contentBody = content == null ? String.Empty : content.GetBody() ?? String.Empty;
				return Json(new { result = true, body = (!string.IsNullOrEmpty(contentBody) ? contentBody : content.GetHead()) });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public ActionResult SelectChoice(int sectionId, int contentId)
		{
			try
			{
				var contentSite = SiteCache.GetSiteByID(CurrentSite.SiteID);
				HtmlSectionContent selectedChoice = contentSite.HtmlSectionContents.FirstOrDefault(hsc => hsc.HtmlSectionID == sectionId && hsc.HtmlContent.LanguageID == NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID);

				if (selectedChoice == default(HtmlSectionContent))
				{
					selectedChoice = new HtmlSectionContent();
					selectedChoice.StartEntityTracking();
					selectedChoice.SiteID = CurrentSite.SiteID;
					selectedChoice.HtmlSectionID = sectionId;
				}
				selectedChoice.HtmlContentID = contentId;

				selectedChoice.Save();

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

		[CorporateEditAuthorize]
		public ActionResult DeleteChoice(int sectionId, int choiceId)
		{
			try
			{
				HtmlSection section = HtmlSection.Load(sectionId);

				HtmlSectionChoice choice = HtmlSectionChoice.LoadFull(choiceId);
				choice.HtmlContent.HtmlSectionContents.RemoveAllAndMarkAsDeleted();
				choice.HtmlContent.HtmlElements.RemoveAllAndMarkAsDeleted();
				if (choice.HtmlContent.ChangeTracker.State != ObjectState.Added)
					choice.HtmlContent.MarkAsDeleted();
				choice.Delete();

				SiteCache.ExpireSite(CurrentSite.SiteID);

				Site.UpdateSiteLastEditDate(CurrentSite.SiteID);

				return Json(new { result = true, choices = Regex.Replace(RenderPartialToString("ChoicesList", section), "[\\r\\n\\t]", string.Empty) });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[CorporateEditAuthorize]
		public ActionResult ChoicesSort(int sectionId)
		{
			return PartialView(HtmlSection.Load(sectionId));
		}

		[CorporateEditAuthorize]
		public ActionResult SaveChoicesSort(int sectionId, List<int> choices)
		{
			try
			{
				var allChoices = HtmlSectionChoice.LoadBatch(choices);
				for (int i = 0; i < choices.Count; i++)
				{
					var choice = allChoices.First(c => c.HtmlSectionChoiceID == choices[i]);
					if (choice.SortIndex != i)
					{
						choice.SortIndex = i;
						choice.Save();
					}
				}

				SiteCache.ExpireSite(CurrentSite.SiteID);

				Site.UpdateSiteLastEditDate(CurrentSite.SiteID);

				HtmlSection htmlSection = HtmlSection.Load(sectionId);

				return Json(new { result = true, choices = Regex.Replace(RenderPartialToString("ChoicesList", htmlSection), "[\\r\\n\\t]", string.Empty) });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		[ValidateInput(false)]
		[CorporateEditAuthorize]
		[HttpPost]
		public ActionResult SetPreview(int sectionId, string body)
		{
			try
			{
				var content = new HtmlContent();
				content.SetBody(body);
				Session["PreviewContent"] = new Tuple<int, HtmlContent>(sectionId, content);
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region Media Uploader Methods
		public ActionResult UploadFile()
		{
			string cleanedFileName = string.Empty;
			string fileName = string.Empty;

			bool nonHtml5Browser = Request.Files.Count > 0;

			if (nonHtml5Browser)
				fileName = Request.Files[0].FileName;
			else if (Request.ContentType.Equals("application/octet-stream", StringComparison.InvariantCultureIgnoreCase))
				fileName = Request.Params["qqfile"];
			else
				return Json(new { success = false, error = "No files uploaded." });

			//Save the file on the server
			cleanedFileName = System.Text.RegularExpressions.Regex.Replace(fileName, @"[^a-zA-Z0-9\s_\.]", "");
			string folder = string.Empty;
			switch (cleanedFileName.GetFileType())
			{
				case Common.Constants.FileType.Image:
					folder = "Images";
					break;
				case Common.Constants.FileType.Video:
					folder = "Videos";
					break;
				case Common.Constants.FileType.Flash:
					folder = "Flash";
					break;
				default:
					folder = "Documents";
					break;
			}
			string fullPath = ConfigurationManager.GetAbsoluteFolder("CMS") + folder.AppendBackSlash() + Path.GetFileName(cleanedFileName);

			if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
				Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

			if (nonHtml5Browser)
				Request.Files[0].SaveAs(fullPath);
			else if (Request.ContentType.Equals("application/octet-stream", StringComparison.InvariantCultureIgnoreCase))
				Request.SaveAs(fullPath, false);

			var json = new { success = true, folder = folder.ToLower(), uploaded = DateTime.Now.ToString(), filePath = fullPath.AbsoluteUploadPathToWebUploadPath() };
			if (nonHtml5Browser)
				return Content(json.ToJSON(), "text/html");
			return this.Json(json);
		}

		public ActionResult DeleteFile(string path)
		{
			try
			{
				System.IO.File.Delete(path.WebUploadPathToAbsoluteUploadPath());
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		public ActionResult SetContentDate(DateTime date)
		{
			try
			{
				NetSteps.Data.Entities.ApplicationContext.Instance.DateTimeNow = date;
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public void KeepAlive()
		{
			Session["KeepAlive"] = DateTime.Now;
		}

		[CorporateEditAuthorize]
		public ActionResult PhotoEdit(int sectionId, int? contentId)
		{
			ViewBag.SiteDesignContent = CurrentSite.GetHtmlSectionByName("SiteDesignContent");
			ViewBag.MessageCount = GetMessagesCount();
			ViewData["ContentId"] = contentId;
			var section = HtmlSection.LoadFull(sectionId);

			var model = new HtmlSectionEditModel();
			ViewBag.Title = "Edit " + SmallCollectionCache.Instance.HtmlSectionEditTypes.GetById(section.HtmlSectionEditTypeID).GetTerm() + " Section";
			model.LoadModel(section, ApplicationContext.Instance.CurrentUser, CurrentSite);
			return View(model);
		}

		public ActionResult GetPhotoCropper(int sectionId, int contentId)
		{
			var section = HtmlSection.Load(sectionId);
			var content = HtmlContent.Load(contentId);
			var status = Constants.HtmlContentStatus.Production;
			PhotoCropperModel model = new PhotoCropperModel()
			{
				TargetWidth = section.Width.ToInt(),
				TargetHeight = section.Height.ToInt(),
				Content = content,
				Folder = section.SectionName,
				Mode = status.ToString().ToLower()
			};

			string contentImage = content.GetImage();
			if (!string.IsNullOrEmpty(contentImage))
			{
				try
				{
					System.Drawing.Image image = System.Drawing.Image.FromFile(contentImage.GetHtmlAttributeValue("src").WebUploadPathToAbsoluteUploadPath());

					decimal targetRatio = model.TargetWidth / (decimal)model.TargetHeight;
					decimal ratio = image.Width / (decimal)image.Height;
					if (ratio > targetRatio)
						model.BoxHeight = model.TargetHeight;
					else
						model.BoxWidth = model.TargetWidth;

					model.OriginalWidth = image.Width;
					model.OriginalHeight = image.Height;
				}
				catch (Exception ex)
				{
					//We weren't able to load the original image for whatever reason - DES
					EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				}
			}

			return Json(new { result = true, data = RenderPartialToString("PhotoCropper", model) });
		}
		public static int GetMessagesCount()
		{
			bool hasCMSContentApprovingFunction = ApplicationContext.Instance.CurrentUser.HasFunction("CMS-Content Approving");
			return HtmlContentHistory.GetUnseenHistoryCountForUser(CurrentSite.SiteID, ApplicationContext.Instance.CurrentUserID, hasCMSContentApprovingFunction, CurrentSite.IsBase ? 0 : CurrentSite.BaseSiteID.Value);
		}
	}
}
