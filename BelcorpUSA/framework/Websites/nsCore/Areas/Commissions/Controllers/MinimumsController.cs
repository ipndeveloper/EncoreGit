using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Common.Globalization;

namespace nsCore.Areas.Commissions.Controllers
{
    public class MinimumsController : Controller
    {
        //
        // GET: /Commissions/Minimums/

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
                    var listDisburmentMinimums = Minimum.Get(new MinimumSearchParameters());
                    var editDisburmentMinimum = listDisburmentMinimums.FirstOrDefault(x => x.DisbursementMinimumID == (int)id);
                    ViewBag.EditDisburmentMinimum = editDisburmentMinimum;
                }

                ViewBag.DisbursementTypes = Minimum.ListDropDownTypes("DisbursementTypes");
                ViewBag.Currencies = Minimum.ListDropDownTypes("Currencies");
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

                var minimums = Minimum.Get(new MinimumSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                });
                foreach (var minimum in minimums)
                {
                    builder.Append("<tr>");
                    builder.Append(String.Format("<td><input type='checkbox' class='DisbursementFeeID' value='{0}' /></td>", minimum.DisbursementMinimumID));
                    builder.Append(String.Format("<td><a href='Edit/{0}'>{1}</a></td>", minimum.DisbursementMinimumID, minimum.DisbursementMinimumID));
                    builder.Append(String.Format("<td>{0}</td>", Translation.GetTerm(minimum.DisbursementTypeTermName, minimum.DisbursementType)));
                    builder.Append(String.Format("<td>{0}</td>", minimum.CurrencyName));
                    builder.Append(String.Format("<td>{0}</td>", minimum.MinimumAmount.ToString("C",CoreContext.CurrentCultureInfo)));
                    builder.Append("</tr>");
                }
                return Json(new { result = true, totalPages = minimums.TotalPages, page = minimums.TotalCount == 0 ? String.Format("<tr><td colspan=\"6\">{0}</td></tr>", Translation.GetTerm("ThereAreNoMinimums", "There are no Minimums")) : builder.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult Save(MinimumSearchParameters searchParameter)
        {
            try
            {
                int newDisbursementMinimumID = Minimum.Save(new MinimumSearchParameters()
                {
                    DisbursementMinimumID = searchParameter.DisbursementMinimumID,
                    MinimumAmount  = searchParameter.MinimumAmount,
                    CurrencyID = searchParameter.CurrencyID,
                    DisbursementTypeID = searchParameter.DisbursementTypeID
                });

                return Json(new { result = true, disbursementMinimumID = newDisbursementMinimumID });
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
                    return Json(new { result = false, message = Translation.GetTerm("SelectMinimums", "You must select at least a minimum") });
                }

                string disbursementMinimunIDs = "";
                
                foreach (int item in items)
                {
                    disbursementMinimunIDs += item + ",";
                }

                if (disbursementMinimunIDs.Length > 0) disbursementMinimunIDs = disbursementMinimunIDs.Substring(0, disbursementMinimunIDs.Length - 1);
                
                Minimum.Delete(new MinimumSearchParameters() { DisbursementMinimumIDs = disbursementMinimunIDs });
                
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
