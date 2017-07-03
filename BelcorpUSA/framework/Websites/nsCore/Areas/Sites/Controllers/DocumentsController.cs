using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using nsCore.Areas.Sites.Models;
using nsCore.Models;

namespace nsCore.Areas.Sites.Controllers
{
	public class DocumentsController : BaseSitesController
	{
		/// <summary>
		/// Displays the ResourceLibrary view
		/// </summary>
		/// <param name="id">A site id</param>
		/// <returns></returns>
		[FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
		public virtual ActionResult Index(int? id)
		{
			try
			{
				if (id.HasValue && id.Value > 0)
					CurrentSite = Site.LoadSiteWithSiteURLs(id.Value);
				if (CurrentSite == null)
					return RedirectToAction("Index", "Landing");
				return View();
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var filter = filterContext;

        //    return;
        //}
		/// <summary>
		/// Populates the grid on the ResourceLibrary view
		/// </summary>
		public virtual ActionResult Get(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, int? category, bool? active, string name)
		{
			try
			{

                var global = System.Globalization.CultureInfo.CurrentCulture;
				var archiveItems = Archive.Search(new ArchiveSearchParameters()
				{
					SiteID = CurrentSite.SiteID,
					CategoryID = category,
					Active = active,
					Query = name,
					PageIndex = page,
					PageSize = pageSize,
					OrderBy = orderBy,
					OrderByDirection = orderByDirection
				});
				StringBuilder builder = new StringBuilder();
				int count = 0;
				foreach (ArchiveSearchData item in archiveItems)
				{
					var filePath = item.ArchivePath.Substring(item.ArchivePath.LastIndexOf("/") + 1);
					var thumbnail = string.IsNullOrEmpty(item.ArchiveImage) ? Translation.GetTerm("NA", "N/A") : item.ArchiveImage.Substring(item.ArchiveImage.LastIndexOf("/") + 1);
					builder.Append("<tr id=\"archive").Append(item.ArchiveID).Append("\">")
						.AppendCheckBoxCell(value: item.ArchiveID.ToString())
						.AppendLinkCell("~/Sites/Documents/Edit/" + item.ArchiveID, item.Name)
						.AppendCell(!item.StartDate.HasValue ? "N/A" : item.StartDate.ToShortDateString())
						.AppendCell(!item.EndDate.HasValue ? "N/A" : item.EndDate.ToShortDateString())
						.AppendCell(item.Active ? "Active" : "Inactive")
						.AppendCell(filePath)
						.AppendCell(thumbnail)
						.AppendCell(item.IsDownloadable.ToString())
						.AppendCell(item.IsEmailable.ToString())
						.Append("</tr>");
					++count;
				}
				return Json(new { totalPages = archiveItems.TotalPages, page = builder.ToString() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		/// <summary>
		/// Changes the status on a list of resources, called from the ResourceLibrary View
		/// </summary>
		[FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
		public virtual ActionResult ChangeStatus(List<int> items, bool active)
		{
			try
			{
				foreach (Archive item in Archive.LoadBatch(items))
				{
					if (item.Active != active)
					{
						item.Active = active;
						item.Save();
					}
				}
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		/// <summary>
		/// Displays the EditResource view
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
		public virtual ActionResult Edit(int? id)
		{
			try
			{
				if (CurrentSite == null)
					return RedirectToAction("Index", "Landing");

				EditDocumentModel Model = new EditDocumentModel();
				Archive item;
				if (id.HasValue && id.Value > 0)
					item = Archive.LoadFull(id.ToInt());
				else
				{
					item = new Archive()
					{
						StartDate = DateTime.Today,
						Active = true
					};
				}
				Model.Archive = item;
				DocumentCategoriesModel CategoriesModel = new DocumentCategoriesModel();
				CategoriesModel.ParentCategory = Category.LoadFullTree(Category.LoadFullTopLevelByCategoryTypeId((int)Constants.CategoryType.Archive).First().CategoryID);
				CategoriesModel.SelectedCategories = item.Categories != null ? item.Categories.Select(c => c.CategoryID) : new List<int>();
				Model.DocumentCategories = CategoriesModel;

				return View(Model);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		public virtual ActionResult UploadFile()
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
				case NetSteps.Common.Constants.FileType.Image:
					folder = "Images";
					break;
				case NetSteps.Common.Constants.FileType.Video:
					folder = "Videos";
					break;
				case NetSteps.Common.Constants.FileType.Flash:
					folder = "Flash";
					break;
				default:
					folder = "Documents";
					break;
			}
			string fullPath = ConfigurationManager.GetAbsoluteFolder("DocumentLibrary") + folder.AppendBackSlash() + Path.GetFileName(cleanedFileName);

			if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
				Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

			if (nonHtml5Browser)
				Request.Files[0].SaveAs(fullPath);
			else if (Request.ContentType.Equals("application/octet-stream", StringComparison.InvariantCultureIgnoreCase))
				Request.SaveAs(fullPath, false);

			var json = new { success = true, folder = folder.ToLower(), uploaded = DateTime.Now.ToString(), filePath = ConfigurationManager.GetWebFolder("DocumentLibrary") + folder.AppendForwardSlash() + Path.GetFileName(cleanedFileName) };
			if (nonHtml5Browser)
				return Content(json.ToJSON(), "text/html");
			else
				return Json(json);
		}

		public virtual ActionResult GetTranslation(int? archiveId, int languageId)
		{
			try
			{
				if (!archiveId.HasValue)
					return Json(new { result = true, name = "" });

				Archive archive = Archive.LoadFull(archiveId.Value);
				DescriptionTranslation translation = archive.Translations.GetByLanguageIdOrDefault(languageId);
				return Json(new { result = true, name = translation.Name, description = translation.ShortDescription });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		/// <summary>
		/// Saves changes to a resource, called from the EditResource view
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[AcceptVerbs(HttpVerbs.Post)]
		[FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
		public virtual ActionResult Save(int? archiveId, int languageId, string name, string description, string filePath, string thumbnail, List<int> categories, DateTime startDate, DateTime? endDate, bool active, bool isDownloadable, bool canBeEmailed, bool isFeatured)
		{
			Archive archive;
			if (archiveId.HasValue && archiveId.Value > 0)
			{
				archive = Archive.LoadFull(archiveId.Value);
			}
			else
			{
				archive = new Archive();
				archive.Sites.Add(Site.Load(CurrentSite.SiteID));
			}

			try
			{
				archive.ArchivePath = filePath.Replace(ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadAbsoluteWebPath).AppendForwardSlash(), "<!--filepath-->");
				archive.ArchiveImage = thumbnail.Replace(ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadAbsoluteWebPath).AppendForwardSlash(), "<!--filepath-->");
				archive.StartDate = startDate;
				archive.EndDate = endDate;
				archive.Active = active;
				archive.IsDownloadable = isDownloadable;
				archive.IsEmailable = canBeEmailed;
				archive.IsFeatured = isFeatured;
				archive.ArchiveDate = DateTime.Now;
				archive.LanguageID = languageId;

				archive.ArchiveTypeID = (int)Constants.ArchiveType.Image;

				var translation = archive.Translations.GetByLanguageIdOrDefaultInList(languageId);
				translation.Name = name;
				translation.ShortDescription = description;

				if (categories != null && categories.Count > 0)
				{
					archive.Categories.SyncTo(categories, c => c.CategoryID, id => Category.Load(id));
				}
				else
					archive.Categories.RemoveAll();

				archive.Save();

				// Save current site to force-update cache 
				CurrentSite.DateLastModified = DateTime.Now;
				CurrentSite.Save();

				return Json(new { result = true, archiveId = archive.ArchiveID });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		/// <summary>
		/// Displays the EditCategory view
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
		public virtual ActionResult Categories(int? categoryId)
		{
			try
			{
				if (CurrentSite == null)
					return RedirectToAction("Index", "Landing");

				List<Category> categories = Category.LoadFullTopLevelByCategoryTypeId((int)Constants.CategoryType.Archive);
				Category tree;

				if (categories == null || categories.Count == 0)
				{
					tree = new Category()
					{
						CategoryTypeID = (int)Constants.CategoryType.Archive
					};
					tree.Translations.Add(new CategoryTranslation()
					{
						LanguageID = (int)Constants.Language.English,
						Name = "Archive Categories"
					});

					tree.Save();
				}
				else
				{
					tree = categories.FirstOrDefault(c => !c.ParentCategoryID.HasValue);
				}

				if (categoryId.HasValue)
					ViewData["CategoryToEdit"] = categoryId.Value;

				ViewData["RootCategoryId"] = tree.CategoryID;
				var categoryBuilder = new StringBuilder();
				BuildTree(Category.LoadFullTree(tree.CategoryID), categoryBuilder, 1);

				var model = new EditCategoryTreeModel()
				{
					CategoryType = Constants.CategoryType.Archive,
					CategoryTree = tree,
					EditingCategoryID = categoryId,
					SaveImageURL = "~/Sites/Documents/SaveCategoryImage",
					DeleteImageURL = "~/Sites/Documents/DeleteCategoryImage",
					SaveURL = "~/Sites/Documents/SaveCategory",
					DeleteURL = "~/Sites/Documents/DeleteCategory",
					GetCategoryURL = "~/Sites/Documents/GetCategory",
					MoveURL = "~/Sites/Documents/MoveCategories",
					Links = new StringBuilder("<a href=\"").Append("~/Documents".ResolveUrl()).Append("\">Document Library</a> | <a href=\"").Append("~/Products/Documents/Edit".ResolveUrl()).Append("\">Add Document</a>").Append(" | Manage Document Categories").ToString(),
					Categories = categoryBuilder.ToString(),
				};

				return View("EditCategoryTree", "Sites", model);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		protected virtual void BuildTree(Category parent, StringBuilder builder, int level)
		{
			if (parent.ChildCategories.Count > 0)
			{
				builder.Append("<ul>");
				foreach (Category childCategory in parent.ChildCategories.OrderBySortIndex())
				{
					builder.Append("<li id=\"category").Append(childCategory.CategoryID).Append("\"><a href=\"javascript:void(0);\" class=\"category\" style=\"padding:0px 4px;\">").Append(childCategory.Translations.Name())
						.Append("</a>");
					if (level < 6)
					{
						builder.Append("<span class=\"AddCat\" id=\"addChildTo").Append(childCategory.CategoryID).Append("\" ><a id=\"addChildTo").Append(childCategory.CategoryID).Append("\" title=\"Add Child Category\" href=\"javascript:void(0);\">+</a></span>");
					}
					if (childCategory.ChildCategories.Count > 0)
					{
						BuildTree(childCategory, builder, level + 1);
					}
					builder.Append("</li>");
				}
				builder.Append("</ul>");
			}
		}

		public virtual ActionResult GetCategory(int categoryId, int? languageId)
		{
			try
			{
				if (!languageId.HasValue)
					languageId = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID;
				var contentName = String.Empty;
				var contentBody = String.Empty;
				string imageString = String.Empty;
                int IndexSort = 0;

				var content = Category.LoadTranslation(categoryId, languageId.Value);
				if (content != null)
				{
					contentName = String.IsNullOrWhiteSpace(content.Name) ? String.Empty : content.Name;
					if (content.HtmlContent != null)
					{
						contentBody = content.HtmlContent.FirstOrEmptyElement(Constants.HtmlElementType.Body).Contents ?? String.Empty;
						if (!String.IsNullOrWhiteSpace(content.HtmlContent.FirstOrEmptyElement(Constants.HtmlElementType.Image).Contents))
						{
							imageString = content.HtmlContent.FirstOrEmptyElement(Constants.HtmlElementType.Image).Contents.ToXElement().Descendant("Src").Value;
						}
                        IndexSort = content.HtmlContent.SortIndex;
					}
				}
				return Json(new
				{
					result = true,
					name = contentName,
					content = contentBody,
					image = imageString,
                    sortIndex = IndexSort,
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult MoveCategories(int parentId, List<int> categoryIds)
		{
			try
			{
				List<Category> categories = Category.LoadBatch(categoryIds);
				for (short i = 0; i < categoryIds.Count; i++)
				{
					Category category = categories.First(c => c.CategoryID == categoryIds[i]);
					if (category.SortIndex != i || category.ParentCategoryID != parentId)
					{
						category.SortIndex = i;
						category.ParentCategoryID = parentId;
						category.Save();
					}
				}

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult SaveCategoryImage()
		{
			try
			{
				if (Request.Files.Count > 0)
				{
					string fileName = Request.Files[0].FileName, folder = "Categories";
					Request.Files[0].SaveAs(ConfigurationManager.GetAbsoluteFolder(folder) + fileName);

					return Content(new
					{
						result = true,
						imagePath = ConfigurationManager.GetWebFolder(folder) + fileName
					}.ToJSON(), "text/html");
				}
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Content(new { result = false, message = exception.PublicMessage }.ToJSON(), "text/html");
			}
			return Content(new { result = false, message = Translation.GetTerm("NoFileUploaded", "No file uploaded") }.ToJSON(), "text/html");
		}

		[ValidateInput(false)]
		public virtual ActionResult SaveCategory(int? categoryId, string name, int sortIndex, string content, string image, int parentId, int languageId)
		{
			try
			{
				Category category;
				if (categoryId.HasValue)
					category = Category.LoadFull(categoryId.Value);
				else
				{
					category = new Category()
					{
						CategoryTypeID = (int)Constants.CategoryType.Archive
					};
				};

				var categoryContent = category.Translations.GetByLanguageIdOrDefaultInList(languageId);

				categoryContent.Name = name;
				categoryContent.LanguageID = languageId;
				//For some reason, sometimes the ckeditor likes to send back just a <br />, so we'll add a check for that - DES
				if ((!string.IsNullOrEmpty(content) && content.Trim() != "<br />") || !string.IsNullOrEmpty(image))
				{
					HtmlContent htmlContent = categoryContent.HtmlContent;
					if (htmlContent == null)
						htmlContent = new HtmlContent()
						{
							HtmlContentStatusID = (int)Constants.HtmlContentStatus.Production,
							PublishDate = DateTime.Now,
							Name = name,
							LanguageID = languageId
						};

					if (!string.IsNullOrEmpty(image))
					{
						Image i;
						if (htmlContent.Images.Count > 0)
						{
							i = htmlContent.Images.First();
							i.Src = image;
						}
						else
						{
							i = new Image(NetSteps.Common.Constants.ImageFolder.Categories)
							{
								Src = image
							};
							htmlContent.Images.Add(i);
						}
					}

					if (!string.IsNullOrEmpty(content))
					{
						HtmlElement bodyElement = htmlContent.FirstOrNewElement(Constants.HtmlElementType.Body);
						bodyElement.Contents = content;
					}

					if (categoryContent.HtmlContent == null)
						categoryContent.HtmlContent = htmlContent;
				}

				category.SortIndex = sortIndex;

				if (categoryId == null || parentId != categoryId)
				{
					category.ParentCategoryID = parentId;
				}

				category.Save();
				//var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				//return Json(new { result = false, message = "" });


				StringBuilder sb = new StringBuilder();
				BuildTree(Category.LoadFullTree(category.CategoryTreeID), sb, 1);
				return Json(new
				{
					result = true,
					categoryId = category.CategoryID,
					categories = sb.ToString()
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult DeleteCategory(int categoryId)
		{
			try
			{
				Category.Delete(categoryId);
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		[ValidateInput(false)]
		public virtual ActionResult DeleteCategoryImage(int? categoryId, string image, int languageId)
		{
			try
			{
				Category category = Category.LoadFull(categoryId.Value);
				var categoryContent = category.Translations.GetByLanguageIdOrDefaultInList(languageId);

				HtmlContent htmlContent = categoryContent.HtmlContent;

				//For some reason, sometimes the ckeditor likes to send back just a <br />, so we'll add a check for that - DES
				if (!string.IsNullOrEmpty(image))
				{
					Image i;
					if (htmlContent.Images.Count > 0)
					{
						i = htmlContent.Images.FirstOrDefault();
						i.Src = image;
						htmlContent.Images.Remove(i);
					}
				}

				category.Save();

				return Json(new { result = true, image = false });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
	}
}
