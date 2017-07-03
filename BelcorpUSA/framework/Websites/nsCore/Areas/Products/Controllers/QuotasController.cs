using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Common.Globalization;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;
using nsCore.Areas.Products.Helpers;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Products.Controllers
{


    public class QuotasController : BaseProductsController
    {
        //
        // GET: /Products/Quotas/
        [FunctionFilter("Products", "~/Accounts")]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetQuotas(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            //int? productoId,
            string status = null, string SKUorName = null)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                int Active = -1;

                switch (status)
                {
                    case "true":
                        Active = 1;
                        break;
                    case "false":
                        Active = 0;
                        break;
                    default:
                        Active = -1;
                        break;
                }
                if (SKUorName.Length > 0)
                    pageSize = 800;

                var quotas = ProductQuotas.Search(new FilterPaginatedListParameters<ProductQuotaSearchData>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                }, Active, CoreContext.CurrentLanguageID, SKUorName);

                //var LquotaStatus = quotas.Where(x => ((status == "true") ? x.Active == true : ((status == "false") ? x.Active == false : (x.Active == true || x.Active == false)))).ToList();
                //var LquotasSkus = LquotaStatus.Where(x => x.ProductName.ToUpper().Contains(SKUorName.ToUpper()) || x.SKU.Contains(SKUorName)).ToList();

                //foreach (var quota in quotas.Where(x => ((status == "true") ? x.Active == true : ((status == "false") ? x.Active == false : (x.Active == true || x.Active == false)))
                //    && (x.ProductName.ToUpper().Contains(SKUorName.ToUpper()) || x.SKU.Contains(SKUorName)  )).ToList())
                foreach (var quota in quotas)
                //foreach (var quota in LquotasSkus.Skip(page * pageSize).Take(pageSize))
                {
                    builder.Append("<tr>");

                    builder.AppendCheckBoxCell(value: quota.RestrictionID.ToString(), name: "");

                    builder
                        .AppendLinkCell("~/Products/Quotas/Create/" + quota.RestrictionID, quota.Name)
                        .AppendCell(quota.SKU)
                        .AppendCell(quota.ProductName)
                        .AppendCell(quota.Active ? Translation.GetTerm("True", "True") : Translation.GetTerm("False", "False"), Translation.GetTerm("Active", "Active"))
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = quotas.TotalPages, page = quotas.TotalCount == 0 ? "<tr><td colspan=\"8\">There are no restrictions</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        //[FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Delete(List<int> items)
        {
            try
            {
                if (items.Count() > 0) ProductQuotas.Delete(items);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult ChangeQuotaStatus(List<int> items, bool active)
        {
            try
            {
                if (items.Count() > 0) ProductQuotas.ChangeQuotaStatus(items, active);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult Create(int id = 0)
        {
            var quotaEntity = ProductQuotas.Instance.LoadFullQuotaByID(id, CoreContext.CurrentLanguageID);
            ViewBag.StartPeriodID = quotaEntity.StartPeriodID.ToString();
            ViewBag.EndPeriodID = quotaEntity.EndPeriodID.ToString();
            return View(quotaEntity);
        }

        public ActionResult Save
            (
                string name,
                bool active,
                int RestrictionType,
                int StartPeriodId,
                int EndPeriodId,

                bool hasAccountTypes,
                string[] accountTypeIDs,

                bool hasTitleTypes, //EB-227
                string[] paidAsTitleIDs,
                string[] recognizedTitleIDs,

                bool hasAccount,
                string[] accountIDs,

                int productId,
                int quantity
                )
        {
            try
            {
                int restrictionID = 0;

                // Restriction
                var restriction = new ProductQuotaSearchData()
                {
                    Name = name,
                    ProductID = productId,
                    Quantity = quantity,
                    Active = active,
                    RestricionType = RestrictionType,
                    StartPeriodID = StartPeriodId,
                    EndPeriodID = EndPeriodId
                };

                restriction.TermName = String.Empty; //No definid
                restrictionID = ProductQuotas.Save(restriction,
                                                   hasAccountTypes,
                                                   accountTypeIDs,
                                                   hasTitleTypes,
                                                   paidAsTitleIDs,
                                                   recognizedTitleIDs,
                                                   hasAccount,
                                                   accountIDs); // Se incluye periodo wv: 20160427

                return Json(new { result = true, id = restrictionID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public ActionResult UpdateRestrictionState(int restrictionID, bool state, bool hasAccount, string[] accountIDs)
        {
            try
            {
                ProductQuotas.Instance.UpdateRestrictionState(restrictionID, state, hasAccount, accountIDs);

                return Json(new { result = true, id = restrictionID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

    }
}
