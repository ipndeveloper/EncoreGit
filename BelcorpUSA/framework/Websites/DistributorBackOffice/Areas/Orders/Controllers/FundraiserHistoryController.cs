using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Account.Controllers;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;

namespace DistributorBackOffice.Areas.Orders.Controllers
{
	public class FundraiserHistoryController : BaseAccountsController
	{
		[OutputCache(CacheProfile = "PagedGridData")]
		[FunctionFilter("Orders-Order History", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Get(int page, int pageSize, int? status, DateTime? startDate, DateTime? endDate, string accountNumberOrName, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string orderNumber, bool searchOpenParties = false)
		{
			try
			{
				int type = (int)Constants.OrderType.FundraiserOrder;

				if (startDate.HasValue && startDate.Value.Year < 1900)
					startDate = null;
				if (endDate.HasValue && endDate.Value.Year < 1900)
					endDate = null;

				var cache = Create.New<IOrderSearchCache>();
				var orders = cache.Search(new OrderSearchParameters()
				{
					PageIndex = page,
					PageSize = pageSize,
					OrderStatusID = status,
					OrderTypeID = type,
					StartDate = startDate.StartOfDay(),
					EndDate = endDate.EndOfDay(),
					ConsultantAccountID = CurrentAccount.AccountID,
					OrderNumber = orderNumber,
					OrderBy = orderBy,
					OrderByDirection = orderByDirection,
					SearchOpenParties = searchOpenParties
				});

				if (orders.Count > 0)
				{
					var builder = OverridableHistoryGridView(orders);
					return Json(new { totalPages = orders.TotalPages, page = builder.ToString() });
				}

				return Json(new { totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		protected virtual StringBuilder OverridableHistoryGridView(IEnumerable<OrderSearchData> orders)
		{
			var builder = new StringBuilder();
			foreach (OrderSearchData order in orders)
			{
				var currency = SmallCollectionCache.Instance.Currencies.GetById(order.CurrencyID);
				builder.Append("<tr>")
					.AppendLinkCell("~/Orders/Details/Index/" + order.OrderNumber, order.OrderNumber)
					.AppendCell(order.FirstName)
					.AppendCell(order.LastName)
					.AppendCell(order.OrderStatus + GetResumeLink(order))
					.AppendCell(order.OrderType)
					.AppendCell(order.CompleteDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
					.AppendCell(order.DateShipped.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
					.AppendCell(order.Subtotal.ToString("C", currency.Culture), style: "text-align: right; padding-right: 10px;")
					.AppendCell(order.GrandTotal.ToString("C", currency.Culture), style: "text-align: right; padding-right: 10px;")
					.AppendCell(order.CommissionDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
					.AppendCell(order.Sponsor)
					.Append("</tr>");
			}

			return builder;
		}

		[OutputCache(CacheProfile = "PagedGridData")]
		[FunctionFilter("Orders-Party Order History", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult GetOpenFundraisers(int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection)
		{
			try
			{
				var orderStatuses = new List<int>
                                        {
                                            (int) Constants.OrderStatus.Pending,
                                            (int) Constants.OrderStatus.PartiallyPaid,
                                            (int) Constants.OrderStatus.CreditCardDeclined
                                        };

				var openParties = Party.Search(new PartySearchParameters()
				{
					PageIndex = page,
					PageSize = pageSize,
					OrderBy = orderBy,
					OrderByDirection = orderByDirection,
					OrderStatuses = orderStatuses,
					AcountID = CurrentAccount.AccountID,
					ExcludedOrderTypes = new List<int>() { Constants.OrderType.OnlineOrder.ToInt() }
					//NumberOfOpenDays = 60
				});

				// this is going to cause loading to be very slow after a while, need to revisit
				var filteredOpenParties = openParties.Where(x => Order.Load(x.OrderID).OrderTypeID == (int)Constants.OrderType.FundraiserOrder);

				var builder = new StringBuilder();
				var count = 0;
				foreach (var party in filteredOpenParties)
				{
					builder.Append("<tr").Append(count % 2 == 1 ? " class=\"Alt\"" : "").Append(">")
						.AppendCell(party.UseEvites ? "<a href=\"javascript:void(0);\" class=\"UI-icon-container eviteStatsLink\" title=\"" + Translation.GetTerm("GetPartyStats", "Get Party Statistics") + "\"><span class=\"UI-icon icon-info\"></span><input type=\"hidden\" class=\"partyId\" value=\"" + party.PartyID + "\" /></a>" : "<a href=\"javascript:void(0);\" style=\"display:none;\"></a>", "icon-24 IconCol")
						.AppendLinkCell(string.Format("~/Orders/FundRaisers/DetermineStep?partyId={0}", party.PartyID), party.OrderNumber)
						.AppendCell(party.Name)
						.AppendCell(party.StartDate.ToString(CoreContext.CurrentCultureInfo))
						.AppendCell(NetSteps.Data.Entities.Account.ToFullName(party.HostFirstName, string.Empty, party.HostLastName, CoreContext.CurrentCultureInfo.Name))
						.AppendCell(party.Total.ToDecimal().ToString(party.CurrencyID))
						.Append("</tr>");
					++count;
				}

				return Json(new { result = true, totalPages = openParties.TotalPages, page = builder.ToString() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}


		protected virtual string GetResumeLink(OrderSearchData osd)
		{
			if (osd.OrderStatusID == (int)Constants.OrderStatus.Pending && osd.AccountNumber == CurrentAccount.AccountNumber)
			{
				return string.Format(" <a href=\"{0}\" class=\"ResumeOrderLink\">Resume</a>", "Orders/OrderEntry?orderTypeID=" + osd.OrderTypeID + "&OrderId=" + osd.OrderID);
			}
			return string.Empty;
		}

	}
}
