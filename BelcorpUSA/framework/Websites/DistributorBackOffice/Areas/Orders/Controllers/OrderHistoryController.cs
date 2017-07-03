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
using NetSteps.Data.Common.Services;
using System.Data;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Business.Logic;//CMR


namespace DistributorBackOffice.Areas.Orders.Controllers
{
	public class OrderHistoryController : BaseAccountsController
	{
        [FunctionFilter("Orders-Order History", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult Index(int? type = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            /*CS.09JUL2016.Inicio.Retoma de Primer Pedido a PWS*/
            var account = CurrentAccount;
            if (account != null && account.AccountStatusID == (short)NetSteps.Data.Entities.Constants.AccountStatus.BegunEnrollment && account.EnrollmentDateUTC == null)
            {
                Tuple<int, int, int> result = new OrderBusinessLogic().CheckOrdersByAccountID(account.AccountID);
                var otherOrders = result.Item2;
                if (otherOrders == 0)
                {
                    string token = "";
                    token = NetSteps.Data.Entities.Account.GetSingleSignOnToken(account.AccountID);

                    if (token != "")
                    {
                        Response.Redirect(ProxyLink.Load(2).URL + "?token=" + token + "&kitInicio=true");
                    }
                }
            }
            /*CS.09JUL2016.Fin.Retoma de Primer Pedido a PWS*/


            // Null out the existing order to remove any unsaved modifications
            // when a customer leaves an order without saving.
            // The assumption is that to get back to the party you have to go through this page.
            // (Unless you click back.)
            if (OrderContext.Order != null) OrderContext.Order = null;

            ViewData["SelectedReport"] = type.HasValue ? type.Value.ToString() : (startDate.HasValue ? "Last30Days" : "NA");

            if (startDate.HasValue && startDate.Value.Year < 1900)
                startDate = new DateTime(2000, 1, 1);
            if (endDate.HasValue && endDate.Value.Year < 1900)
                endDate = DateTime.Now;

            var orderSearchParameters = new NetSteps.Data.Entities.Business.OrderSearchParameters()
            {
                OrderTypeID = type,
                StartDate = startDate.StartOfDay(),
                EndDate = endDate.EndOfDay(),
                ConsultantOrCustomerAccountID = CurrentAccount.AccountID
            };

            return View(orderSearchParameters);
        }

        ////[FunctionFilter("Orders-Order History", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        ////public virtual ActionResult Index(int? type = null, DateTime? startDate = null, DateTime? endDate = null)
        ////{
        ////    // Null out the existing order to remove any unsaved modifications
        ////    // when a customer leaves an order without saving.
        ////    // The assumption is that to get back to the party you have to go through this page.
        ////    // (Unless you click back.)
        ////    if (OrderContext.Order != null) OrderContext.Order = null;

        ////    ViewData["SelectedReport"] = type.HasValue ? type.Value.ToString() : (startDate.HasValue ? "Last30Days" : "NA");

        ////    if (startDate.HasValue && startDate.Value.Year < 1900)
        ////        startDate = new DateTime(2000, 1, 1);
        ////    if (endDate.HasValue && endDate.Value.Year < 1900)
        ////        endDate = DateTime.Now;

        ////    var orderSearchParameters = new NetSteps.Data.Entities.Business.OrderSearchParameters()
        ////    {
        ////        OrderTypeID = type,
        ////        StartDate = startDate.StartOfDay(),
        ////        EndDate = endDate.EndOfDay(),
        ////        ConsultantOrCustomerAccountID = CurrentAccount.AccountID
        ////    };

        ////    return View(orderSearchParameters);
        ////}

		[OutputCache(CacheProfile = "PagedGridData")]
		[FunctionFilter("Orders-Order History", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Get(int page, int pageSize, int? status, int? type, DateTime? startDate, DateTime? endDate, string accountNumberOrName, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string orderNumber, bool searchOpenParties = false)
		{
			try
			{
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
					StringBuilder builder = OverridableHistoryGridView(orders);
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

        //Developed by Wesley Campos S. - CSTI
        private decimal GetTotalQV(int ORDERID)
        {
            decimal result = 0;
            IDataReader dr = DataAccess.ExecuteReader(DataAccess.GetCommand("upsGetItemPrices", new Dictionary<string, object>() { {"@ORDERID", ORDERID} }, "Core"));
            while (dr.Read())
            {
                result = Convert.ToDecimal(dr["TotalQV"]);
                break;
            }
            dr.Close();
            return result;
        }

		protected virtual StringBuilder OverridableHistoryGridView(IEnumerable<OrderSearchData> orders)
		{
			var builder = new StringBuilder();
            decimal TotalQV;
			foreach (OrderSearchData order in orders)
			{
                TotalQV = 0;
				var currency = SmallCollectionCache.Instance.Currencies.GetById(order.CurrencyID);
                TotalQV = GetTotalQV(order.OrderID);
				builder.Append("<tr>")
					.AppendLinkCell("~/Orders/Details/Index/" + order.OrderNumber, order.OrderNumber)
                    //INI - GR4172 
                    .AppendCell(order.PeriodID)
                    //FIN - GR4172
                    .AppendCell(order.FirstName)
					.AppendCell(order.LastName)
					.AppendCell(order.OrderStatus + GetResumeLink(order))
					.AppendCell(order.OrderType)
                    .AppendCell(order.CompleteDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                    .AppendCell(order.DateShipped.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))



                    //.AppendCell(order.CompleteDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                    //.AppendCell(order.DateShipped.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                    //INI - GR4176 - No mostrar campo QVTotal en formato moneda.
                    //.AppendCell(TotalQV.ToString("C", currency.Culture), style: "text-align: right; padding-right: 10px;") //CSTI(WCS)-09/01/2015
                      .AppendCell(TotalQV.GetRoundedNumber(0).ToString())
                    //FIN - GR4176 - No mostrar campo QVTotal en formato moneda.
                    //.AppendCell(order.Subtotal.ToString("C", currency.Culture), style: "text-align: right; padding-right: 10px;") //CGI(CMR)-24/10/2014
                    //.AppendCell(order.TotalQV.ToString("C", currency.Culture), style: "text-align: right; padding-right: 10px;") ////CGI(CMR)-24/10/2014
                    .AppendCell(order.GrandTotal.ToString("C",CoreContext.CurrentCultureInfo), style: "text-align: right; padding-right: 10px;")
                    //.AppendCell(order.GrandTotal.ToString("C", currency.Culture), style: "text-align: right; padding-right: 10px;")
					.AppendCell(order.CommissionDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
					.AppendCell(order.Sponsor)
					.Append("</tr>");
			}

			return builder;
		}


		[OutputCache(CacheProfile = "PagedGridData")]
		[FunctionFilter("Orders-Party Order History", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult GetOpenParties(int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection)
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
				var filteredOpenParties = openParties.Where(x => Order.Load(x.OrderID).OrderTypeID != (int)Constants.OrderType.FundraiserOrder);

				var builder = new StringBuilder();
				var count = 0;
				foreach (var party in filteredOpenParties)
				{
					builder.Append("<tr").Append(count % 2 == 1 ? " class=\"Alt\"" : "").Append(">")
						.AppendCell(party.UseEvites ? "<a href=\"javascript:void(0);\" class=\"UI-icon-container eviteStatsLink\" title=\"" + Translation.GetTerm("GetPartyStats", "Get Party Statistics") + "\"><span class=\"UI-icon icon-info\"></span><input type=\"hidden\" class=\"partyId\" value=\"" + party.PartyID + "\" /></a>" : "<a href=\"javascript:void(0);\" style=\"display:none;\"></a>", "icon-24 IconCol")
						.AppendLinkCell(String.Concat("~/Orders/Party/DetermineStep?partyId=", party.PartyID), party.OrderNumber)
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

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult GetOrdersCount()
		{
			try
			{
				var searchParams = new NetSteps.Data.Entities.Business.OrderSearchParameters()
				{
					PageIndex = 0,
					PageSize = 1,
					ConsultantAccountID = CurrentAccount.AccountID
				};

				var allOrdersCount = Order.Count(searchParams);

				searchParams.StartDate = DateTime.Now.AddDays(-30).StartOfDay();
				searchParams.EndDate = DateTime.Now.EndOfDay();

				var last30DaysOrdersCount = Order.Count(searchParams);

				searchParams.StartDate = null;
				searchParams.EndDate = null;
				searchParams.OrderTypeID = Constants.OrderType.OnlineOrder.ToInt();
				//searchParams.AccountTypeID = Constants.AccountType.RetailCustomer.ToInt();

				var retailOrdersCount = Order.Count(searchParams);

				//searchParams.OrderTypeID = Constants.OrderType.OnlineOrder.ToInt();
				searchParams.AccountTypeID = Constants.AccountType.PreferredCustomer.ToInt();

				var pcOrdersCount = Order.Count(searchParams);

				return Json(new { result = true, allOrdersCount = allOrdersCount, last30DaysOrdersCount = last30DaysOrdersCount, retailOrdersCount = retailOrdersCount, pcOrdersCount = pcOrdersCount });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
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
