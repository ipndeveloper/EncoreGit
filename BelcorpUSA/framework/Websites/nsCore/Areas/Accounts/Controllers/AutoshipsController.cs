using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Comparer;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models.Autoships;

namespace nsCore.Areas.Accounts.Controllers
{
	public class AutoshipsController : nsCore.Areas.Orders.Controllers.OrdersBaseController
	{
		#region Helper methods

		protected virtual void UpdateOrderShipmentAddress(OrderShipment shipment, int addressId)
		{
			try
			{
				OrderContext.Order.AsOrder().UpdateOrderShipmentAddress(shipment, addressId);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
			}
		}

		private void UpdateSiteStatus(Site site, short? siteStatusID, Order order)
		{
			if (order.OrderStatusID == Constants.OrderStatus.Cancelled.ToShort())
			{
				site.SiteStatusID = Constants.SiteStatus.InActive.ToShort();

				// Delete the site's SiteUrl data
				site.SiteUrls.RemoveAllAndMarkAsDeleted();
			}
			else if (siteStatusID.HasValue)
			{
				site.SiteStatusID = siteStatusID.Value;
			}
		}

		private Site NewSite(AutoshipOrder autoshipOrder, AutoshipSchedule schedule)
		{
			//Grab the current account's main or shipping address
			Account coreContextAccount = CoreContext.CurrentAccount;

			if (coreContextAccount.Addresses == null)
				Account.LoadAddresses(coreContextAccount);

			Address tempAddress = coreContextAccount.Addresses.FirstOrDefault(ad => ad.IsDefault
				&& (ad.AddressTypeID == (int)Constants.AddressType.Main
				|| ad.AddressTypeID == (int)Constants.AddressType.Shipping)
			);

			//Load the appropriate country to obtain a marketID. If there is no address, load the base site's MarketID (US)
			int marketID = 0;
			if (tempAddress != null)
			{
				var country = tempAddress.GetCountryFromCache();
				if (country != null)
				{
					marketID = country.MarketID;
				}
			}
			if (marketID == 0)
			{
				if (schedule.BaseSiteID.HasValue)
				{
					marketID = Site.Load(schedule.BaseSiteID.Value).MarketID;
				}
				else
				{
					var market = SmallCollectionCache.Instance.Markets.FirstOrDefault(x => x.Active);
					if (market != null)
					{
						marketID = market.MarketID;
					}
				}
			}
			if (marketID == 0)
			{
				throw new Exception(Translation.GetTerm("ErrorThereAreNoActiveMarkets", "There are no active markets."));
			}

			//Use this new marketID 
			return new Site
			{
				AccountID = CoreContext.CurrentAccount.AccountID,
				AccountNumber = CoreContext.CurrentAccount.AccountNumber,
				CreatedByUserID = ApplicationContext.Instance.CurrentUserID,
				AutoshipOrderID = autoshipOrder.AutoshipOrderID,
				BaseSiteID = schedule.BaseSiteID,
				MarketID = marketID,
				IsBase = false,
				DateCreated = DateTime.Now,
				DateSignedUp = DateTime.Now,
				SiteTypeID = (int)Constants.SiteType.Replicated,
				SiteStatusID = (short)Constants.SiteStatus.Active
			};
		}

