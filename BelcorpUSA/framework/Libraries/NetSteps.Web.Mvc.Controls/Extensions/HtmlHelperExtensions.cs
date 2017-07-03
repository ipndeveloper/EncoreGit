using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Controls.Controllers;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Extensions;

namespace NetSteps.Web.Mvc.Controls
{

	public static class HtmlHelperExtensions
	{
		public static HtmlString Navigation(this HtmlHelper helper, IEnumerable<Navigation> navigation, bool checkRouteData, int? selectedNavigationId = null)
		{
			if (!selectedNavigationId.HasValue)
			{
				var currentRouteData = helper.ViewContext.RouteData;
				if (navigation != null)
				{
					foreach (var nav in navigation.OrderBy(x => x.SortIndex))
					{
						if (checkRouteData)
						{
							var linkUrl = nav.LinkUrl;
							var routeData = helper.FakeRoute(ref linkUrl);

							if (routeData != null)
							{
								if (currentRouteData.Values["action"] == routeData.Values["action"] &&
								currentRouteData.Values["controller"] == routeData.Values["controller"])
								{
									if (currentRouteData.DataTokens["area"] == null)
									{
										if (currentRouteData.Values.ContainsKey("path"))
										{
											var currentPath = currentRouteData.Values["path"].ToString();
											if (nav.LinkUrl.StartsWith(currentPath))
											{
												selectedNavigationId = nav.NavigationID;
												break;
											}
										}
									}
									else
									{
										if (currentRouteData.DataTokens["area"] == routeData.DataTokens["area"])
										{
											selectedNavigationId = nav.NavigationID;
											break;
										}
									}
								}
							}
						}
						else
						{
							if (nav.LinkUrl == helper.ViewContext.HttpContext.Request.Url.LocalPath)
							{
								selectedNavigationId = nav.NavigationID;
								break;
							}
						}
					}
				}
			}

			return new HtmlString(BuildNavigation(navigation, selectedNavigationId));
		}

		private static string BuildNavigation(IEnumerable<NetSteps.Data.Entities.Navigation> navigation, int? selectedNavigationId)
		{
			var activeNav = navigation.Where(n => n.Active && DateTime.Now.ApplicationNow().IsBetween(n.StartDate, n.EndDate)).OrderBy(x => x.SortIndex);
			if (!activeNav.Any())
				return "";

			TagBuilder navBuilder = new TagBuilder("ul");
			navBuilder.AddCssClass("navigation");

			StringBuilder childBuilder = new StringBuilder();
			foreach (var nav in activeNav)
			{
				TagBuilder navItemBuilder = new TagBuilder("li");
				navItemBuilder.AddCssClass("navigationItem");

				TagBuilder aBuilder = new TagBuilder("a");
				bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
				var localPath = HttpContext.Current.Request.Url.LocalPath;
				aBuilder.MergeAttribute("href", (isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1))) + nav.LinkUrl);

				if (selectedNavigationId.HasValue && selectedNavigationId.Value == nav.NavigationID)
					aBuilder.AddCssClass("current");

				TagBuilder spanBuilder = new TagBuilder("span");
				spanBuilder.InnerHtml = nav.Translations.GetByLanguageIdOrDefaultForDisplay().LinkText;

				aBuilder.InnerHtml = spanBuilder.ToString();

				if (nav.ChildNavigations != null && nav.IsDropDown && nav.ChildNavigations.Count(n => n.Active && DateTime.Now.ApplicationNow().IsBetween(n.StartDate, n.EndDate)) > 0)
				{
					navItemBuilder.InnerHtml = aBuilder.ToString() + BuildNavigation(nav.ChildNavigations, selectedNavigationId);
				}
				else if (nav.IsChildNavTree)
				{
					var inventory = Create.New<InventoryBaseRepository>();
					var currentAccount = ApplicationContext.Instance.CurrentAccount;
					var accountTypeID = (currentAccount != null && !currentAccount.IsTempAccount)
						? currentAccount.AccountTypeID
						: (short)Constants.AccountType.RetailCustomer;

					var activeCategories = inventory.GetActiveCategories(ApplicationContext.Instance.StoreFrontID, accountTypeID);
					navItemBuilder.InnerHtml = aBuilder + BuildCatalogs(activeCategories);
				}
				else
				{
					navItemBuilder.InnerHtml = aBuilder.ToString();
				}

