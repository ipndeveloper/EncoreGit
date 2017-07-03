using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Products.Controllers
{
    public class CatalogsController : BaseProductsController
    {
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                var catalogs = Catalog.Search(new FilterPaginatedListParameters<Catalog>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });
                foreach (var catalog in catalogs)
                {
                    builder.Append("<tr>");

                    if (catalog.Editable)
                    {
                        builder.AppendCheckBoxCell(value: catalog.CatalogID.ToString());
                    }
                    else
                    {
                        builder.AppendCell(string.Empty);
                    }
                    builder
                        .AppendCell(!string.IsNullOrEmpty(catalog.Markets) ? catalog.Markets : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                        .AppendCell(!string.IsNullOrEmpty(catalog.StoreFronts) ? catalog.StoreFronts : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                        .AppendLinkCell("~/Products/Catalogs/Edit/" + catalog.CatalogID, catalog.Name)
                        .AppendLinkCell("~/Products/Categories/EditTree/" + catalog.CategoryTreeID, catalog.CategoryTreeName)
                        .AppendCell(catalog.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
                        .AppendCell((catalog.StartDate.HasValue ? catalog.StartDate.Value.ToString("g") : "N/A") + " - " + (catalog.EndDate.HasValue ? catalog.EndDate.Value.ToString("g") : "N/A"))
                        .Append("</tr>");
                    ++count;
                }
                return Json(new { result = true, totalPages = catalogs.TotalPages, page = catalogs.TotalCount == 0 ? "<tr><td colspan=\"5\">There are no catalogs</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult ChangeStatus(List<int> items, bool active)
        {
            if (items != null && items.Count > 0)
            {
                try
                {
                    foreach (var catalog in Catalog.LoadBatch(items))
                    {
                        if (catalog.Active != active)
                        {
                            catalog.Active = active;
                            catalog.Save();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage });
                }
            }
            return Json(new { result = true });
        }

        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Edit(int? id)   
        {
            try
            {
				var catalog = id.HasValue ? Catalog.LoadFull(id.Value) : new Catalog { Editable = true };
                ViewBag.CatalogTypes = SmallCollectionCache.Instance.CatalogTypes
                    .Where(x => x.Active)
                    .OrderBy(x => x.CatalogTypeID) // So that "Normal" is the default selection
                    .Select(x => new SelectListItem { Text = x.GetTerm(), Value = x.CatalogTypeID.ToString() });
                ViewBag.AccountTypes = SmallCollectionCache.Instance.AccountTypes.ToList();

                //Developed by BAL - CSTI - AINI
                ViewData["Plans"] = from plan in Periods.GetPlans()
                                    select new SelectListItem()
                                    {
                                        Text = plan.Value,
                                        Value = plan.Key,
                                        Selected = false
                                    };

                var catalogsPeriods = Catalog.SearchCatalogPeriods(id.HasValue ? id.Value : 0);
                if (catalogsPeriods != null)
                {
                    ViewData["CatalogPeriods"] = from period in catalogsPeriods
                                                 select new SelectListItem()
                                                 {
                                                     Text = period.PeriodDescription,
                                                     Value = period.PeriodID.ToString(),
                                                     Selected = false
                                                 };
                }
                else
                {
                    ViewData["CatalogPeriods"] = new List<SelectListItem>();
                }

                ViewData["Periods"] = new List<SelectListItem>();
                
                //Developed by BAL - CSTI - AFIN

                ViewBag.CurrentYear = DateTime.Now.Year.ToString();
                return View(catalog);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        //Developed by BAL - CSTI - AINI
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult GetPeriodsByPlan(int planId, string yearFilter, string catalogPeriods)
        {
            try
            {
                List<PeriodSearchData> periods = null;

                var lstCatalogPeriods = catalogPeriods.Split('|');
                if (!lstCatalogPeriods[0].IsNullOrEmpty())
                {
                    periods = Periods.Search().FindAll(x => x.PlanID == planId &&
                                                            x.StartDate.Year == Convert.ToInt32(yearFilter) &&
                                                            !lstCatalogPeriods.Contains(x.PeriodID.ToString()));
                }
                else
                {
                    periods = Periods.Search().FindAll(x => x.PlanID == planId && x.StartDate.Year == Convert.ToInt32(yearFilter));
                }

                var periodsAdd = from period in periods
                              select new SelectListItem()
                              {
                                  Text = period.Description,
                                  Value = period.PeriodID.ToString(),
                                  Selected = false
                              };

                return Json(new { result = true, data = periodsAdd });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        //Developed by BAL - CSTI - AFIN

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetDescription(int? catalogId, int languageId)
        {
            try
            {
                if (!catalogId.HasValue)
                    return Json(new { result = true, name = "", shortDescription = "", longDescription = "" });

                Catalog catalog = Catalog.Load(catalogId.Value);

                var description = catalog.Translations.GetByLanguageID(languageId);
                if (description == null)
                    description = new DescriptionTranslation() { LanguageID = languageId };

                return Json(new { result = true, name = description.Name, shortDescription = description.ShortDescription, longDescription = description.LongDescription });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult SearchPossibleProducts(int catalogId, string query)
        {
            try
            {
                return Json(Product.SlimSearch(query).Select(p => new { id = p.ProductID, text = p.SKU + " - " + p.Name }));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetItems(int page, int pageSize, int catalogId)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                var catalogItems = CatalogItem.Search(new FilterPaginatedListParameters<CatalogItem>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = "Product.SKU",
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                    WhereClause = ci => ci.CatalogID == catalogId
                });

                foreach (CatalogItem item in catalogItems)
                {
                    Product product = Inventory.GetProduct(item.ProductID);
                    builder.Append("<tr id=\"catalogItem").Append(item.CatalogItemID).Append("\">")
                        .AppendCheckBoxCell(value: item.CatalogItemID.ToString())
                        .AppendLinkCell(string.Format("~/Products/Products/Overview/{0}/{1}", product.ProductBaseID, product.ProductID), product.SKU)
                        .AppendCell(product.Translations.Name())
                        .AppendCell(item.StartDate.HasValue ? item.StartDate.Value.ToString("g") : "N/A", "startDate")
                        .AppendCell(item.EndDate.HasValue ? item.EndDate.Value.ToString("g") : "N/A", "endDate")
                        .Append("</tr>");
                }
                return Json(new { result = true, resultCount = catalogItems.TotalCount /*catalogItems.Count */, catalogItems = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult ChangeItemSchedules(List<int> catalogItems, DateTime? startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime)
        {
            try
            {
                foreach (CatalogItem catalogItem in CatalogItem.LoadBatch(catalogItems))
                {
                    if (catalogItem != default(CatalogItem))
                    {
                        catalogItem.StartDate = startDate.AddTime(startTime);
                        catalogItem.EndDate = endDate.AddTime(endTime);
                        catalogItem.Save();
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
        public virtual ActionResult RemoveItems(List<int> catalogItems)
        {
            try
            {
                foreach (int catalogItemId in catalogItems)
                    CatalogItem.Delete(catalogItemId);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult AddItem(int catalogId, int productId, DateTime? startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime)
        {
            try
            {
                Catalog.BulkAddCatalogItems(catalogId, new List<int>() { productId }, startDate.AddTime(startTime), endDate.AddTime(endTime));
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult TryAddItem(int catalogId, string query)
        {
            try
            {
                var products = Product.SlimSearch(query);
                if (products.Count > 0)
                {
                    Catalog.BulkAddCatalogItems(catalogId, new List<int>() { products.First().ProductID }, null, null);
                    return Json(new { result = true });
                }
                else
                    return Json(new { result = false });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult BulkAddItems(int catalogId, DateTime? startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime, List<int> products)
        {
            try
            {
                Catalog.BulkAddCatalogItems(catalogId, products, startDate.AddTime(startTime), endDate.AddTime(endTime));
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
        public virtual ActionResult Copy(int catalogId, int copyCatalogId)
        {
            try
            {
                Catalog.Copy(copyCatalogId, catalogId);
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
        public virtual ActionResult Save(int? catalogId, int languageId, string name, string shortDescription, string longDescription, bool active, short catalogTypeID,
            DateTime? startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime, int categoryTreeId, List<int> storeFronts, List<int> accountTypes
            //Developed by BAL - CSTI - A01
            , string catalogPeriods)
        {
            try
            {
                var catalog = catalogId.HasValue ? Catalog.LoadFull(catalogId.Value) : new Catalog();

                catalog.Active = active;
                catalog.StartDate = startDate.AddTime(startTime);
                catalog.EndDate = endDate.AddTime(endTime);
                catalog.CategoryID = categoryTreeId;
                catalog.CatalogTypeID = catalogTypeID;

                var description = catalog.Translations.GetByLanguageIdOrDefaultInList(languageId);

                description.LanguageID = languageId;
                description.Name = name;
                description.ShortDescription = shortDescription;
                description.LongDescription = longDescription;

                if (storeFronts == null)
                    storeFronts = new List<int>();
                catalog.StoreFronts.SyncTo(storeFronts, s => s.StoreFrontID, id => StoreFront.Load(id));
                if ((catalog.CatalogTypeID == (int)ConstantsGenerated.CatalogType.EnrollmentKits ||
                    catalog.CatalogTypeID == (int)ConstantsGenerated.CatalogType.AutoshipBundles) && accountTypes.Any())
                {
                    catalog.AccountTypes.SyncTo(accountTypes, s => s.AccountTypeID, id => AccountType.Load((short)id));
                }
                else
                {
                    catalog.AccountTypes.RemoveAll();
                }

                catalog.Save();

                //Developed by BAL - CSTI - A01
                Catalog.SaveCatalogsPeriods(catalog.CatalogID, catalogPeriods);

                return Json(new { result = true, catalogId = catalog.CatalogID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
