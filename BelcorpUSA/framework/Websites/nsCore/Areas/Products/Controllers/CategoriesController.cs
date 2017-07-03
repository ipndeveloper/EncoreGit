using System;
using System.Collections.Generic;
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
using NetSteps.Web.Mvc.Helpers;
using nsCore.Models;

namespace nsCore.Areas.Products.Controllers
{
    public class CategoriesController : BaseProductsController
    {
        #region Category Trees
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetTrees(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                var categoryTrees = Category.Search(new NetSteps.Common.Base.FilterPaginatedListParameters<Category>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    WhereClause = c => !c.ParentCategoryID.HasValue && c.CategoryTypeID == (int)Constants.CategoryType.Product
                });
                StringBuilder builder = new StringBuilder();
                int count = 0;
                foreach (var category in categoryTrees)
                {
                    builder.Append("<tr>")
                        .AppendCell(category.UsedBy.Count() == 0 ? "<input class=\"categorySelector\" type=\"checkbox\" value=\"" + category.TreeID + "\" />" : "")
                        .AppendLinkCell("~/Products/Categories/EditTree/" + category.TreeID, category.TreeName)
                        .AppendCell(category.UsedBy.Count() == 0 ? "Not in use" : category.UsedBy.Select(ub => ub.StoreFronts + "/<a href=\"" + "~/Products/Catalogs/Edit/".ResolveUrl() + ub.CatalogID + "\">" + ub.CatalogName + "</a>").Join("<br />"))
                        .AppendCell(category.ProductCount.ToString())
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = categoryTrees.TotalPages, page = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult DeleteTrees(List<int> items)
        {
            try
            {
                foreach (int categoryTreeId in items)
                    Category.DeleteTree(categoryTreeId);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpGet]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult NewTree()
        {
            return View();
        }

        [HttpPost]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult NewTree(string treeName)
        {
            try
            {
                Category newCategoryTree = new Category()
                {
                    CategoryTypeID = (int)Constants.CategoryType.Product
                };
                //Save category content
                newCategoryTree.Translations.Add(new CategoryTranslation()
                {
                    Name = treeName,
                    LanguageID = CoreContext.CurrentLanguageID
                });
                newCategoryTree.Save();
                //nsCoreInventoryCache.Instance.Categories.Add(newCategoryTree);
                return RedirectToAction("EditTree", new { id = newCategoryTree.CategoryID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return View();
            }
        }

        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult EditTree(int? id, int? categoryId)
        {
            try
            {
                Category category;
                if (categoryId.HasValue)
                {
                    ViewData["CategoryToEdit"] = categoryId.Value;
                    if (!id.HasValue)
                    {
                        category = Category.LoadFull(categoryId.Value); // gets the current category
                        id = category.CategoryTreeID;
                    }
                }
                if (id.HasValue)
                {
                    category = Category.LoadFullTree(id.Value);
                }
                else
                {
                    return RedirectToAction("Index");
                }
                StringBuilder categoryBuilder = new StringBuilder();
                BuildTree(category, categoryBuilder, 1);

                EditCategoryTreeModel model = new EditCategoryTreeModel()
                {
                    CategoryType = Constants.CategoryType.Product,
                    CategoryTree = category,
                    EditingCategoryID = categoryId,
                    SaveImageURL = "~/Products/Categories/SaveImage",
                    SaveURL = "~/Products/Categories/Save",
					DeleteURL = "~/Products/Categories/Delete",
					DeleteImageURL = "~/Products/Categories/DeleteImage",
                    GetCategoryURL = "~/Products/Categories/Get",
                    MoveURL = "~/Products/Categories/Move",
                    Links = new StringBuilder("<a href=\"").Append("~/Products/Categories".ResolveUrl()).Append("\">Browse Category Trees</a> | <a href=\"").Append("~/Products/Categories/NewTree".ResolveUrl()).Append("\">Create a New Category Tree</a>").ToString(),
                    Categories = categoryBuilder.ToString(),
                };

                return View("EditCategoryTree", "Catalogs", model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [FunctionFilter("Products", "~/Accounts")]
        protected virtual void BuildTree(Category parent, StringBuilder builder, int level)
        {
            if (parent.ChildCategories.Count > 0)
            {
                builder.Append("<ul>");
                foreach (Category childCategory in parent.ChildCategories.OrderBy(c => c.SortIndex))
                {
                    builder.Append("<li id=\"category").Append(childCategory.CategoryID).Append("\"><a href=\"javascript:void(0);\" class=\"category\" style=\"padding:0px 4px;\">").Append(childCategory.Translations.Name())
                        .Append("</a>");
                    if (level < 6)
                    {
                        builder.Append("<span class=\"AddCat\"><a id=\"addChildTo").Append(childCategory.CategoryID).Append("\" title=\"Add Child Category\" href=\"javascript:void(0);\">+</a></span>");
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
        #endregion

        #region Single Categories
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int categoryId, int? languageId)
        {
            try
            {
                //Category category = Category.LoadFull(categoryId);
                //category.LanguageID = languageId;

                if (!languageId.HasValue)
                    languageId = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentLanguageID;
                //var content = category.Translations.GetByLanguageIdOrDefault(languageId.Value);
                var content = Category.LoadTranslation(categoryId, languageId.Value);
                if (content == null)
                    content = new CategoryTranslation();
                return Json(new
                {
                    result = true,
                    name = content.Name,
                    content = content.HtmlContent != null ? content.HtmlContent.FirstOrEmptyElement(Constants.HtmlElementType.Body).Contents : "",
                    image = content.HtmlContent != null ? content.HtmlContent.Images.Count > 0 ? content.HtmlContent.Images.UpdateFolder(NetSteps.Common.Constants.ImageFolder.Categories).First().Src : "" : "",
                    sortIndex = content.HtmlContent != null ? content.HtmlContent.SortIndex : 0,
                    //sortIndex = content.HtmlContent.SortIndex,
                    //parentId = category.ParentCategoryID
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Move(int parentId, List<int> categoryIds)
        {
            try
            {
                var categories = Category.LoadBatch(categoryIds);
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

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult SaveImage()
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
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Save(int? categoryId, string name, int sortIndex, string content, string image, int parentId, int languageId)
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
                        CategoryTypeID = (int)Constants.CategoryType.Product
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
                            i.HtmlElement.SortIndex = 0;
                        }
                    }
                    else 
                    {
                        if (htmlContent.Images.Count > 0)
                        {
                            var currentImage = htmlContent.Images.FirstOrDefault();
                            currentImage.Src = "";
                        }
                    }

                    if (!string.IsNullOrEmpty(content))
                    {
                        HtmlElement bodyElement = htmlContent.FirstOrNewElement(Constants.HtmlElementType.Body);
                        bodyElement.SortIndex = 1;
                        bodyElement.Contents = content;
                    }

                    if (categoryContent.HtmlContent == null)
                        categoryContent.HtmlContent = htmlContent;
                }

                category.SortIndex = sortIndex;
                category.ParentCategoryID = parentId;
                category.Save();

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

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Delete(int categoryId)
        {
            try
            {
                Category.DeleteTree(categoryId);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Products", "~/Accounts")]
		public virtual ActionResult DeleteImage(int? categoryId, string image, int languageId)
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
        #endregion
    }
}