				childBuilder.Append(navItemBuilder.ToString());
			}

			navBuilder.InnerHtml = childBuilder.ToString();

			return navBuilder.ToString().ReplaceCmsTokens();
		}

		private static string BuildCatalogs(List<Category> activeCategories)
		{
			var inventory = Create.New<InventoryBaseRepository>();
			var builder = new StringBuilder();
			var trees = new List<int>();
			foreach (Catalog catalog in inventory.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID)
																	.Where(c => c.CatalogTypeID == (short)NetSteps.Data.Entities.Constants.CatalogType.Normal))
			{
				if (!trees.Contains(catalog.CategoryID))
				{
					Category root = inventory.GetCategoryTree(catalog.CategoryID);
					SetActiveCategories(root, activeCategories);
					builder.Append(BuildCategory(root));
					trees.Add(catalog.CategoryID);
				}
			}

			return builder.ToString();
		}

		private static void SetActiveCategories(Category parent, IEnumerable<Category> activeCategories)
		{
			if (activeCategories.Any(c => c.CategoryID == parent.CategoryID))
				parent.Display = true;

			if (parent.ChildCategories != null && parent.ChildCategories.Count > 0)
			{
				foreach (var child in parent.ChildCategories)
				{
					SetActiveCategories(child, activeCategories);
					if (child.Display)
					{
						parent.Display = true;
					}
				}
			}
		}

		private static string BuildCategory(Category parent)
		{
			if (parent.ChildCategories != null && parent.ChildCategories.Any(c => c.Display))
			{
				var navBuilder = new TagBuilder("ul");
				navBuilder.AddCssClass("navigation");

				var builder = new StringBuilder();
				foreach (var category in parent.ChildCategories.OrderBy(c => c.SortIndex).Where(c => c.Display))
				{
					var navItemBuilder = new TagBuilder("li");
					navItemBuilder.AddCssClass("navigationItem");

					var aBuilder = new TagBuilder("a");
					var isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
					var localPath = HttpContext.Current.Request.Url.LocalPath;
					var url = string.Format("~{0}/Shop/Category/{1}", isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1)), category.CategoryID).ResolveUrl();
					aBuilder.MergeAttribute("href", url);

					var spanBuilder = new TagBuilder("span") { InnerHtml = category.Translations.GetByLanguageIdOrDefaultForDisplay().Name };

					aBuilder.InnerHtml = spanBuilder.ToString();

					navItemBuilder.InnerHtml = aBuilder + BuildCategory(category);

					builder.Append(navItemBuilder.ToString());
				}

				navBuilder.InnerHtml = builder.ToString();

				return navBuilder.ToString();
			}

			return string.Empty;
		}

		public static HtmlString Navigation(this HtmlHelper helper, Site site, int navigationTypeID, int? parentID = null, int? selectedNavigationID = null, int? pageID = null)
		{
			if (!site.IsBase)
			{
				site = SiteCache.GetSiteByID(site.BaseSiteID.ToInt());
			}
			if (site.Navigations == null || site.Navigations.Count == 0)
			{
				return new HtmlString("");
			}

			var result = helper.Navigation(site.Navigations.Where(n => n.NavigationTypeID == navigationTypeID && n.ParentID == parentID).OrderBy(n => n.SortIndex), true, selectedNavigationID);

			return result;
		}

		public static void GetEditor(this HtmlHelper<HtmlSectionEditModel> helper, NetSteps.Common.Constants.ViewingMode pageMode, bool useBase = false, bool corporateMode = false)
		{
			Constants.HtmlContentStatus status;
			switch (pageMode)
			{
				case Common.Constants.ViewingMode.Archive:
					status = Constants.HtmlContentStatus.Archive;
					break;
				case Common.Constants.ViewingMode.Staging:
					status = Constants.HtmlContentStatus.Draft;
					break;
				default:
					status = Constants.HtmlContentStatus.Production;
					break;
			}
			var section = helper.ViewData.Model;

			var site = EditController.CurrentSite;
			if (useBase)
			{
				site = site.IsBase ? site : SiteCache.GetSiteByID(site.BaseSiteID.ToInt());
			}
			var content = helper.ViewData.Model.Section.ContentByStatus(site, status);

			helper.GetEditor(content, status, corporateMode);
		}

		public static void GetEditor(this HtmlHelper<HtmlSectionEditModel> helper, int? contentId, Constants.HtmlContentStatus status, bool corporateMode = false)
		{
			HtmlContent content;
			if (contentId.HasValue)
				content = HtmlContent.LoadFull(contentId.Value);
			else
				content = new HtmlContent() { HtmlContentStatusID = (int)Constants.HtmlContentStatus.Production };

			helper.GetEditor(content, status, corporateMode);
		}

		public static void GetEditor(this HtmlHelper<HtmlSectionEditModel> helper, HtmlContent content, Constants.HtmlContentStatus status, bool corporateMode = false)
		{
			var section = helper.ViewData.Model.Section;

			if (content == null)
				content = new HtmlContent();

			if (section.HtmlContentEditorTypeID == (int)Constants.HtmlContentEditorType.PhotoCropper)
			{
				var model = new PhotoCropperModel()
				{
					TargetWidth = section.Width.ToInt(),
					TargetHeight = section.Height.ToInt(),
					Content = content,
					Folder = section.SectionName,
					Mode = status.ToString().ToLower()
				};

				var contentImage = content.GetImage();
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

				helper.RenderPartial("PhotoCropper", model);
			}
			else if (section.HtmlContentEditorTypeID == (int)Constants.HtmlContentEditorType.Photo)
			{
				helper.RenderPartial("PhotoUploader", new PhotoUploaderModel()
				{
					Content = content,
					Folder = section.SectionName,
					Mode = status.ToString().ToLower()
				});
			}
			else
			{
				helper.RenderPartial("TextEditor", new TextEditorModel()
				{
					ContentBody = content != null ? (!string.IsNullOrEmpty(content.GetBody()) ? content.GetBody() : content.GetHead()) : string.Empty,
					ContentName = content != null ? content.Name : string.Empty,
					ShowName = section.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Choices || section.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ChoicesPlus,
					IsRichText = section.HtmlContentEditorTypeID == (int)Constants.HtmlContentEditorType.RichText,
					InstanceName = status.ToString().ToLower() + "Body",
					ShowTabbedHeader = corporateMode,
					HtmlSectionID = section.HtmlSectionID,
					ShowMediaLibraryLink = true,
					ShowPreviewLink = true
				});
			}
		}


		public static void GetCorporateEditor(this HtmlHelper<HtmlSectionEditModel> helper, HtmlContent content, Constants.HtmlContentStatus status, bool corporateMode = false)
		{
			var section = helper.ViewData.Model.Section;

			if (content == null)
				content = new HtmlContent();

			if (section.HtmlContentEditorTypeID == (int)Constants.HtmlContentEditorType.PhotoCropper)
			{
				PhotoCropperModel model = new PhotoCropperModel()
				{
					TargetWidth = section.Width.ToInt(),
					TargetHeight = section.Height.ToInt(),
					Content = content,
					Folder = section.SectionName,
					Mode = status.ToString().ToLower()
				};

				if (!string.IsNullOrEmpty(content.GetImage()))
				{
					try
					{
						System.Drawing.Image image = System.Drawing.Image.FromFile(content.GetImage().GetHtmlAttributeValue("src").WebUploadPathToAbsoluteUploadPath());

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

				helper.RenderPartial("PhotoPreview", model);
			}
			else if (section.HtmlContentEditorTypeID == (int)Constants.HtmlContentEditorType.Photo)
			{
				helper.RenderPartial("PhotoUploader", new PhotoUploaderModel()
				{
					Content = content,
					Folder = section.SectionName,
					Mode = status.ToString().ToLower()
				});
			}
			else
			{
				string contentBody = content != null ? (!string.IsNullOrEmpty(content.GetBody()) ? content.GetBody() : content.GetHead()) : string.Empty;
				helper.RenderPartial("TextEditor", new TextEditorModel()
				{
					ContentBody = contentBody,
					ContentName = content != null ? content.Name : string.Empty,
					ShowName = section.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.Choices || section.HtmlSectionEditTypeID == (int)Constants.HtmlSectionEditType.ChoicesPlus,
					IsRichText = section.HtmlContentEditorTypeID == (int)Constants.HtmlContentEditorType.RichText,
					InstanceName = status.ToString().ToLower() + "Body",
					ShowTabbedHeader = corporateMode,
					HtmlSectionID = section.HtmlSectionID,
					ShowMediaLibraryLink = true,
					ShowPreviewLink = true
				});
			}
		}
	}
}