		protected ActionResult EditOrNew(int? autoshipOrderID, int? accountID, int? autoshipScheduleID)
		{
			try
			{
				AutoshipOrder autoshipOrder = null;
				Account account;
				AutoshipSchedule schedule;

				if (autoshipOrderID.HasValue)
				{
					autoshipOrder = AutoshipOrder.LoadFull(autoshipOrderID.Value);
					account = Account.LoadForSession(autoshipOrder.AccountID);
					schedule = AutoshipSchedule.LoadFull(autoshipOrder.AutoshipScheduleID);
				}
				else if (accountID.HasValue && autoshipScheduleID.HasValue)
				{
					account = Account.LoadForSession(accountID.Value);
					schedule = AutoshipSchedule.LoadFull(autoshipScheduleID.Value);
				}
				else
				{
					TempData["Error"] = "Either the Edit action providing an id matching an existing AutoShipOrderID must be set, or the New action providing an ID matching an Account and an autoshipScheduleID matching an AutoshipSchedule must be used";
					return RedirectToAction("Index", "Overview");
				}

				bool newAutoship = false;

				if (autoshipOrder == null)
				{
					autoshipOrder = AutoshipOrder.GenerateTemplateFromSchedule
					(
						autoshipScheduleID.Value,
						account,
						CoreContext.CurrentMarketId,
						saveAndChargeNewOrder: false
					);

					autoshipOrder.StartTracking();
					autoshipOrder.AccountID = account.AccountID;
					newAutoship = true;
				}

				CoreContext.CurrentAccount = account;
				CoreContext.CurrentAutoship = autoshipOrder;
				OrderContext.Order = autoshipOrder.Order;

				Address defaultShippingAddress = CoreContext.CurrentAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);

				if (defaultShippingAddress == null)
				{
					TempData["Error"] = Translation.GetTerm("NoDefaultShippingAddressOnRecord", "No Default Shipping Address On Record.");
					return RedirectToAction("Index", "Overview");
				}

				IEnumerable<ShippingMethodWithRate> shippingMethods = null;
				if (!schedule.IsVirtualSubscription)
				{
					if (newAutoship || OrderContext.Order.AsOrder().GetDefaultShipmentNoDefault() == null)
					{
						shippingMethods = defaultShippingAddress != null ? GetNewShippingMethods(defaultShippingAddress.AddressID) : new List<ShippingMethodWithRate>();
					}
					else
					{
						SetBackToDefaultShippingAddressIfOrderShipmentInvalid(autoshipOrder, defaultShippingAddress.AddressID);
						shippingMethods = ShippingCalculator.GetShippingMethodsWithRates(OrderContext.Order.AsOrder().OrderCustomers[0], OrderContext.Order.AsOrder().GetDefaultShipment());
					}
				}
				else
				{
					if (newAutoship || autoshipOrder.Order.GetDefaultShipmentNoDefault() == null)
					{
						UpdateOrderShipmentAddress(OrderContext.Order.AsOrder().GetDefaultShipment(), defaultShippingAddress.AddressID);
						//Our code requires all orders to have a shipping method for reasons I cannot understand.
						//Get the available shipping methods
						shippingMethods = ShippingCalculator.GetShippingMethodsWithRates(OrderContext.Order.AsOrder().OrderCustomers.First(), OrderContext.Order.AsOrder().GetDefaultShipment()).OrderBy(sm => sm.ShippingAmount).ToList();
						//This sets the correct shipping method on the order
						OrderContext.Order.AsOrder().ValidateOrderShipmentShippingMethod(OrderContext.Order.AsOrder().GetDefaultShipment(), shippingMethods);
						//The site autoship page will not actually display shippingmethod options, so null out the shippingMethods var
						shippingMethods = null;
					}
				}

				if (shippingMethods != null)
				{
					ViewData["ShippingMethods"] = shippingMethods;
				}

				OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
				OrderService.UpdateOrder(OrderContext);

				ViewData["NewAutoship"] = newAutoship;
				ViewData["Catalogs"] = Inventory.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID);
				ViewData["AutoshipSchedule"] = schedule;
				Session["AutoshipSchedule" + "_" + autoshipOrder.Order.OrderID.ToString()] = schedule;
				ViewData["AutoshipDay"] = autoshipOrder.Day;
				ViewBag.Dates = AutoshipDates(autoshipOrder);

				if (schedule.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.SiteSubscription)
				{
					var site = autoshipOrder.AutoshipOrderID > 0 ? Site.LoadByAutoshipOrderID(autoshipOrder.AutoshipOrderID) : null;
					if (site == null)
					{
						site = new Site();
						Address mainAddress = CurrentAccount.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Generated.ConstantsGenerated.AddressType.Main);
						int marketID = mainAddress != null ? SmallCollectionCache.Instance.Countries.GetById(mainAddress.CountryID).MarketID : 0;
						Site pwsSite = Site.LoadBaseSiteForNewPWS(marketID);
						ViewData["Domains"] = pwsSite != null ? pwsSite.GetDomains().ToArray() : new List<string>().ToArray();
					}
					else
					{
						Site baseSite = Site.LoadSiteWithSiteURLs(site.IsBase ? site.SiteID : site.BaseSiteID.ToInt());
						ViewData["Domains"] = baseSite != null ? baseSite.GetDomains().ToArray() : new List<string>().ToArray();
					}
					ViewData["Site"] = site;
				}

				CoreContext.CurrentAutoship = autoshipOrder;

