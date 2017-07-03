using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Account.Controllers;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Web.Mvc.Helpers;

namespace DistributorBackOffice.Controllers
{
	public class DocumentsController : BaseAccountsController
	{
		[FunctionFilter("Documents", "~/", ConstantsGenerated.SiteType.BackOffice)]
		public virtual ActionResult Index(int? category)
		{
			var archiveCategories = Category.LoadAllArchiveCategories();
			if (archiveCategories != null && archiveCategories.Count > 0)
			{
				var topLevel = archiveCategories.FirstOrDefault(c => !c.ParentCategoryID.HasValue);

				ViewData["Categories"] = BuildCategoryTree(topLevel.CategoryID, archiveCategories, category);
				ViewData["CategoryContent"] = GetCategoryHtmlContent(category == null ? topLevel.CategoryID : (int)category);
			}
			else
				ViewData["Categories"] = "";

			return View();
		}

		protected virtual string GetCategoryHtmlContent(int categoryID)
		{
			var content = Category.LoadFull(categoryID).Translations.FirstOrDefault();

			if (content != null)
				return content.HtmlContent == null ? String.Empty : content.HtmlContent.GetBody() ?? String.Empty;

			return String.Empty;
		}

		protected virtual string BuildCategoryTree(int parentCategoryId, IEnumerable<ArchiveCategorySearchData> categories, int? selectedCategory = null, bool topLevel = true)
		{
			StringBuilder builder = new StringBuilder();

			var children = categories.Where(c => c.ParentCategoryID == parentCategoryId);
			if (children.Count() > 0)
			{
				builder.Append("<ul>");
				if (topLevel)
				{
					builder.Append("<li><a href=\"javascript:void(0);\" class=\"documentCategory\" id=\"allCategories\"").Append(!selectedCategory.HasValue ? " class=\"current\"" : "").Append("><span>").Append(Translation.GetTerm("AllCategories", "All Categories")).Append("</span><span style=\"display: none;\" class=\"FR\">(").Append(Archives.Where(a => a.Active && a.IsDownloadable).Count()).Append(")</span></a></li>");
				}
				foreach (var category in children.Where(c => HasChildArchives(c.CategoryID, categories)))
				{
					if (category.ArchiveCount > 0)
					{
						builder.Append("<li><a href=\"javascript:void(0);\" class=\"documentCategory\" id=\"")
							.Append(category.CategoryID)
							.Append("\"")
							.Append(selectedCategory == category.CategoryID ? " class=\"current\"" : "")
							.Append("><span>")
							.Append(category.Name)
							.Append("</span><span style=\"display: none;\" class=\"FR\">(")
							.Append(category.ArchiveCount)
							.Append(")</span></a></li>");
					}
					if (categories.Count(c => c.ParentCategoryID == category.CategoryID) > 0)
					{
						builder.Append(BuildCategoryTree(category.CategoryID, categories, selectedCategory, false));
					}
				}
				builder.Append("</ul>");
			}
			return builder.ToString();
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Documents", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Get(int? category, string query, List<Constants.FileType> fileTypes, int page, int? pageSize, string orderBy, Constants.SortDirection orderByDirection)
		{
			try
			{
				query = query == null ? "" : query.ToLower();

				var builder = new StringBuilder();
				string fileUploadWebPath = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadWebPath);
				var archives = Archive.Search(new ArchiveSearchParameters()
				{
					Active = true,
					SiteID = CurrentSite.SiteID,
					StartDate = DateTime.Now.ApplicationNow(),
					EndDate = DateTime.Now.ApplicationNow(),
					Query = query,
					CategoryID = category,
					PageIndex = page,
					PageSize = pageSize,
					OrderBy = orderBy,
					OrderByDirection = orderByDirection,
					LanguageID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID,
					FileTypes = fileTypes
				});

				foreach (var archive in archives)
				{
					string icon, fileName = archive.ArchivePath.Substring(archive.ArchivePath.LastIndexOf('/') + 1);
					var archiveImage = archive.ArchiveImage.ReplaceFileUploadPathToken();
					var archivePath = archive.ArchivePath.ReplaceFileUploadPathToken();
					var type = fileName.GetFileType();
					switch (type)
					{
						case NetSteps.Common.Constants.FileType.PDF:
							icon = "pdf.png";
							break;
						case NetSteps.Common.Constants.FileType.Audio:
							icon = "audio.png";
							break;
						case NetSteps.Common.Constants.FileType.Flash:
							icon = "flash.png";
							break;
						case NetSteps.Common.Constants.FileType.Image:
							icon = "image.png";
							break;
						case NetSteps.Common.Constants.FileType.Video:
							icon = "wmv.png";
							break;
						case NetSteps.Common.Constants.FileType.Powerpoint:
							icon = "powerpoint.png";
							break;
						case NetSteps.Common.Constants.FileType.Excel:
							icon = "excel.png";
							break;
						case NetSteps.Common.Constants.FileType.Word:
							if (archive.ArchivePath.EndsWith(".txt"))
								icon = "text.png";
							else
								icon = "word.png";
							break;
						default:
							icon = "file.png";
							break;
					}

					if (archive.IsDownloadable)
					{
						builder.Append("<tr>")
							.AppendCell("<img src=\"" + "~/Resource/Content/Images/DocumentTypes/Icons/".ResolveUrl() + icon + "\" alt=\"\" />")
							.AppendCell("<a href=\"" + archivePath + "\" target=\"_blank\" rel=\"external\">" + archive.Name + "</a>");
					}
					else
					{
						builder.Append("<tr style=\"cursor: default !important\">")
							.AppendCell("<img src=\"" + "~/Resource/Content/Images/DocumentTypes/Icons/".ResolveUrl() + icon + "\" alt=\"\" />")
							.AppendCell(archive.Name);
					}

					builder.AppendCell(System.IO.Path.GetFileName(archivePath))
						.AppendCell(archive.StartDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));

					if (archive.IsEmailable)
					{
						builder.Append("<td><a href=\"javascript:void(0);\" name=\"").Append(archive.Name).Append("\" id=\"archiveAddLink")
							.Append(archive.ArchiveID).Append("\" class=\"archiveAddLink\">")
							.Append(Translation.GetTerm("Add to media bundle", "Add to media bundle")).Append("</a></td>");
					}
					else
					{
						builder.Append("<td />");
					}

					builder.Append("</tr>");

					//builder.Append("<div class=\"Resource\"><div class=\"FL Rthumb\"><span class=\"Rwrapper\"><img src=\"");
					//if (string.IsNullOrEmpty(archive.ArchiveImage))
					//{
					//    builder.Append("~/Resource/Content/Images/DocumentTypes/file.png".ResolveUrl());
					//}
					//else
					//{
					//    builder.Append(archiveImage);
					//}
					//builder.Append("\" /></span></div><div class=\"FL Rdesc\"><span class=\"FileType\"><img src=\"")
					//    .Append("~/Resource/Content/Images/DocumentTypes/Icons/".ResolveUrl()).Append(icon).Append("\" /></span><a href=\"").Append(archivePath)
					//    .Append("\" target=\"_blank\" rel=\"external\" class=\"Rtitle\">").Append(archive.Name)
					//    .Append("</a><a href=\"javascript:void(0);\" name=\"").Append(archive.Name).Append("\" id=\"archiveAddLink")
					//    .Append(archive.ArchiveID).Append("\" class=\"archiveAddLink\">")
					//    .Append(Translation.GetTerm("Add to media bundle", "Add to media bundle")).Append("</a></div><span class=\"ClearAll\"></span></div>");
				}

				if (category == null)
				{
					var topLevelCategory =
						Category.LoadAllArchiveCategories().FirstOrDefault(c => !c.ParentCategoryID.HasValue);
					if (topLevelCategory != null)
						category = topLevelCategory.CategoryID;
				}

				var categoryContent = category != null ? GetCategoryHtmlContent((int)category) : String.Empty;

				return Json(new { result = true, totalPages = archives.TotalPages, page = builder.ToString(), content = categoryContent });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Documents", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Details(int id)
		{
			return Redirect(CurrentSite.Archives.FirstOrDefault(a => a.ArchiveID == id).ArchivePath.ReplaceFileUploadPathToken());
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Documents-Bundle Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult BundleEmail(List<int> archiveIDs)
		{
			try
			{
				TempData["BundleEmail"] = archiveIDs;

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "AutoCompleteData")]
		public virtual ActionResult Search(string query)
		{
			int langID = ApplicationContext.Instance.CurrentLanguageID;
			var results = Archives.Where(a => a.Translations.Any(t => t.LanguageID == langID) &&
				a.Translations.First(t => t.LanguageID == langID).Name.ContainsIgnoreCase(query) &&
				a.Active &&
				(!a.StartDate.HasValue || (a.StartDate.HasValue && a.StartDate.Value <= DateTime.Now.ApplicationNow())) &&
				(!a.EndDate.HasValue || (a.EndDate.HasValue && a.EndDate.Value >= DateTime.Now.ApplicationNow())));
			return Json(results.ToDictionary(a => a.ArchiveID, a => a.Translations.First(t => t.LanguageID == langID).Name).ToAJAXSearchResults());
		}

		public virtual bool HasChildArchives(int categoryId, IEnumerable<ArchiveCategorySearchData> categories)
		{
			var children = categories.Where(c => c.ParentCategoryID == categoryId);
			if (children.Any())
			{
				return children.Any(c => HasChildArchives(c.CategoryID, categories));
			}
			return categories.FirstOrDefault(c => c.CategoryID == categoryId).ArchiveCount > 0;
		}
	}
}
