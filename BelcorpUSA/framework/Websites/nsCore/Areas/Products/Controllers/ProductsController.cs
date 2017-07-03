using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Comparer;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Diagnostics.Utilities;
using nsCore.Areas.Products.Models;
using System.Diagnostics.Contracts;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business;
using System.Transactions;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Globalization;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.EntityModels;

namespace nsCore.Areas.Products.Controllers
{
    public class ProductsController : BaseProductsController
    {
        /// <summary>
        /// Gets the inventory repository.
        /// </summary>
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult ReloadInventory()
        {
            // This call can take over a minute at times.
            this.Server.ScriptTimeout = 300;
            try
            {
                Inventory.ExpireCache();

                return Json(new { result = true, message = Translation.GetTerm("InventoryReloaded", "Inventory reloaded.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #region Browse Products
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Index(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                ViewData["Query"] = query;
                var products = Product.SlimSearch(query);
                if (products.Count() == 1)
                    return RedirectToAction("Overview", new { productId = products.First().ProductID });
            }
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        //public virtual ActionResult Get(string query, int? type, bool? active, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        public virtual ActionResult Get(string query, string bpcs, string sapSku, int? type, bool? active, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                var repo = new ProductBaseRepository();
                var productBases = repo.Search(new ProductBaseSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    Query = query,
                    ProductTypeID = type,
                    Active = active,
                    BPCS = bpcs,
                    SAPSKU = sapSku
                });

                StringBuilder builder = new StringBuilder();
                int count = 0;
                foreach (var product in productBases)
                {
                    builder.Append("<tr>")
                         .AppendCheckBoxCell(value: product.ProductBaseID.ToString())
                         .AppendLinkCell("~/Products/Products/Overview/" + product.ProductBaseID, product.SKU)
                         .AppendCell(product.Name)
                         .AppendCell(product.ProductType)
                         .AppendCell(product.Categories)
                         .AppendCell(product.Catalogs)
                         .AppendCell(product.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))

                         .Append("</tr>");
                    ++count;
                }

                return Json(new { totalPages = productBases.TotalPages, page = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetCampaignInformation(string query, string sapSku, int? type, int? periodID, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                var repo = new ProductBaseRepository();
                var products = repo.GetCampaignInformation(new ProductBaseSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    Query = query,
                    ProductTypeID = type,
                    //Active = active,
                    SAPSKU = sapSku,
                    PeriodID = Convert.ToInt32(periodID),
                    LanguageID = CoreContext.CurrentLanguageID
                });

                StringBuilder builder = new StringBuilder();
                int count = 0;
                foreach (var product in products)
                {
                    builder.Append("<tr>")
                         .AppendCell(Convert.ToString(product.PeriodID))
                         .AppendCell(Convert.ToString(product.CUV))
                         .AppendCell(Convert.ToString(product.ExternalCode))
                         .AppendCell(product.Name)
                         .AppendCell(product.SAPSKU)
                         .AppendCell(product.IsKit.Equals(2) ? Translation.GetTerm("True") : Translation.GetTerm("False"))

                         .AppendCell(Convert.ToString(product.OfferType))
                         .AppendCell(product.PrecioTabela.ToString("C",CoreContext.CurrentCultureInfo))
                         .AppendCell(product.PrecioComision.ToString("C",CoreContext.CurrentCultureInfo))
                         .AppendCell(product.Puntos.ToString(CoreContext.CurrentCultureInfo))
                         .AppendCell(product.PrecioPracticado.ToString("C",CoreContext.CurrentCultureInfo))
                         //.AppendCell(Convert.ToString(product.OfferType))
                         //.AppendCell(Convert.ToString(product.PrecioTabela))
                         //.AppendCell(Convert.ToString(product.PrecioComision))
                         //.AppendCell(Convert.ToString(product.Puntos))
                         //.AppendCell(Convert.ToString(product.PrecioPracticado))
                         .AppendCell(product.Tipo)
                         .AppendCell(product.Categorias)
                         .AppendCell(product.Catalogos)
                         .AppendCell(product.Situacion ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
                         .Append("</tr>");
                    ++count;
                }

                return Json(new { totalPages = products.TotalPages, page = builder.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult CampaignInformationExport(string parameters)
        {
            try
            {
                #region Parameters

                string[] parameter = parameters.Split(Convert.ToChar("*"));
                string query = parameter[0];
                string sapSku = parameter[1];
                int? type = string.IsNullOrEmpty(parameter[2]) ? 0 : Convert.ToInt32(parameter[2]);
                int periodID = Convert.ToInt32(parameter[3]);

                #endregion

                #region GetData

                var repo = new ProductBaseRepository();
                var data = repo.GetCampaignInformation(new ProductBaseSearchParameters()
                {
                    PageSize = 0,
                    Query = query,
                    ProductTypeID = type,
                    SAPSKU = sapSku,
                    PeriodID = periodID,
                    LanguageID = CoreContext.CurrentLanguageID
                });

                #endregion

                #region SetColumns

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("CampaignInformation", "Campaign Information"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                var columns = new Dictionary<string, string>
				{
					{"PeriodID", Translation.GetTerm("Period")},
					{"CUV", Translation.GetTerm("CUV","CUV")},
                    {"Name", Translation.GetTerm("Product","Product")},
                    {"ExternalCode", Translation.GetTerm("ExternalCode","ExternalCode")},
                    {"IsKit", Translation.GetTerm("IsKit","IsKit")},
                    {"SAPSKU", Translation.GetTerm("Material","Material")},
                    {"OfferType", Translation.GetTerm("OfferType","OfferType")},
                    {"PrecioTabela", Translation.GetTerm("Retail","Retail")},
                    {"PrecioComision", Translation.GetTerm("CommissionablePrice","CommissionablePrice")},
                    {"Puntos", Translation.GetTerm("QV","QV")},
                    {"PrecioPracticado", Translation.GetTerm("Wholesale","Wholesale")},
                    {"Tipo", Translation.GetTerm("Type","Type")},
                    {"Categorias", Translation.GetTerm("Categories","Categories")},
                    {"Catalogos", Translation.GetTerm("Catalogs","Catalogs")},
                    {"Situacion", Translation.GetTerm("Status","Status")},
					
				};

                #endregion

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<ProductSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }


        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult ChangeStatus(List<int> items, bool active)
        {
            try
            {
                foreach (int productBaseID in items)
                {
                    ProductBase.ChangeActiveStatus(productBaseID, active);
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
        public virtual ActionResult ChangeVariantProductStatus(List<int> items, bool active)
        {
            try
            {
                foreach (int productID in items)
                {
                    Product.ChangeActiveStatus(productID, active);
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
        public virtual ActionResult Search(string query)
        {
            try
            {
                query = query.ToCleanString();
                var searchResults = Product.SlimSearch(query);
                return Json(searchResults.Select(p => new { id = p.ProductID, text = p.SKU + " - " + p.Name, isVariant = p.IsVariant, productBaseId = p.ProductBaseID, isVariantTemplate = p.IsVariantTemplate, productBaseHasProperties = p.ProductBaseHasProperties }));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult DeleteVariantProduct(List<int> items)
        {
            try
            {
                Product product = Product.Load(items[0]);
                ProductBase productBase = ProductBase.LoadFull(product.ProductBaseID);

                foreach (int productID in items)
                {
                    Product.Delete(productID);
                }

                if (productBase.Products.Count() == 1)
                {
                    var variantTemplate = productBase.Products.FirstOrDefault();
                    if (variantTemplate.IsVariantTemplate)
                    {
                        variantTemplate.IsVariantTemplate = false;
                        variantTemplate.Save();
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
        public virtual ActionResult Delete(List<int> items)
        {
            try
            {
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        ProductBase.Delete(item);
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

        #region Product Creation
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Create()
        {
            var allCategories = Category.LoadFullTopLevelByCategoryTypeId((int)Constants.CategoryType.Product);
            ViewData["Categories"] = allCategories;
            if (allCategories.Count > 0)
            {
                ViewData["CategoryTree"] = BuildCategoryTree(Category.LoadFullTree(allCategories.First().CategoryID).ChildCategories, new List<int>());
            }

            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetProperties(int productTypeId)
        {
            StringBuilder builder = new StringBuilder();
            ProductType productType = ProductType.LoadFull(productTypeId);
            if (productType.ProductPropertyTypes.Count > 0)
            {
                foreach (ProductPropertyType propertyType in productType.ProductPropertyTypes)
                {
                    builder.Append("<li>").Append(propertyType.Name).Append(": ");
                    if (propertyType.ProductPropertyValues.Count == 0)
                    {
                        builder.Append("<input type=\"text\" class=\"propertyValue").Append(propertyType.DataType == "System.DateTime" ? " DatePicker" : "")
                             .Append(propertyType.DataType == "System.Int32" ? " numeric" : "")
                             .Append(propertyType.Required ? " required\" name=\"" + propertyType.Name + " is required.\"" : "")
                             .Append("\" id=\"propertyValueFor").Append(propertyType.ProductPropertyTypeID).Append("\" />");
                    }
                    else
                    {
                        builder.Append("<select class=\"propertyValue\" id=\"propertyValueFor").Append(propertyType.ProductPropertyTypeID).Append("\">");
                        if (!propertyType.Required)
                        {
                            builder.Append("<option value=\"\">No value</option>");
                        }

                        foreach (ProductPropertyValue value in propertyType.ProductPropertyValues)
                        {
                            builder.Append("<option value=\"").Append(value.ProductPropertyValueID).Append("\">").Append(value.Value).Append("</option>");
                        }

                        builder.Append("</select>");
                    }

                    builder.Append("</li>");
                }
            }
            else
            {
                builder.Append("<li>" + Translation.GetTerm("NoPropertiesForThisProductTypeAndProductBase", "No properties for this product type") + "</li>");
            }

            return Content(builder.ToString());
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetCategoryTree(int categoryTreeId, int? productId)
        {
            IEnumerable<int> selectedCategories = null;
            if (productId.HasValue)
            {
                var product = Product.Load(productId.Value);
                selectedCategories = ProductBase.LoadFull(product.ProductBaseID).Categories.Select(c => c.CategoryID);
            }
            else
            {
                selectedCategories = new List<int>();
            }

            return Content(BuildCategoryTree(Category.LoadFullTree(categoryTreeId).ChildCategories, selectedCategories));
        }

        protected virtual string BuildCategoryTree(IList<Category> categories, IEnumerable<int> selectedCategories)
        {
            var builder = new StringBuilder();
            if (categories.Count > 0)
            {
                builder.Append("<div id=\"categoryContainer").Append(categories.First().ParentCategoryID).Append("\" style=\"margin-left:15px;\">");
                foreach (Category category in categories.OrderBy(c => c.SortIndex))
                {
                    builder.Append("<input type=\"checkbox\" class=\"category\" value=\"")
                             .Append(category.CategoryID)
                             .Append(selectedCategories.Contains(category.CategoryID) ? "\" checked=\"checked" : "")
                             .Append("\" /><span id=\"category")
                             .Append(category.CategoryID)
                             .Append("\">")
                             .Append(category.Translations.Name())
                             .Append("</span><br />");


                    if (category.ChildCategories.Count > 0)
                    {
                        builder.Append(BuildCategoryTree(category.ChildCategories, selectedCategories));
                    }
                }

                builder.Append("</div>");
            }
            return builder.ToString();
        }

        public virtual ActionResult GetCatalogs(int? baseProductId, int? productId)
        {
            try
            {
                var builder = new StringBuilder();
                var product = this.GetRequestedProduct(baseProductId, productId);
                var catalogs = Catalog.LoadAllFullByMarkets(SmallCollectionCache.Instance.Markets.Select(m => m.MarketID)).Where(c => c.Active).ToList();
                foreach (var catalog in catalogs)
                {
                    CatalogItem catalogItem;
                    if (product != null && product.CatalogItems.Count(ci => ci.CatalogID == catalog.CatalogID) > 0)
                    {
                        catalogItem = product.CatalogItems.First(ci => ci.CatalogID == catalog.CatalogID);
                    }
                    else
                    {
                        catalogItem = new CatalogItem();
                    }

                    builder.Append(string.Format("<input type=\"checkbox\" class=\"catalog\" onclick=\"$(this).prop('checked') && $('#catalog{0}').fadeIn('fast') ||  $('#catalog{0}').fadeOut('fast');\" value=\"{0}\"{1} />", catalog.CatalogID, catalogItem.CatalogItemID > 0 ? " checked=\"checked\"" : ""))
                         .Append("<b>").Append(catalog.Translations.Name()).Append("</b> ")
                         .Append("<span class=\"Schedule\" id=\"catalog").Append(catalog.CatalogID).Append("\"").Append(catalogItem.CatalogItemID > 0 ? "" : " style=\"display: none;\"").Append(">")
                              .Append("<input type=\"text\" class=\"TextInput DatePicker StartDate\" value=\"").Append(catalogItem.CatalogItemID == 0 || !catalogItem.StartDate.HasValue ? "Start Date" : catalogItem.StartDate.Value.ToShortDateString()).Append("\" />")
                              .Append("<input type=\"text\" class=\"TimePicker StartTime\" value=\"").Append(catalogItem.CatalogItemID == 0 || !catalogItem.StartDate.HasValue ? "Start Time" : catalogItem.StartDate.Value.ToShortTimeString()).Append("\" />")
                              .Append(" to ")
                              .Append("<input type=\"text\" class=\"TextInput DatePicker EndDate\" value=\"").Append(catalogItem.CatalogItemID == 0 || !catalogItem.EndDate.HasValue ? "End Date" : catalogItem.EndDate.Value.ToShortDateString()).Append("\" />")
                              .Append("<input type=\"text\" class=\"TimePicker EndTime\" value=\"").Append(catalogItem.CatalogItemID == 0 || !catalogItem.EndDate.HasValue ? "End Time" : catalogItem.EndDate.Value.ToShortTimeString()).Append("\" />")
                         .Append("</span><br />");
                }

                if (builder.Length == 0)
                {
                    builder.Append("No catalogs");
                }

                return Json(new { result = true, catalogs = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void UpdateProduct(Product product, string sku, List<ProductProperty> properties, string name = null)
        {
            product.SKU = sku;
            if (!name.IsNullOrEmpty())
            {
                var description = product.Translations.GetByLanguageIdOrDefaultInList(CoreContext.CurrentLanguageID);
                if (description == null)
                {
                    var newDescription = new DescriptionTranslation();
                    newDescription.LanguageID = CoreContext.CurrentLanguageID;
                    newDescription.Name = name;
                    product.Translations.Add(newDescription);
                }
                else
                {
                    description.Name = name;
                }
            }

            if (properties != null)
            {
                product.Properties.SyncTo(
                     properties,
                     new LambdaComparer<ProductProperty>((p1, p2) => p1.ProductPropertyID == p2.ProductPropertyID, p => p.ProductPropertyID),
                     (p1, p2) =>
                     {
                         p1.PropertyValue = p2.PropertyValue.ToCleanString();
                         p1.ProductPropertyValueID = p2.ProductPropertyValueID;
                     },
                     (list, itemToRemove) =>
                     {
                         list.RemoveAndMarkAsDeleted(itemToRemove);
                     }
                );
            }
            else
            {
                product.Properties.RemoveAllAndMarkAsDeleted();
            }

            product.Save();
        }

        private Product SaveProductVariantCUV(ProductBase productBase, string sku, List<ProductProperty> properties, string name = null, double? weight = null, short? backOrderBehaviorId = null, List<CatalogItem> catalogItems = null)//, List<WarehouseProduct> warehouses = null)
        {
            var newProduct = new Product()
            {
                SKU = sku,
                ProductBaseID = productBase.ProductBaseID,
                Active = true
            };

            var variantTemplate = productBase.Products.FirstOrDefault(p => p.IsVariantTemplate);
            if (backOrderBehaviorId != null)
            {
                newProduct.ProductBackOrderBehaviorID = backOrderBehaviorId.Value;
            }
            else if (variantTemplate != null)
            {
                newProduct.ProductBackOrderBehaviorID = variantTemplate.ProductBackOrderBehaviorID;
            }

            if (weight != null)
            {
                newProduct.Weight = weight;
            }
            else if (variantTemplate != null)
            {
                newProduct.Weight = variantTemplate.Weight;
            }

            if (name != null)
            {
                var newDescription = new DescriptionTranslation();
                newDescription.LanguageID = CoreContext.CurrentLanguageID;
                newDescription.Name = name;
                newProduct.Translations.Add(newDescription);
            }
            else if (variantTemplate != null)
            {
                var newDescription = new DescriptionTranslation();
                newDescription.LanguageID = CoreContext.CurrentLanguageID;
                newDescription.Name = variantTemplate.Translations.Name();
                newProduct.Translations.Add(newDescription);
                //newProduct.Translations.Add(variantTemplate.Translations.FirstOrDefault());
            }

            if (catalogItems != null && catalogItems.Count > 0)
            {
                newProduct.CatalogItems.AddRange(catalogItems.Each(ci => { ci.Active = true; }));
            }
            else if (variantTemplate != null)
            {
                var productCatalogItems = new List<CatalogItem>();
                foreach (var catalogItem in variantTemplate.CatalogItems)
                {
                    productCatalogItems.Add(new CatalogItem { CatalogID = catalogItem.CatalogID, StartDate = catalogItem.StartDate, EndDate = catalogItem.EndDate, Active = true });
                }
                newProduct.CatalogItems.AddRange(productCatalogItems);
                //newProduct.CatalogItems.AddRange(variantTemplate.CatalogItems.Each(ci => { ci.Active = true; }));
            }

            //if (warehouses != null && warehouses.Count > 0)
            //{
            //    newProduct.WarehouseProducts.AddRange(warehouses.Each(wp => { wp.IsAvailable = true; }));
            //}
            else if (variantTemplate != null)
            {
                var productWarehouseProducts = new List<WarehouseProduct>();
                foreach (var warehouseProduct in variantTemplate.WarehouseProducts)
                {
                    productWarehouseProducts.Add(
                         new WarehouseProduct
                         {
                             WarehouseID = warehouseProduct.WarehouseID,
                             QuantityOnHand = warehouseProduct.QuantityOnHand,
                             QuantityBuffer = warehouseProduct.QuantityBuffer,
                             ReorderLevel = warehouseProduct.ReorderLevel,
                             IsAvailable = true
                         });
                }
                newProduct.WarehouseProducts.AddRange(productWarehouseProducts);
                //newProduct.WarehouseProducts.AddRange(variantTemplate.WarehouseProducts.Where(wp => wp.IsAvailable).ToTrackableCollection());
            }

            //if (variantTemplate != null)
            //{
            //    var productPrices = new List<ProductPrice>();
            //    foreach (var productPrice in variantTemplate.Prices)
            //    {
            //        productPrices.Add(
            //             new ProductPrice
            //             {
            //                 CurrencyID = productPrice.CurrencyID,
            //                 Price = productPrice.Price,
            //                 ProductPriceTypeID = productPrice.ProductPriceTypeID
            //             });
            //    }
            //    newProduct.Prices.AddRange(productPrices);
            //}

            if (properties != null && properties.Count > 0)
            {
                newProduct.Properties.AddRange(properties);
            }
            else if (variantTemplate != null)
            {
                newProduct.Properties.AddRange(variantTemplate.Properties);
            }

            if (productBase.Products.Count > 1)
            {
                if (newProduct.ProductVariantInfo == null)
                {
                    newProduct.ProductVariantInfo = new ProductVariantInfo
                    {
                        UseMasterPricing = true
                    };
                }
            }

            if (productBase.ProductBaseID != 0)
            {
                newProduct.Save();
            }

            return newProduct;
        }

        private Product SaveProduct(ProductBase productBase, string sku, List<ProductProperty> properties, string name = null, double? weight = null, short? backOrderBehaviorId = null, List<CatalogItem> catalogItems = null)//, List<WarehouseProduct> warehouses = null)
        {
            var newProduct = new Product()
                 {
                     SKU = sku,
                     ProductBaseID = productBase.ProductBaseID,
                     Active = true
                 };

            var variantTemplate = productBase.Products.FirstOrDefault(p => p.IsVariantTemplate);
            if (backOrderBehaviorId != null)
            {
                newProduct.ProductBackOrderBehaviorID = backOrderBehaviorId.Value;
            }
            else if (variantTemplate != null)
            {
                newProduct.ProductBackOrderBehaviorID = variantTemplate.ProductBackOrderBehaviorID;
            }

            if (weight != null)
            {
                newProduct.Weight = weight;
            }
            else if (variantTemplate != null)
            {
                newProduct.Weight = variantTemplate.Weight;
            }

            if (name != null)
            {
                var newDescription = new DescriptionTranslation();
                newDescription.LanguageID = CoreContext.CurrentLanguageID;
                newDescription.Name = name;
                newProduct.Translations.Add(newDescription);
            }
            else if (variantTemplate != null)
            {
                var newDescription = new DescriptionTranslation();
                newDescription.LanguageID = CoreContext.CurrentLanguageID;
                newDescription.Name = variantTemplate.Translations.Name();
                newProduct.Translations.Add(newDescription);
                //newProduct.Translations.Add(variantTemplate.Translations.FirstOrDefault());
            }

            if (catalogItems != null && catalogItems.Count > 0)
            {
                newProduct.CatalogItems.AddRange(catalogItems.Each(ci => { ci.Active = true; }));
            }
            else if (variantTemplate != null)
            {
                var productCatalogItems = new List<CatalogItem>();
                foreach (var catalogItem in variantTemplate.CatalogItems)
                {
                    productCatalogItems.Add(new CatalogItem { CatalogID = catalogItem.CatalogID, StartDate = catalogItem.StartDate, EndDate = catalogItem.EndDate, Active = true });
                }
                newProduct.CatalogItems.AddRange(productCatalogItems);
                //newProduct.CatalogItems.AddRange(variantTemplate.CatalogItems.Each(ci => { ci.Active = true; }));
            }

            //if (warehouses != null && warehouses.Count > 0)
            //{
            //    newProduct.WarehouseProducts.AddRange(warehouses.Each(wp => { wp.IsAvailable = true; }));
            //}
            else if (variantTemplate != null)
            {
                var productWarehouseProducts = new List<WarehouseProduct>();
                foreach (var warehouseProduct in variantTemplate.WarehouseProducts)
                {
                    productWarehouseProducts.Add(
                         new WarehouseProduct
                         {
                             WarehouseID = warehouseProduct.WarehouseID,
                             QuantityOnHand = warehouseProduct.QuantityOnHand,
                             QuantityBuffer = warehouseProduct.QuantityBuffer,
                             ReorderLevel = warehouseProduct.ReorderLevel,
                             IsAvailable = true
                         });
                }
                newProduct.WarehouseProducts.AddRange(productWarehouseProducts);
                //newProduct.WarehouseProducts.AddRange(variantTemplate.WarehouseProducts.Where(wp => wp.IsAvailable).ToTrackableCollection());
            }

            if (variantTemplate != null)
            {
                var productPrices = new List<ProductPrice>();
                foreach (var productPrice in variantTemplate.Prices)
                {
                    productPrices.Add(
                         new ProductPrice
                         {
                             CurrencyID = productPrice.CurrencyID,
                             Price = productPrice.Price,
                             ProductPriceTypeID = productPrice.ProductPriceTypeID
                         });
                }
                newProduct.Prices.AddRange(productPrices);
            }

            if (properties != null && properties.Count > 0)
            {
                newProduct.Properties.AddRange(properties);
            }
            else if (variantTemplate != null)
            {
                newProduct.Properties.AddRange(variantTemplate.Properties);
            }

            if (productBase.Products.Count > 1)
            {
                if (newProduct.ProductVariantInfo == null)
                {
                    newProduct.ProductVariantInfo = new ProductVariantInfo
                                                                    {
                                                                        UseMasterPricing = true
                                                                    };
                }
            }

            if (productBase.ProductBaseID != 0)
            {
                newProduct.Save();
            }

            return newProduct;
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult RenderVariantSKU(int productBaseId)
        {
            try
            {
                var productBase = ProductBase.LoadFull(productBaseId);

                var viewDataDictionary = new ViewDataDictionary();
                viewDataDictionary.Add("ProductBase", productBase);

                return Json(new { result = true, variantSKUHTML = RenderPartialToString("VariantSKU", viewDataDictionary, new VariantProductModel { Product = new Product { } }) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult SaveVariantProducts(int productBaseId, List<VariantProductModel> variantProductModels)
        {
            try
            {
                List<VariantProductModel> lisTValidacionRetorno = new List<VariantProductModel>();
                int index = 0; bool findError = false;
                foreach (var objValidSKU in variantProductModels.Where(x => x.ProductId == 0).ToList())
                {
                    var product = Product.LoadFullBySKU(objValidSKU.Sku);
                    if (product != null)
                    {
                        lisTValidacionRetorno.Add(new VariantProductModel
                        {
                            ProductId = index,
                            Sku = objValidSKU.Sku
                        });
                        findError = true;
                        break;
                    }
                    //else
                    //{
                    //    lisTValidacionRetorno.Add(new VariantProductModel
                    //    {
                    //        ProductId = index
                    //    });
                    //}
                    //index++;
                }
                if (findError)
                {
                    return Json(new
                    {
                        result = false,
                        lisTValidacionRetorno = lisTValidacionRetorno.Select(x => x.Sku).ToList()
                    });
                }


                bool productCreated = false;
                ProductBase pb = ProductBase.LoadFull(productBaseId);

                if (pb == null)
                {
                    return Json(new { result = false, message = Translation.GetTerm("BaseProductNotFound", "Could not find the base product") });
                }

                if (pb.Products.Count == 1)
                {
                    var firstChild = pb.Products[0];
                    if (!firstChild.IsVariantTemplate)
                    {
                        firstChild.IsVariantTemplate = true;
                        firstChild.Save();
                    }
                }

                foreach (VariantProductModel vpm in variantProductModels)
                {

                    int ProductID = vpm.ProductId.ToInt();
                    if (!vpm.Sku.IsNullOrEmpty())
                    {
                        var actualProperties = vpm.Properties.Where(prop => prop.ProductPropertyValueID != 0).ToList();
                        actualProperties.Each(x => x.ProductPropertyTypeID = ProductPropertyValue.Load(x.ProductPropertyValueID.ToInt()).ProductPropertyTypeID);

                        if (actualProperties != null && actualProperties.Count > 0)
                        {
                            if (vpm.Name.IsNullOrEmpty())
                                return Json(new { result = false, message = Translation.GetTerm("ProductNameIsRequired", "Product Name is required.") });
                            if (vpm.ProductId != null && vpm.ProductId > 0)
                            {
                                UpdateProduct(pb.Products.FirstOrDefault(p => p.ProductID == vpm.ProductId), vpm.Sku, actualProperties, vpm.Name);
                            }
                            else
                            {
                                Product newProduct = SaveProductVariantCUV(pb, vpm.Sku, actualProperties, vpm.Name);
                                ProductID = newProduct.ProductID;
                            }

                            ProductExtensions.InsertVariants(productBaseId, vpm.CodigoSap, vpm.OffertType, vpm.ExternalCode, ProductID);
                            productCreated = true;
                        }
                    }
                }

                if (!productCreated)
                {
                    return Json(new { result = false, message = Translation.GetTerm("PleaseEnterTheSKUAndSetAtLeastOneProperty", "Please enter the sku and set at least one property") });
                }

                CopyExcludedShippingMethodsFromProductBase(pb);

                return Json(new { result = true, productId = pb.ProductBaseID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

        }


        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Save(int productTypeId, string sku, string name,
            short backOrderBehaviorId, double? weight, int? taxCategoryId,
            bool chargeShipping, bool chargeTax, bool chargeTaxOnShipping,
            bool isShippable, int currencyId,
            List<int> categories, List<CatalogItem> catalogs, //List<WarehouseProduct> warehouses,
            List<ProductProperty> properties, List<ProductPricesCatalog> prices)
        {
            // Make this transactional so that if an exception occurs in the middle of this process we don't have a successful ProductBase saved
            //  but an unsaved Product. - JHE
            try
            {
                if (Product.LoadFullBySKU(sku) != null)
                {
                    return Json(new { result = false, message = "CUV already registered." });
                }
                else
                {
                    var newProductBase = new ProductBase()
                    {
                        BaseSKU = sku,
                        ProductTypeID = productTypeId,
                        TaxCategoryID = taxCategoryId,
                        ChargeShipping = chargeShipping,
                        ChargeTax = chargeTax,
                        ChargeTaxOnShipping = chargeTaxOnShipping,
                        IsShippable = isShippable
                    };

                    if (categories != null && categories.Count > 0)
                    {
                        foreach (int categoryId in categories.Distinct())
                        {
                            newProductBase.Categories.Add(Category.Load(categoryId));
                        }
                    }

                    DescriptionTranslation newBaseDescription = new DescriptionTranslation();
                    newBaseDescription.LanguageID = CoreContext.CurrentLanguageID;
                    newBaseDescription.Name = name;
                    newProductBase.Translations.Add(newBaseDescription);

                    Product newProduct = SaveProduct(newProductBase, sku, properties, name, weight, backOrderBehaviorId, catalogs);//, warehouses

                    newProductBase.Products.Add(newProduct);
                    newProductBase.Save();

                    foreach (var price in prices)
                    {
                        //if (price == null)
                        //{
                        var pp = new ProductPricesCatalog
                        {
                            currencyId = currencyId,
                            Price = price.Price,
                            productId = newProduct.ProductID,
                            ProductPriceTypeID = price.ProductPriceTypeID,
                            catalogID = price.catalogID
                        };
                        PricesPerCatalogDataAcces.InsertProductPrices(pp);
                        //newProduct.Prices.Add(price);
                        //}
                        //else
                        //{
                        //    price.Price = pp.Value;
                        //}
                    }
                    return Json(new { result = true, productId = newProductBase.ProductBaseID });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region Product Edit

        protected virtual ActionResult CheckForProduct(int? baseProductId, int? productId, string viewName = "", bool? reloadProduct = false)
        {
            try
            {
                Product product = GetRequestedProduct(baseProductId, productId);

                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                ViewBag.DisplayChargeTaxOnShipping = ShouldDisplayChargeTaxOnShipping();

                if (!string.IsNullOrEmpty(viewName))
                {
                    return View(viewName, product);
                }

                return View(product);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
        #endregion

        #region Overview
        public virtual ActionResult Overview(int? baseProductId, int? productId)
        {
            //Create model
            var model = new ProductOverviewModel();

            //Load the product and check if it exists
            var product = this.GetRequestedProduct(baseProductId, productId);

            //If the product doesn't exist, send back to all products page
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            int languageID = CoreContext.CurrentLanguageID;
            var catalogos = PricesPerCatalogDataAcces.SearchCatalogs(product.ProductID, languageID);
            if (catalogos == null) { catalogos = new List<PricesPerCatalogsData>(); }
            ViewData["CatalogsID"] = catalogos;

            var activeCurrencies = (from country in SmallCollectionCache.Instance.Countries
                                    join currency in SmallCollectionCache.Instance.Currencies on country.CurrencyID equals currency.CurrencyID
                                    where country.Active
                                    select currency).Distinct().ToList();
            Currency firstCurrency = activeCurrencies.FirstOrDefault();

            var catalogID = 0;
            if (catalogos.Count != 0)
            {

                catalogID = catalogos.First().CatalogID;
            }

            var p = (PricesPerCatalogDataAcces.SearchProductPrices(product.ProductID, firstCurrency.CurrencyID, catalogID)).Select(pp => new { priceTypeId = pp.ProductPriceTypeID, price = pp.Price }).ToList();
            List<ProductPrice> listReg = new List<ProductPrice>();
            int o = 0;
            for (o = 0; o < p.Count; o++)
            {
                listReg.Add(new ProductPrice { Price = p[o].price, ProductPriceTypeID = p[o].priceTypeId });
            }

            ViewData["ProductPrices"] = listReg;

            model.Product = product;
            model.VariantProductPropertyValues = GetVariantProductPropertyValues(product);
            model.VariantProductTranslations = GetVariantProductTranslations(product);

            return View(model);
        }

        public virtual ActionResult GetPricesCatalogsOverview(int? baseProductId, int productId, int catalogID)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                product.ProductBase = ProductBase.LoadFull(product.ProductBaseID);
                int currencyId = CoreContext.CurrentLanguageID;

                var activeCurrencies = (from country in SmallCollectionCache.Instance.Countries
                                        join currency in SmallCollectionCache.Instance.Currencies on country.CurrencyID equals currency.CurrencyID
                                        where country.Active
                                        select currency).Distinct().ToList();
                Currency firstCurrency = activeCurrencies.FirstOrDefault();


                return Json(new
                {
                    result = true,
                    prices = (PricesPerCatalogDataAcces.SearchProductPrices(productId, firstCurrency.CurrencyID, catalogID)).Select(pp => new { priceTypeId = pp.ProductPriceTypeID, price = pp.Price }),
                    CurrencySymbol = firstCurrency.CurrencySymbol
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// ProductProperties and ProductPropertyValues are not loaded with the Product.LoadFull() for variant products.
        /// Instead of adding this overhead information into the LoadFull, this will get access to the needed info.
        /// </summary>
        /// <param name="product">The Product that we need all of the variant ProductProperty information for</param>
        /// <returns>Key: Variant's ProductID - Value: ProductProperty's ProductPropertyValue</returns>
        public Dictionary<int, ProductPropertyValue> GetVariantProductPropertyValues(Product product)
        {

            //Create List
            var variantProductPropertyValues = new Dictionary<int, ProductPropertyValue>();

            //Loop through the variant products if there are any and load the product properties for each product
            if (product.ProductBase != null)
            {
                foreach (var variantProduct in product.ProductBase.Products.Where(x => x.ProductID != product.ProductID))
                {
                    //Grab the productProperties and load the ProductProperty fresh.
                    var properties = ProductProperty.LoadByProductID(variantProduct.ProductID);

                    //The ProductProperty will have a ProductPropertyValueID
                    //The productPropertyValue for variants is not included in the Product.LoadFull()
                    foreach (var property in properties)
                    {
                        //If the property has a valueID
                        //Not all productProperties are associated with a value
                        if (property.ProductPropertyValueID.HasValue)
                        {
                            var value = ProductPropertyValue.Load(property.ProductPropertyValueID.Value);
                            if (value != null)
                            {
                                //load the value into the list to be used by the view.
                                if (!variantProductPropertyValues.ContainsKey(variantProduct.ProductID))
                                {
                                    variantProductPropertyValues.Add(variantProduct.ProductID, value);
                                }
                            }
                        }
                    }
                }
            }
            return variantProductPropertyValues;
        }

        /// <summary>
        /// Product Description Translations are not loaded for variant products in the normal Product.LoadFull()
        ///	Instead of adding this overhead information into the LoadFull, this will get access to the needed info.
        /// </summary>
        /// <param name="product">The Product that we need the variant translations for</param>
        /// <returns>Key: The variant ProductID - Value: The Translation for this Product</returns>
        public Dictionary<int, IEnumerable<DescriptionTranslation>> GetVariantProductTranslations(Product product)
        {
            //Product Description Translations are not loaded for variant products in the normal Product.LoadFull()
            //Instead of adding this overhead information into the loadFull, this will get access to the needed info

            //Create
            var variantProductTranslations = new Dictionary<int, IEnumerable<DescriptionTranslation>>();
            foreach (var variantProduct in product.ProductBase.Products.Where(x => x.ProductID != product.ProductID))
            {
                //For each variant product, check to see if there is a translation.
                //This is a list, but from what I can tell, the relationship between translation and product is a 1-1
                //The load returns a list just in case this variant load needs to be altred later to accept multiple properties
                var translations = DescriptionTranslation.LoadByProductID(variantProduct.ProductID);

                //Check to see if a translation was returned.
                //If a translation is returned, add the first DescriptionTranslation to the dictionary with the key being the Variant's ProductID
                if (translations.Count() > 0)
                {
                    variantProductTranslations.Add(variantProduct.ProductID, translations);
                }
            }
            return variantProductTranslations;
        }

        public virtual ActionResult ToggleStatus(int? baseProductId, int? productId)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                if (product != null)
                {
                    product.Active = !product.Active;
                    product.Save();
                    return Json(new { result = true });
                }
                else
                {
                    return Json(new { result = false });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult ChangeCatalogItemSchedule(int? baseProductId, int? productId, int catalogItemId, DateTime? startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                var catalogItem = product.CatalogItems.FirstOrDefault(ci => ci.CatalogItemID == catalogItemId);
                if (catalogItem != default(CatalogItem))
                {
                    catalogItem.StartDate = startDate.AddTime(startTime);
                    catalogItem.EndDate = endDate.AddTime(endTime);
                    product.Save();
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
            return Json(new { result = true });
        }
        #endregion

        #region Details

        public virtual ActionResult Details(int? baseProductId, int? productId)
        {
            var product = this.GetRequestedProduct(baseProductId, productId);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ProductDetailsModel(product)
                                 {
                                     AvailableShippingMethods = ShippingMethod.LoadAll(),
                                     ExcludedShippingMethodIds = product.ExcludedShippingMethods.Select(esm => esm.ShippingMethodID).ToList(),
                                     DisplayChargeTaxOnShipping = ShouldDisplayChargeTaxOnShipping()
                                 };

            return View(model);
        }

        public virtual ActionResult SaveDetails(int? baseProductId, int? productId, ProductDetailsModel model)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                if (product != null)
                {
                    UpdateCurrentProductFromProductDetailsViewModel(product, model);
                    UpdateBaseProductFromProductDetailsViewModel(product, model);

                    product.Save();

                    return Json(new { result = true });
                }
                return Json(new { result = false, message = Translation.GetTerm("SessionTimedOut", "Session timed out.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #endregion

        #region Pricing
        public virtual ActionResult Pricing(int? baseProductId, int? productId)
        {
            int languageID = CoreContext.CurrentLanguageID;

            var catalogos = PricesPerCatalogDataAcces.SearchCatalogs(productId.Value, languageID);
            if (catalogos == null)
            {
                catalogos = new List<PricesPerCatalogsData>();
            }
            {
                ViewData["CatalogsID"] = catalogos;
            }

            return CheckForProduct(baseProductId, productId);

        }

        public virtual ActionResult GetPrices(int? baseProductId, int? productId, int currencyId)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                product.ProductBase = ProductBase.LoadFull(product.ProductBaseID);

                return Json(new
                {
                    result = true,
                    currencySymbol = SmallCollectionCache.Instance.Currencies.GetById(currencyId).CurrencySymbol,
                    prices = product.Prices.Where(pp => pp.CurrencyID == currencyId).Select(pp => new { priceTypeId = pp.ProductPriceTypeID, price = pp.Price }),
                    isVariant = product.IsVariant(),
                    useMasterPricing = product.ProductVariantInfo == null || product.ProductVariantInfo.UseMasterPricing
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }



        //KLC - CSTI (Precios por catalgo)
        public virtual ActionResult GetPricesCatalogs(int? baseProductId, int productId, int currencyId, int catalogID)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                product.ProductBase = ProductBase.LoadFull(product.ProductBaseID);

                return Json(new
                {
                    result = true,
                    currencySymbol = SmallCollectionCache.Instance.Currencies.GetById(currencyId).CurrencySymbol,
                    prices = (PricesPerCatalogDataAcces.SearchProductPrices(productId, currencyId, catalogID)).Select(pp => new { priceTypeId = pp.ProductPriceTypeID, price = pp.Price }),
                    isVariant = product.IsVariant(),
                    useMasterPricing = product.ProductVariantInfo == null || product.ProductVariantInfo.UseMasterPricing
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        //


        public virtual ActionResult SavePrices(int? baseProductId, int? productId, int currencyId, Dictionary<int, string> prices, int catalogID)
        {
            prices = prices ?? new Dictionary<int, string>();
            try
            {



                PricesPerCatalogDataAcces.DeleteProductPrices(new ProductPricesCatalog { catalogID = catalogID, productId = productId.ToInt() });

                var product = this.GetRequestedProduct(baseProductId, productId);
                foreach (var p in prices)
                {
                    //var price = product.Prices.FirstOrDefault(pp => pp.ProductPriceTypeID == p.Key && pp.CurrencyID == currencyId);
                    //KLC -CSTI
                    //if (price == null)
                    //{
                    var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
                    var culture = CultureInfoCache.GetCultureInfo("En");

                    var price = new ProductPricesCatalog
                        {
                            ProductPriceTypeID = p.Key,
                            productId = product.ProductID,
                            currencyId = currencyId,
                            catalogID = catalogID,
                            //Price = Convert.ToDecimal(p.Value.ToString(), cultures),


                            Price = (KeyDecimals == "ES") ? Convert.ToDecimal(p.Value.ToString(), culture) : Convert.ToDecimal(p.Value.ToString())

                        };
                    PricesPerCatalogDataAcces.InsertProductPrices(price);
                    //    product.Prices.Add(price);
                    //}
                    //else
                    //{						
                    //    price.Price = p.Value;
                    //}

                }

                var pricesToRemove = product.Prices.Where(x => x.CurrencyID == currencyId && !prices.ContainsKey(x.ProductPriceTypeID)).ToList();
                foreach (var productPrice in pricesToRemove.Where(productPrice => productPrice != null))
                {
                    product.Prices.RemoveAndMarkAsDeleted(productPrice);
                }

                var productPriceTypesToRemove = pricesToRemove.Select(x => x.ProductPriceTypeID);

                //product.Save();
                /*
				if (product.IsVariantTemplate)
				{
					IEnumerable<Product> variantProducts = Product.GetVariants(product.ProductID);

					foreach (var variant in variantProducts)
					{
						if (variant.ProductVariantInfo == null)
						{
							variant.ProductVariantInfo = new ProductVariantInfo { UseMasterPricing = true };
						}

						if (variant.ProductVariantInfo.UseMasterPricing)
						{
							foreach (var p in product.Prices)
							{
								var variantPrice = variant.Prices.FirstOrDefault(x => x.ProductPriceTypeID == p.ProductPriceTypeID && x.CurrencyID == p.CurrencyID);

								// add
								if (variantPrice == null)
								{
									variant.Prices.Add(
										 new ProductPrice
										 {
											 Price = p.Price,
											 ProductPriceTypeID = p.ProductPriceTypeID,
											 CurrencyID = p.CurrencyID
										 });
									continue; // exit loop
								}

								// delete
								if (productPriceTypesToRemove.Contains(variantPrice.ProductPriceTypeID))
								{
									variant.Prices.RemoveAndMarkAsDeleted(variantPrice);
									continue;
								}

								// update
								if (variantPrice.Price != p.Price)
								{
									var newPrice = ProductPrice.Load(variantPrice.ProductPriceID);
									newPrice.Price = p.Price;
									newPrice.Save();
								}
							}
						}

						variant.Save();
					}
				}
                */
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult ChangeUseMasterPricingInfo(int? baseProductId, int? productId, bool useMasterPricing)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                if (product.ProductVariantInfo != null)
                {
                    product.ProductVariantInfo.UseMasterPricing = useMasterPricing;
                }
                else
                {
                    product.ProductVariantInfo = new ProductVariantInfo
                                                                {
                                                                    UseMasterPricing = useMasterPricing
                                                                };
                }

                if (useMasterPricing)
                {
                    UpdateVariantWithMasterPricing(product);
                }

                product.Save();

                return Json(new
                                     {
                                         result = true
                                     });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public void UpdateVariantWithMasterPricing(Product variant)
        {
            var productBase = ProductBase.LoadFull(variant.ProductBaseID);
            var masterProduct = productBase.Products.FirstOrDefault(p => p.IsVariantTemplate);

            if (masterProduct == null) return;

            foreach (var price in masterProduct.Prices)
            {
                if (!variant.Prices.Select(i => i.ProductPriceTypeID).Contains(price.ProductPriceTypeID))
                    variant.Prices.Add(new ProductPrice { Price = price.Price, ProductPriceTypeID = price.ProductPriceTypeID, CurrencyID = price.CurrencyID });
                else
                {
                    var variantPrice = variant.Prices.FirstOrDefault(pp => pp.ProductPriceTypeID == price.ProductPriceTypeID);
                    if (variantPrice != null)
                    {
                        variantPrice.Price = price.Price;
                    }
                }
            }
        }
        #endregion

        #region CMS
        public virtual ActionResult CMS(int? baseProductId, int? productId)
        {
            return CheckForProduct(baseProductId, productId);
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult GetProductToolTip(int? baseProductId, int? productId)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                var applicableProperties = product.Properties.Where(p => p.ProductPropertyValue != null && !p.ProductPropertyValue.Value.IsNullOrEmpty()).ToList();
                int propertyCount = applicableProperties.Count();
                string toolTip = product.SKU + "(";
                for (int i = 0; i < propertyCount; i++)
                {
                    ProductProperty property = applicableProperties[i];
                    if (i != 0)
                    {
                        toolTip += ", " + property.ProductPropertyValue.Value;
                    }
                    else
                    {
                        toolTip += property.ProductPropertyValue.Value;
                    }

                }
                toolTip += ")";
                return Json(new { result = true, toolTip = toolTip });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult RenderProductCMS(int? baseProductId, int? productId)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                var productBase = ProductBase.LoadFull(product.ProductBaseID);
                product = productBase.Products.FirstOrDefault(p => p.ProductID == productId);

                var productCMSModel = new ProductCMSModel(product);
                return Json(new { result = true, productCMSHTML = RenderPartialToString("ProductCMS", productCMSModel) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetDescription(int? baseProductId, int? productId, int languageId)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                if (product != null)
                {
                    var description = product.Translations.GetByLanguageIdOrDefault(languageId);

                    return Json(new { result = true, name = description.Name, shortDescription = description.ShortDescription, longDescription = description.LongDescription });
                }
                return Json(new { result = false });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [ValidateInput(false)]
        public virtual ActionResult SaveDescription(int? baseProductId, int? productId, int languageId, string name, string shortDescription, string longDescription, bool isVariant = false)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                if (product != null)
                {
                    var description = product.Translations.GetByLanguageIdOrDefaultInList(languageId);
                    description.Name = name;
                    description.ShortDescription = shortDescription;
                    description.LongDescription = longDescription;
                    description.Save();

                    if (!isVariant)
                    {
                        var baseDescription = product.ProductBase.Translations.GetByLanguageIdOrDefaultInList(languageId);
                        baseDescription.Name = name;
                        baseDescription.ShortDescription = shortDescription;
                        baseDescription.LongDescription = longDescription;
                        baseDescription.Save();
                    }

                    product.Save();

                    return Json(new { result = true });
                }

                return Json(new { result = false });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult SaveImage(int? baseProductId, int? productId)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var product = this.GetRequestedProduct(baseProductId, productId);
                    if (product != null)
                    {
                        var fileInfo = new FileInfo(Request.Files[0].FileName);
                        string fileName = fileInfo.Name,
                            folder = "Products",
                             absoluteFolder = System.Configuration.ConfigurationManager.AppSettings["ProductImagesUploadPath"],//ConfigurationManager.GetAbsoluteFolder(folder),
                            //absoluteFolder = ConfigurationManager.GetAbsoluteFolder(folder),
                             imagePath = Path.Combine(absoluteFolder, fileName);//ConfigurationManager.GetWebFolder(folder) + fileName;


                        using (FileStream file = new FileStream(imagePath, FileMode.Create, FileAccess.Write))
                        {
                            Request.Files[0].InputStream.CopyTo(file);
                        }

                        //Request.Files[0].SaveAs(absoluteFolder + fileName);

                        var newImage = new ProductFile()
                        {
                            FilePath = "<!--filepath-->Products/" + fileName,
                            ProductFileTypeID = Constants.ProductFileType.Image.ToInt(),
                            ProductID = product.ProductID,
                            SortIndex = product.Files.GetNextSortIndex(Constants.ProductFileType.Image.ToInt())
                        };
                        product.Files.Add(newImage);

                        product.Save();
                        this.OnProductImageSaved(product);

                        //AJAX Upload doesn't support application/json as a content type, so we have to send back a json object with text/html mime type - DES
                        return Content(new
                        {
                            result = true,
                            fileId = newImage.ProductFileID,
                            imagePath = System.Configuration.ConfigurationManager.AppSettings["FileUploadWebPath"]
                        }.ToJSON(), "text/html");
                    }
                    return Content(new
                    {
                        result = false,
                        message = "Session has timed out"
                    }.ToJSON(), "text/html");
                    //return Json(new { result = true, imagePath = NetSteps.Common.CustomConfigurationHandler.Config.FilePaths.ImagesWebPath + fileName });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Content(new { result = false, message = exception.PublicMessage }.ToJSON(), "text/html");
            }

            return Content(new { result = false, message = Translation.GetTerm("NoFileUploaded", "No file uploaded") }.ToJSON(), "text/html");
            //return Json(new { result = false });
        }

        protected virtual void OnProductImageSaved(Product product)
        { }

        public virtual ActionResult SaveImagesSort(int? baseProductId, int? productId, List<int> productFiles)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                if (product != null && productFiles != null)
                {
                    for (int i = 0; i < productFiles.Count; i++)
                    {
                        var image = product.Files.First(pf => pf.ProductFileID == productFiles[i]);
                        if (image.SortIndex != i)
                        {
                            image.SortIndex = i;
                        }
                    }

                    product.Save();
                    this.OnProductImageSaved(product);
                }

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult DeleteImages(int? baseProductId, int? productId, List<int> productFiles)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                if (product != null && productFiles != null)
                {
                    foreach (int productFileId in productFiles)
                    {
                        product.Files.RemoveAndMarkAsDeleted(product.Files.First(f => f.ProductFileID == productFileId));
                        product.Save();
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

        #region Categories
        public virtual ActionResult Categories(int? baseProductId, int? productId)
        {
            try
            {
                var result = CheckForProduct(baseProductId, productId);
                var categoryTrees = Category.LoadFullTopLevelByCategoryTypeId((int)Constants.CategoryType.Product);
                ViewData["CategoryTrees"] = categoryTrees;
                if (!(result is RedirectToRouteResult))
                {
                    var product = this.GetRequestedProduct(baseProductId, productId);
                    ViewData["CategoryTree"] = BuildCategoryTree(Category.LoadFullTree(categoryTrees.First().CategoryID).ChildCategories, product.ProductBase.Categories.Select(c => c.CategoryID));
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public virtual ActionResult SaveCategories(int? baseProductId, int? productId, int categoryTree, List<int> categories)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                var productBase = ProductBase.LoadFull(product.ProductBaseID);

                if (categories != null)
                {
                    //List<Category> cats = Category.LoadBatch(categories);

                    foreach (var categoryID in categories.Where(c => !productBase.Categories.Any(cat => cat.CategoryID == c)))
                    {
                        //productBase.Categories.Add(cats.First(c => c.CategoryID == categoryID));
                        productBase.Categories.Add(Category.Load(categoryID));
                    }

                    productBase.Categories.RemoveWhere(c => c.CategoryTreeID == categoryTree && !categories.Contains(c.CategoryID));
                }
                else
                {
                    productBase.Categories.RemoveAll();
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
        #endregion

        #region Properties
        public virtual ActionResult Properties(int? baseProductId, int? productId)
        {
            return CheckForProduct(baseProductId, productId);
        }

        public virtual ActionResult SaveProperties(int? baseProductId, int? productId, List<ProductProperty> properties)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                if (properties != null)
                {
                    product.Properties.SyncTo(
                         properties,
                         new LambdaComparer<ProductProperty>((p1, p2) => p1.ProductPropertyID == p2.ProductPropertyID && p2.ProductPropertyID != 0, p => p.ProductPropertyID),
                         (p1, p2) =>
                         {
                             p1.PropertyValue = p2.PropertyValue.ToCleanString();
                             p1.ProductPropertyValueID = p2.ProductPropertyValueID;
                         },
                         (list, itemToRemove) =>
                         {
                             list.RemoveAndMarkAsDeleted(itemToRemove);
                         }
                    );
                }
                else
                {
                    product.Properties.RemoveAllAndMarkAsDeleted();
                }

                product.Save();

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region Resources
        public virtual ActionResult Resources(int? baseProductId, int? productId)
        {
            return CheckForProduct(baseProductId, productId);
        }

        public virtual ActionResult UploadResource(int? baseProductId, int? productId, Constants.ProductFileType resourceType)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var product = this.GetRequestedProduct(baseProductId, productId);
                    if (product != null)
                    {
                        var fileInfo = new FileInfo(Request.Files[0].FileName);
                        string fileName = fileInfo.Name,
                             folder = resourceType.ToString(),
                             absoluteFolder = ConfigurationManager.GetAbsoluteFolder(folder),
                             path = ConfigurationManager.GetWebFolder(folder) + fileName;

                        if (System.IO.File.Exists(absoluteFolder + fileName))
                            fileName.Insert(fileName.LastIndexOf('.'), Guid.NewGuid().ToString("N"));

                        Request.Files[0].SaveAs(absoluteFolder + fileName);

                        var newImage = new ProductFile()
                        {
                            FilePath = "<!--filepath-->" + (!string.IsNullOrEmpty(folder) ? (folder + "/") : "") + fileName,
                            ProductFileTypeID = (int)resourceType,
                            SortIndex = product.Files.GetNextSortIndex((int)resourceType)
                        };
                        product.Files.Add(newImage);

                        product.Save();

                        var builder = new StringBuilder();

                        string icon;
                        var fileType = fileName.GetFileType();
                        switch (fileType)
                        {
                            case NetSteps.Common.Constants.FileType.PDF:
                                icon = "acrobatICO.gif";
                                break;
                            case NetSteps.Common.Constants.FileType.Audio:
                                icon = "audioICO.gif";
                                break;
                            case NetSteps.Common.Constants.FileType.Flash:
                                icon = "flashICO.gif";
                                break;
                            case NetSteps.Common.Constants.FileType.Image:
                                icon = "jpegICO.gif";
                                break;
                            case NetSteps.Common.Constants.FileType.Video:
                                icon = "movieICO.gif";
                                break;
                            case NetSteps.Common.Constants.FileType.Powerpoint:
                                icon = "powerpointICO.gif";
                                break;
                            default:
                                icon = "genericdocICO.gif";
                                break;
                        }

                        builder.Append("<li><img src=\"").Append("~/Content/Images/Icons/DocumentTypes/".ResolveUrl()).Append(icon).Append("\" alt=\"").Append(fileType.ToString()).Append("\" title=\"").Append(fileType.ToString()).Append("\" />")
                             .Append(fileName).Append("<input type=\"hidden\" class=\"resourceId\" value=\"").Append(newImage.ProductFileID).Append("\" /><select class=\"resourceType\">");
                        foreach (ProductFileType type in SmallCollectionCache.Instance.ProductFileTypes.Where(pft => pft.ProductFileTypeID != (int)Constants.ProductFileType.Image && pft.Active))
                        {
                            builder.Append("<option value=\"").Append(type.ProductFileTypeID).Append("\">").Append(type.GetTerm()).Append("</option>");
                        }
                        builder.Append("</select>").Append("<a href=\"javascript:void(0);\" class=\"delete\" style=\"margin-left: 3px;\"><img src=\"")
                            .Append("~/Content/Images/Icons/remove-trans.png".ResolveUrl()).Append("\" alt=\"Delete\" /></a></li>");

                        return Content(new
                        {
                            result = true,
                            resource = builder.ToString()
                        }.ToJSON(), "text/html");
                    }
                    return Content(new
                    {
                        result = false,
                        message = "Session has timed out"
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

        public virtual ActionResult DeleteResource(int? baseProductId, int? productId, int resourceId)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                var file = product.Files.FirstOrDefault(f => f.ProductFileID == resourceId);
                if (file != default(ProductFile))
                {
                    try
                    {
                        System.IO.File.Delete(file.FilePath.ReplaceFileUploadPathToken().WebUploadPathToAbsoluteUploadPath());
                    }
                    catch (Exception excp)
                    {
                        excp.TraceException(excp);
                    }

                    product.Files.RemoveAndMarkAsDeleted(file);
                    product.Save();
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult SaveResources(int? baseProductId, int? productId, Dictionary<int, int> resources)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                if (resources != null)
                {
                    foreach (var resource in resources)
                    {
                        var file = product.Files.FirstOrDefault(f => f.ProductFileID == resource.Key);
                        if (file != default(ProductFile))
                        {
                            file.ProductFileTypeID = resource.Value;
                        }
                    }

                    product.Save();
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

        #region Relations

        /*CS.17JUL2016.Inicio*/
        public virtual ActionResult ExisteOferTypeRestrictionsByValue(OferTypeRestrictionsTable entidad)
        {
            try
            {
                bool existe = false;
                bool ingresa = false;
                string mensaje = string.Empty;
                if (entidad.ProductRelationTypeID == (int)Constants.ProductRelationsType.RelatedItem)
                    mensaje = Translation.GetTerm("OfertTypeIsNotRegistered", "Ofert type is not registered");
                else
                    mensaje = Translation.GetTerm("OfertTypeIsAlreadyRegistered", "Ofert type is already registered");
                if (entidad != null)
                {
                    existe = OferTypeRestrictions.ExisteOferTypeRestrictionsByValue(entidad.OferTypeRestrictionValue);
                    if (entidad.ProductRelationTypeID == (int)Constants.ProductRelationsType.RelatedItem && existe)
                        ingresa = true;
                    else
                        if (entidad.ProductRelationTypeID == (int)Constants.ProductRelationsType.Kit && !existe)
                            ingresa = true;
                }

                return Json(new { result = ingresa, message = mensaje });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
        /*CS.17JUL2016.Fin*/
        public virtual ActionResult Relations(int? baseProductId, int? productId)
        {
            try
            {
                var productBase = ProductBase.LoadFull(baseProductId.Value);
                var product = productBase.Products.First(p => (productId.HasValue && p.ProductID == productId.Value) || (!productId.HasValue && !p.IsVariantTemplate));

                if (product.IsVariantTemplate)
                {
                    if (product.ProductBase.Products.Select(a => a.IsVariantTemplate).Contains(false))
                        return RedirectToAction("Relations", new { productId = product.ProductBase.Products.First(p => !p.IsVariantTemplate).ProductID });
                    else
                        return View(new ProductRelationModel().LoadResources(product));
                }
                string valorParticipacionPorcentaje = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "PPR");
                ViewBag.ParticipacionPorcentaje = valorParticipacionPorcentaje;
                return View(new ProductRelationModel().LoadResources(product));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public virtual ActionResult AddRelation(int? baseProductId, int? productId, int relationshipTypeId, int childProductId, int? childProductQuantity = 1)
        {
            try
            {
                // Only allow Kit relationships to be one to many.
                if (relationshipTypeId != (int)Constants.ProductRelationsType.Kit) childProductQuantity = 1;

                var product = Product.LoadFull(productId.Value);
                var childProduct = Product.LoadFull(childProductId);

                if (relationshipTypeId != (int)Constants.ProductRelationsType.Kit
                    && product.ChildProductRelations.ContainsRelation(relationshipTypeId, childProductId))
                {
                    return Content(string.Empty);
                }

                for (int i = 0; i < childProductQuantity; i++)
                {
                    product.AddChildProductRelation(relationshipTypeId, childProduct);
                }

                product.Save();

                var type = ProductRelationsType.LoadAll().First(rt => rt.ProductRelationTypeID == relationshipTypeId);

                var singleRelationHtml =
                    new StringBuilder("<option value=\"").Append(relationshipTypeId).Append(",").Append(childProductId).Append("\">").
                        Append(type.Name).Append(": ").Append(childProduct.SKU).Append(" - ").Append(childProduct.Translations.Name()).
                        Append("</option>").ToString();
                var relationHtml = new StringBuilder(singleRelationHtml);
                for (var i = 1; i < childProductQuantity; i++)
                {
                    relationHtml.Append(singleRelationHtml);
                }

                return Json(new
                {
                    result = true,
                    relation = relationHtml.ToString()
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public class MaterialRelationModel
        {
            public int ParentProductID { get; set; }
            public List<Relation> Relations { get; set; } //Added by ECH - BACK            
            public List<Replacement> Replacements { get; set; } //Added by ECH - BACK

        }
        public class Relation
        {
            public int ProductRelationID { get; set; }
            public int RelationsTypeID { get; set; }
            public int ChildProductID { get; set; }
            public string ParentSku { get; set; } //Added by WCS - CSTI
            public string ChildSku { get; set; } //Added by WCS - CSTI
            public decimal ParticipationPercentage { get; set; } //Added by WCS - CSTI
            public int OfertType { get; set; } //Added by ECH - BACK
            public int ExternalCode { get; set; } //Added by ECH - BACK
            public int MaterialID { get; set; } //Added by ECH - BACK
            public string Name { get; set; } //Added by ECH - BACK
            public string SKU { get; set; } //Added by ECH - BACK
            public string RelationsTypeName { get; set; } //Added by ECH - BACK
            public List<Replacement> Replacements { get; set; }
            public List<ProductRelationsPerPeriod> ProductRelationsPerPeriod { get; set; }
        }
        public class Replacement
        {
            public int ProductRelationID { get; set; } //Added by ECH - BACK
            public int ParentMaterialID { get; set; } //Added by ECH - BACK
            public int Priority { get; set; } //Added by ECH - BACK
            public int MaterialID { get; set; } //Added by ECH - BACK
            public string Name { get; set; } //Added by ECH - BACK
            public string SKU { get; set; } //Added by ECH - BACK
        }

        public class ProductRelationsPerPeriod
        {
            public int ProductRelationPerPeriodID { get; set; }
            public int PeriodID { get; set; }
            public int ParentProductID { get; set; }
            public int MaterialID { get; set; }
            public int OfferType { get; set; }
            public int ExternalCode { get; set; }
        }

        public virtual ActionResult RemoveRelations(int? baseProductId, int? productId, List<Relation> relationships)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);

                foreach (var requestedForDelete in relationships)
                {
                    var relation =
                        product.ChildProductRelations.FirstOrDefault(
                            r => r.ProductRelationsTypeID == requestedForDelete.RelationsTypeID
                            && r.ChildProductID == requestedForDelete.ChildProductID);

                    if (relation != null)
                    {
                        relation.Delete();
                        product.ChildProductRelations.Remove(relation);
                    }
                }

                product.Save();

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult SaveKit(int? baseProductId, int? productId, List<Relation> relationships)
        {
            #region Validation

            decimal participationT = relationships.Sum(x => x.ParticipationPercentage);
            if (System.Math.Abs(100 - participationT) > Convert.ToDecimal(0.01))
            {
                return Json(new { result = false, message = "% Participation of Kit Items must sum 100%" });
            }

            #endregion

            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();
                // Start a local transaction.ProductPrices
                SqlTransaction tr = connection.BeginTransaction();
                try
                {
                    #region Punto 2 y 3

                    DataAccess.ExecuteNonQuery(DataAccess.GetCommand("upsDeleteRelations", new Dictionary<string, object>() { { "@ParentProductID", productId } }, connection, tr));

                    #endregion

                    #region Punto 4

                    var parent = Product.Load(Convert.ToInt32(productId));
                    foreach (var item in relationships)
                    {
                        item.ParentSku = parent.SKU;
                        var RelationMaterialID = DataAccess.ExecuteScalar(DataAccess.GetCommand("upsInsertRelations", new Dictionary<string, object>() { { "@ProductRelationsTypeID", item.RelationsTypeID },
                                                                                                                              { "@ParentProductID", productId },
                                                                                                                              { "@ChildProductID", item.ChildProductID },
                                                                                                                              { "@ParticipationPercentage", item.ParticipationPercentage},
                                                                                                                              { "@OfertType", item.OfertType },
                                                                                                                              { "@ExternalCode", item.ExternalCode },
                                                                                                                              { "@MaterialID", item.MaterialID },
                                                                                                                              }, connection, tr));
                        foreach (var replace in item.Replacements ?? new List<Replacement>())
                        {
                            replace.ProductRelationID = Convert.ToInt32(RelationMaterialID);
                            DataAccess.ExecuteNonQuery(DataAccess.GetCommand("upsInsertProductRelationReplacements", new Dictionary<string, object>() { { "@ProductRelationID", replace.ProductRelationID },
                                                                                                                              { "@Priority", replace.Priority },
                                                                                                                              { "@MaterialID", replace.MaterialID },
                                                                                                                              }, connection, tr));
                        }

                        foreach (var prpp in item.ProductRelationsPerPeriod ?? new List<ProductRelationsPerPeriod>())
                        {
                            DataAccess.ExecuteNonQuery(DataAccess.GetCommand("upsInsertProductRelationsPerPeriod", new Dictionary<string, object>() { { "@PeriodID", prpp.PeriodID },
                                                                                                                              { "@ParentProductID", productId },
                                                                                                                              { "@MaterialID", prpp.MaterialID },
                                                                                                                              { "@OfferType", prpp.OfferType },
                                                                                                                              { "@ExternalCode", prpp.ExternalCode }
                                                                                                                              }, connection, tr));
                        }
                    }

                    #endregion

                    #region Punto 5

                    var q = relationships.GroupBy(i => new { i.ParentSku, i.ChildSku }).Select(j => new { j.Key.ParentSku, j.Key.ChildSku }).ToList();
                    decimal participationPercentage;
                    foreach (var item1 in q)
                    {
                        participationPercentage = 0;
                        participationPercentage = relationships.FindAll(x => x.ParentSku == item1.ParentSku && x.ChildSku == item1.ChildSku).Sum(x => x.ParticipationPercentage) / 100;
                        DataAccess.ExecuteNonQuery(DataAccess.GetCommand("uspInsertKitItemsValuations", new Dictionary<string, object>() { { "@ParentSku", item1.ParentSku },
                                                                                                                                       { "@ChildSku", item1.ChildSku },
                                                                                                                                       { "@ParticipationPercentage", participationPercentage }
                                                                                                                                       }, connection, tr));
                    }

                    #endregion

                    tr.Commit();
                    return Json(new { result = true });
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage });
                }
            }
        }

        public virtual ActionResult GetMaterials(int? baseProductId, int productId)
        {

            MaterialsBusinessLogic MaterialBL = new MaterialsBusinessLogic();
            var materialsPrincipal = MaterialBL.GetAllMaterialsByParentProductID(productId);

            MaterialRelationModel MaterialRelation = new MaterialRelationModel();
            MaterialRelation.ParentProductID = productId;

            MaterialRelation.Relations = new List<Relation>();
            Relation MaterialRelationReplacement;
            foreach (var item in materialsPrincipal)
            {
                //TODO: Refactorizar 
                #region GetReplacement

                MaterialRelationReplacement = new Relation();
                MaterialRelationReplacement.ProductRelationID = item.ProductRelationID;
                MaterialRelationReplacement.RelationsTypeID = item.RelationsTypeID;
                MaterialRelationReplacement.MaterialID = item.MaterialID;
                MaterialRelationReplacement.OfertType = item.OfertType;
                MaterialRelationReplacement.ExternalCode = item.ExternalCode;
                MaterialRelationReplacement.RelationsTypeName = item.RelationsTypeName;
                MaterialRelationReplacement.SKU = item.SKU;
                MaterialRelationReplacement.Name = item.Name;
                MaterialRelationReplacement.ParticipationPercentage = item.ParticipationPercentage;

                MaterialRelationReplacement.Replacements = new List<Replacement>();
                foreach (var replace in MaterialBL.GetReplacementsByProductRelationID(item.ProductRelationID))
                {
                    var MaterialRelationReplacement2 = new Replacement
                    {
                        ParentMaterialID = replace.ParentMaterialID,
                        MaterialID = replace.MaterialID,
                        Name = replace.Name,
                        SKU = replace.SKU,
                        Priority = replace.Priority
                    };
                    MaterialRelationReplacement.Replacements.Add(MaterialRelationReplacement2);
                }

                MaterialRelationReplacement.ProductRelationsPerPeriod = new List<ProductRelationsPerPeriod>();


                //************
                foreach (var prpp in MaterialBL.GetProductRelationsPerPeriod(item.ProductRelationID))
                {
                    var MaterialRelationReplacement3 = new ProductRelationsPerPeriod
                    {
                        PeriodID = prpp.PeriodID,
                        //ParentProductID = prpp.MaterialID,
                        MaterialID = prpp.MaterialID,
                        OfferType = prpp.OfferType,
                        ExternalCode = prpp.ExternalCode
                    };
                    MaterialRelationReplacement.ProductRelationsPerPeriod.Add(MaterialRelationReplacement3);
                }
                //************

                MaterialRelation.Relations.Add(MaterialRelationReplacement);
                #endregion

                GetyProductRelationPerPeriod(MaterialBL, item, MaterialRelationReplacement);
            }


            return Json(new { result = MaterialRelation });
        }

        private static void GetyProductRelationPerPeriod(MaterialsBusinessLogic MaterialBL, dynamic item, Relation MaterialRelationReplacement)
        {
            var getProductRelationsPerPeriod = MaterialBL.GetReplacementsByProductRelationPerPeriod(item.ProductRelationID);
            MaterialRelationReplacement.ProductRelationsPerPeriod = new List<ProductRelationsPerPeriod>();
            foreach (var PPPDynamic in getProductRelationsPerPeriod)
            {
                MaterialRelationReplacement.ProductRelationsPerPeriod.Add(new ProductRelationsPerPeriod()
                {
                    ExternalCode = PPPDynamic.ExternalCode,
                    MaterialID = PPPDynamic.MaterialID,
                    OfferType = PPPDynamic.OfferType,
                    ParentProductID = PPPDynamic.ParentProductID,
                    PeriodID = PPPDynamic.PeriodID,
                    ProductRelationPerPeriodID = PPPDynamic.ProductRelationPerPeriodID
                });
            }
        }

        private static void getProductRelationPerPeriod(MaterialsBusinessLogic materialBl, dynamic item,
            MaterialRelationModel materialRelation)
        {

        }

        #region Dynamic Kits

        public virtual ActionResult ExcludeSelectedRules(int? baseProductId, int? productId, List<ProductRelationKitGroupRuleModel> ruleList)
        {
            if (ruleList.Any(x => x.ProductTypeID != null))
            {
                return
                     Json(new
                                 {
                                     success = false,
                                     response = ruleList,
                                     message =
                                Translation.GetTerm("ProductTypesCannotBeExcluded", "Product Types cannot be excluded")
                                 });
            }
            var returnModel = UpdateSelectedRulesFlags(baseProductId, productId, ruleList, false, false);

            return returnModel != null
                 ? Json(new { success = true, result = new { DynamicKitGroups = returnModel }, message = Translation.GetTerm("RuleUpdated") })

                 : Json(new { success = false, response = ruleList });
        }

        public virtual ActionResult IncludeSelectedRules(int? baseProductId, int? productId, List<ProductRelationKitGroupRuleModel> ruleList)
        {
            var returnModel = UpdateSelectedRulesFlags(baseProductId, productId, ruleList, true, null);

            return returnModel != null
                 ? Json(new { success = true, result = new { DynamicKitGroups = returnModel }, message = Translation.GetTerm("RuleUpdated") })
                 : Json(new { success = false, response = ruleList });
        }

        public virtual ActionResult RemoveDefaultSelectedRules(int? baseProductId, int? productId, List<ProductRelationKitGroupRuleModel> ruleList)
        {
            var returnModel = UpdateSelectedRulesFlags(baseProductId, productId, ruleList, null, false);

            return returnModel != null
                 ? Json(new { success = true, result = new { DynamicKitGroups = returnModel }, message = Translation.GetTerm("RuleUpdated") })
                 : Json(new { success = false, response = ruleList });
        }

        public virtual ActionResult AddDefaultSelectedRules(int? baseProductId, int? productId, List<ProductRelationKitGroupRuleModel> ruleList)
        {
            if (ruleList.Any(x => x.ProductTypeID != null))
            {
                return
                     Json(new
                     {
                         success = false,
                         response = ruleList,
                         message =
                    Translation.GetTerm("ProductTypesCannotBeDefaulted", "Product Types cannot be a default")
                     });
            }
            var returnModel = UpdateSelectedRulesFlags(baseProductId, productId, ruleList, null, true);

            return returnModel != null
                 ? Json(new { success = true, result = new { DynamicKitGroups = returnModel }, message = Translation.GetTerm("RuleUpdated") })
                 : Json(new { success = false, response = ruleList });
        }

        public virtual ActionResult RemoveRule(int? baseProductId, int? productId, ProductRelationKitGroupRuleModel rule)
        {
            var product = this.GetRequestedProduct(baseProductId, productId);
            var group = product.DynamicKits.FirstOrDefault().DynamicKitGroups.FirstOrDefault(
                      x => x.DynamicKitGroupID == rule.GroupID);

            if (product.DynamicKits != null)
            {
                var returnValue = DeleteRuleFromGroup(group, rule);

                if (!returnValue)
                {
                    return Json(new
                    {
                        success = false,
                        response = rule,
                        message = Translation.GetTerm("UnableToDeleteRule", "Unable To Delete Rule")
                    });
                }

                product.Save();

                var returnModel = new ProductRelationModel().LoadResources(product).DynamicKitGroups;
                return Json(new { success = true, result = new { DynamicKitGroups = returnModel }, message = Translation.GetTerm("RuleDeleted") });
            }

            return Json(new { success = false, response = rule });
        }

        public virtual ActionResult UndefaultRule(int? baseProductId, int? productId, ProductRelationKitGroupRuleModel rule)
        {
            var ruleList = new List<ProductRelationKitGroupRuleModel> { rule };

            var returnModel = UpdateSelectedRulesFlags(baseProductId, productId, ruleList, null, false);

            return returnModel != null
                 ? Json(new { success = true, result = new { DynamicKitGroups = returnModel }, message = Translation.GetTerm("RuleUpdated") })
                 : Json(new { success = false, response = ruleList });
        }

        public virtual ActionResult RemoveSelectedRules(int? baseProductId, int? productId, List<ProductRelationKitGroupRuleModel> ruleList)
        {
            var product = this.GetRequestedProduct(baseProductId, productId);
            if (product.DynamicKits != null)
            {
                var group = product.DynamicKits.FirstOrDefault().DynamicKitGroups.FirstOrDefault(
                     x => x.DynamicKitGroupID == ruleList[0].GroupID);

                foreach (var rule in ruleList)
                {
                    if (!DeleteRuleFromGroup(group, rule))
                    {
                        return Json(new
                        {
                            success = false,
                            response = rule,
                            message = Translation.GetTerm("UnableToDeleteSelectedRules", "Unable To Delete Selected Rules")
                        });
                    }
                }

                product.Save();

                var returnModel = new ProductRelationModel().LoadResources(product).DynamicKitGroups;
                return Json(new { success = true, result = new { DynamicKitGroups = returnModel }, message = Translation.GetTerm("RuleDeleted") });
            }

            return Json(new { success = false, response = ruleList });
        }

        public ActionResult AddGroup(int? baseProductId, int? productId, int languageID, string groupName, int? minProductCount, int? maxProductCount)
        {
            var groupModel = new ProductRelationKitGroupModel
                                         {
                                             SelectedLanguageID = languageID,
                                             Name = groupName,
                                             MinimumProductCount = minProductCount == null ? 0 : (int)minProductCount,
                                             MaximumProductCount = maxProductCount == null ? 0 : (int)maxProductCount
                                         };

            try
            {
                var returnModel = SaveDynamicKitGroup(baseProductId, productId, groupModel);
                return Json(new { success = true, result = new { DynamicKitGroups = returnModel }, message = Translation.GetTerm("GroupSaved") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult DeleteGroup(int? baseProductId, int? productId, ProductRelationKitGroupModel groupModel)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                var kit = product.DynamicKits.FirstOrDefault();
                var group = kit.DynamicKitGroups.FirstOrDefault(g => g.DynamicKitGroupID == groupModel.GroupID);

                group.Translations.RemoveAllAndMarkAsDeleted();
                group.DynamicKitGroupRules.RemoveAllAndMarkAsDeleted();
                kit.DynamicKitGroups.RemoveAndMarkAsDeleted(group);
                if (kit.DynamicKitGroups.Count() == 0)
                {
                    product.DynamicKits.RemoveAllAndMarkAsDeleted();
                }
                product.Save();
                var returnModel =
                     new ProductRelationModel().LoadResources(product).DynamicKitGroups;

                return Json(new { success = true, result = new { DynamicKitGroups = returnModel }, message = Translation.GetTerm("GroupDeleted") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult SaveGroup(int? baseProductId, int? productId, ProductRelationKitGroupModel groupModel)
        {
            try
            {
                var returnModel = SaveDynamicKitGroup(baseProductId, productId, groupModel);
                return Json(new { success = true, result = new { DynamicKitGroups = returnModel }, message = Translation.GetTerm("GroupSaved") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult AddRule(int? baseProductId, int? productId, ProductRelationKitGroupModel groupModel)
        {
            try
            {
                if (groupModel.ProductRuleSelectionType == "Product" && (groupModel.SelectedProductIDValue == 0 || groupModel.SelectedProductIDValue == null))
                {
                    return Json(new { result = false, message = Translation.GetTerm("ProductMustBeSpecifiedForDymamicKitGroupRuleAdded", "A product must be specified for the dynamic kit group rule to be added.") });
                }
                var newRule = new DynamicKitGroupRule
                {
                    DynamicKitGroupID = (int)groupModel.GroupID,
                    ProductTypeID = groupModel.ProductRuleSelectionType == "ProductType" ? groupModel.SelectedProductTypeIDValue : null,
                    ProductID = groupModel.ProductRuleSelectionType == "Product" && groupModel.SelectedProductIDValue > 0 ? groupModel.SelectedProductIDValue : null,
                    Include = !groupModel.SelectedExclude,
                    Default = groupModel.SelectedDefault
                };

                if (newRule.ProductID.HasValue)
                {
                    var newRuleProduct = Inventory.GetProduct(newRule.ProductID.Value);
                    if (newRuleProduct.IsDynamicKit())
                    {
                        return Json(new { result = false, message = Translation.GetTerm("CannotAssociateDynamicKitWithAnotherDynamicKit", "A dynamic kit cannot be associated with another dynamic kit.") });
                    }
                }

                var product = this.GetRequestedProduct(baseProductId, productId);
                var kit = product.DynamicKits.FirstOrDefault();
                var group = kit.DynamicKitGroups.FirstOrDefault(g => g.DynamicKitGroupID == groupModel.GroupID);
                group.DynamicKitGroupRules.Add(newRule);

                product.Save();
                var returnModel =
                     new ProductRelationModel().LoadResources(product).DynamicKitGroups;

                return Json(new { success = true, result = new { DynamicKitGroups = returnModel }, message = Translation.GetTerm("RuleAdded") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult UpdateKitPricingType(int? baseProductId, int? productId, int pricingTypeID)
        {
            var product = this.GetRequestedProduct(baseProductId, productId);
            if (product.DynamicKits.Count > 0)
            {
                product.DynamicKits[0].DynamicKitPricingTypeID = pricingTypeID;
                product.Save();
                return Json(new { success = true, message = Translation.GetTerm("DynamicKitPricingTypeUpdated", "Dynamic Kit Pricing Type Updated") });
            }

            return Json(new { success = false, message = Translation.GetTerm("DynamicKitPricingTypeNotUpdated", "The Dynamic Kit Pricing Type was not updated") });
        }

        #region privateKitMethods

        private List<ProductRelationKitGroupModel> SaveDynamicKitGroup(int? baseProductId, int? productId, ProductRelationKitGroupModel groupModel)
        {
            if (groupModel.IsMinMoreThanMax())
            {
                throw new NetStepsException(Translation.GetTerm("MinProductCountGreaterThanMaxProductCount", "The minimum product count is greater than the maximum product count"));
            }

            var product = this.GetRequestedProduct(baseProductId, productId);
            DynamicKitGroup group = null;
            if (groupModel.GroupID.HasValue && groupModel.GroupID.Value > 0)
            {
                group = product.DynamicKits.FirstOrDefault().DynamicKitGroups.FirstOrDefault(g => g.DynamicKitGroupID == groupModel.GroupID.Value);
            }
            if (group == null)
            {
                group = new DynamicKitGroup();
                if (product.DynamicKits.Count == 0)
                {
                    product.DynamicKits.Add(new DynamicKit());
                }

                product.DynamicKits.FirstOrDefault().DynamicKitGroups.Add(group);
            }

            var translation = group.Translations.GetByLanguageIdOrDefaultInList(groupModel.SelectedLanguageID);
            translation.Name = groupModel.Name;
            translation.ShortDescription = groupModel.Description;

            group.MinimumProductCount = groupModel.MinimumProductCount;
            group.MaximumProductCount = groupModel.MaximumProductCount;

            // update sort order for all default rules
            if (groupModel.DefaultRules != null)
            {
                foreach (var ruleModel in groupModel.DefaultRules)
                {
                    var rule =
                         group.DynamicKitGroupRules.FirstOrDefault(x => x.DynamicKitGroupRuleID == ruleModel.RuleID);
                    if (rule != null) rule.SortOrder = ruleModel.SortOrder;
                }
            }

            product.Save();
            return new ProductRelationModel().LoadResources(product).DynamicKitGroups;
        }

        private List<ProductRelationKitGroupModel> UpdateSelectedRulesFlags(int? baseProductId, int? productId, List<ProductRelationKitGroupRuleModel> ruleList, bool? includeRule, bool? defaultRule)
        {
            if (ruleList == null)
                return null;

            var product = this.GetRequestedProduct(baseProductId, productId);
            if (product.DynamicKits != null)
            {
                var group = product.DynamicKits.FirstOrDefault().DynamicKitGroups.FirstOrDefault(
                     x => x.DynamicKitGroupID == ruleList[0].GroupID);

                foreach (var rule in ruleList)
                {
                    var changeRule = group.DynamicKitGroupRules.FirstOrDefault(
                         x => x.DynamicKitGroupRuleID == rule.RuleID);

                    if (changeRule == null)
                        return null;

                    if (includeRule != null)
                        changeRule.Include = (bool)includeRule;

                    if (defaultRule != null)
                        changeRule.Default = (bool)defaultRule;
                }

                product.Save();

                return new ProductRelationModel().LoadResources(product).DynamicKitGroups;
            }

            return null;
        }

        private bool DeleteRuleFromGroup(DynamicKitGroup group, ProductRelationKitGroupRuleModel rule)
        {
            var changeRule = group.DynamicKitGroupRules.FirstOrDefault(
                            x => x.DynamicKitGroupRuleID == rule.RuleID);

            if (changeRule == null)
            {
                return false;
            }

            group.DynamicKitGroupRules.RemoveAndMarkAsDeleted(changeRule);
            return true;
        }

        #endregion
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult AddProductPropertyType(int productBaseId, int productPropertyTypeId, string name = null)
        {
            try
            {
                var productBase = ProductBase.LoadFull(productBaseId);

                if (!productBase.ProductBaseProperties.Any(pbp => pbp.ProductPropertyTypeID == productPropertyTypeId))
                {
                    var productBaseProperty = new ProductBaseProperty
                                                            {
                                                                ProductBaseID = productBaseId,
                                                                ProductPropertyTypeID = productPropertyTypeId
                                                            };
                    if (name != null)
                        productBaseProperty.Name = name;

                    productBaseProperty.Save();
                }

                var variantProperties = SmallCollectionCache.Instance.ProductPropertyTypes.Where(ppt => ppt.IsProductVariantProperty && ppt.IsMaster && !productBase.ProductBaseProperties.Any(pbp => pbp.ProductPropertyTypeID == ppt.ProductPropertyTypeID));

                string masterProductProperTypesHTML = "<select id=\"masterProductPropertyTypeId\"><option value=\"0\"></option>";
                foreach (ProductPropertyType propertyType in variantProperties)
                {
                    masterProductProperTypesHTML += string.Format("<option value=\"{0}\">{1}</option>", propertyType.ProductPropertyTypeID, propertyType.GetTerm());
                }

                masterProductProperTypesHTML += "</select>";

                CopyExcludedShippingMethodsFromProductBase(productBase);

                return Json(new
                {
                    result = true,
                    productBaseId = productBase.ProductBaseID,
                    masterProductProperTypesHTML = masterProductProperTypesHTML
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
        public virtual ActionResult RemoveProductPropertyType(int productBaseId, int productPropertyTypeId)
        {
            try
            {
                ProductBase productBase = ProductBase.LoadFull(productBaseId);

                if (productBase.ProductBaseProperties.Any(pbp => pbp.ProductPropertyTypeID == productPropertyTypeId))
                {
                    productBase.ProductBaseProperties.RemoveAndMarkAsDeleted(productBase.ProductBaseProperties.FirstOrDefault(pbp => pbp.ProductPropertyTypeID == productPropertyTypeId));

                    productBase.Save();
                }

                var variantProperties = SmallCollectionCache.Instance.ProductPropertyTypes.Where(ppt => ppt.IsProductVariantProperty && ppt.IsMaster && !productBase.ProductBaseProperties.Any(pbp => pbp.ProductPropertyTypeID == ppt.ProductPropertyTypeID));

                var masterProductProperTypesHTML = "<select id=\"masterProductPropertyTypeId\"><option value=\"0\"></option>";
                foreach (var propertyType in variantProperties)
                {
                    masterProductProperTypesHTML += string.Format("<option value=\"{0}\">{1}</option>", propertyType.ProductPropertyTypeID, propertyType.GetTerm());
                }
                masterProductProperTypesHTML += "</select>";

                return Json(new
                {
                    result = true,
                    productBaseId = productBase.ProductBaseID,
                    masterProductProperTypesHTML = masterProductProperTypesHTML
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetVariantSKUs(int? baseProductId, int? productId)
        {
            var builder = new StringBuilder();

            var product = this.GetRequestedProduct(baseProductId, productId);
            var variantProducts = Product.GetVariants(product.ProductID);
            int index = 0;

            foreach (Product productVariant in variantProducts.Where(x => x.ProductID != productId).ToList())
            {
                VariantsCUVsSearchData v = ProductExtensions.GetVariantCUVByProduct(productVariant.ProductID);
                builder.Append(RenderPartialToString("VariantSKU", new ViewDataDictionary { { "productBase", product.ProductBase } }, new VariantProductModel { Product = productVariant, Index = index, ProductVariant = v }));
                index++;
            }

            int cant = 0;
            foreach (var item in product.ProductBase.ProductBaseProperties.ToList())
            {
                cant = cant + item.ProductPropertyType.ProductPropertyValues.Count;

            }



            if (variantProducts.Count < cant)
            {
                var ini = variantProducts.Where(x => x.ProductID != productId).ToList().Count;

                for (var i = ini; i < cant; i++)
                {
                    VariantsCUVsSearchData v = new VariantsCUVsSearchData();
                    builder.Append(RenderPartialToString("VariantSKU", new ViewDataDictionary { { "productBase", product.ProductBase } }, new VariantProductModel { Product = new Product { }, Index = i, ProductVariant = v }));
                }
            }

            return Json(new { result = true, page = builder.ToString() });
        }

        #endregion

        #endregion

        #region Availability
        public virtual ActionResult Availability(int? baseProductId, int? productId)
        {
            return CheckForProduct(baseProductId, productId);
        }

        public virtual ActionResult SaveAvailability(int? baseProductId, int? productId, List<CatalogItem> catalogs)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                if (catalogs != null)
                {
                    foreach (CatalogItem catalogItem in catalogs)
                    {
                        CatalogItem newCatalogItem = product.CatalogItems.FirstOrDefault(ci => ci.CatalogID == catalogItem.CatalogID);
                        if (newCatalogItem == default(CatalogItem))
                        {
                            newCatalogItem = new CatalogItem
                                                        {
                                                            CatalogID = catalogItem.CatalogID,
                                                            Active = true,
                                                            StartDate = catalogItem.StartDate,
                                                            EndDate = catalogItem.EndDate,
                                                            SortIndex = 0
                                                        };
                            product.CatalogItems.Add(newCatalogItem);
                        }
                        else
                        {
                            newCatalogItem.Active = true;
                            newCatalogItem.EndDate = catalogItem.EndDate;
                            newCatalogItem.StartDate = catalogItem.StartDate;
                        }
                    }
                    product.CatalogItems.RemoveWhereAndMarkAsDeleted(ci => !catalogs.Any(c => c.CatalogID == ci.CatalogID),
                                                                                                    (list, catItem) => list.RemoveAndMarkAsDeleted(catItem));
                }
                else
                {
                    product.CatalogItems.RemoveAllAndMarkAsDeleted();
                }

                product.Save();

                if (product.IsVariantTemplate)
                {
                    IEnumerable<Product> products = product.ProductBase.Products.Where(p => !p.IsVariantTemplate);
                    foreach (var p in products)
                    {
                        List<int> variantCatalogsToRemove = new List<int>();

                        p.CatalogItems.Where(x => !product.CatalogItems.Select(i => i.CatalogID).Contains(x.CatalogID)).Each(x => variantCatalogsToRemove.Add(x.CatalogID));

                        foreach (var catalogID in variantCatalogsToRemove)
                        {
                            var catalogItem = p.CatalogItems.FirstOrDefault(i => i.CatalogID == catalogID);

                            if (catalogItem != null)
                            {
                                p.CatalogItems.RemoveAndMarkAsDeleted(catalogItem);
                            }
                        }
                        foreach (var catalogItem in product.CatalogItems)
                        {
                            if (!p.CatalogItems.Select(i => i.CatalogID).Contains(catalogItem.CatalogID))
                                p.CatalogItems.Add(new CatalogItem
                                {
                                    CatalogID = catalogItem.CatalogID,
                                    Active = true,
                                    StartDate = catalogItem.StartDate,
                                    EndDate = catalogItem.EndDate,
                                    SortIndex = 0
                                });
                        }
                    }

                    Product.SaveBatch(products);
                }

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region Inventory
        [ActionName("Inventory")]
        public virtual ActionResult ProductInventory(int? baseProductId, int? productId)
        {
            return CheckForProduct(baseProductId, productId, "Inventory", reloadProduct: true);
        }

        public virtual ActionResult SaveInventory(int? baseProductId, int? productId, List<WarehouseProduct> warehouseProducts)
        {
            try
            {
                var product = this.GetRequestedProduct(baseProductId, productId);
                var childProducts = product.ProductBase.Products;
                List<int> productIds = warehouseProducts.Select(wp => wp.ProductID).Distinct().ToList();

                foreach (int childProductId in productIds)
                {
                    var childProduct = childProducts.FirstOrDefault(p => p.ProductID == childProductId);
                    var productWarehouseProducts = warehouseProducts.Where(wp => wp.ProductID == childProductId);
                    childProduct.StartEntityTracking();
                    childProduct.WarehouseProducts.SyncTo(productWarehouseProducts, new LambdaComparer<WarehouseProduct>((wp1, wp2) => wp1.WarehouseID == wp2.WarehouseID
                        && wp1.ProductID == wp2.ProductID, wp => wp.WarehouseID + wp.ProductID), (wp1, wp2) =>
                    {
                        wp1.QuantityOnHand = wp2.QuantityOnHand;
                        wp1.QuantityBuffer = wp2.QuantityBuffer;
                        wp1.ReorderLevel = wp2.ReorderLevel;
                        wp1.IsAvailable = wp2.IsAvailable;
                    });
                    childProduct.Save();
                    childProduct.StopTracking();
                }


                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region StateProvinceExclusions

        public virtual ActionResult StateProvinceExclusions(int? baseProductId, int? productId)
        {
            var product = this.GetRequestedProduct(baseProductId, productId);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            int EnviromentCountry = Convert.ToInt32(ConfigurationManager.AppSettings["EnvironmentCountry"]);

            var model = new StateProvinceExclusionsModel()
            {
                Product = product,
                AvailableStateProvinces = SmallCollectionCache.Instance.Countries
                    .Where(c => c.CountryID == EnviromentCountry && SmallCollectionCache.Instance.StateProvinces.Any(s => s.CountryID == c.CountryID))
                    .ToDictionary(c => c,
                        c => SmallCollectionCache.Instance.StateProvinces
                            .Where(s => s.CountryID == c.CountryID)
                            .OrderBy(s => s.StateAbbreviation)
                            .Cast<StateProvince>()),
                ExistingExcludedStateProvinceIDs = product.ProductBase.ExcludedStateProvinces.Select(sp => sp.StateProvinceID).ToArray()
            };

            return View(model);
        }

        public virtual ActionResult SaveStateProvinceExclusions(int? baseProductId, int? productId, List<int> stateProvinceIds)
        {
            try
            {
                var pbaseRepo = NetSteps.Encore.Core.IoC.Create.New<IProductBaseRepository>() as ProductBaseRepository;
                var productBase = pbaseRepo.FirstOrDefault(pb => pb.ProductBaseID == baseProductId.Value, ProductBase.Relations.ExcludedStateProvinces);
                if (productBase != null)
                {
                    productBase.StartEntityTracking();
                    if (stateProvinceIds == null || !stateProvinceIds.Any())
                    {
                        productBase.ExcludedStateProvinces.RemoveAll();
                    }
                    else
                    {
                        productBase.ExcludedStateProvinces.RemoveAll(sp => !stateProvinceIds.Contains(sp.StateProvinceID));
                        var additional = stateProvinceIds.Where(i => !productBase.ExcludedStateProvinces.Any(sp => sp.StateProvinceID == i)).ToArray();
                        if (additional.Any())
                        {
                            productBase.ExcludedStateProvinces.AddRange(SmallCollectionCache.Instance.StateProvinces.Where(sp => additional.Contains(sp.StateProvinceID)));
                        }
                    }
                    productBase.Save();

                    return Json(new { result = true });
                }
                return Json(new { result = false, message = Translation.GetTerm("SessionTimedOut", "Session timed out.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #endregion


        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult AuditHistory(int productId)
        {
            try
            {
                if (productId == 0)
                {
                    return RedirectToAction("Index");
                }

                var product = Product.LoadFull(productId);
                ViewData["EntityName"] = "Product";
                ViewData["ID"] = productId;
                ViewData["LoadedEntitySessionVarKey"] = "Product";

                ViewData["Links"] = new StringBuilder("<a href=\"")
                    .Append(string.Format("~/Products/Products/Overview/{0}/{1}", product.ProductBaseID, productId).ResolveUrl())
                    .Append("\">" + Translation.GetTerm("Overview") + "</a> | " + Translation.GetTerm("AuditHistory", "Audit History"))
                    .ToString();

                return View("AuditHistory", "Product", product);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public virtual ActionResult Variants(int? baseProductId, int? productId)
        {
            return CheckForProduct(baseProductId, productId, reloadProduct: true);
        }

        public virtual ActionResult VariantSKUS(int? baseProductId, int? productId)
        {
            return CheckForProduct(baseProductId, productId, reloadProduct: true);
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult RenderVariantGroup(int productPropertyTypeId)
        {
            try
            {
                ProductPropertyType type = ProductPropertyType.LoadFull(productPropertyTypeId);

                return Json(new { result = true, variantGroupHTML = RenderPartialToString("VariantGroup", type, this.ControllerContext) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult RenderVariantGroupAttribute(int productPropertyTypeId, string name = null, string value = null)
        {
            try
            {
                var type = ProductPropertyType.LoadFull(productPropertyTypeId);
                var propertyValue = new ProductPropertyValue();

                if (name != null)
                {
                    propertyValue.Name = name.Trim();
                }
                if (value != null)
                {
                    propertyValue.Value = value.Trim();
                }
                type.ProductPropertyValues.Add(propertyValue);
                return Json(new { result = true, variantGroupAttributeHTML = RenderPartialToString("VariantGroupAttribute", propertyValue) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void UpdateCurrentProductFromProductDetailsViewModel(Product product, ProductDetailsModel model)
        {
            product.StartEntityTrackingAndEnableLazyLoading();

            product.Weight = model.Weight;
            product.ProductBackOrderBehaviorID = model.BackOrderBehaviorId;
            product.ShowKitContents = model.ShowKitContents.HasValue && model.ShowKitContents.Value;
            product.Save();

            product.ExcludedShippingMethods.SyncTo(model.ExcludedShippingMethodIds ?? new List<int>(), esm => esm.ShippingMethodID, methodId => ShippingMethod.Load(methodId));
        }

        private void UpdateBaseProductFromProductDetailsViewModel(Product product, ProductDetailsModel model)
        {
            var productBase = product.ProductBase;
            productBase.StartEntityTrackingAndEnableLazyLoading();

            productBase.ProductTypeID = model.ProductTypeId;
            productBase.TaxCategoryID = model.TaxCategoryId;
            productBase.ChargeShipping = model.ChargeShipping;
            productBase.ChargeTax = model.ChargeTax;
            productBase.ChargeTaxOnShipping = model.ChargeTaxOnShipping;
            productBase.IsShippable = model.IsShippable;
            productBase.UpdateInventoryOnBase = model.UpdateInventoryOnBase;
            productBase.Save();

            SetProductVariantsDetails(model, productBase, product);
        }

        protected virtual void SetProductVariantsDetails(ProductDetailsModel model, ProductBase productBase, Product product)
        {
            var excludedShippingMethodIds = model.ExcludedShippingMethodIds ?? new List<int>();
            if (product.IsVariantTemplate)
            {
                var productIDs = productBase.Products.Select(m => m.ProductID);
                foreach (var productID in productIDs)
                {
                    var variantProduct = Product.LoadFull(productID);
                    variantProduct.ProductBackOrderBehaviorID = model.BackOrderBehaviorId;
                    variantProduct.ExcludedShippingMethods.RemoveAll();
                    variantProduct.Save();

                    variantProduct.ExcludedShippingMethods.RemoveAll();

                    if (excludedShippingMethodIds.Any())
                    {
                        variantProduct.ExcludedShippingMethods.AddRange(ShippingMethod.LoadBatch(excludedShippingMethodIds));
                        variantProduct.Save();
                    }

                }
            }
            productBase.Save();
        }

        protected virtual bool ShouldDisplayChargeTaxOnShipping()
        {
            return !NetSteps.Data.Entities.AvataxAPI.Util.IsAvataxEnabled();
        }

        private void CopyExcludedShippingMethodsFromProductBase(ProductBase productBase)
        {
            var nonVariantProduct = productBase.Products.FirstOrDefault(p => !p.IsVariant());
            if (nonVariantProduct != null)
            {
                var productIds = productBase.Products.Where(p => p.IsVariant()).Select(m => m.ProductID);
                if (productIds.Any())
                {
                    nonVariantProduct = Product.LoadFull(nonVariantProduct.ProductID);
                    var excludedShippingMethods = nonVariantProduct.ExcludedShippingMethods.Select(nvp => nvp.ShippingMethodID);
                    var containsExcludedShippingMethods = excludedShippingMethods.Any();
                    var excludedShippingMethodsList = excludedShippingMethods.ToList();

                    var products = Product.LoadBatchFull(productIds.ToList());
                    for (var index = 0; index < products.Count(); index++)
                    {
                        var product = products.ElementAt(index);
                        product.ExcludedShippingMethods.RemoveAll();
                        product.Save();
                        if (containsExcludedShippingMethods)
                        {
                            product = Product.LoadFull(product.ProductID);
                            product.ExcludedShippingMethods.AddRange(ShippingMethod.LoadBatch(excludedShippingMethodsList));
                            product.Save();
                        }
                    }
                }
            }
        }

        private Product GetRequestedProduct(int? productBaseId, int? productId)
        {
            Contract.Assert(productBaseId.HasValue || productId.HasValue);

            if (productId.HasValue)
            {
                var product = Product.LoadFull(productId.Value);
                if (product != null)
                {
                    if (productBaseId.HasValue && product.ProductBaseID != productBaseId.Value)
                    {
                        throw new ArgumentException("productBaseId and product.ProductBaseID do not match");
                    }
                    else if (!product.IsVariantTemplate)
                    {
                        return product;
                    }
                }

            }

            var productBase = ProductBase.LoadFull(productBaseId.Value);
            return productBase.Products.Count > 1 ?
                productBase.Products.FirstOrDefault(p => p.IsVariantTemplate) : productBase.Products.First();
        }
    }
}
