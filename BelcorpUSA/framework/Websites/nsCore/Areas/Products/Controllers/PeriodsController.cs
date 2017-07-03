using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Common.Globalization;
using System.Globalization;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Products.Controllers
{
    public class PeriodsController : BaseController
    {
        //
        // GET: /Products/Periods/
        [FunctionFilter("Products", "~/Accounts")]
        public ActionResult Index(int? planId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                PeriodSearchParameters parameters = Periods.SetDefaultValuesToFilters(planId, startDate, endDate);

                // Plans to filter
                TempData["Plans"] = Periods.GetPlans();

                return View(parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetPeriods(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? planId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var periods = Periods.Search(new PeriodSearchParameters()
                {
                    PlanID = planId,
                    StartDate = startDate,
                    EndDate = endDate,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var period in periods)
                {
                    builder.Append("<tr>");

                    builder
                        .AppendLinkCell("~/Products/Periods/Edit/" + period.PeriodID, period.PeriodID.ToString())
                        .AppendCell(period.PlanName)
                        .AppendCell(period.Description)
                        .AppendCell(period.StartDate.ToString("g"))
                        .AppendCell(period.LockDate.ToString("g"))
                        .AppendCell(period.EndDate.ToString("g"))
                        .AppendCell(period.ClosedDate.HasValue ? period.ClosedDate.Value.ToString("g") : "N/A", Translation.GetTerm("ClosedDate", "Closed Date"))
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = periods.TotalPages, page = periods.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no periods</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Edit(int? id)
        {
            try
            {
                var period = id.HasValue ? Periods.Search().Find(x => x.PeriodID == id) : new PeriodSearchData();

                // Default values, new Period
                Periods.SetDefaultValuesToViewNewPeriod(period, id);

                // Plans to select
                var plans = Periods.GetPlans();
                TempData["Plans"] = from plan in plans
                                    select new SelectListItem()
                                    {
                                        Text = plan.Value,
                                        Value = plan.Key,
                                        Selected = period.PlanID.ToString() == plan.Key ? true : false
                                    };

                return View(period);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Save(
            int? periodId,
            string startDate,
            string endDate,
            string lockDate,
            int planId,
            string description)
        {
            try
            {
                List<PeriodSearchData> periods = Periods.Search();
                var period = periodId.HasValue ? periods.Find(x => x.PeriodID == periodId) : new PeriodSearchData();
                period.PlanID = planId;
                try
                {
                    IFormatProvider culture = new CultureInfo("es-Es", true);
                    period.StartDate = Convert.ToDateTime(startDate, culture);
                    period.LockDate = Convert.ToDateTime(lockDate, culture);
                    period.EndDate = Convert.ToDateTime(string.Format("{0} 23:59:59", endDate), culture);
                }
                catch
                {
                    return Json(new { result = false, message = Translation.GetTerm("CheckInputValues") });
                }

                period.Description = description;
                Periods.SetDefaultValuesToSave(period);
                int VerifyInputGeneralRules = Periods.VerifyInputGeneralRules(period, periodId);

                switch (VerifyInputGeneralRules)
                {
                    case 1:
                        return Json(new { result = false, periodId = period.PeriodID, message = Translation.GetTerm("CheckInputValues") });
                    case 2:
                        return Json(new { result = false, periodId = period.PeriodID, message = Translation.GetTerm("PeriodCantModified") });
                    default:
                        Periods.Save(period);
                        if (period.PeriodID <= 0) return Json(new { result = false, periodId = period.PeriodID, message = Translation.GetTerm("PeriodExists") });
                        else return Json(new { result = true, periodId = period.PeriodID });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult GetStartDateToSelectedPlan(int planId, int? periodID)
        {
            try
            {
                //**************************************++
                if (periodID.HasValue)//Edit
                {
                    var period = Periods.Search().Find(x => x.PeriodID == periodID);
                    return Json(new
                    {
                        result = true,
                        isEdit = true,
                        //startDate = String.Format("{0:MM/dd/yyyy}", period.StartDate),
                        //endDate = String.Format("{0:MM/dd/yyyy}", period.EndDate),
                        //lockDate = String.Format("{0:MM/dd/yyyy}", period.LockDate)

                        // inicio 06042017  por IPN
                        //startDate = String.Format("{0:dd/MM/yyyy}", period.StartDate),
                        //endDate = String.Format("{0:dd/MM/yyyy}", period.EndDate),
                        //lockDate = String.Format("{0:dd/MM/yyyy}", period.LockDate)

                        // FIN 06042014  

                        // INICIO AGREGADO POR IPN ==> PARA EL FORMATO DINAMICO DE LAS FECHAS 
                        startDate = period.StartDate.ToShortDateString(),
                        endDate = period.EndDate.ToShortDateString(),
                        lockDate = period.LockDate.ToShortDateString()

                        // FIN 

                    });
                }
                else//New
                {
                    DateTime? NewStartDate = Periods.GetStartDateCreatingMode(planId);
                    if (NewStartDate == null) return Json(new { result = false });
                    //else return Json(new { result = true, isEdit = false, startDate = String.Format("{0:MM/dd/yyyy}", NewStartDate.Value) });
                    else return Json(new { result = true, isEdit = false, startDate = String.Format("{0:dd/MM/yyyy}", NewStartDate.Value) });
                }
                //******************************************
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult VerifyOverlapDate(int? periodId, int planId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return Json(new { result = Periods.VerifyOverlapDate(periodId, planId, startDate, endDate) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
