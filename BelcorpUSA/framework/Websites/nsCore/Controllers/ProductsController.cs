using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Controllers
{
    public class ProductsController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Inventory.InventoryLoaded && (filterContext.ActionDescriptor.ActionName != "Wait" && filterContext.ActionDescriptor.ActionName != "LoadInventory"))
            {
                Session["ProductsReturnUrl"] = filterContext.HttpContext.Request.Url.AbsoluteUri;
                filterContext.Result = RedirectToAction("Wait");
                return;
            }
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Wait()
        {
            return View();
        }

        public void LoadInventory()
        {
            Inventory.LoadInventoryCache();
        }

        #region Catalogs
        public ActionResult Index()
        {
            return RedirectToAction("CatalogManagement");
        }

        public ActionResult CatalogManagement()
        {
            return View(Inventory.Catalogs);
        }

        public ActionResult EditCatalog(int? id)
        {
            return View(id.HasValue ? Inventory.GetCatalog(id.Value) : new Catalog());
        }

        public ActionResult GetCatalogDescription(int? catalogId, int languageId)
        {
            if (!catalogId.HasValue)
                return Json(new { result = true, name = "", shortDescription = "", longDescription = "" });

            Catalog catalog = Inventory.GetCatalog(catalogId.Value);
            //DescriptionTranslation description = catalog.Descriptions.ContainsKey(languageId) ? catalog.Descriptions[languageId] : new DescriptionTranslation();

            var description = CurrentProduct.Descriptions.GetByLanguageID(languageId);
            if (description == null)
                description = new DescriptionTranslation() { LanguageID = languageId };

            return Json(new { result = true, name = description.Name, shortDescription = description.ShortDescription, longDescription = description.LongDescription });
        }

        public ActionResult SearchPossibleProducts(int catalogId, string query)
        {
            //Get the markets available to this catalog and make sure we only show products that would show up for this catalog
            IEnumerable<int> marketsToSearch = Inventory.GetCatalog(catalogId).StoreFronts.Select(sf => sf.Markets.Select(m => m.MarketID)).Flatten<int, IEnumerable<int>>();
            query = query.ToLower();
            return Json(Inventory.Products.Where(p => p.Active.ToBool() && p.Markets.Select(m => m.MarketID).Intersect(marketsToSearch).Count() > 0 && (p.SKU.ToLower().Contains(query) || p.Descriptions.Any(d => d.Name.ToLower().Contains(query) || d.ShortDescription.ToLower().Contains(query) || d.LongDescription.ToLower().Contains(query)))).Select(p => new { id = p.ProductID, text = p.SKU + " - " + p.Descriptions.Name() }));
        }

        public ActionResult GetCatalogItems(int page, int pageSize, int catalogId)
        {
            StringBuilder builder = new StringBuilder();
            List<CatalogItem> catalogItems = Inventory.GetCatalogItems(catalogId);
            foreach (CatalogItem item in catalogItems.OrderBy(ci => ci.Product.SKU).Skip(page * pageSize).Take(pageSize))
            {
                builder.Append("<tr id=\"catalogItem").Append(item.CatalogItemID).Append("\"><td><input type=\"checkbox\" class=\"catalogItemSelector\" value=\"").Append(item.CatalogItemID)
                    .Append("\" /></td><td><a href=\"").Append("~/Products/Overview/".ResolveUrl()).Append(item.Product.ProductID).Append("?isChild=true\">").Append(item.Product.SKU)
                    .Append("</a></td><td>").Append(item.Product.Descriptions.Name()).Append("</td><td class=\"startDate\">")
                    .Append(item.StartDate.HasValue ? item.StartDate.Value.ToString("g") : "N/A").Append("</td><td class=\"endDate\">")
                    .Append(item.EndDate.HasValue ? item.EndDate.Value.ToString("g") : "N/A").Append("</td></tr>");
            }
            return Json(new { result = true, resultCount = catalogItems.Count, catalogItems = builder.ToString() });
        }

        public ActionResult ChangeCatalogItemSchedules(List<int> catalogItems, DateTime? startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime)
        {
            foreach (int catalogItemId in catalogItems)
            {
                CatalogItem catalogItem = Inventory.GetCatalogItem(catalogItemId);
                if (catalogItem != default(CatalogItem))
                {
                    catalogItem.StartDate = startDate.AddTime(startTime);
                    catalogItem.EndDate = endDate.AddTime(endTime);
                    catalogItem.Save();
                }
            }
            return Json(new { result = true });
        }

        public ActionResult RemoveCatalogItems(List<int> catalogItems)
        {
            foreach (int catalogItemId in catalogItems)
            {
                CatalogItem catalogItem = Inventory.GetCatalogItem(catalogItemId);
                if (catalogItem != default(CatalogItem))
                {
                    catalogItem.Delete();
                    Inventory.CatalogItems.Remove(catalogItem);
                }
            }
            return Json(new { result = true });
        }

        public ActionResult AddCatalogItem(int catalogId, int productId, DateTime? startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime)
        {
            bool newCatalogItem = false;
            CatalogItem catalogItem = Inventory.CatalogItems.FirstOrDefault(ci => ci.CatalogID == catalogId && ci.ProductID == productId);
            if (catalogItem == default(CatalogItem))
            {
                catalogItem = new CatalogItem()
                {
                    ProductID = productId,
                    CatalogID = catalogId
                };
                newCatalogItem = true;
            }
            catalogItem.StartDate = startDate.AddTime(startTime);
            catalogItem.EndDate = endDate.AddTime(endTime);
            catalogItem.Save();
            if (newCatalogItem)
                Inventory.CatalogItems.Add(catalogItem);
            return Json(new { result = true });
        }

        public ActionResult TryAddCatalogItem(int catalogId, string query)
        {
            try
            {
                Product product = Inventory.Products.FirstOrDefault(p => p.SKU.Equals(query, StringComparison.InvariantCultureIgnoreCase) || p.Descriptions.Name().Equals(query, StringComparison.InvariantCultureIgnoreCase));
                if (product != default(Product))
                {
                    CatalogItem catalogItem = Inventory.CatalogItems.FirstOrDefault(ci => ci.CatalogID == catalogId && ci.ProductID == product.ProductID);
                    if (catalogItem == default(CatalogItem))
                    {
                        catalogItem = new CatalogItem()
                        {
                            ProductID = product.ProductID,
                            CatalogID = catalogId
                        };
                        catalogItem.Save();
                        Inventory.CatalogItems.Add(catalogItem);
                    }
                    return Json(new { result = true });
                }
            }
            catch { }
            return Json(new { result = false });
        }

        public ActionResult BulkAddCatalogItems(int catalogId, DateTime? startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime, List<int> products)
        {
            foreach (int productId in products)
            {
                bool newCatalogItem = false;
                CatalogItem catalogItem = Inventory.CatalogItems.FirstOrDefault(ci => ci.CatalogID == catalogId && ci.ProductID == productId);
                if (catalogItem == default(CatalogItem))
                {
                    catalogItem = new CatalogItem()
                    {
                        ProductID = productId,
                        CatalogID = catalogId
                    };
                    newCatalogItem = true;
                }
                catalogItem.StartDate = startDate.AddTime(startTime);
                catalogItem.EndDate = endDate.AddTime(endTime);
                catalogItem.Save();
                if (newCatalogItem)
                    Inventory.CatalogItems.Add(catalogItem);
            }
            return Json(new { result = true });
        }

        public ActionResult CopyCatalog(int catalogId, int copyCatalogId)
        {
            List<CatalogItem> existingCatalogItems = Inventory.GetCatalogItems(catalogId);
            foreach (CatalogItem item in Inventory.GetCatalogItems(copyCatalogId))
            {
                if (!existingCatalogItems.Any(ci => ci.ProductID == item.ProductID))
                {
                    CatalogItem newItem = new CatalogItem()
                    {
                        CatalogID = catalogId,
                        ProductID = item.ProductID,
                        Active = item.Active,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        SortIndex = item.SortIndex
                    };
                    newItem.Save();
                    Inventory.CatalogItems.Add(newItem);
                }
            }
            return Json(new { result = true });
        }

        public ActionResult SaveCatalog(int? catalogId, int languageId, string name, string shortDescription, string longDescription, bool active,
            DateTime? startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime, int categoryTreeId, List<int> storeFronts)
        {
            try
            {
                Catalog catalog;
                if (catalogId.HasValue)
                    catalog = Inventory.GetCatalog(catalogId.Value);
                else
                    catalog = new Catalog();

                catalog.Active = active;
                catalog.StartDate = startDate.AddTime(startTime);
                catalog.EndDate = endDate.AddTime(endTime);
                catalog.CategoryID = categoryTreeId;
                catalog.StoreFronts.Clear();
                if (storeFronts != null)
                {
                    foreach (int storeFrontId in storeFronts)
                    {
                        catalog.StoreFronts.Add(Inventory.GetStoreFront(storeFrontId));
                    }
                }

                catalog.Save();


                var description = CurrentProduct.Descriptions.GetByLanguageID(languageId);
                if (description == null)
                    description = new DescriptionTranslation() { LanguageID = languageId };

                //DescriptionTranslation description = catalog.Descriptions.ContainsKey(languageId) ? catalog.Descriptions[languageId] : new DescriptionTranslation();
                description.LanguageID = languageId;
                description.Name = name;
                description.ShortDescription = shortDescription;
                description.LongDescription = longDescription;
                //description.Save<Catalog>(catalog);

                //if (catalog.Descriptions.ContainsKey(languageId))
                //    catalog.Descriptions[languageId] = description;
                //else
                catalog.Descriptions.Add(description);

                catalog.Save();

                return Json(new { result = true, catalogId = catalog.CatalogID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region Category Trees
        public ActionResult CategoryTrees()
        {
            return View(Inventory.Categories.Where(c => !c.ParentCategoryID.HasValue));
        }

        public ActionResult DeleteCategoryTrees(List<int> categoryTrees)
        {
            foreach (int categoryId in categoryTrees)
            {
                if (Inventory.GetCatalogsUsingCategoryTree(categoryId).Count == 0)
                {
                    Category categoryTree = Inventory.GetCategory(categoryId);
                    categoryTree.Delete();
                }
            }
            return Json(new { result = true });
        }

        public ActionResult EditCategoryTree(int? id, int? categoryId)
        {
            Category category;
            if (categoryId.HasValue)
            {
                ViewData["CategoryToEdit"] = categoryId.Value;
                if (!id.HasValue)
                {
                    category = Inventory.GetCategory(categoryId.Value); // gets the current category
                    id = category.CategoryTreeID;
                }
            }
            if (id.HasValue)
            {
                category = Inventory.GetCategory(id.Value);
            }
            else
            {
                category = new Category();
            }
            StringBuilder categoryBuilder = new StringBuilder();
            BuildCategoryTree(category, categoryBuilder);
            ViewData["Categories"] = categoryBuilder.ToString();
            ViewData["Languages"] = Language.LoadAll();
            return View(category);
        }

        private void BuildCategoryTree(Category parent, StringBuilder builder)
        {
            if (parent.ChildCategories.Count > 0)
            {
                builder.Append("<ul>");
                foreach (Category childCategory in parent.ChildCategories)
                {
                    builder.Append("<li id=\"category").Append(childCategory.CategoryID).Append("\"><a href=\"javascript:void(0);\" class=\"category\" style=\"padding:0px 4px;\">").Append(childCategory.Descriptions.Name())
                        .Append("</a><span class=\"AddCat\"><a id=\"addChildTo").Append(childCategory.CategoryID).Append("\" title=\"Add Child Category\" href=\"javascript:void(0);\">+</a></span>");
                    if (childCategory.ChildCategories.Count > 0)
                    {
                        BuildCategoryTree(childCategory, builder);
                    }
                    builder.Append("</li>");
                }
                builder.Append("</ul>");
            }
        }

        public ActionResult GetCategory(int categoryId, int languageId)
        {
            Category category = Inventory.GetCategory(categoryId);
            //category.LanguageID = languageId;

            int startIndex;
            var content = category.Descriptions.GetByLanguageID(languageId);
            if (content == null)
                content = new CategoryTranslation();
            return Json(new
            {
                result = true,
                name = string.IsNullOrEmpty(content.Name) ? "" : content.Name,
                content = content.HtmlContent != null ? HtmlBuilder.GetBody(content.HtmlContent) : "",
                image = content.HtmlContent != null && !string.IsNullOrEmpty(content.HtmlContent.ParseElement("img", "image", out startIndex)) ? content.HtmlContent.ParseAttribute(content.HtmlContent.ParseElement("img", "image", out startIndex), "src") : "",
                parentId = category.ParentCategoryID
            });
        }

        public ActionResult MoveCategory(int parentId, List<int> categoryIds)
        {
            try
            {
                for (short i = 0; i < categoryIds.Count; i++)
                {
                    Category category = Inventory.GetCategory(categoryIds[i]);
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

        public ActionResult SaveImage(string folder, bool? createProductFile)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    string fileName = Request.Files[0].FileName;
                    if (!string.IsNullOrEmpty(folder) && !Directory.Exists(ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ImagesAbsolutePath) + folder))
                        Directory.CreateDirectory(ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ImagesAbsolutePath) + folder);
                    Request.Files[0].SaveAs(ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ImagesAbsolutePath) + (!string.IsNullOrEmpty(folder) ? (folder + "\\") : "") + fileName);
                    if (createProductFile.HasValue && createProductFile.Value && CurrentProduct != null)
                    {
                        ProductFile newImage = new ProductFile()
                        {
                            FilePath = "<!--imagepath-->" + (!string.IsNullOrEmpty(folder) ? (folder + "/") : "") + fileName,
                            ProductFileTypeID = Constants.ProductFileType.LargeImage.ToInt(),
                            ProductID = CurrentProduct.ProductID,
                            SortIndex = CurrentProduct.Files.GetNextSortIndex(Constants.ProductFileType.LargeImage.ToInt())
                        };
                        //newImage.Save();

                        //if (CurrentProduct.Files.ContainsProductFileTypeID(Constants.ProductFileType.LargeImage.ToInt()))
                        //    CurrentProduct.Files.Add(newImage);
                        //else
                        //    CurrentProduct.Files.Add(1, new ProductFileCollection() { newImage });
                        CurrentProduct.Save();

                        return Content(new
                        {
                            result = true,
                            fileId = newImage.ProductFileID,
                            imagePath = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ImagesWebPath) + "/" + (!string.IsNullOrEmpty(folder) ? (folder + "/") : "") + fileName
                        }.ToJSON(), "text/html");
                    }
                    return Content(new
                    {
                        result = true,
                        imagePath = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ImagesWebPath) + "/" + (!string.IsNullOrEmpty(folder) ? (folder + "/") : "") + fileName
                    }.ToJSON(), "text/html");
                    //return Json(new { result = true, imagePath = NetSteps.Common.CustomConfigurationHandler.Config.FilePaths.ImagesWebPath + fileName });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Content(new { result = false, message = exception.PublicMessage }.ToJSON(), "text/html");
            }
            return Content(new { result = false, message = "No file uploaded" }.ToJSON(), "text/html");
            //return Json(new { result = false });
        }

        [ValidateInput(false)]
        public ActionResult SaveCategory(int? categoryId, string name, int sortIndex, string content, string image, int parentId, int languageId)
        {
            try
            {
                bool newCategory = false;
                Category category;
                if (categoryId.HasValue)
                    category = Inventory.GetCategory(categoryId.Value);
                else
                {
                    newCategory = true;
                    category = new Category();
                }

                //category.LanguageID = languageId;
                //CategoryTranslation categoryContent = category.CategoryTranslations.ContainsKey(languageId) ? category.CategoryTranslations[languageId] : new CategoryTranslation();

                var categoryContent = category.Descriptions.GetByLanguageID(languageId);
                if (categoryContent == null)
                    categoryContent = new CategoryTranslation();

                categoryContent.Name = name;
                categoryContent.LanguageID = languageId;
                if (!string.IsNullOrEmpty(content) || !string.IsNullOrEmpty(image))
                {
                    HtmlContent htmlContent = categoryContent.HtmlContent;
                    if (htmlContent == null)
                        htmlContent = new HtmlContent();
                    HtmlBuilder builder = new HtmlBuilder("");
                    if (!string.IsNullOrEmpty(content))
                        builder.AppendBody(content);
                    if (!string.IsNullOrEmpty(image))
                        builder.AppendImage(image, 0, 0, "");
                    htmlContent.Html = builder.ToString();
                    htmlContent.HtmlContentStatusID = Constants.HtmlContentStatus.Production.ToInt();
                    htmlContent.Save();
                    categoryContent.HtmlContent = htmlContent;
                }
                //if (category.CategoryTranslations.ContainsLanguageID(languageId))
                //    category.CategoryTranslations[languageId] = categoryContent;
                //else
                //    category.CategoryTranslations.Add(languageId, categoryContent);

                category.Descriptions.Add(categoryContent);

                category.SortIndex = sortIndex;
                category.ParentCategoryID = parentId;
                category.Save();
                if (newCategory)
                {
                    Inventory.Categories.Add(category);
                }

                StringBuilder sb = new StringBuilder();
                BuildCategoryTree(Inventory.GetCategory(category.CategoryTreeID), sb);
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

        public ActionResult DeleteCategory(int categoryId)
        {
            try
            {
                Category category = Inventory.GetCategory(categoryId);
                category.Delete();
                Inventory.Categories.Remove(category);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpGet]
        public ActionResult NewCategoryTree()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewCategoryTree(string treeName)
        {
            try
            {
                Category newCategoryTree = new Category();
                CategoryTranslation categoryTranslation = new CategoryTranslation()
                {
                    Name = treeName,
                    LanguageID = CoreContext.CurrentLanguageID
                };
                //Save category content
                newCategoryTree.Descriptions.Add(categoryTranslation);
                newCategoryTree.Save();
                Inventory.Categories.Add(newCategoryTree);
                return RedirectToAction("EditCategoryTree", new { id = newCategoryTree.CategoryID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return View();
            }
        }
        #endregion

        #region Browse Products
        public ActionResult BrowseProducts(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                ViewData["Query"] = query;
                IEnumerable<Product> products = Inventory.Products.Where(p => p.SKU.Equals(query, StringComparison.InvariantCultureIgnoreCase) || p.Descriptions.Name().Equals(query, StringComparison.InvariantCultureIgnoreCase));
                if (products.Count() == 1)
                    return RedirectToAction("Overview", new { id = products.First().ProductID, isChild = true });
            }
            //ViewData["Categories"] = Inventory.ProductBases.Select(pb => pb.Categories).Flatten<Category, List<Category>>().Distinct();
            return View();
        }

        public ActionResult GetProducts(int page, int pageSize, string query, int? productType, bool? status, /*List<int> categories,*/ string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            IEnumerable<ProductBase> productBases = Inventory.ProductBases;
            if (!string.IsNullOrEmpty(query))
                productBases = productBases.Where(pb => pb.Products.Any(p => p.SKU.ContainsIgnoreCase(query) || p.Descriptions.Name().ContainsIgnoreCase(query)));
            if (productType.HasValue)
                productBases = productBases.Where(pb => pb.ProductTypeID == productType.Value);
            if (status.HasValue)
                productBases = productBases.Where(pb => pb.Products.Any(p => p.Active.ToBool()) == status.Value);
            //if (categories != null && categories.Count > 0)
            //{
            //    productBases = productBases.Where(p => p.Categories.Select(c => c.Id).Intersect(categories).Count() > 0);
            //}

            var pbs = productBases.Select(pb => new
            {
                Id = pb.ProductBaseID,
                SKU = pb.BaseSKU,
                Name = pb.Products.OrderBy(p => p.ProductID).First().Descriptions.Name(),
                Type = SmallCollectionCache.Instance.ProductTypes.First(pt => pt.ProductTypeID == pb.ProductTypeID).Name,
                Categories = string.Join(", ", pb.Categories.Select(c => c.Descriptions.Name())),
                Catalogs = string.Join(", ", pb.Products.Select(p => string.Join(", ", Inventory.GetCatalogItemsForProduct(p.ProductID).Select(ci => ci.Catalog.Descriptions.Name())))),
                Status = pb.Products.Any(p => p.Active.ToBool()) ? Terms.Get(CoreContext.CurrentLanguageID, "Active") : Terms.Get(CoreContext.CurrentLanguageID, "Inactive")
            });

            int resultCount = pbs.Count();

            if (resultCount == 0)
            {
                return Json(new { resultCount = resultCount, products = "<tr><td colspan=\"7\">There are no products that matched that criteria</td></tr>" });
            }

            pbs = orderByDirection == NetSteps.Common.Constants.SortDirection.Ascending ? pbs.OrderBy(orderBy) : pbs.OrderByDescending(orderBy);

            StringBuilder builder = new StringBuilder();
            int count = 0;
            foreach (var product in pbs.Skip(page * pageSize).Take(pageSize))
            {
                builder.Append("<tr class=\"GridRow").Append(count % 2 == 1 ? " Alt" : "").Append("\"><td><input id=\"chkProductBase").Append(product.Id)
                    .Append("\" type=\"checkbox\" class=\"productSelector\" /></td><td><a href=\"").Append("~/Products/Overview/".ResolveUrl()).Append(product.Id).Append("\">")
                    .Append(product.SKU).Append("</a></td><td>").Append(product.Name).Append("</td><td>").Append(product.Type)
                    .Append("</td><td>").Append(product.Categories).Append("</td><td>").Append(product.Catalogs).Append("</td><td>").Append(product.Status).Append("</td></tr>");
                ++count;
            }

            return Json(new { resultCount = resultCount, products = builder.ToString() });
        }

        public ActionResult ChangeProductStatus(List<int> productBases, bool active)
        {
            try
            {
                foreach (int productBaseId in productBases)
                {
                    ProductBase productBase = Inventory.GetProductBase(productBaseId);
                    foreach (Product product in productBase.Products)
                    {
                        if (product.Active != active)
                        {
                            product.Active = active;
                            product.Save();
                        }
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
        #endregion

        public ActionResult NewProduct()
        {
            CurrentProduct = null;
            ViewData["CategoryTree"] = BuildCategoryTree(Inventory.Categories.First(c => !c.ParentCategoryID.HasValue).ChildCategories, new List<int>());
            return View();
        }

        //public ActionResult GetProperties(int productTypeId)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    ProductType productType = SmallCollectionCache.ProductTypes.First(pt => pt.Id == productTypeId);
        //    if (productType.PropertyTypes.Count > 0)
        //    {
        //        foreach (ProductPropertyType propertyType in productType.PropertyTypes)
        //        {
        //            //ProductPropertyValueCollection values = new ProductPropertyValueCollection(propertyType.Id);
        //            //values.
        //            builder.Append("<li>").Append(propertyType.Name).Append(": <input type=\"text\" class=\"propertyValue\" id=\"propertyValueFor").Append(propertyType.Id).Append("\" /></li>");
        //        }
        //    }
        //    else
        //    {
        //        builder.Append("<li>No properties for this product type</li>");
        //    }

        //    return Content(builder.ToString());
        //}

        public ActionResult GetCategoryTree(int categoryTreeId, int? productId)
        {
            IEnumerable<int> selectedCategories = productId.HasValue ? Inventory.GetProduct(productId.Value).ProductBase.Categories.Select(c => c.CategoryID) : new List<int>();

            return Content(BuildCategoryTree(Inventory.GetCategory(categoryTreeId).ChildCategories, selectedCategories));
        }

        private string BuildCategoryTree(IList<Category> categories, IEnumerable<int> selectedCategories)
        {
            StringBuilder builder = new StringBuilder();
            if (categories.Count > 0)
            {
                builder.Append("<div id=\"categoryContainer").Append(categories.First().ParentCategoryID).Append("\" style=\"margin-left:15px;\">");
                foreach (Category category in categories)
                {
                    builder.Append("<input type=\"checkbox\" class=\"category\" value=\"").Append(category.CategoryID).Append(selectedCategories.Contains(category.CategoryID) ? "\" checked=\"checked" : "")
                        .Append("\" /><span id=\"category").Append(category.CategoryID).Append("\">").Append(category.Descriptions.Name()).Append("</span><br />");
                    if (category.ChildCategories.Count > 0)
                        builder.Append(BuildCategoryTree(category.ChildCategories, selectedCategories));
                }
                builder.Append("</div>");
            }
            return builder.ToString();
        }

        public ActionResult GetCatalogsForMarkets(List<int> markets)
        {
            StringBuilder builder = new StringBuilder();
            if (markets != null && markets.Count > 0)
            {
                foreach (Catalog catalog in Inventory.Catalogs.Where(c => c.StoreFronts.Select(sf => sf.Markets.Select(m => m.MarketID)).Flatten<int, IEnumerable<int>>().Intersect(markets).Count() > 0))
                {
                    CatalogItem catalogItem;
                    if (CurrentProduct != null && CurrentProduct.CatalogItems.Count(ci => ci.CatalogID == catalog.CatalogID) > 0)
                        catalogItem = CurrentProduct.CatalogItems.First(ci => ci.CatalogID == catalog.CatalogID);
                    else
                        catalogItem = new CatalogItem();
                    builder.Append("<input type=\"checkbox\" class=\"catalog\" onclick=\"$(this).is(':checked') && $('#catalog").Append(catalog.CatalogID)
                        .Append("').fadeIn('fast') ||  $('#catalog").Append(catalog.CatalogID).Append("').fadeOut('fast');\" value=\"").Append(catalog.CatalogID)
                        .Append(catalogItem.CatalogItemID > 0 ? "\" checked=\"checked" : "").Append("\" /><b>").Append(catalog.Descriptions.Name()).Append("</b> <span class=\"Schedule\" id=\"catalog")
                        .Append(catalog.CatalogID).Append("\"	").Append(catalogItem.CatalogItemID > 0 ? "" : "style=\"display: none;\"")
                        .Append("><input type=\"text\" class=\"TextInput DatePicker StartDate\" value=\"")
                        .Append(catalogItem.CatalogItemID == 0 || !catalogItem.StartDate.HasValue ? "Start Date" : catalogItem.StartDate.Value.ToShortDateString())
                        .Append("\" /><input type=\"text\" class=\"TimePicker StartTime\" value=\"").Append(catalogItem.CatalogItemID == 0 || !catalogItem.StartDate.HasValue ? "Start Time" : catalogItem.StartDate.Value.ToShortTimeString())
                        .Append("\" /> to <input type=\"text\" class=\"TextInput DatePicker EndDate\" value=\"")
                        .Append(catalogItem.CatalogItemID == 0 || !catalogItem.EndDate.HasValue ? "End Date" : catalogItem.EndDate.Value.ToShortDateString()).Append("\" /><input type=\"text\" class=\"TimePicker EndTime\" value=\"")
                        .Append(catalogItem.CatalogItemID == 0 || !catalogItem.EndDate.HasValue ? "End Time" : catalogItem.EndDate.Value.ToShortTimeString()).Append("\" /></span><br />");
                }
                if (builder.Length == 0)
                    builder.Append("No catalogs in the markets selected");
            }
            else
            {
                builder.Append("No markets selected");
            }

            return Json(new { result = true, catalogs = builder.ToString() });
        }

        public ActionResult SaveProduct(int productTypeId, string sku, string name, decimal weight, bool chargeShipping, bool chargeTax, bool chargeTaxOnShipping, bool isShippable,
            List<int> categories, List<int> markets, List<CatalogItem> catalogs, List<WarehouseProduct> warehouses)
        {
            try
            {
                ProductBase newProductBase = new ProductBase()
                {
                    BaseSKU = sku,
                    ProductTypeID = productTypeId,
                    TaxCategoryID = 1,
                    ChargeShipping = chargeShipping,
                    ChargeTax = chargeTax,
                    ChargeTaxOnShipping = chargeTaxOnShipping,
                    IsShippable = isShippable
                };

                if (categories != null && categories.Count > 0)
                {
                    foreach (int categoryId in categories)
                    {
                        newProductBase.Categories.Add(Inventory.GetCategory(categoryId));
                    }
                }

                //newProductBase.Save();

                DescriptionTranslation newBaseDescription = new DescriptionTranslation();
                newBaseDescription.LanguageID = CoreContext.CurrentLanguageID;
                newBaseDescription.Name = name;
                //newBaseDescription.Save(newProductBase);
                newProductBase.Descriptions.Add(newBaseDescription);
                newProductBase.Save();

                Inventory.ProductBases.Add(newProductBase);

                Product newProduct = new Product()
                {
                    SKU = sku,
                    ProductBaseID = newProductBase.ProductBaseID,
                    Active = true,
                    Weight = weight.ToDouble()
                };

                if (markets != null && markets.Count > 0)
                {
                    var allMarkets = Market.LoadAllBySiteIDAndUserID(0, 0);
                    foreach (int marketId in markets)
                    {
                        newProduct.Markets.Add(allMarkets.First(m => m.MarketID == marketId));
                    }
                }

                newProduct.Save();

                Inventory.Products.Add(newProduct);

                DescriptionTranslation newDescription = new DescriptionTranslation();
                newDescription.LanguageID = CoreContext.CurrentLanguageID;
                newDescription.Name = name;
                //newDescription.Save(newProduct);

                newProduct.Descriptions.Add(newDescription);
                newProduct.Save();

                if (catalogs != null && catalogs.Count > 0)
                {
                    foreach (CatalogItem catalogItem in catalogs)
                    {
                        catalogItem.ProductID = newProduct.ProductID;

                        catalogItem.Save();
                        Inventory.CatalogItems.Add(catalogItem);
                    }
                }

                if (warehouses != null && warehouses.Count > 0)
                {
                    foreach (WarehouseProduct warehouseProduct in warehouses)
                    {
                        warehouseProduct.ProductID = newProduct.ProductID;

                        warehouseProduct.Save();
                    }
                }

                return Json(new { result = true, productId = newProductBase.ProductBaseID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult NewProductBundle()
        {
            return View();
        }

        #region Customer Types
        public ActionResult CustomerTypes()
        {
            return View();
        }

        public ActionResult GetCustomerTypes(int storeFrontId)
        {
            return Json(Inventory.AccountPriceTypes.Where(apt => apt.StoreFrontID == storeFrontId).Select(apt => new
            {
                accountType = apt.AccountTypeID,
                relationshipType = apt.PriceRelationshipTypeID,
                priceType = apt.ProductPriceTypeID
            }));
        }

        public ActionResult SaveCustomerTypes(List<AccountPriceType> accountTypes)
        {
            foreach (AccountPriceType accountPriceType in accountTypes)
            {
                accountPriceType.Save();
                AccountPriceType cachedLookup = Inventory.AccountPriceTypes.FirstOrDefault(apt => apt.AccountTypeID == accountPriceType.AccountTypeID && apt.PriceRelationshipTypeID == accountPriceType.PriceRelationshipTypeID && apt.StoreFrontID == accountPriceType.StoreFrontID);
                if (cachedLookup == default(AccountPriceType))
                    Inventory.AccountPriceTypes.Add(accountPriceType);
                else
                    cachedLookup.ProductPriceTypeID = accountPriceType.ProductPriceTypeID;
            }
            return Json(new { result = true });
        }
        #endregion

        #region Product Edit
        private Product CurrentProduct
        {
            get { return Session["Product"] as Product; }
            set { Session["Product"] = value; }
        }

        private ActionResult CheckForProduct(int? id, string viewName = "")
        {
            Product product = null;
            if (id.HasValue)
            {
                if (!string.IsNullOrEmpty(Request.Params["isChild"]) && Request.Params["isChild"].ToBool())
                    product = CurrentProduct = Inventory.GetProduct(id.Value);
                else
                    product = CurrentProduct = Inventory.GetProductBase(id.Value).Products.First();
            }
            else if (CurrentProduct != null)
                product = CurrentProduct;

            if (product == null)
                return RedirectToAction("BrowseProducts");

            if (!string.IsNullOrEmpty(viewName))
                return View(viewName, product);
            return View(product);
        }

        public ActionResult Overview(int? id)//, bool? isBase)
        {
            return CheckForProduct(id);
        }

        public ActionResult ToggleStatus()
        {
            try
            {
                if (CurrentProduct != null)
                {
                    CurrentProduct.Active = !CurrentProduct.Active;
                    CurrentProduct.Save();
                    return Json(new { result = true });
                }
            }
            catch { }
            return Json(new { result = false });
        }

        public void ChangeCatalogItemSchedule(int catalogItemId, DateTime? startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime)
        {
            CatalogItem catalogItem = Inventory.GetCatalogItem(catalogItemId);
            catalogItem.StartDate = startDate.AddTime(startTime);
            catalogItem.EndDate = endDate.AddTime(endTime);
            catalogItem.Save();
        }

        public ActionResult Details(int? id)
        {
            return CheckForProduct(id);
        }

        public ActionResult SaveProductDetails(string sku, int productTypeId, decimal weight, bool chargeShipping, bool chargeTax, bool chargeTaxOnShipping, bool isShippable)
        {
            if (CurrentProduct != null)
            {
                CurrentProduct.SKU = sku;
                CurrentProduct.Weight = weight.ToDouble();
                CurrentProduct.Save();
                ProductBase baseProduct = Inventory.GetProductBase(CurrentProduct.ProductBaseID);
                baseProduct.ProductTypeID = productTypeId;
                baseProduct.ChargeShipping = chargeShipping;
                baseProduct.ChargeTax = chargeTax;
                baseProduct.ChargeTaxOnShipping = chargeTaxOnShipping;
                baseProduct.IsShippable = isShippable;
                baseProduct.Save();
                CurrentProduct.ProductBase = baseProduct;
                return Json(new { result = true });
            }
            return Json(new { result = false, message = "Session timed out." });
        }

        //public ActionResult Properties()
        //{
        //    return View();
        //}
        //public ActionResult ProductTypes()
        //{
        //    return View();
        //}

        public ActionResult Pricing(int? id)
        {
            return CheckForProduct(id);
        }

        public ActionResult GetPrices(int currencyId)
        {
            return Json(new
            {
                result = true,
                currencySymbol = Inventory.GetCurrency(currencyId).CurrencySymbol,
                prices = CurrentProduct.Prices.Where(pp => pp.CurrencyID == currencyId).Select(pp => new { priceTypeId = pp.ProductPriceTypeID, price = pp.Price })
            });
        }

        public ActionResult SavePrices(int currencyId, Dictionary<int, decimal> prices)
        {
            try
            {
                foreach (KeyValuePair<int, decimal> p in prices)
                {
                    ProductPrice price = CurrentProduct.Prices.FirstOrDefault(pp => pp.ProductPriceTypeID == p.Key && pp.CurrencyID == currencyId);
                    if (price == null)
                    {
                        price = new ProductPrice()
                        {
                            CurrencyID = currencyId,
                            Price = p.Value,
                            ProductID = CurrentProduct.ProductID,
                            ProductPriceTypeID = p.Key
                        };
                        price.Save();
                        CurrentProduct.Prices.Add(price);
                    }
                    else
                    {
                        price.Price = p.Value;
                        price.Save();
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

        //public ActionResult ProductVariants()
        //{
        //    return View();
        //}

        public ActionResult CMS(int? id)
        {
            return CheckForProduct(id);
        }

        public ActionResult GetProductDescription(int languageId)
        {
            if (CurrentProduct != null)
            {
                var description = CurrentProduct.Descriptions.GetByLanguageID(languageId);
                if (description == null)
                    description = new DescriptionTranslation() { LanguageID = languageId };

                return Json(new { result = true, name = description.Name, shortDescription = description.ShortDescription, longDescription = description.LongDescription });
            }
            return Json(new { result = false });
        }

        [ValidateInput(false)]
        public ActionResult SaveProductDescription(int languageId, string name, string shortDescription, string longDescription)
        {
            if (CurrentProduct != null)
            {
                var description = CurrentProduct.Descriptions.GetByLanguageID(languageId);
                if (description == null)
                {
                    description = new DescriptionTranslation() { LanguageID = languageId };
                    CurrentProduct.Descriptions.Add(description);
                }

                description.LanguageID = languageId;
                description.Name = name;
                description.ShortDescription = shortDescription;
                description.LongDescription = longDescription;
                //description.Save<Product>(CurrentProduct);

                CurrentProduct.Save();

                //if (!CurrentProduct.Descriptions.ContainsKey(languageId))
                //    CurrentProduct.Descriptions.Add(languageId, description);

                return Json(new { result = true });
            }
            return Json(new { result = false });
        }

        public ActionResult SaveImagesSort(List<int> productFiles)
        {
            try
            {
                List<ProductFile> currentProductFiles = null;
                if (CurrentProduct != null && CurrentProduct.Files.GetByProductFileTypeID(Constants.ProductFileType.LargeImage.ToInt()) != null)
                    currentProductFiles = CurrentProduct.Files.GetByProductFileTypeID(Constants.ProductFileType.LargeImage.ToInt());

                if (CurrentProduct != null && currentProductFiles != null)
                {
                    for (int i = 0; i < productFiles.Count; i++)
                    {
                        ProductFile image = currentProductFiles.First(pf => pf.ProductFileID == productFiles[i]);
                        if (image.SortIndex != i)
                        {
                            image.SortIndex = i;
                            image.Save();
                        }
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

        public ActionResult DeleteImages(List<int> productFiles)
        {
            try
            {
                List<ProductFile> currentProductFiles = null;
                if (CurrentProduct != null && CurrentProduct.Files.GetByProductFileTypeID(Constants.ProductFileType.LargeImage.ToInt()) != null)
                    currentProductFiles = CurrentProduct.Files.GetByProductFileTypeID(Constants.ProductFileType.LargeImage.ToInt());

                if (CurrentProduct != null && currentProductFiles != null)
                {
                    foreach (int productFileId in productFiles)
                    {
                        ProductFile image = currentProductFiles.First(pf => pf.ProductFileID == productFileId);
                        image.Delete();
                        CurrentProduct.Files.Remove(image);
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

        public ActionResult ProductCategories(int? id)
        {
            var result = CheckForProduct(id);
            if (!(result is RedirectToRouteResult))
                ViewData["CategoryTree"] = BuildCategoryTree(Inventory.Categories.First(c => !c.ParentCategoryID.HasValue).ChildCategories, CurrentProduct.ProductBase.Categories.Select(c => c.CategoryID));
            return result;
        }

        public ActionResult SaveProductCategories(int categoryTree, List<int> categories)
        {
            try
            {
                ProductBase productBase = CurrentProduct.ProductBase;
                while (productBase.Categories.Count(c => c.CategoryTreeID == categoryTree) > 0)
                {
                    Category category = productBase.Categories.First(c => c.CategoryTreeID == categoryTree);
                    productBase.Categories.Remove(category);
                }
                if (categories != null)
                {
                    foreach (int categoryId in categories)
                    {
                        productBase.Categories.Add(Inventory.GetCategory(categoryId));
                    }
                }
                productBase.Save();

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult Relations(int? id)
        {
            return CheckForProduct(id);
        }

        public ActionResult AddRelation(int productId, int relationshipTypeId, int childProductId)
        {
            Product product = Product.LoadFull(productId), childProduct = Product.LoadFull(childProductId);
            if (product.ChildProductRelations.ContainsRelation(relationshipTypeId, childProductId))
                return Content("");

            // TODO: We need to make sure the Inventory get updated with product changes if it is not already. - JHE
            Inventory.InsertProductRelationship(relationshipTypeId, productId, childProductId, false);
            //if (product.Relations.ContainsKey(relationshipTypeId))
            //    product.Relations[relationshipTypeId].Add(childProductId);
            //else
            //    product.Relations.Add(relationshipTypeId, new List<int>() { childProductId });

            ProductRelation productRelation = new ProductRelation();
            productRelation.ProductRelationsTypeID = relationshipTypeId;
            productRelation.ParentProductID = productId;
            productRelation.ChildProductID = childProductId;
            productRelation.Save();

            ProductRelationsType type = ProductRelationsType.LoadAll().First(rt => rt.ProductRelationTypeID == relationshipTypeId);
            return Content(new StringBuilder("<option value=\"").Append(relationshipTypeId).Append(",").Append(childProductId).Append("\">").Append(type.Name).Append(": ").Append(childProduct.SKU)
                .Append(" - ").Append(childProduct.Descriptions.Name()).Append("</option>").ToString());
        }

        public void RemoveRelations(int productId, List<KeyValuePair<int, int>> relationships)
        {
            foreach (KeyValuePair<int, int> relationship in relationships)
            {
                Inventory.DeleteProductRelationship(relationship.Key, productId, relationship.Value);
            }
        }

        public ActionResult Availability(int? id)
        {
            return CheckForProduct(id);
        }

        public ActionResult SaveProductAvailability(List<int> markets, List<CatalogItem> catalogs)
        {
            CurrentProduct.Markets.Clear();
            List<Market> allMarkets = Market.LoadAllBySiteIDAndUserID(0, 0);
            foreach (int marketId in markets)
            {
                CurrentProduct.Markets.Add(allMarkets.First(m => m.MarketID == marketId));
            }
            CurrentProduct.Save();
            foreach (CatalogItem catalogItem in catalogs)
            {
                bool newItem = false;
                CatalogItem newCatalogItem = CurrentProduct.CatalogItems.FirstOrDefault(ci => ci.CatalogID == catalogItem.CatalogID);
                if (newCatalogItem == default(CatalogItem))
                {
                    newCatalogItem = new CatalogItem()
                    {
                        Active = true,
                        CatalogID = catalogItem.CatalogID,
                        EndDate = catalogItem.EndDate,
                        ProductID = CurrentProduct.ProductID,
                        SortIndex = 0,
                        StartDate = catalogItem.StartDate
                    };
                    newItem = true;
                }
                else
                {
                    newCatalogItem.Active = true;
                    newCatalogItem.EndDate = catalogItem.EndDate;
                    newCatalogItem.StartDate = catalogItem.StartDate;
                }
                newCatalogItem.Save();
                if (newItem)
                {
                    Inventory.CatalogItems.Add(newCatalogItem);
                    CurrentProduct.CatalogItems.Add(newCatalogItem);
                }
            }
            return Json(new { result = true });
        }

        [ActionName("Inventory")]
        public ActionResult ProductInventory(int? id)
        {
            return CheckForProduct(id, "Inventory");
        }

        public ActionResult SaveProductInventory(List<WarehouseProduct> warehouseProducts)
        {
            try
            {
                foreach (WarehouseProduct warehouseProduct in warehouseProducts)
                {
                    warehouseProduct.Save();
                    if (Inventory.WarehouseProducts.Any(wp => wp.ProductID == warehouseProduct.ProductID && wp.WarehouseID == warehouseProduct.WarehouseID))
                    {
                        WarehouseProduct cachedProduct = Inventory.WarehouseProducts.First(wp => wp.ProductID == warehouseProduct.ProductID && wp.WarehouseID == warehouseProduct.WarehouseID);
                        cachedProduct.IsAvailable = warehouseProduct.IsAvailable;
                        cachedProduct.QuantityBuffer = warehouseProduct.QuantityBuffer;
                        cachedProduct.QuantityOnHand = warehouseProduct.QuantityOnHand;
                    }
                    else
                    {
                        Inventory.WarehouseProducts.Add(warehouseProduct);
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
        #endregion

        #region Warehouses

        public ActionResult Warehouses()
        {
            return View();
        }

        public ActionResult SaveWarehouseInventory(List<WarehouseProduct> wp)
        {
            foreach (WarehouseProduct warehouseProduct in wp)
            {
                warehouseProduct.Save();
            }
            Inventory.WarehouseProducts = wp;

            return Json(new { result = true });
        }

        #endregion

        #region Shipping Regions
        public ActionResult ShippingRegions()
        {
            return View();
        }

        private Dictionary<int, List<StateProvince>> Countries
        {
            get
            {
                if (Session["ShippingRegionCountries"] == null)
                    Session["ShippingRegionCountries"] = new Dictionary<int, List<StateProvince>>();
                return Session["ShippingRegionCountries"] as Dictionary<int, List<StateProvince>>;
            }
        }

        public ActionResult GetStates(int countryId, int regionId)
        {
            StringBuilder unusedStates = new StringBuilder(), regionStates = new StringBuilder();
            List<StateProvince> states;
            if (!Countries.ContainsKey(countryId))
                Countries.Add(countryId, SmallCollectionCache.Instance.StateProvinces.GetByCountryID(countryId));
            states = Countries[countryId];
            List<ShippingRegion> allShippingRegions = SmallCollectionCache.Instance.ShippingRegions.ToList();
            foreach (StateProvince state in states)
            {
                if (state.ShippingRegionID.HasValue && state.ShippingRegionID == regionId)
                    regionStates.Append("<option value=\"").Append(state.StateProvinceID).Append("\">").Append(state.Name).Append("</option>");
                else
                    unusedStates.Append("<option value=\"").Append(state.StateProvinceID).Append("\">").Append(state.Name).Append(state.ShippingRegionID.HasValue ? " (" + allShippingRegions.First(sr => sr.ShippingRegionID == state.ShippingRegionID.Value).Name + ")" : "").Append("</option>");
            }
            return Json(new { regionStates = regionStates.ToString(), unusedStates = unusedStates.ToString() });
        }

        public ActionResult AddStates(int regionId, int countryId, List<int> states)
        {
            List<StateProvince> stateProvinces;
            if (!Countries.ContainsKey(countryId))
                Countries.Add(countryId, SmallCollectionCache.Instance.StateProvinces.GetByCountryID(countryId));
            stateProvinces = Countries[countryId];
            foreach (int stateId in states)
            {
                StateProvince state = stateProvinces.FirstOrDefault(s => s.StateProvinceID == stateId);
                if (state != default(StateProvince))
                {
                    state.ShippingRegionID = regionId;
                    state.Save();
                }
            }
            return Json(new { result = true });
        }

        public ActionResult RemoveStates(int regionId, int countryId, List<int> states)
        {
            List<StateProvince> stateProvinces;
            if (!Countries.ContainsKey(countryId))
                Countries.Add(countryId, StateProvince.LoadStatesByCountry(countryId));
            stateProvinces = Countries[countryId];
            foreach (int stateId in states)
            {
                StateProvince state = stateProvinces.FirstOrDefault(s => s.StateProvinceID == stateId);
                if (state != default(StateProvince))
                {
                    state.ShippingRegionID = null;
                    state.Save();
                }
            }
            return Json(new { result = true });
        }

        public ActionResult GetWarehouses(int regionId)
        {
            StringBuilder unusedWarehouses = new StringBuilder(), regionWarehouses = new StringBuilder();
            ShippingRegion shippingRegion = ShippingRegion.LoadFull(regionId);
            foreach (var regionWarehouse in shippingRegion.ShippingRegionWarehouses.Select(s => s.Warehouse))
            {
                regionWarehouses.Append("<option value=\"").Append(regionWarehouse.WarehouseID).Append("\">").Append(regionWarehouse.Name).Append("</option>");
            }
            foreach (Warehouse warehouse in Inventory.Warehouses.Where(w => !shippingRegion.ShippingRegionWarehouses.Select(s => s.Warehouse).Select(ware => ware.WarehouseID).Contains(w.WarehouseID)))
            {
                unusedWarehouses.Append("<option value=\"").Append(warehouse.WarehouseID).Append("\">").Append(warehouse.Name).Append("</option>");
            }
            return Json(new { regionWarehouses = regionWarehouses.ToString(), unusedWarehouses = unusedWarehouses.ToString() });
        }

        public ActionResult AddWarehouses(int regionId, List<int> warehouseIDs)
        {
            ShippingRegion shippingRegion = ShippingRegion.LoadFull(regionId);
            //int max = region.ShippingRegionWarehouses.Select(s => s.Warehouse).Count() == 0 ? 0 : region.Warehouses.Keys.OrderBy(i => i).Last();
            foreach (int warehouseId in warehouseIDs)
            {
                //region.Warehouses.Add(++max, Inventory.GetWarehouse(warehouseId));
                shippingRegion.ShippingRegionWarehouses.Add(new ShippingRegionWarehouse()
                {
                    WarehouseID = warehouseId,
                    SortIndex = shippingRegion.ShippingRegionWarehouses.GetNextInt(sw => sw.SortIndex)
                });
            }
            //region.Warehouses.ReIndex();
            shippingRegion.ShippingRegionWarehouses.ReIndex();
            shippingRegion.Save();
            return Json(new { result = true });
        }

        public ActionResult RemoveWarehouses(int regionId, List<int> warehouseIDs)
        {
            ShippingRegion shippingRegion = ShippingRegion.LoadFull(regionId);
            shippingRegion.RemoveWarehouses(warehouseIDs);
            return Json(new { result = true });
        }

        public ActionResult SaveWarehouseOrder(int regionId, List<int> warehouseIDs)
        {
            ShippingRegion shippingRegion = ShippingRegion.LoadFull(regionId);
            //region.Warehouses.Clear();
            for (int i = 1; i <= warehouseIDs.Count; i++)
            {
                //region.Warehouses.Add(i, Warehouse.Load(warehouses[i - 1]));

                var shippingRegionWarehouse = shippingRegion.ShippingRegionWarehouses.FirstOrDefault(sw => sw.WarehouseID == warehouseIDs[i - 1]);
                if (shippingRegionWarehouse != null)
                    shippingRegionWarehouse.SortIndex = i;
            }
            shippingRegion.Save();
            return Json(new { result = true });
        }
        #endregion

        #region Shipping Region Management
        public ActionResult ShippingRegionManagement()
        {
            return View(SmallCollectionCache.Instance.ShippingRegions.ToList());
        }

        public ActionResult SaveShippingRegions(List<ShippingRegion> shippingRegions)
        {
            List<ShippingRegion> oldRegions = ShippingRegion.LoadAll();
            foreach (ShippingRegion eachShippingRegion in shippingRegions)
            {
                ShippingRegion shippingRegion;
                if (eachShippingRegion.ShippingRegionID > 0)
                    shippingRegion = oldRegions.First(sr => sr.ShippingRegionID == eachShippingRegion.ShippingRegionID);
                else
                {
                    shippingRegion = new ShippingRegion();
                    oldRegions.Add(shippingRegion);
                }
                if (shippingRegion.Name != eachShippingRegion.Name && !string.IsNullOrEmpty(eachShippingRegion.Name))
                {
                    shippingRegion.Name = eachShippingRegion.Name;
                    shippingRegion.Save();
                }
            }
            StringBuilder builder = new StringBuilder();
            foreach (ShippingRegion region in oldRegions)
            {
                builder.Append("<li><input type=\"text\" name=\"value").Append(region.ShippingRegionID).Append("\" value=\"").Append(region.Name)
                    .Append("\" class=\"shippingRegion\" /><a href=\"javascript:void(0);\" class=\"delete\" style=\"margin-left: 3px;\"><img src=\"../../Content/Images/Icons/remove-trans.png\" alt=\"Delete\" /></a></li>");
            }
            return Json(new { result = true, shippingRegions = builder.ToString() });
        }

        public ActionResult DeleteShippingRegion(int shippingRegionId)
        {
            ShippingRegion.Delete(shippingRegionId);
            return Json(new { result = true });
        }
        #endregion

        private void BuildCategories(int parentId, StringBuilder builder)
        {
            List<Catalog> catalogs = Inventory.GetCatalogs(parentId);
            if (catalogs.Count > 0)
            {
                builder.Append("<ul>");
                foreach (Catalog catalog in catalogs)
                {
                    builder.Append("<li").Append(parentId == 0 ? " class=\"Root\"" : "").Append("><a href=\"").Append("~/Products/Category/".ResolveUrl()).Append(catalog.CatalogID).Append("\">")
                        .Append(catalog.Descriptions.Name()).Append("</a> <span><a href=\"javascript:void(0);\" class=\"AddCat\">+</a>").Append(catalog.CatalogItems.Count).Append("</span>");
                    BuildCategories(catalog.CatalogID, builder);
                    builder.Append("</li>");
                }
                builder.Append("</ul>");
            }
        }

        public ActionResult Search(string query)
        {
            return Json(Inventory.Products.Where(p => p.Descriptions.Name().ContainsIgnoreCase(query) || p.SKU.ContainsIgnoreCase(query)).Select(p => new { id = p.ProductID, text = p.SKU + " - " + p.Descriptions.Name() }));
        }



        #region Price Types
        public ActionResult PriceTypes()
        {
            return View(Inventory.PriceTypes);
        }

        public ActionResult SavePriceTypes(List<ProductPriceType> priceTypes)
        {
            List<ProductPriceType> oldTypes = Inventory.PriceTypes;
            foreach (ProductPriceType priceType in priceTypes)
            {
                ProductPriceType productPriceType;
                if (priceType.ProductPriceTypeID > 0)
                    productPriceType = oldTypes.First(l => l.ProductPriceTypeID == priceType.ProductPriceTypeID);
                else
                {
                    productPriceType = new ProductPriceType();
                }
                if (productPriceType.Name != priceType.Name && !string.IsNullOrEmpty(priceType.Name))
                {
                    productPriceType.Name = priceType.Name;

                    productPriceType.Save();

                    if (priceType.ProductPriceTypeID == 0)
                        Inventory.PriceTypes.Add(productPriceType);
                }
            }
            StringBuilder builder = new StringBuilder();
            foreach (ProductPriceType type in Inventory.PriceTypes)
            {
                builder.Append("<li><input type=\"text\" name=\"value").Append(type.ProductPriceTypeID).Append("\" value=\"").Append(type.Name)
                    .Append("\" class=\"priceType\" /><a href=\"javascript:void(0);\" class=\"delete\" style=\"margin-left: 3px;\"><img src=\"../../Content/Images/Icons/remove-trans.png\" alt=\"Delete\" /></a></li>");
            }
            return Json(new { result = true, priceTypes = builder.ToString() });
        }

        public ActionResult DeletePriceType(int priceTypeId)
        {
            ProductPriceType productPriceType = Inventory.PriceTypes.FirstOrDefault(pt => pt.ProductPriceTypeID == priceTypeId);
            if (productPriceType != null)
            {
                productPriceType.Delete();
                Inventory.PriceTypes.Remove(productPriceType);
            }
            return Json(new { result = true });
        }
        #endregion

        //public ActionResult ProductProperties(int id)
        //{
        //	return View(Inventory.GetSingleCachedProduct(id));
        //}

        #region Promotions
        public ActionResult Promotions()
        {
            return View();
        }

        public ActionResult GetPromotions(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            StringBuilder builder = new StringBuilder();
            var promotions = Inventory.Promotions.Select(p => new
            {
                Id = p.PromotionID,
                Name = p.Name,
                Markets = string.Join(", ", p.Markets.Select(m => m.Name)),
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Active = p.Active
            });
            promotions = orderByDirection == NetSteps.Common.Constants.SortDirection.Ascending ? promotions.OrderBy(orderBy) : promotions.OrderByDescending(orderBy);

            if (promotions.Count() == 0)
                return Json(new { result = true, resultCount = 0, promotions = "<tr><td colspan=\"6\">There are no promotions</td></tr>" });

            foreach (var promotion in promotions.Skip(page * pageSize).Take(pageSize))
            {
                builder.Append("<tr><td><input type=\"checkbox\" class=\"promoSelector\" value=\"").Append(promotion.Id).Append("\" /></td><td><a href=\"")
                    .Append("~/Products/EditPromotion/".ResolveUrl()).Append(promotion.Id).Append("\">").Append(promotion.Name).Append("</a></td><td>").Append(promotion.Markets)
                    .Append("</td><td>").Append(promotion.StartDate.ToString("g")).Append("</td><td>").Append(promotion.EndDate.ToString("g")).Append("</td><td>")
                    .Append(promotion.Active ? "Active" : "Inactive").Append("</td></tr>");
            }

            return Json(new { result = true, resultCount = Inventory.Promotions.Count, promotions = builder.ToString() });
        }

        public ActionResult ChangePromotionStatus(bool active, List<int> promotions)
        {
            foreach (int promotionId in promotions)
            {
                Promotion promotion = Inventory.GetPromotion(promotionId);
                if (promotion.Active != active)
                {
                    promotion.Active = active;
                    promotion.Save();
                }
            }
            return Json(new { result = true });
        }

        public ActionResult DeletePromotions(List<int> promotions)
        {
            foreach (int promotionId in promotions)
            {
                Promotion promotion = Inventory.GetPromotion(promotionId);
                promotion.Delete();
                Inventory.Promotions.Remove(promotion);
            }
            return Json(new { result = true });
        }

        public ActionResult EditPromotion(int? id)
        {
            Promotion promotion = null;
            if (id.HasValue)
                promotion = Inventory.Promotions.FirstOrDefault(p => p.PromotionID == id.Value);
            if (promotion == null)
                promotion = new Promotion();
            return View(promotion);
        }

        public ActionResult SavePromotion(int? promotionId, string name, Constants.PromotionType type, bool active, DateTime startDate, DateTime startTime, DateTime endDate, DateTime endTime, string couponCode, float? percentDiscount, bool? oneTimeUse, List<int> markets, List<int> titles)
        {
            Promotion promotion = promotionId.HasValue ? Inventory.Promotions.First(p => p.PromotionID == promotionId.Value) : new Promotion();

            promotion.Name = name;
            promotion.PromotionTypeID = type.ToInt();
            promotion.Active = active;
            promotion.StartDate = startDate.AddTime(startTime);
            promotion.EndDate = endDate.AddTime(endTime);
            promotion.CouponCode = couponCode;
            promotion.PercentDiscount = percentDiscount.ToDecimal();
            promotion.OneTimeUse = oneTimeUse;
            promotion.Markets.Clear();
            if (markets != null && markets.Count > 0)
            {
                List<Market> allMarkets = Market.LoadAllBySiteIDAndUserID(0, 0);
                foreach (var item in markets.Select(m => allMarkets.First(ma => ma.MarketID == m)))
                    promotion.Markets.Add(item);
            }
            //promo.Titles.Clear();
            //if (titles != null)
            //    promo.Titles.AddRange(titles);

            promotion.Save();

            if (!promotionId.HasValue)
                Inventory.Promotions.Add(promotion);

            return Json(new { result = true, promotionId = promotion.PromotionID });
        }

        #region Products Panel
        public ActionResult GetPromoProducts(int promotionId, int currencyId, int page, int pageSize)
        {
            Promotion promotion = Inventory.GetPromotion(promotionId);
            StringBuilder builder = new StringBuilder();
            var products = promotion.PromotionProducts.Where(p => p.CurrencyID == currencyId);
            if (products.Count() == 0)
                return Json(new { result = true, resultCount = 0, products = "<tr><td colspan=\"4\">There are no products for this promotion with a price defined for the " + Inventory.GetCurrency(currencyId).Name + " currency</td></tr>" });

            foreach (PromotionProduct product in products.Skip(page * pageSize).Take(pageSize))
            {
                builder.Append("<tr><td><input type=\"checkbox\" class=\"promoProductSelector\" value=\"").Append(product.ProductID)
                    .Append("\" /></td><td><a href=\"").Append("~/Products/Overview/".ResolveUrl()).Append(product.ProductID).Append("?isChild=true\">").Append(product.Product.SKU)
                    .Append("</a></td><td>").Append(product.Product.Descriptions.Name()).Append("</td><td class=\"discountPrice\">").Append(product.Currency.CurrencySymbol)
                    .Append(product.DiscountPrice.ToString("F2")).Append("</td></tr>");
            }
            return Json(new { result = true, resultCount = products.Count(), products = builder.ToString() });
        }

        public ActionResult SearchPossibleProductsForPromotion(int promotionId, string query)
        {
            Promotion promotion = Inventory.GetPromotion(promotionId);
            return Json(Inventory.Products.Where(p => p.Markets.Select(m => m.MarketID).Intersect(promotion.Markets.Select(m => m.MarketID)).Count() > 0 && (p.SKU.ToLower().Contains(query) || p.Descriptions.Any(d => d.Name.ToLower().Contains(query) || d.ShortDescription.ToLower().Contains(query) || d.LongDescription.ToLower().Contains(query)))).Select(p => new { id = p.ProductID, text = p.SKU + " - " + p.Descriptions.Name() }));
        }

        public ActionResult RemovePromoProducts(int promotionId, List<int> products, int currencyId)
        {
            if (products != null)
            {
                Promotion promotion = Inventory.GetPromotion(promotionId);
                promotion.PromotionProducts.RemoveAll(p => products.Contains(p.ProductID) && p.CurrencyID == currencyId);
                promotion.Save();
            }
            return Json(new { result = true });
        }

        public ActionResult AddPromoProduct(int promotionId, int currencyId, int productId, decimal discountPrice)
        {
            Promotion promotion = Inventory.GetPromotion(promotionId);
            PromotionProduct promotionProduct = promotion.PromotionProducts.FirstOrDefault(p => p.ProductID == productId && p.CurrencyID == currencyId);
            if (promotionProduct == default(PromotionProduct))
            {
                promotionProduct = new PromotionProduct()
                {
                    ProductID = productId,
                    CurrencyID = currencyId
                };
                promotion.PromotionProducts.Add(promotionProduct);
            }
            promotionProduct.DiscountPrice = discountPrice;
            promotion.Save();
            return Json(new { result = true });
        }

        public ActionResult TryAddPromoProduct(int promotionId, int currencyId, string query, decimal discountPrice)
        {
            try
            {
                Product product = Inventory.Products.FirstOrDefault(p => p.SKU.Equals(query, StringComparison.InvariantCultureIgnoreCase) || p.Descriptions.Name().Equals(query, StringComparison.InvariantCultureIgnoreCase));
                if (product != default(Product))
                {
                    Promotion promotion = Inventory.GetPromotion(promotionId);
                    PromotionProduct promotionProduct = promotion.PromotionProducts.FirstOrDefault(p => p.ProductID == product.ProductID && p.CurrencyID == currencyId);
                    if (promotionProduct == default(PromotionProduct))
                    {
                        promotionProduct = new PromotionProduct()
                        {
                            ProductID = product.ProductID,
                            CurrencyID = currencyId
                        };
                        promotion.PromotionProducts.Add(promotionProduct);
                    }
                    promotionProduct.DiscountPrice = discountPrice;
                    promotion.Save();
                    return Json(new { result = true });
                }
            }
            catch { }
            return Json(new { result = false });
        }

        public ActionResult BulkAddPromoProducts(int promotionId, int currencyId, Dictionary<int, decimal> products)
        {
            Promotion promotion = Inventory.GetPromotion(promotionId);
            foreach (KeyValuePair<int, decimal> product in products)
            {
                PromotionProduct promotionProduct = promotion.PromotionProducts.FirstOrDefault(p => p.ProductID == product.Key && p.CurrencyID == currencyId);
                if (promotionProduct == default(PromotionProduct))
                {
                    promotionProduct = new PromotionProduct()
                    {
                        ProductID = product.Key,
                        CurrencyID = currencyId
                    };
                    promotion.PromotionProducts.Add(promotionProduct);
                }
                promotionProduct.DiscountPrice = product.Value;
            }
            promotion.Save();
            return Json(new { result = true });
        }

        public ActionResult CopyPromotionProducts(int promotionId, int copyPromotionId)
        {
            Promotion promotion = Inventory.GetPromotion(promotionId), copyPromo = Inventory.GetPromotion(copyPromotionId);
            foreach (PromotionProduct product in copyPromo.PromotionProducts)
            {
                PromotionProduct promotionProduct = promotion.PromotionProducts.FirstOrDefault(p => p.ProductID == product.ProductID && p.CurrencyID == product.CurrencyID);
                if (promotionProduct == default(PromotionProduct))
                {
                    promotionProduct = new PromotionProduct()
                    {
                        ProductID = product.ProductID,
                        CurrencyID = product.CurrencyID
                    };
                    promotion.PromotionProducts.Add(promotionProduct);
                }
                promotionProduct.DiscountPrice = product.DiscountPrice;
            }
            promotion.Save();
            return Json(new { result = true });
        }
        #endregion

        #region Accounts Panel
        public ActionResult GetPromoAccounts(int promotionId, int page, int pageSize)
        {
            StringBuilder builder = new StringBuilder();

            Promotion promotion = Inventory.GetPromotion(promotionId);
            var accountHeaders = Promotion.LoadAccountHeadersPaginated(promotion.PromotionID, new NetSteps.Common.Base.PaginatedListParameters() { PageIndex = page, PageSize = pageSize });

            if (accountHeaders.Count() == 0)
                return Json(new { result = true, resultCount = 0, products = "<tr><td colspan=\"4\">There are no accounts tied to this promotion</td></tr>" });

            //TODO: make a static method on account to pull accounts based on a list of account ids
            foreach (var accountHeader in accountHeaders)
            {
                builder.Append("<tr><td><input type=\"checkbox\" class=\"promoAccountSelector\" value=\"").Append(accountHeader.AccountID)
                    .Append("\" /></td><td><a href=\"").Append("~/Accounts/Overview/".ResolveUrl()).Append(accountHeader.AccountNumber).Append("\">").Append(accountHeader.AccountNumber)
                    .Append("</a></td><td>").Append(accountHeader.FullName).Append("</td></tr>");
            }
            return Json(new { result = true, resultCount = accountHeaders.TotalCount, accounts = builder.ToString() });
        }

        public ActionResult RemovePromoAccounts(int promotionId, List<int> accounts)
        {
            if (accounts != null)
            {
                Promotion promotion = Inventory.GetPromotion(promotionId);
                promotion.Accounts.RemoveAll(a => accounts.Contains(a.AccountID));
                promotion.Save();
            }
            return Json(new { result = true });
        }

        public ActionResult AddPromoAccount(int promotionId, int accountId)
        {
            Promotion promotion = Inventory.GetPromotion(promotionId);
            if (!Promotion.ContainsAccount(promotion.PromotionID, accountId))
            {
                Account account = Account.Load(accountId);
                promotion.Accounts.Add(account);
                promotion.Save();
            }
            return Json(new { result = true });
        }

        public ActionResult TryAddPromoAccount(int promotionId, string query)
        {
            try
            {
                List<AccountSearchData> accounts = Account.SearchNameAndAccountNumber(query);
                if (accounts.Count > 0)
                {
                    Promotion promotion = Inventory.GetPromotion(promotionId);
                    //TODO: add AccountId to AccountSearchData or just make a search method on account so this next line doesn't happen
                    Account account = Account.LoadByAccountNumber(accounts.First().AccountNumber);
                    if (!Promotion.ContainsAccount(promotion.PromotionID, account.AccountID))
                    {
                        promotion.Accounts.Add(account);
                        promotion.Save();
                    }
                    return Json(new { result = true });
                }
            }
            catch { }
            return Json(new { result = false });
        }

        public ActionResult BulkAddPromoAccounts(int promotionId, List<int> accounts)
        {
            List<int> accountIDsToAdd = new List<int>();
            Promotion promotion = Promotion.LoadFull(promotionId);
            foreach (int accountId in accounts)
            {
                if (!Promotion.ContainsAccount(promotion.PromotionID, accountId))
                    accountIDsToAdd.Add(accountId);
            }

            foreach (var account in Account.LoadBatch(accountIDsToAdd))
                promotion.Accounts.Add(account);

            promotion.Save();
            return Json(new { result = true });
        }

        public ActionResult CopyPromotionAccounts(int promotionId, int copyPromotionId)
        {
            Promotion promotion = Promotion.LoadFull(promotionId), copyPromo = Promotion.LoadFull(copyPromotionId);
            foreach (var account in copyPromo.Accounts)
            {
                if (!Promotion.ContainsAccount(promotion.PromotionID, account.AccountID))
                    promotion.Accounts.Add(account);
            }
            promotion.Save();
            return Json(new { result = true });
        }
        #endregion
        #endregion
    }
}
