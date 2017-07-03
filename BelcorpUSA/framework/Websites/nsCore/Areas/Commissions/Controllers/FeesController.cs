using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business;
using NetSteps.Common.Globalization;

namespace nsCore.Areas.Commissions.Controllers
{
    public class FeesController : Controller
    {
        //
        // GET: /Commissions/Fees/

        public ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Edit(int? id)
        {
            try
            {
                if (id != null)
                {
                    var listDisburmentFees = Fee.Get(new FeeSearchParameters());
                    var editDisburmentFee = listDisburmentFees.FirstOrDefault(x => x.DisbursementFeeID == (int)id);
                    ViewBag.EditDisburmentFee = editDisburmentFee;
                }
                ViewBag.DisbursementTypes = Minimum.ListDropDownTypes("DisbursementTypes");
                ViewBag.Currencies = Minimum.ListDropDownTypes("Currencies");
                ViewBag.DisbursementFeeTypes = Minimum.ListDropDownTypes("DisbursementFeeTypes");
                return View();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                var fees = Fee.Get(new FeeSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                });
                foreach (var fee in fees)
                {
                    builder.Append("<tr>");
                    builder.Append(String.Format("<td><input type='checkbox' class='DisbursementFeeID' value='{0}' /></td>", fee.DisbursementFeeID));
                    builder.Append(String.Format("<td><a href='Edit/{0}'>{1}</a></td>", fee.DisbursementFeeID, fee.DisbursementFeeID));
                    builder.Append(String.Format("<td>{0}</td>", fee.FeeType));
                    builder.Append(String.Format("<td>{0}</td>", Translation.GetTerm(fee.DisbursementTypeTermName, fee.DisbursementType)));
                    builder.Append(String.Format("<td>{0}</td>", fee.CurrencyName));
                    builder.Append(String.Format("<td>{0}</td>", fee.Amount.ToString("N",CoreContext.CurrentCultureInfo)));
                    //builder.Append(String.Format("<td>{0}</td>", fee.Amount));
                    builder.Append("</tr>");
                }
                return Json(new { result = true, totalPages = fees.TotalPages, page = fees.TotalCount == 0 ? String.Format("<tr><td colspan=\"6\">{0}</td></tr>", Translation.GetTerm("ThereAreNoFees", "There are no Fees")) : builder.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult Save(FeeSearchParameters searchParameter)
        {
            try
            {
                int newDisbursementFeeID = Fee.Save(new FeeSearchParameters()
                {
                    DisbursementFeeID = searchParameter.DisbursementFeeID,
                    DisbursementFeeTypeID = searchParameter.DisbursementFeeTypeID,
                    DisbursementTypeID = searchParameter.DisbursementTypeID,
                    CurrencyID = searchParameter.CurrencyID,
                    Amount = searchParameter.Amount
                });

                return Json(new { result = true, disbursementFeeID = newDisbursementFeeID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult Delete(List<int> items, bool active)
        {
            try
            {

                if (items == null || items.Count == 0)
                {
                    return Json(new { result = false, message = Translation.GetTerm("SelectFeeds", "You must select at least a fee") });
                }

                string disbursementFeeIDs = "";

                foreach (int item in items)
                {
                    disbursementFeeIDs += item + ",";
                }

                if (disbursementFeeIDs.Length > 0) disbursementFeeIDs = disbursementFeeIDs.Substring(0, disbursementFeeIDs.Length - 1);

                Fee.Delete(new FeeSearchParameters() { DisbursementFeeIDs = disbursementFeeIDs });

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
