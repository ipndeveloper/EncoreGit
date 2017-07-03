using System;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Accounts.Controllers
{
    public class OrderHistoryController : BaseAccountsController
    {
        [FunctionFilter("Accounts", "~/Accounts")]
        public virtual ActionResult Index(string id)
        {
            this.AccountNum = id;
            return View();
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult Get(int page, int pageSize, int? status, int? type, DateTime? startDate, DateTime? endDate, string accountNumberOrName, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string orderNumber, int accountID)
        {
            try
            {
                if (startDate.HasValue && startDate.Value.Year < 1900)
                {
                    return Json(new { result = false, message = Translation.TermTranslation.GetTerm("PleaseEnterAValidStartDate", "Please enter a valid start date.") });
                }

                if (startDate.HasValue && startDate.Value.Year < 1900)
                {
                    return Json(new { result = false, message = Translation.TermTranslation.GetTerm("PleaseEnterAValidEndDate", "Please enter a valid end date.") });
                }

                if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                {
                    string errorMessage = Translation.TermTranslation.GetTerm("StartDateCannotBeGreaterThanEndDateMessage", "Start Date ({0}) can not exceed End Date ({1})");
                    return Json(new { result = false, message = String.Format(errorMessage, startDate.ToShortDateString(), endDate.ToShortDateString()) });
                }

				var cache = Create.New<IOrderSearchCache>();
                var orders = cache.Search(new OrderSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderStatusID = status,
                    OrderTypeID = type,
                    StartDate = startDate.StartOfDay(),
                    EndDate = endDate.EndOfDay(),
                    ConsultantOrCustomerAccountID = accountID,
                    OrderNumber = orderNumber,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                if (orders.Count > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (OrderSearchData order in orders)
                    {
                        var currency = SmallCollectionCache.Instance.Currencies.GetById(order.CurrencyID);
                        builder.Append("<tr>")
                            .AppendLinkCell("~/Orders/Details/Index/" + order.OrderNumber, order.OrderNumber)
                            .AppendCell(order.FirstName)
                            .AppendCell(order.LastName)
                            .AppendCell(order.OrderStatus)
                            .AppendCell(order.OrderType)
                            .AppendCell(order.CompleteDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                            .AppendCell(order.DateShipped.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))

                            .AppendCell(order.Subtotal.ToString("C", CoreContext.CurrentCultureInfo), style: "text-align: right; padding-right: 10px;")
                            .AppendCell(order.GrandTotal.ToString("C", CoreContext.CurrentCultureInfo), style: "text-align: right; padding-right: 10px;")
                            // comentado por IPN => inicio 15042017 => para generalizar el formato de modea
                            //.AppendCell(order.Subtotal.ToString("C", currency.Culture), style: "text-align: right; padding-right: 10px;")
                            //.AppendCell(order.GrandTotal.ToString("C", currency.Culture), style: "text-align: right; padding-right: 10px;")
                            // fin 15042017
                            .AppendCell(order.CommissionDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                            .AppendLinkCell("~/Accounts/Overview/Index/" + order.SponsorAccountNumber, order.Sponsor)
                            .Append("</tr>");
                    }
                    return Json(new { totalPages = orders.TotalPages, page = builder.ToString() });
                }

                return Json(new { totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
