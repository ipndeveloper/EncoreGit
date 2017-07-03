using System;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;

namespace nsCore.Areas.Orders.Controllers
{
    public class AutoshipsController : BaseController
    {
        [FunctionFilter("Orders", "~/Orders")]
        public virtual ActionResult Index()
        {
            return View();
        }

        [FunctionFilter("Orders", "~/Orders")]
        public virtual ActionResult AutoshipBatchView(int autoshipBatchId)
        {
            ViewData["AutoshipBatchID"] = autoshipBatchId;
            return PartialView();
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetAutoshipBatchLog(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                switch (orderBy)
                {
                    case "ID":
                        orderBy = "AutoshipBatchID";
                        break;
                    case "Succeeded":
                        orderBy = "SucceededCount";
                        break;
                    case "Failure":
                        orderBy = "FailureCount";
                        break;
                    case "UserName":
                        orderBy = "Username";
                        break;
                    case "EndDate":
                        orderBy = "EndDateUTC";
                        break;
                    default:
                        orderBy = "StartDateUTC";
                        break;
                }

                var autoshipBatches = AutoshipOrder.GetAutoshipRunReport(new NetSteps.Data.Entities.Business.AutoshipOrderSearchParameters()
                {
                    PageSize = pageSize,
                    PageIndex = page,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                StringBuilder builder = new StringBuilder();
                if (autoshipBatches.Count > 0)
                {
                    int count = 0;
                    int languageID = ApplicationContext.Instance.CurrentLanguageID;
                    foreach (var autoShipBatch in autoshipBatches)
                    {
                        builder.Append("<tr id=\"row").Append(autoShipBatch.AutoshipBatchID).Append("\" class=\"").Append(count % 2 == 0 ? "GridRow" : "GridRowAlt").Append("\" ")
                                .Append("style=\"cursor:pointer;\" onclick=\"showBatchDetails('").Append(autoShipBatch.AutoshipBatchID)
                                .Append("');\" onmouseover=\"highlightRow('").Append(autoShipBatch.AutoshipBatchID).Append("');\" >")
                                .AppendCell(string.Format("<span id=\"row{0}img\" class=\"UI-icon icon-arrowNext-circle\" ></span>", autoShipBatch.AutoshipBatchID))
                                .AppendCell(autoShipBatch.AutoshipBatchID.ToString())
                                .AppendCell(autoShipBatch.StartDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                                .AppendCell(autoShipBatch.EndDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                                .AppendCell(autoShipBatch.SucceededCount.ToString())
                                .AppendCell(autoShipBatch.FailureCount.ToString())
                                .AppendCell(autoShipBatch.UserName)
                                .AppendCell(autoShipBatch.Notes)
                            .Append("</tr>");
                        count++;
                    }
                    return Json(new { result = true, resultCount = autoshipBatches.TotalCount, data = builder.ToString() });
                }
                else
                    return Json(new { result = true, resultCount = 0, data = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetAutoshipLog(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, int autoshipBatchID)
        {
            try
            {
                switch (orderBy)
                {
                    case "TemplateID":
                        orderBy = "TemplateOrderID";
                        break;
                    case "OrderID":
                        orderBy = "NewOrderID";
                        break;
                    case "DateRan":
                        orderBy = "DateAutoshipRanUTC";
                        break;
                    case "Results":
                        orderBy = "Results";
                        break;
                    default:
                        orderBy = "AutoshipLogID";
                        break;
                }

                var autoshipLogs = AutoshipOrder.LoadAutoshipLogsByAutoshipBatchID(new NetSteps.Data.Entities.Business.AutoshipLogSearchParameters()
                {
                    AutoshipBatchID = autoshipBatchID,
                    PageSize = pageSize,
                    PageIndex = page,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                StringBuilder builder = new StringBuilder();
                if (autoshipLogs.Count > 0)
                {
                    int count = 0;
                    foreach (var logRow in autoshipLogs)
                    {
                        string templateUrl = string.Format("~/Accounts/Autoships/Edit/{0}?autoshipScheduleId={1}", logRow.AccountNumber, logRow.AutoshipScheduleID);
                        builder.Append("<tr class=\"").Append(count % 2 == 0 ? "GridRow" : "GridRowAlt").Append("\" >")
                            .AppendCell(logRow.AutoshipLogID.ToString())
                            .AppendLinkCell(templateUrl, logRow.TemplateOrderID.ToString())
                            .AppendLinkCell("~/Orders/Details/Index/" + logRow.NewOrderNumber, logRow.NewOrderNumber)
                            .AppendCell(Translation.GetTerm(logRow.Succeeded.ToString()))
                            .AppendCell(logRow.Results)
                            .AppendCell(logRow.DateAutoshipRan.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                            .Append("</tr>");
                        count++;
                    }
                    return Json(new { result = true, resultCount = autoshipLogs.TotalCount, data = builder.ToString() });
                }
                else
                    return Json(new { result = true, resultCount = 0, data = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