				return View("Edit", OrderContext.Order.AsOrder());
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				TempData["Error"] = exception.PublicMessage;
				return RedirectToAction("Index", "Overview");
			}
		}

		protected virtual void SetBackToDefaultShippingAddressIfOrderShipmentInvalid(AutoshipOrder autoshipOrder, int defaultShippingAddressID)
		{
			OrderShipment currentOrderShipment = autoshipOrder.Order.GetDefaultShipment();
			if (currentOrderShipment.StateProvinceID == null && String.IsNullOrWhiteSpace(currentOrderShipment.State))
			{
				UpdateOrderShipmentAddress(currentOrderShipment, defaultShippingAddressID);
			}
		}

		protected virtual AutoshipDateViewModel AutoshipDates(AutoshipOrder autoshipOrder)
		{
			return new AutoshipDateViewModel
			{
				StartDate = autoshipOrder.StartDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo),
				EndDate = autoshipOrder.EndDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo),
				NextRunDate = autoshipOrder.NextRunDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo)
			};
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult ChangeCommissionConsultant(int commissionConsultantId)
		{
			try
			{
				OrderContext.Order.ConsultantID = commissionConsultantId;

				return Json(new
				{
					result = true,
					accountNumber = OrderContext.Order.AsOrder().ConsultantInfo.AccountNumber,
					fullName = OrderContext.Order.AsOrder().ConsultantInfo.FullName
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (CoreContext.CurrentOrder != null) ? CoreContext.CurrentOrder.OrderID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult ChangeCommissionDate(DateTime commissionDate)
		{
			try
			{
				OrderContext.Order.AsOrder().CommissionDate = commissionDate;

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (CoreContext.CurrentOrder != null) ? CoreContext.CurrentOrder.OrderID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpPost]
		public ActionResult CancelAutoship(int scheduleId)
		{
			try
			{
				AutoshipOrder autoshipOrder = CoreContext.CurrentAutoship;
				OrderContext.Order.AsOrder().OrderStatusID = (short)Constants.OrderStatus.Cancelled;
				autoshipOrder.Order = OrderContext.Order.AsOrder();
				autoshipOrder.Save();

				AutoshipSchedule schedule = AutoshipSchedule.Load(scheduleId);
				if (schedule.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.SiteSubscription)
				{
					Site site = Site.LoadByAutoshipOrderID(autoshipOrder.AutoshipOrderID);
					UpdateSiteStatus(site, null, autoshipOrder.Order);
					site.Save();
				}

				DomainEventQueueItem.AddAutoshipCanceledEventToQueue(autoshipOrder.Order.OrderID, bccToSponsor: true);

				return JsonSuccess();
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult Submit(string invoiceNotes, int scheduleId, int paymentMethodId, int? autoshipScheduleDay,
			string siteName, string siteDescription, short? siteStatusId, int? siteDefaultLanguageId, List<SiteUrl> urls, DateTime nextRunDate, DateTime? endDate, DateTime? startDate)
		{
			AutoshipOrder autoshipOrder = CoreContext.CurrentAutoship;
			autoshipOrder.StartEntityTracking();

			// OrderID needs to be set in order to save the order - JHE
			foreach (var orderCustomer in OrderContext.Order.AsOrder().OrderCustomers)
			{
				foreach (var orderPayment in orderCustomer.OrderPayments)
				{
					orderPayment.OrderID = OrderContext.Order.OrderID;
				}
			}

			try
			{
				if (!invoiceNotes.ToCleanString().IsNullOrEmpty())
				{
					var invoiceNote = OrderContext.Order.AsOrder().Notes.FirstOrDefault(n => n.NoteTypeID == Constants.NoteType.OrderInvoiceNotes.ToInt());
					if (invoiceNote == null)
					{
						invoiceNote = new Note()
						{
							DateCreated = DateTime.Now,
							UserID = CoreContext.CurrentUser.UserID,
							NoteTypeID = Constants.NoteType.OrderInvoiceNotes.ToInt(),
						};
						OrderContext.Order.AsOrder().Notes.Add(invoiceNote);
					}
					invoiceNote.NoteText = invoiceNotes.Trim();
				}

				if (autoshipScheduleDay.HasValue)
				{
					autoshipOrder.SetScheduleDay(autoshipScheduleDay.Value);
				}

				autoshipOrder.NextRunDate = nextRunDate;
				autoshipOrder.StartDate = startDate;
				autoshipOrder.EndDate = endDate;
				autoshipOrder.Save();

				if (OrderContext.Order.AsOrder().OrderStatusID != (short)Constants.OrderStatus.Cancelled)
				{
					OrderService.SubmitOrder(OrderContext);
				}

				autoshipOrder.Order = OrderContext.Order.AsOrder();
				CoreContext.CurrentAutoship = autoshipOrder;
				AutoshipSchedule schedule = AutoshipSchedule.Load(scheduleId);

				if (schedule.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.SiteSubscription)
				{
					var syncUrls = true;
					Site site = Site.LoadByAutoshipOrderID(autoshipOrder.AutoshipOrderID) ?? NewSite(autoshipOrder, schedule);
					site.Name = siteName;
					site.Description = siteDescription;
					site.DefaultLanguageID = siteDefaultLanguageId.ToInt();
					UpdateSiteStatus(site, siteStatusId, OrderContext.Order.AsOrder());

					if (urls != null && urls.Count > 0)
					{
						if (site.SiteStatusID != (short)Constants.SiteStatus.InActive)
						{
							foreach (SiteUrl siteUrl in urls)
							{
								if (!site.SiteUrls.Any(su => su.Url.Contains(siteUrl.Url)) && !SiteUrl.IsAvailable(siteUrl.Url))
								{
									//workaround - allow a user to claim a url that is already associated to their account 
									var existingSite = Site.LoadByUrl(siteUrl.Url);
									if (existingSite != null && existingSite.AccountID == autoshipOrder.AccountID)
									{
										site = existingSite;
										site.AutoshipOrder = autoshipOrder;
										syncUrls = false;
									}
									else
										return Json(new { result = false, message = string.Format("The site url '{0}' is unavailable please enter a different one.", siteUrl.Url) });
								}
							}

							urls[0].IsPrimaryUrl = true;
							if (syncUrls)
								site.SiteUrls.SyncTo(urls, new LambdaComparer<SiteUrl>((su1, su2) => su1.SiteUrlID == su2.SiteUrlID, su => (su.SiteUrlID > 0) ? su.SiteUrlID : su.Url.GetHashCode()), (su1, su2) => su1.Url = su2.Url);
						}
					}
					else
						site.SiteUrls.RemoveAllAndMarkAsDeleted();

					site.Save();
				}

				return Json(new { result = true, orderId = OrderContext.Order.AsOrder().OrderNumber });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		/// <summary>
		/// Checks the list of URLs entered by the user to see if URL already exists in the system.
		/// </summary>
		/// <param name="urls">List of URLs entered by the user.</param>
		/// <param name="scheduleId">The schedule Id.</param>
		/// <returns>A Json response with a list of duplicate URLs.</returns>
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult CheckForDuplicateUrls(List<SiteUrl> urls)
		{
			AutoshipOrder autoshipOrder = CoreContext.CurrentAutoship;
			List<String> duplicateUrls = new List<string>();

			try
			{
				Site site = Site.LoadByAutoshipOrderID(autoshipOrder.AutoshipOrderID);

				if (site != null)
				{
					if (urls == null || urls.Count == 0)
						return JsonError(Translation.GetTerm("PleaseAddUrlForThisSubscription", "Please add a url for this subscription."));

					// Get the list of new URLs.
					var syncList = site.SiteUrls.GetSyncToLists(
						urls,
						new LambdaComparer<SiteUrl>((su1, su2) => CompareURIStrings(su1.Url, su2.Url),
							su => (su.SiteUrlID > 0) ? su.SiteUrlID : su.Url.GetHashCode()));

					foreach (var url in syncList.ItemsToAdd)
					{
						if (Site.LoadByUrl(url.Url) != null)
						{
							duplicateUrls.Add(url.Url);
						}
					}
				}
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}

			return Json(duplicateUrls);
		}

		/// <summary>
		/// Compares the URI strings to see if they are equal.
		/// </summary>
		/// <param name="existingUrl">Existing URL for the user.</param>
		/// <param name="newUrl">New URL from the list.</param>
		/// <returns>True if the URLs are equal. Otherwise returns false.</returns>
		protected bool CompareURIStrings(string existingUrl, string newUrl)
		{
			bool result = false;
			Uri uri1 = null;
			Uri uri2 = null;

			if (Uri.TryCreate(existingUrl, UriKind.Absolute, out uri1) && Uri.TryCreate(newUrl, UriKind.Absolute, out uri2))
			{
				result = Uri.Compare(uri1, uri2, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.Ordinal) == 0;
			}

			return result;
		}

		#endregion

		public virtual ActionResult View(int id)
		{
			var autoshipOrder = AutoshipOrder.Load(id);
			if (autoshipOrder.AutoshipScheduleID > 0)
				autoshipOrder.AutoshipSchedule = AutoshipSchedule.Load(autoshipOrder.AutoshipScheduleID);
			var account = Account.Load(autoshipOrder.AccountID);
			if (account == null)
			{
				return Redirect("~/Accounts");
			}
			//this.AccountNum = account.AccountNumber;

			return View(autoshipOrder);
		}

		[OutputCache(CacheProfile = "PagedGridData")]
		public virtual ActionResult Get(int id, DateTime? startDate, DateTime? endDate, int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection)
		{
			try
			{
				if (startDate.HasValue && startDate.Value.Year < 1900)
					startDate = null;
				if (endDate.HasValue && endDate.Value.Year < 1900)
					endDate = null;
				StringBuilder builder = new StringBuilder();

				PaginatedList<OrderSearchData> orders = Order.Search(new OrderSearchParameters()
				{
					CustomerAccountID = CoreContext.CurrentAccount.AccountID,
					AutoshipOrderID = id,
					PageIndex = page,
					PageSize = pageSize,
					OrderBy = orderBy,
					OrderByDirection = orderByDirection,
					StartDate = startDate,
					EndDate = endDate
				});
				if (orders.Count > 0)
				{
					int count = 0;
					foreach (OrderSearchData order in orders)
					{
						builder.Append("<tr>")
							.AppendLinkCell("~/Orders/Details/Index/" + order.OrderNumber, order.OrderNumber)
							.AppendCell(order.FirstName)
							.AppendCell(order.LastName)
							.AppendCell(order.OrderStatus)
							.AppendCell(order.OrderType)
							.AppendCell(!order.CompleteDate.HasValue ? "N/A" : order.CompleteDate.ToShortDateString())
							.AppendCell(!order.DateShipped.HasValue ? "N/A" : order.DateShipped.ToShortDateString())
							.AppendCell(order.Subtotal.ToString("C"), style: "text-align: right; padding-right: 10px;")
							.AppendCell(order.GrandTotal.ToString("C"), style: "text-align: right; padding-right: 10px;")
							.AppendCell(!order.CommissionDate.HasValue ? "N/A" : order.CommissionDate.ToShortDateString())
							.AppendLinkCell("~/Accounts/Overview/Index/" + order.SponsorAccountNumber, order.Sponsor)
							.Append("</tr>");
						++count;
					}
					return Json(new { result = true, totalPages = orders.TotalPages, page = builder.ToString() });
				}
				else
				{
					return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
				}
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Orders", "~/Accounts")]
		public virtual ActionResult AuditHistory(int id)
		{
			AutoshipOrder autoshipOrder = AutoshipOrder.Load(id);

			ViewData["EntityName"] = "AutoshipOrder";
			ViewData["ID"] = autoshipOrder.AutoshipOrderID;
			AutoshipSchedule schedule = null;
			StringBuilder links = new StringBuilder();
			if (autoshipOrder.AutoshipScheduleID > 0)
				schedule = AutoshipSchedule.Load(autoshipOrder.AutoshipScheduleID);

			string link;
			if (schedule != null && schedule.IsTemplateEditable)
			{
				link = string.Format("{0}/{1}", System.Web.VirtualPathUtility.ToAbsolute("~/Accounts/Autoships/Edit"), id);
				links.Append("<a id=\"orderDetails\" href=\"" + link + "\">" + Translation.GetTerm("EditTemplate", "Edit Template") + "</a> | ");
			}
			link = string.Format("{0}/{1}", System.Web.VirtualPathUtility.ToAbsolute("~/Accounts/Autoships/View"), id);
			links.Append("<a id=\"orderDetails\" href=\"" + link + "\">" + Translation.GetTerm("ViewOrders", "View Orders") + "</a> | ");
			links.Append(Translation.GetTerm("AuditHistory", "Audit History")).ToString();

			ViewData["Links"] = links;

			return View("AuditHistory", "Accounts", autoshipOrder);
		}

		/// <summary>
		/// Initiates creating a new autoship order using for the account
		/// matching the provided id using the autoship schedule matching the provided autoshipScheduleID.
		/// </summary>
		/// <param name="id">Specifies the ID for the autoship schedule to use for the new autoship order</param>
		/// <returns></returns>
		public virtual ActionResult New(int id, int autoshipScheduleID)
		{
			return EditOrNew(null, id, autoshipScheduleID);
		}

		/// <summary>
		/// Initiates editing an existing autoship order matching the 
		/// provided id.
		/// </summary>
		/// <param name="id">Specifies the ID for the autoship order to edit</param>
		/// <returns></returns>
		public virtual ActionResult EditAutoship(int id)
		{
			return EditOrNew(id, null, null);
		}

		/// <summary>
		/// Initiates editing an existing autoship order matching the 
		/// provided id.
		/// </summary>
		/// <param name="id">Specifies the ID for the autoship order to edit</param>
		/// <param name="autoshipScheduleId">Autoship Schedule Id</param>
		/// <returns></returns>
		[HttpGet]
		public virtual ActionResult Edit(int id, int? autoshipScheduleId)
		{
			return EditOrNew(autoshipOrderID: id, accountID: null, autoshipScheduleID: autoshipScheduleId);
		}
	}
}
