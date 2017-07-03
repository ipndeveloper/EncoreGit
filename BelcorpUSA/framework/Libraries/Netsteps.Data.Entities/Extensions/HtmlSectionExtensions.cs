using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Extensions
{
	public static class HtmlSectionExtensions
	{
		public static HtmlContent ProductionContent(this HtmlSection section, Site site, bool useCache = true)
		{
			return section.ProductionContent(site, ApplicationContext.Instance.CurrentLanguageID, useCache);
		}

		public static HtmlContent ProductionContent(this HtmlSection section, Site site, int languageId, bool useCache = true)
		{
			return section.ContentByStatus(site, Constants.HtmlContentStatus.Production, languageId, useCache);
		}

		public static HtmlContent ProductionContentForDisplay(this HtmlSection section, Site site, bool useCache = true)
		{
			return section.ProductionContentForDisplay(site, ApplicationContext.Instance.CurrentLanguageID, useCache);
		}

		public static HtmlContent ProductionContentForDisplay(this HtmlSection section, Site site, int languageId, bool useCache = true)
		{
			var productionContent = section.ContentByStatusForDisplay(site, Constants.HtmlContentStatus.Production, languageId, useCache);
			var stagingContent = section.ContentByStatus(site, Constants.HtmlContentStatus.Draft, languageId);

			// Promote staging content if the publish date has arrived.  Hellz Yeah
			if (stagingContent != null && stagingContent.PublishDateUTC <= DateTime.UtcNow)
			{
				var reloadedProductionContent = HtmlContent.Load(productionContent.HtmlContentID);
				productionContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Archive;
				reloadedProductionContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Archive;
				reloadedProductionContent.Save();

				var reloadedStagingContent = HtmlContent.Load(stagingContent.HtmlContentID);
				stagingContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Production;
				reloadedStagingContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Production;
				reloadedStagingContent.Save();

				return stagingContent;
			}
			else
				return productionContent;
		}

		public static HtmlContent WorkingDraft(this HtmlSection section, Site site)
		{
			return section.WorkingDraft(site, ApplicationContext.Instance.CurrentLanguageID);
		}

		public static HtmlContent WorkingDraft(this HtmlSection section, Site site, int languageId)
		{
			return section.AllContentByStatus(site, Constants.HtmlContentStatus.Submitted, languageId).FirstOrDefault(c => c.CreatedByUserID.HasValue && c.CreatedByUserID == ApplicationContext.Instance.CurrentUserID);
		}

		public static HtmlContent ContentByStatus(this HtmlSection section, Site site, Constants.HtmlContentStatus status)
		{
			return section.ContentByStatus(site, status, ApplicationContext.Instance.CurrentLanguageID);
		}

		public static HtmlContent ContentByStatus(this HtmlSection section, Site site, Constants.HtmlContentStatus status, int languageId, bool useCache = true)
		{
			var siteContent = useCache ? SiteCache.GetSiteByID(site.SiteID) : site;

			var sectionContent = siteContent.HtmlSectionContents.OrderByDescending(hsc => hsc.HtmlContent.PublishDateUTC).FirstOrDefault(hsc => hsc.HtmlSectionID == section.HtmlSectionID && hsc.HtmlContent.HtmlContentStatusID == (int)status && hsc.HtmlContent.LanguageID == languageId);

			if (sectionContent == default(HtmlSectionContent))
			{
				if (site.IsBase)
					return null;
				else
				{
					var baseSiteContent = SiteCache.GetSiteByID((site.BaseSiteID.ToInt()));
					sectionContent = baseSiteContent.HtmlSectionContents.OrderByDescending(hsc => hsc.HtmlContent.PublishDateUTC).FirstOrDefault(hsc => hsc.HtmlSectionID == section.HtmlSectionID && hsc.HtmlContent.HtmlContentStatusID == (int)status && hsc.HtmlContent.LanguageID == languageId);

					if (sectionContent == default(HtmlSectionContent))
						return null;
				}
			}

			return sectionContent.HtmlContent;
		}

		public static List<HtmlContent> AllContentByStatus(this HtmlSection section, Site site, Constants.HtmlContentStatus status)
		{
			return section.AllContentByStatus(site, status, ApplicationContext.Instance.CurrentLanguageID);
		}
		public static List<HtmlContent> AllContentByStatus(this HtmlSection section, Site site, Constants.HtmlContentStatus status, int languageId, bool useCache = true)
		{
			var siteContent = useCache ? SiteCache.GetSiteByID(site.SiteID) : site;

			var content = siteContent.HtmlSectionContents.Where(hsc => hsc.HtmlSectionID == section.HtmlSectionID && hsc.HtmlContent.HtmlContentStatusID == (int)status && hsc.HtmlContent.LanguageID == languageId).Select(hsc => hsc.HtmlContent);

			if (!content.Any())
			{
				if (site.IsBase)
					return new List<HtmlContent>();
				else
				{
					var baseSiteContent = SiteCache.GetSiteByID(site.BaseSiteID.ToInt());
					content = baseSiteContent.HtmlSectionContents.Where(hsc => hsc.HtmlSectionID == section.HtmlSectionID && hsc.HtmlContent.HtmlContentStatusID == (int)status && hsc.HtmlContent.LanguageID == languageId).Select(hsc => hsc.HtmlContent);

					if (!content.Any())
						return new List<HtmlContent>();
				}
			}
			return content.ToList();
		}

		public static HtmlContent ContentByStatusForDisplay(this HtmlSection section, Site site, Constants.HtmlContentStatus status)
		{
			return section.ContentByStatusForDisplay(site, status, ApplicationContext.Instance.CurrentLanguageID);
		}

		public static HtmlContent ContentByStatusForDisplay(this HtmlSection section, Site site, Constants.HtmlContentStatus status, int languageId, bool useCache = true)
		{
			if (section == null)
				return new HtmlContent()
				{
					LanguageID = languageId,
					PublishDate = DateTime.Now.ApplicationNow()
				};

			var siteContent = useCache ? SiteCache.GetSiteByID(site.SiteID) : site;

			HtmlSectionContent sectionContent = siteContent.HtmlSectionContents.OrderByDescending(hsc => hsc.HtmlContent.PublishDateUTC).FirstOrDefault(hsc => hsc.HtmlSectionID == section.HtmlSectionID && hsc.HtmlContent.HtmlContentStatusID == (int)status && hsc.HtmlContent.LanguageID == languageId);
			if (status == Constants.HtmlContentStatus.Production)
			{
				var stagingContent = siteContent.HtmlSectionContents.FirstOrDefault(hsc => hsc.HtmlContent.PublishDateUTC <= DateTime.UtcNow && hsc.HtmlSectionID == section.HtmlSectionID && hsc.HtmlContent.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Draft && hsc.HtmlContent.LanguageID == languageId);
				if (stagingContent != default(HtmlSectionContent))
				{
					var reloadedStagingContent = HtmlContent.Load(stagingContent.HtmlContentID);

					reloadedStagingContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Production;
					reloadedStagingContent.Save();
					stagingContent.HtmlContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Production;

					if (sectionContent != default(HtmlSectionContent))
					{
						var reloadedProductionContent = HtmlContent.Load(sectionContent.HtmlContentID);
						reloadedProductionContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Archive;
						sectionContent.HtmlContent.HtmlContentStatusID = (int)Constants.HtmlContentStatus.Archive;
						reloadedProductionContent.Save();
					}

					sectionContent = stagingContent;
				}
			}

			if (!DateTime.Now.IsEqualUpToSecond(DateTime.Now.ApplicationNow()) && (status == Constants.HtmlContentStatus.Production || status == Constants.HtmlContentStatus.Draft))
			{
				var contentByDate = siteContent.HtmlSectionContents.FirstOrDefault(hsc => hsc.HtmlContent.PublishDateUTC <= DateTime.Now.ApplicationNow().LocalToUTC() && hsc.HtmlSectionID == section.HtmlSectionID && hsc.HtmlContent.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Draft && hsc.HtmlContent.LanguageID == languageId);
				if (contentByDate != default(HtmlSectionContent))
					sectionContent = contentByDate;
			}

			if (sectionContent == default(HtmlSectionContent))
			{
				if (site.IsBase)
				{
					if (languageId == Language.English.LanguageID)
					{
						return new HtmlContent()
						{
							LanguageID = languageId,
							PublishDate = DateTime.Now.ApplicationNow()
						};
					}
					//Default to english if there is no content for that language - DES
					else
						return section.ContentByStatusForDisplay(site, status, Language.English.LanguageID);
				}
				else
				{
					return section.ContentByStatusForDisplay(SiteCache.GetSiteByID(site.BaseSiteID.ToInt()), status, languageId);

				}
			}
			return sectionContent.HtmlContent;
		}

		public static HtmlSection GetBySectionName(this IEnumerable<HtmlSection> sections, string sectionName)
		{
			return sections.FirstOrDefault(s => s.SectionName == sectionName);
		}

		public static HtmlSection GetBySectionNameOrDefault(this IEnumerable<HtmlSection> sections, string sectionName)
		{
			var section = sections.FirstOrDefault(s => s.SectionName == sectionName);
			if (section == null)
				section = new HtmlSection() { SectionName = sectionName };
			return section;
		}

		public static string DisplayHtmlSection(this IEnumerable<HtmlSection> sections, Site site, Constants.ViewingMode pageMode, string sectionName, int? pageID = null)
		{
			var htmlSection = sections.GetBySectionName(sectionName);

			if (htmlSection == null)
			{
				var errorMessage = String.Format(Translation.GetTerm("HtmlSectionNameDoesNotExist", "HtmlSection \"{0}\" does not exist for pageID: {1}"), sectionName, pageID);
				return String.Format("<div class='MissingHtmlSection'>{0}</div>", errorMessage);
			}

			return ToDisplay(htmlSection, site, pageMode, pageID);
		}

		public static string ToDisplay(this HtmlSection section, Site site, Constants.ViewingMode pageMode, int? pageId = null)//, string cssClass = "content")
		{
			return section.ToDisplay(site, pageMode, ApplicationContext.Instance.CurrentLanguageID, pageId);//, cssClass);
		}

		public static string ToDisplay(this HtmlSection section, Site site, Constants.ViewingMode pageMode, int languageId, int? pageId = null)//, string cssClass = "content")
		{
			if (section == null && pageId == 0)
				return Translation.GetTerm("HtmlSectionAndPageDoesNotExist", "HtmlSection and Page does not exist.");
			if (section == null)
				return String.Format(Translation.GetTerm("HtmlSectionDoesNotExist", "HtmlSection does not exist for pageID: {0}"), pageId.ToString());

			Constants.HtmlContentStatus status;
			HtmlContent displayContent = null;
			if (pageMode == Constants.ViewingMode.Preview)
			{
				var previewContent = HttpContext.Current.Session["PreviewContent"] as Tuple<int, HtmlContent>;
				if (previewContent != null && previewContent.Item1 == section.HtmlSectionID)
					displayContent = previewContent.Item2;
			}

			if (displayContent == null)
			{
				switch (pageMode)
				{
					case Constants.ViewingMode.Archive:
						status = Constants.HtmlContentStatus.Archive;
						break;
					case Constants.ViewingMode.Staging:
						status = Constants.HtmlContentStatus.Draft;
						break;
					default:
						status = Constants.HtmlContentStatus.Production;
						break;
				}
				displayContent = section.ContentByStatusForDisplay(site, status);
			}

			var content = displayContent.BuildContent();//cssClass);

			if (pageMode == Constants.ViewingMode.Edit && section != null)
			{
				var urlBuilder = new StringBuilder("~/Edit/CorporateEdit?sectionId=").Append(section.HtmlSectionID);
				if (pageId.HasValue)
					urlBuilder.Append("&pageId=").Append(pageId.Value);
				else if (HttpContext.Current != null && HttpContext.Current.Request.Url != null)
					urlBuilder.Append("&returnUrl=").Append(System.Uri.EscapeUriString(HttpContext.Current.Request.Url.PathAndQuery));
				else if (HttpContext.Current != null)
					urlBuilder.Append("&returnUrl=").Append(System.Uri.EscapeUriString(HttpContext.Current.Request.ApplicationPath));

				var notifications = new StringBuilder();
				if (ApplicationContext.Instance.CurrentUser != null && ApplicationContext.Instance.CurrentUser.HasFunction("CMS-Content Editing"))
				{
					bool anyPushed = site.AllContentByStatus(section.HtmlSectionID, Constants.HtmlContentStatus.Pushed, languageId).Count > 0;
					notifications.Append(string.Format("<span class=\"EditNotification Pushed{0}\" title=\"{1}\"></span>", anyPushed ? " On" : "", anyPushed ? Translation.GetTerm("PushedContentWaiting", "Pushed content waiting") : Translation.GetTerm("NoPushedContent", "No pushed content")));

					bool waitingApproval = section.WorkingDraft(site) != null;

					HtmlContentRepository htmlContentRepository = new HtmlContentRepository();
					var results = htmlContentRepository.GetHtmlSectionContentStatus(section, ApplicationContext.Instance.CurrentUserID);
					bool? approved = results.Item1;
					bool submittedContent = results.Item2;

					notifications.Append(string.Format("<span class=\"EditNotification WorkingDraft{0}\" title=\"{1}\"></span>", waitingApproval ? " On" : (submittedContent && approved.HasValue ? (approved.Value ? " Approved" : " Denied") : ""), waitingApproval ? Translation.GetTerm("WorkingDraftPendingApproval", "Working draft pending approval") : (submittedContent && approved.HasValue ? (approved.Value ? Translation.GetTerm("WorkingDraftApproved", "Working draft approved") : Translation.GetTerm("WorkingDraftDenied", "Working draft denied")) : Translation.GetTerm("NoWorkingDraft", "No working draft"))));
				}

				if (ApplicationContext.Instance.CurrentUser != null && ApplicationContext.Instance.CurrentUser.HasFunction("CMS-Content Approving"))
				{
					bool anyPending = site.AllContentByStatus(section.HtmlSectionID, Constants.HtmlContentStatus.Submitted, languageId).Count > 0;
					notifications.Append(string.Format("<span class=\"EditNotification PendingApproval{0}\" title=\"{1}\"></span>", anyPending ? " On" : "", anyPending ? Translation.GetTerm("ContentPendingApproval", "Content pending approval") : Translation.GetTerm("NoPendingContent", "No pending content")));
				}

				if (section.SectionName != "SiteDesignContent")
				{
					content = string.Format("<div class=\"EditableContent\"><a href=\"{0}\" class=\"Edit\">{1}{2}</a>{3}</div>",
						 urlBuilder.ToString().ResolveUrl(),
						 notifications.ToString(),
						 Translation.GetTerm("Edit"),
						 content);
				}
			}

			if (Container.Current.Registry.IsTypeRegistered(typeof(NetSteps.Common.Interfaces.ITokenValueProviderFactory)))
			{
				content = ReplaceCmsTokens(content);
			}


			return content;
		}

		public static string ToDisplayLogo(this HtmlSection section, Site site, Constants.ViewingMode pageMode, int languageId, string url, int? pageId = null)//, string cssClass = "content")
		{
			Constants.HtmlContentStatus status;
			HtmlContent displayContent = null;
			if (pageMode == Constants.ViewingMode.Preview)
			{
				var previewContent = HttpContext.Current.Session["PreviewContent"] as Tuple<int, HtmlContent>;
				if (previewContent != null && previewContent.Item1 == section.HtmlSectionID)
					displayContent = previewContent.Item2;
			}

			if (displayContent == null)
			{
				switch (pageMode)
				{
					case Constants.ViewingMode.Archive:
						status = Constants.HtmlContentStatus.Archive;
						break;
					case Constants.ViewingMode.Staging:
						status = Constants.HtmlContentStatus.Draft;
						break;
					default:
						status = Constants.HtmlContentStatus.Production;
						break;
				}
				displayContent = section.ContentByStatusForDisplay(site, status);
			}

			var content = string.Format("<a href=\"{0}\">{1}</a>", url, displayContent.GetImage());

			var imageType = SmallCollectionCache.Instance.HtmlElementTypes.GetById((int)Constants.HtmlElementType.Image);
			if (!string.IsNullOrEmpty(imageType.ContainerTagName))
			{
				content = string.Format("<{0}{1}>{2}</{0}>", imageType.ContainerTagName, string.IsNullOrEmpty(imageType.ContainerCssClass) ? "" : string.Format(" class=\"{0}\"", imageType.ContainerCssClass), content);
			}

			if (pageMode == Constants.ViewingMode.Edit)
			{
				var urlBuilder = new StringBuilder("~/Edit/CorporateEdit?sectionId=").Append(section.HtmlSectionID);
				if (pageId.HasValue)
					urlBuilder.Append("&pageId=").Append(pageId.Value);
				else if (HttpContext.Current != null && HttpContext.Current.Request.Url != null)
					urlBuilder.Append("&returnUrl=").Append(System.Uri.EscapeUriString(HttpContext.Current.Request.Url.PathAndQuery));
				else if (HttpContext.Current != null)
					urlBuilder.Append("&returnUrl=").Append(System.Uri.EscapeUriString(HttpContext.Current.Request.ApplicationPath));

				var notifications = new StringBuilder();
				if (ApplicationContext.Instance.CurrentUser.HasFunction("CMS-Content Editing"))
				{
					bool anyPushed = site.AllContentByStatus(section.HtmlSectionID, Constants.HtmlContentStatus.Pushed, languageId).Count > 0;
					notifications.Append(string.Format("<span class=\"EditNotification Pushed{0}\" title=\"{1}\"></span>", anyPushed ? " On" : "", anyPushed ? Translation.GetTerm("PushedContentWaiting", "Pushed content waiting") : Translation.GetTerm("NoPushedContent", "No pushed content")));

					bool waitingApproval = section.WorkingDraft(site) != null;
					var unreadHistory = HtmlContentHistory.GetUnseenHistoryForSectionAndUser(section.HtmlSectionID, ApplicationContext.Instance.CurrentUserID).OrderByDescending(hch => hch.HistoryDate);
					bool submittedContent = unreadHistory.Any(hch => hch.HtmlContentStatusID != (int)Constants.HtmlContentStatus.Pushed);
					bool? approved = null;
					if (submittedContent)
					{
						approved = unreadHistory.FirstOrDefault(hch => hch.HtmlContentStatusID != (int)Constants.HtmlContentStatus.Pushed).HtmlContentStatusID != (int)Constants.HtmlContentStatus.Disapproved;
					}
					notifications.Append(string.Format("<span class=\"EditNotification WorkingDraft{0}\" title=\"{1}\"></span>", waitingApproval ? " On" : (submittedContent && approved.HasValue ? (approved.Value ? " Approved" : " Denied") : ""), waitingApproval ? Translation.GetTerm("WorkingDraftPendingApproval", "Working draft pending approval") : (submittedContent && approved.HasValue ? (approved.Value ? Translation.GetTerm("WorkingDraftApproved", "Working draft approved") : Translation.GetTerm("WorkingDraftDenied", "Working draft denied")) : Translation.GetTerm("NoWorkingDraft", "No working draft"))));
				}

				if (ApplicationContext.Instance.CurrentUser.HasFunction("CMS-Content Approving"))
				{
					bool anyPending = site.AllContentByStatus(section.HtmlSectionID, Constants.HtmlContentStatus.Submitted, languageId).Count > 0;
					notifications.Append(string.Format("<span class=\"EditNotification PendingApproval{0}\" title=\"{1}\"></span>", anyPending ? " On" : "", anyPending ? Translation.GetTerm("ContentPendingApproval", "Content pending approval") : Translation.GetTerm("NoPendingContent", "No pending content")));
				}


				content = string.Format("<div class=\"EditableContent\"><a href=\"{0}\" class=\"Edit\">{1}{2}</a>{3}</div>",
					 urlBuilder.ToString().ResolveUrl(),
					 notifications.ToString(),
					 Translation.GetTerm("Edit"),
					 content);
			}

			return content;
		}

		#region Private Methods

		private static string ReplaceCmsTokens(string content)
		{
			var tokenProviderFactory = Create.New<NetSteps.Common.Interfaces.ITokenValueProviderFactory>();
			var tokenProvider = tokenProviderFactory.GetTokenProvider(NetSteps.Common.Constants.TokenValueProviderType.Cms);
			var tokenReplacer = new NetSteps.Common.TokenReplacement.TokenReplacer(tokenProvider, NetSteps.Common.Constants.BEGIN_TOKEN_DELIMITER, NetSteps.Common.Constants.END_TOKEN_DELIMITER);

			return tokenReplacer.ReplaceTokens(content);
		}

		#endregion
	}
}
