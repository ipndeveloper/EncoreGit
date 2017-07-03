using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models.Overview;

namespace nsCore.Areas.Accounts.Controllers
{

    using Constants = NetSteps.Data.Entities.Constants;
    using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
    using System.Globalization;
    using NetSteps.Common.Base;

	public class OverviewController : BaseAccountsController
	{
		[FunctionFilter("Accounts", "~/Sites")]
		public virtual ActionResult Index(string id)
		{
            int? AccountID_ = 0;
            AccountID_ = id.ToInt();
            if (AccountID_ == 0)
            {
                if (CoreContext.CurrentAccount != null)
                {
                    if (CoreContext.CurrentAccount.AccountID != null && CoreContext.CurrentAccount.AccountID != 0)
                    {

                        AccountID_ = CoreContext.CurrentAccount.AccountID;
                    }
                }
            }

            id = AccountID_.ToString();

			try
			{
				var action = CheckForAccount(id, forceLoadAccount: true);
				if (action is RedirectResult || action is RedirectToRouteResult)
					return action;

				var account = CoreContext.CurrentAccount;

				// isUserActive determines whether to display the account proxy links.
				var isUserActive =
					 account.User != null
					 && account.User.UserStatusID == (short)Constants.UserStatus.Active;

				// Get account proxy links.
				var proxyLinks = ProxyLink.GetAccountProxyLinks(account.AccountID, account.AccountTypeID);

				// Get autoship/subscription data.
				var autoshipScheduleOverviews = AutoshipOrder.GetOverviews(account.AccountID, account.AccountTypeID);

				var model = new IndexModel()
					 .LoadResources(
						  account.AccountID,
						  isUserActive,
						  proxyLinks,
						  autoshipScheduleOverviews
					 );

                //Obteniendo IsLocked                
                var varBlocking = BlockingType.GetAccountIsLocked(new BlockingTypeSearchParameters()
                {
                    AccountID = account.AccountID,
                    LanguageID = CoreContext.CurrentLanguageID
                });
                Session["IsLocked"] = varBlocking.Description;  
                
                Session["Edit"] = "3";
               // model.IsLocked = blockingType;

                /*CS.19AG2016.Inicio.Carga si son Ordenes Propieas o Todos*/
                Dictionary<int, string> dcOwnOrders = new Dictionary<int, string>();
                //dcOwnOrders.Add(1, Translation.GetTerm("OwnOrders", "Own Orders"));
                dcOwnOrders.Add(2, Translation.GetTerm("All", "All"));
                ViewBag.dcOwnOrders = dcOwnOrders;
                /*CS.19AG2016.Fin.Carga si son Ordenes Propieas o Todos*/

                // En el modo de edicion se tiene que excluir algunos estados de accountStatuses segun "   BR-RG-001 - Reglas de reinicio"
                ViewData["ExcludeAccountStatuses"] = "ExcludeAccountStatuses";
				return View(model);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetOrderHistory(int page, int pageSize, byte OwnOrder, string orderNumber, int? orderStatus, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
		{
			try
			{
				if (orderNumber == Translation.GetTerm("OrderID", "Order ID"))
					orderNumber = "";

				var cache = Create.New<IOrderSearchCache>();
				var orders = cache.Search(new NetSteps.Data.Entities.Business.OrderSearchParameters()
				{
					PageIndex = page,
					PageSize = pageSize,
					OrderStatusID = orderStatus,
					ConsultantOrCustomerAccountID = CoreContext.CurrentAccount.AccountID,
					OrderNumber = orderNumber,
					OrderBy = orderBy,
					OrderByDirection = orderByDirection,
				});

				if (orders.TotalCount > 0)
				{
					StringBuilder builder = new StringBuilder();
                    /*CS.02MAY2016.Inicio.Obtener cultureInfo BRA*/
                    string cultureInfo = Currency.Load((int)ConstantsGenerated.Currency.BrazilianReal).CultureInfo.ToString();
                    //string cultureInfo = Language.Load(CoreContext.CurrentLanguageID).CultureInfo.ToString();/*Según Language*/
                    /*CS.02MAY2016.Fin*/

                    /*CS.05/04/2017.Inicio.Obtener cultureInfo configuración */
                    string cultura = CoreContext.CurrentCultureInfo.ToString();
                    //string cultureInfo = Language.Load(CoreContext.CurrentLanguageID).CultureInfo.ToString();/*Según Language*/
                    /*CS.05/04/2017.Fin*/



                    int count = 0;
					foreach (OrderSearchData order in orders)
					{
                        /*CS.19AGO2016.Inicio.Filtrar solo Ordenes propias de la Cuenta*/
                        if (OwnOrder == 1 && order.AccountNumber != CoreContext.CurrentAccount.AccountID.ToString())
                        {
                            continue;
                        }
                        /*CS.19AGO2016.Fin.Filtrar solo Ordenes propias de la Cuenta*/
						builder.Append("<tr class=\"").Append(order.IsReturnOrder() ? "ReturnOrder" : "").Append(order.IsAutoshipOrder() ? "AutoShipOrder" : "").Append("\">")
                             .AppendLinkCell("~/Accounts/Overview/Index/" + order.AccountNumber, order.AccountNumber)
							 .AppendLinkCell("~/Orders/Details/Index/" + order.OrderNumber, order.OrderNumber)
							 .AppendCell(!order.DateCreated.HasValue ? "N/A" : order.DateCreated.ToShortDateString())
							 .AppendCell(order.OrderType)
							 .AppendCell(order.OrderStatus)
							 .AppendCell(!order.DateShipped.HasValue ? "N/A" : order.DateShipped.ToShortDateString())

                              //  comentado por IPN : Se comento porque solo formatea el monto en formato valido para brazil
                            //.AppendCell(order.Subtotal.ToString("C", CultureInfo.CreateSpecificCulture(cultureInfo)), style: "text-align: right; padding-right: 10px;")
                            //.AppendCell(order.GrandTotal.ToString("C", CultureInfo.CreateSpecificCulture(cultureInfo)), style: "text-align: right; padding-right: 10px;")

                            .AppendCell(order.Subtotal.ToString("C", CultureInfo.CreateSpecificCulture(cultura)), style: "text-align: right; padding-right: 10px;")
                            .AppendCell(order.GrandTotal.ToString("C", CultureInfo.CreateSpecificCulture(cultura)), style: "text-align: right; padding-right: 10px;")
							 .Append("</tr>");
						++count;
					}
					return Json(new { result = true, totalPages = orders.TotalPages, page = builder.ToString() });
				}
				else
				{
					return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"7\">" + Translation.GetTerm("NoOrdersFoundWithThatCriteriaPleaseTryAgain", "No orders found with that criteria. Please try again.") + "</td></tr>" });
				}
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Accounts", "~/Sites")]
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult AuditHistory()
		{
			try
			{
				if (CoreContext.CurrentAccount == null)
					return RedirectToAction("Index");

				ViewData["EntityName"] = "Account";
				ViewData["ID"] = CoreContext.CurrentAccount.AccountID;
				ViewData["LoadedEntitySessionVarKey"] = "CurrentAccount";

				ViewData["Links"] = new StringBuilder("<a href=\"").Append("~/Accounts/Overview".ResolveUrl()).Append("\">" + Translation.GetTerm("Overview") + "</a> | <a href=\"").Append("~/Orders/OrderEntry?accountId=".ResolveUrl() + CoreContext.CurrentAccount.AccountID)
					 .Append("\">" + Translation.GetTerm("PlaceNewOrder", "Place New Order") + "</a> | <a href=\"").Append("~/Accounts/Overview/PoliciesChangeHistory".ResolveUrl()).Append("\">" + Translation.GetTerm("PoliciesChangeHistory", "Policies Change History") + "</a> | " + Translation.GetTerm("AuditHistory", "Audit History")).ToString();

				return View("AuditHistory", "Accounts");
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[FunctionFilter("Accounts", "~/Sites")]
		public virtual ActionResult PoliciesChangeHistory(string id)
		{
			try
			{
				if (!string.IsNullOrEmpty(id))
				{
					CoreContext.CurrentAccount = Account.LoadForSessionByAccountNumber(id);
				}
				if (CoreContext.CurrentAccount == null)
					return RedirectToAction("Index");

				return View(AccountPolicy.LoadByAccountID(CoreContext.CurrentAccount.AccountID));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		#region Helper Methods
		protected virtual StringBuilder DisplayProxyLinksIfAccountActive(Account account)
		{
			StringBuilder builder = new StringBuilder();

			if (account.User.IsNotNull() && account.User.UserStatusID != Constants.UserStatus.Active.ToShort())
				return builder.Append(Translation.GetTerm("InActiveUserStatus", "User status is inactive."));

			IList<ProxyLink> accountProxyLinks = GetAccountProxyLinks(account);

			// Process Template data in links
			ProxyLink.GetAccountProxyLinks(account.AccountID, account.AccountTypeID);

			foreach (ProxyLink proxyLink in accountProxyLinks.Where(pl => pl.Active))
			{
				builder.Append(string.Format(
					 "<li><a href=\"{0}\" target=\"_blank\" rel=\"external\">{1}</a></li>", proxyLink.URL,
					 proxyLink.GetTerm()));
			}

			return builder;
		}

		protected virtual IList<ProxyLink> GetAccountProxyLinks(Account account)
		{
			var accountProxyLinks = new List<ProxyLink>();

			if (account.AccountTypeID != (int)ConstantsGenerated.AccountType.PreferredCustomer
				 && account.AccountTypeID != (int)ConstantsGenerated.AccountType.RetailCustomer)
			{
				//var proxyLinks = SmallCollectionCache.Instance.ProxyLinks;
				var proxyLinks = ProxyLink.LoadAllFull();
				foreach (var proxyLink in proxyLinks)
				{
					var proxyLinkClone = proxyLink.Clone();

					var eldResolver = Create.New<IEldResolver>();

					proxyLinkClone.URL = eldResolver.EldDecode(new Uri(proxyLinkClone.URL)).AbsoluteUri;

					accountProxyLinks.Add(proxyLinkClone);
				}
			}

			return accountProxyLinks;
		}
		#endregion
	}
}
