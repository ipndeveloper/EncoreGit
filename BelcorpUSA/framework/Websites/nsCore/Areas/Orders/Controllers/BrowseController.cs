using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Common.Base;

namespace nsCore.Areas.Orders.Controllers
{
	public class BrowseController : BaseController
	{
		[FunctionFilter("Orders", "~/Accounts")]
		public virtual ActionResult Index(string orderNumber, string lastFour, int? status, int? type, DateTime? startDate, DateTime? endDate, int? accountId, int? marketId, int? periodId)
		{
			try
			{
                Session["Edit"] = "3";

				OrderSearchParameters searchParams = new NetSteps.Data.Entities.Business.OrderSearchParameters()
				{
					CreditCardLastFourDigits = lastFour,
					OrderStatusID = status,
					OrderTypeID = type,
					StartDate = startDate,
					EndDate = endDate,
					ConsultantOrCustomerAccountID = accountId,
					OrderNumber = orderNumber,
					PageIndex = 0,
					PageSize = 20,
					OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
					MarketID = marketId,
                    PeriodID = periodId
				};

				if (!String.IsNullOrEmpty(orderNumber))
				{
					try
					{
						var order = Order.LoadByOrderNumber(orderNumber);
						if (order != null)
							return RedirectToAction("Index", "Details", new { id = order.OrderNumber });

					}
					catch (NetStepsDataException ex)
					{
						//Catch Invalid Order Number exceptions and send user to the browse page showing no results
						EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
					}
				}

				return View(searchParams);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		protected virtual bool IsPackingSlipPrintable(OrderSearchData order)
		{
			return (order.OrderStatusID == (short)Constants.OrderStatus.Paid
										  || order.OrderStatusID == (short)Constants.OrderStatus.Printed
										  && IsValidOrderType(order));
		}

		protected virtual bool IsValidOrderType(OrderSearchData order)
		{
			return order.OrderTypeID != (short)Constants.OrderType.AutoshipTemplate
				   && order.OrderTypeID != (short)Constants.OrderType.ReturnOrder;
		}

		[OutputCache(CacheProfile = "PagedGridData")]
		public virtual ActionResult GetOrders(int page, int pageSize, string lastFour, int? status, int? type, DateTime? startDate, DateTime? endDate, int? accountId,
            int? market, int? period, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string orderNumber
            , string invoiceNumber)
		{
			try
			{
                if (startDate.HasValue && startDate.Value.Year < 1900)
                    startDate = null;
                if (endDate.HasValue && endDate.Value.Year < 1900)
                    endDate = null;
                StringBuilder builder = new StringBuilder();

                OrderBusinessLogic orderBL = new OrderBusinessLogic();
               
               
                if (invoiceNumber.Trim().Length > 0)
                    orderNumber = orderBL.GetOrderNumberByInvoiceNumber(invoiceNumber);

                var orders = Order.Search(new OrderSearchParameters()
                {
                    CreditCardLastFourDigits = lastFour,
                    OrderStatusID = status,
                    OrderTypeID = type,
                    StartDate = startDate,
                    EndDate = endDate,
                    ConsultantOrCustomerAccountID = accountId,
                    OrderNumber = orderNumber,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    MarketID = market,
                    PeriodID = period
                });




                var periods = Periods.Search();

                if (orders.Count > 0)
                {
                    
                    int count = 0;
                    foreach (var order in orders)
                    {
                        decimal qvTotal = Order.Repository.GetTotal(order.OrderID, Constants.ProductPriceType.QV);

                        var PaymentMethod = orderBL.GetPaymentConfigurationByOrderID(order.OrderID);

                        var CompleteDateDB = orderBL.GetOrderCompleteDateDB(order.OrderID);


                        string completeDate = order.CompleteDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo);
                        completeDate += completeDate != "N/A" ? (" " + CompleteDateDB.Item2) : string.Empty;

                        string CreationPeriod = CompleteDateDB.Item3 != string.Empty ? periods.Where(x => x.PeriodID.ToString() == CompleteDateDB.Item3).First().Description : string.Empty;
                        string CompletePeriod = CompleteDateDB.Item4 != string.Empty ? periods.Where(x => x.PeriodID.ToString() == CompleteDateDB.Item4).First().Description : string.Empty;

                        string InvoiceNumber = orderBL.GetInvoiceNumberByOrderID(order.OrderID);

                        var currency = SmallCollectionCache.Instance.Currencies.GetById(order.CurrencyID);
                        builder.Append("<tr>")
                            .AppendCheckBoxCell(value: order.OrderNumber, disabled: IsPackingSlipPrintable(order) ? false : true)
                            .AppendLinkCell("~/Orders/Details/Index/" + order.OrderNumber, order.OrderNumber)
                            .AppendLinkCell("~/Orders/Details/Invoices?orderNumber=" + order.OrderNumber, InvoiceNumber)
                            .AppendLinkCell("~/Accounts/Overview/Index/" + order.AccountNumber, order.AccountNumber)
                            .AppendLinkCell("~/Accounts/Overview/Index/" + order.AccountNumber, order.FirstName)
                            .AppendLinkCell("~/Accounts/Overview/Index/" + order.AccountNumber, order.LastName)
                            .AppendCell(order.OrderStatus)
                            .AppendCell(order.OrderType)
                            .AppendCell(CreationPeriod)
                            .AppendCell(CompletePeriod)
                            .AppendCell(completeDate)
                            .AppendCell(PaymentMethod == null ? string.Empty : PaymentMethod.Description)
                           .AppendCell(order.Subtotal.ToString("C", CoreContext.CurrentCultureInfo), style: "text-align: right; padding-right: 10px;")
                            .AppendCell(order.GrandTotal.ToString("C", CoreContext.CurrentCultureInfo), style: "text-align: right; padding-right: 10px;")


                            //.AppendCell(order.Subtotal.ToString("C", currency.Culture), style: "text-align: right; padding-right: 10px;")
                            //.AppendCell(order.GrandTotal.ToString("C", currency.Culture), style: "text-align: right; padding-right: 10px;")
                            .AppendLinkCell("~/Accounts/Overview/Index/" + order.SponsorAccountNumber, order.Sponsor)
                            //INI - GR4176 - No mostrar campo QVTotal en formato moneda.
                            //.AppendCell(qvTotal.ToString("C", currency.Culture), style: "text-align: right; padding-right: 10px;")
                              .AppendCell(qvTotal.GetRoundedNumber(0).ToString())  
                            //FIN - GR4176 - No mostrar campo QVTotal en formato moneda.
                            .Append("</tr>");
                        ++count;

                    }
                    return Json(new { result = true, totalPages = orders.TotalPages, page = builder.ToString() });
                }
                else
                    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
	}
}
