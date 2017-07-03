using System;
using NetSteps.Common.Configuration;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Encore.Core.IoC;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models.Browse;
using System.Collections.Generic;
using System.Data;
using NetSteps.Data.Entities.Business.Logic;

namespace nsCore.Areas.Commissions.Controllers
{
	public class DisbursementsController : BaseCommissionController
    {        
        [FunctionFilter("Commissions", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View();
        }

        [FunctionFilter("Admin-Create and Edit Disbursements", "~/Admin/Disbursements")]
        [HttpPost]
        public virtual ActionResult ProcessDisbursements(int periodID)
        {
            var payoneerDisbursementTypeID = Convert.ToInt32(ConfigurationManager.AppSettings["PayoneerDisbursementTypeID"]);
            //var disbursements = disburse.LoadDisbursementByPeriod(periodID);
            //var payoneerDisbursements = disbursements.Where(d => d.DisbursementProfile.DisbursementTypeID == payoneerDisbursementTypeID).ToList();
            //
            var data = new { PayoneerDisbursementStatus = 1 };//Payoneer.SubmitPayments(payoneerDisbursements) };
            return Json(data);
        }

        public virtual ActionResult BrowsePayments()
        {
            return View();
        }
        //<!-- CSTI - mescobar -INICIO-->
        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult searchAccount(string query)
        {
            try
            {

                var resultado = NetSteps.Web.Mvc.Extensions.DictionaryExtensions.ToAJAXSearchResults(AccountCache.GetAccountSearchByTextResults(query));
                return Json(resultado);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        //<!-- CSTI - mescobar -FIN-->

        //#region CSTI -FHP
        //[OutputCache(CacheProfile = "DontCache")]
        //public virtual ActionResult Get(int page, int pageSize, string orderBy, string orderByDirection)
        //{
        //    try
        //    {
        //        //if (warehouse == null)
        //        //{
        //        //    return Json(new { totalPages = 0, page = "<tr><td colspan=\"7\">There are no WareHouseInventory. Warehouses must be include for searching.</td></tr>", message = "WAREHOUSES MUST BE INCLUDE FOR SEARCHING" });

        //        //}
        //        if (orderByDirection == "Descending")
        //        {
        //            orderByDirection = "desc";
        //        }
        //        else
        //        {
        //            orderByDirection = "asc";
        //        }
        //        StringBuilder builder = new StringBuilder();
        //        int count = 0;
        //        var wareHouseMaterial = WareHouseMaterials.Search(new WareHouseMaterialSearchParameters()
        //        {
        //            WareHouseID = warehouse,
        //            MaterialID = materialId,
        //            ProductID = productoId,
        //            PageIndex = page,
        //            PageSize = pageSize,
        //            OrderBy = orderBy,
        //            Order = orderByDirection
        //        });
        //        foreach (var wareHouseMaterials in wareHouseMaterial)
        //        {
        //            builder.Append("<tr>");
        //            builder
        //               .AppendLinkCell("~/Products/Materials/EditMaterial/" + wareHouseMaterials.MaterialID, wareHouseMaterials.SKU)
        //               .AppendLinkCell("~/Products/Materials/EditMaterial/" + wareHouseMaterials.MaterialID, wareHouseMaterials.MaterialName)
        //               .AppendCell(wareHouseMaterials.CostAvarage.ToString())
        //               .AppendCell(wareHouseMaterials.QuantityOnHand.ToString())
        //               .AppendCell(wareHouseMaterials.QuantityBuffer.ToString())
        //               .AppendCell(wareHouseMaterials.ReorderLevel.ToString())
        //               .AppendCell(wareHouseMaterials.QuantityAllocated.ToString());
        //            builder.AppendLinkCell("javascript:void(0);", Translation.GetTerm("AddManualMovement", "Add Manual Movement"), linkCssClass: "btnViewStats", linkID: wareHouseMaterials.WarehouseMaterialID.ToString());
        //            builder.Append("</tr>");
        //            ++count;
        //        }
        //        return Json(new { result = true, totalPages = wareHouseMaterial.TotalPages, page = wareHouseMaterial.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no WareHouseInventory</td></tr>" : builder.ToString() });

        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
        //        return Json(new { result = false, message = exception.PublicMessage });
        //    }
        //}

    }
}
