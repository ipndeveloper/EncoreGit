using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Orders.Helpers;
using DistributorBackOffice.Areas.Orders.Models;
using DistributorBackOffice.Areas.Orders.Models.Party;
using DistributorBackOffice.Areas.Orders.Models.Shared;
using DistributorBackOffice.Infrastructure;
using DistributorBackOffice.Models;
using NetSteps.Addresses.UI.Common.Services;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Dynamic;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Models;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Mail;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.Common.Services;
using NetSteps.GiftCards.Common;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Plugins.Common.Helpers;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components;
using NetSteps.Promotions.Service;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using WebGrease.Css.Extensions;
using Constants = NetSteps.Data.Entities.Constants;
using NetSteps.Commissions.Common;

namespace DistributorBackOffice.Areas.Orders.Controllers
{
	public abstract class AbstractPartyAndFundraiserController : OrdersBaseController
	{
		protected IProductCreditLedgerService _productCreditLedgerService = Create.New<IProductCreditLedgerService>();

		#region Properties

		protected virtual string _errorInvalidPromotionCode(string promotionCode) { return Translation.GetTerm("ErrorInvalidPromotionCode", "The promotion could not be applied. Invalid promotion code: '{0}'.", promotionCode); }

		protected virtual string _errorNoItemsInOrder { get { return Translation.GetTerm("PleaseAddItemsToOrderBeforeUpdating", "Please add items to Order before updating."); } }

		public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }
		public IShippingCalculator ShippingCalculator { get { return Create.New<IShippingCalculator>(); } }
		public ICatalogRepository CatalogRepository { get { return Create.New<ICatalogRepository>(); } }
		protected IEventScheduler EventScheduler { get { return Create.New<IEventScheduler>(); } }

		public virtual Party CurrentParty
		{
			get
			{
				Party retVal = null;
				if (OrderContext != null && OrderContext.Order != null)
				{
					Order curOrder = OrderContext.Order.AsOrder();
					if (curOrder.Parties != null)
					{
						retVal = curOrder.Parties.FirstOrDefault();
					}
				}
				return retVal;
			}
			set
			{
				if (value == null)
				{
					OrderContext.Order = null;
				}
				else
				{
					OrderContext.Order = value.Order;
				}
			}
		}

		protected List<string> _tempImages;
		public virtual List<string> TempImages
		{
			get
			{
				if (_tempImages == null)
					_tempImages = new List<string>();
				return _tempImages;
			}
		}

		protected virtual object Totals
		{
			get
			{
				Order order = OrderContext.Order.AsOrder();
				if (order == null)
				{
					return null;
				}

				decimal paymentTotal = order.OrderCustomers[0].OrderPayments.Where(p => p.OrderPaymentStatusID != Constants.OrderPaymentStatus.Cancelled.ToShort()).Sum(p => p.Amount);
				return new
				{
					subtotalAdjusted = order.OrderCustomers[0].AdjustedSubTotal.ToString(order.CurrencyID),
					subtotal = order.Subtotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
					commissionableTotal = order.CommissionableTotal.ToDecimal().ToString(order.CurrencyID),
					taxTotal = (order.TaxAmountTotalOverride != null) ? order.TaxAmountTotalOverride.ToString(order.CurrencyID) : order.TaxAmountTotal.ToString(order.CurrencyID),
					shippingTotal = (order.ShippingTotalOverride != null) ? order.ShippingTotalOverride.ToString(order.CurrencyID) : order.ShippingTotal.ToString(order.CurrencyID),
					handlingTotal = order.HandlingTotal.ToString(order.CurrencyID),
					grandTotal = order.OrderPendingState != Constants.OrderPendingStates.Open ? order.GrandTotal.ToString(order.CurrencyID) : order.Subtotal.ToString(order.CurrencyID),
					paymentTotal = paymentTotal.ToString(order.CurrencyID),
					balanceDue = order.Balance.ToString(order.CurrencyID),
					balanceAmount = order.Balance
				};
			}
		}

		protected virtual string ShippingMethods
		{
			get
			{
				OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipment();
				OrderCustomer customer = OrderContext.Order.AsOrder().OrderCustomers[0];
				var shipmentAdjustmentAmount = customer.ShippingAdjustmentAmount;

				if (shipment != null)
				{
					var builder = new StringBuilder();

					try
					{
						IEnumerable<ShippingMethodWithRate> shippingMethods = OrderContext.Order.AsOrder().GetShippingMethods(shipment);
						if (shippingMethods != null)
						{
							shippingMethods = shippingMethods.OrderBy(sm => sm.ShippingAmount).ToList();
						}

						if (shippingMethods != null)
						{
							if (!shippingMethods.Select(sm => sm.ShippingMethodID).Contains(shipment.ShippingMethodID.ToInt()) && shippingMethods.Any())
							{
								var cheapestShippingMethod = shippingMethods.First();
								shipment.ShippingMethodID = cheapestShippingMethod.ShippingMethodID;
								OrderService.UpdateOrder(OrderContext);
							}

							if (shippingMethods.Any())
							{
								foreach (var shippingMethod in shippingMethods)
								{
									builder.Append("<li class=\"AddressProfile\"><input value=\"").Append(shippingMethod.ShippingMethodID)
										.Append("\"")
										.Append(" id=\"shippingMethod").Append(shippingMethod.ShippingMethodID)
										.Append("\"")
										.Append(shipment.ShippingMethodID == shippingMethod.ShippingMethodID ? " checked=\"checked\"" : "")
										.Append(" type=\"radio\" name=\"shippingMethod\" class=\"Radio\" />")
										.Append("<label for=\"shippingMethod")
										.Append(shippingMethod.ShippingMethodID)
										.Append("\"><b class=\"mr10\">")
										.Append(shippingMethod.Name)
										.Append("</b>");

									if (shipmentAdjustmentAmount != 0)
									{
										builder.Append("<span class=\"shipMethodPrice originalPrice strikethrough\">")
											.Append(shippingMethod.ShippingAmount.ToString(OrderContext.Order.AsOrder().CurrencyID))
											.Append("</span>&nbsp;")
											.Append("<span class=\"shipMethodPrice discountPrice\">")
											.Append((shippingMethod.ShippingAmount - shipmentAdjustmentAmount).ToString(OrderContext.Order.AsOrder().CurrencyID))
											.Append("</span>");
									}
									else
									{
										builder.Append(shippingMethod.ShippingAmount.ToString(OrderContext.Order.AsOrder().CurrencyID)).Append("</label></li>");
									}
								}
							}
							else
							{
								builder.Append("<li class=\"AddressProfile\">").Append(Translation.GetTerm("NoShippingRatesAvailable", "No shipping rates available. Please enter a shipping address if one is not yet added to order.")).Append("</li>");
							}
						}

						return builder.ToString();
					}
					catch (Exception ex)
					{
						var productShippingExcludedShippingException = ex as ProductShippingExcludedShippingException;
						if (productShippingExcludedShippingException != null)
						{
							builder.Append("<li class=\"AddressProfile\"><div>").Append(Translation.GetTerm("InvalidShippingForProducts", "No available shipping methods, try changing your shipping address, updating your order subtotal or call customer service.")).Append("</div><ul>");
							foreach (var product in (productShippingExcludedShippingException).ProductsThatHaveExcludedShipping)
							{
								builder.Append(string.Format("<li>{0}</li>", product.Name));
							}
							builder.Append("</ul></li>");

							return builder.ToString();
						}

						throw new Exception(ex.Message, ex);
					}
				}

				return null;
			}
		}

		/// <summary>
		/// Gets or sets the cart item partial name.
		/// </summary>
		protected string CartItemPartialName { get; set; }

		/// <summary>
		/// Gets or sets the payments grid partial name.
		/// </summary>
		protected string PaymentsGridPartialName { get; set; }

		/// <summary>
		/// Gets or sets the party submitted action name.
		/// </summary>
		protected string PartySubmittedActionName { get; set; }

		/// <summary>
		/// Gets or sets the receipt view name.
		/// </summary>
		protected string ReceiptViewName { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AbstractPartyAndFundraiserController"/> class.
		/// </summary>
		protected AbstractPartyAndFundraiserController()
		{
			PartySubmittedActionName = "Receipt";
			ReceiptViewName = "Receipt";
			CartItemPartialName = "PartialCartItems";
			PaymentsGridPartialName = "PaymentsGrid";
		}

		#endregion

		#region Accounts
		[PartySetup]
		[OutputCache(CacheProfile = "AutoCompleteData")]
		public virtual ActionResult SearchAccounts(string query)
		{
			try
			{
				var prospects = AccountCache.GetAccountSearchByTextAndSponsorIdResults(query, (int)ConstantsGenerated.AccountType.Prospect, CurrentAccount.AccountID);
				var accounts = prospects.Select(p => new
				{
					id = p.Key,
					text = p.Value
				});

				var retailCustomers = AccountCache.GetAccountSearchByTextAndSponsorIdResults(query, (int)ConstantsGenerated.AccountType.RetailCustomer, CurrentAccount.AccountID);
				accounts = accounts.Union(retailCustomers.Select(p => new
				{
					id = p.Key,
					text = p.Value
				}));

				if (ApplicationContext.Instance.UsesEncoreCommissions)
				{
					var matchingItems = DownlineCache.SearchDownline(CurrentAccount.AccountID, query);
					if (matchingItems.Any())
					{
						accounts = accounts.Union((matchingItems as IEnumerable<dynamic>).Select(a => new
						{
							id = (int)a.AccountID,
							text = (string)Server.HtmlEncode(string.Format("{0} (#{1})", NetSteps.Data.Entities.Account.ToFullName(a.FirstName, string.Empty, a.LastName, CoreContext.CurrentCultureInfo.Name), a.AccountNumber))
						}));
					}
				}

				string currentAccountDisplayName = string.Format("{0} {1} (#{2})", CurrentAccount.FirstName, CurrentAccount.LastName, CurrentAccount.AccountNumber);
				if (currentAccountDisplayName.ContainsIgnoreCase(query.ToCleanString()))
				{
					var temp = new Dictionary<int, string>();
					temp.Add(CurrentAccount.AccountID, currentAccountDisplayName);
					accounts = accounts.Union(temp.Select(p => new
					{
						id = p.Key,
						text = p.Value
					}));
				}

				return Json(accounts);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[PartySetup]
		public virtual ActionResult GetAccount(int accountId)
		{
			try
			{
				var account = NetSteps.Data.Entities.Account.LoadFull(accountId);
				var address = GetDefaultAddress(account);

				return Json(new
				{
					result = true,
					firstName = account.FirstName,
					lastName = account.LastName,
					phone = account.MainPhone.PhoneFormat(NetSteps.Common.Extensions.StringExtensions.PhoneFormats.OnlyNumbers),
					email = account.EmailAddress,
					address1 = address.Address1,
					address2 = address.Address2,
					address3 = address.Address3,
					postalCode = address.PostalCode,
					city = address.City.ToPascalCase(),
					county = address.County.ToPascalCase(),
					state = address.StateProvinceID.HasValue && address.StateProvinceID.Value > 0 ? address.StateProvinceID.ToString() : address.State,
					country = address.CountryID
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		#endregion

		#region Party

		#region Redemptions

		public virtual ActionResult RedeemHostCredit(int productId, int quantity, int hostRewardRuleId, Dictionary<string, string> productProperties = null)
		{
			try
			{
				var hostess = OrderContext.Order.AsOrder().GetHostess();
				var product = Inventory.GetProduct(productId);
				var orderItem = (OrderItem)OrderContext.Order.AsOrder().AddItem(hostess, product, quantity, Constants.OrderItemType.HostCredit, -1, -1, false, hostRewardRuleId: hostRewardRuleId);
				var isDynamicKit = product.IsDynamicKit();
				if (productProperties != null && productProperties.Any())
				{
					foreach (var kvp in productProperties)
					{
						orderItem.AddOrderItemProperty(kvp.Key, kvp.Value);
					}
				}
				OrderService.UpdateOrder(OrderContext);

				var additionalParameters = new Dictionary<string, object>
				{
					{"isBundle", isDynamicKit},
					{"bundleGuid", isDynamicKit ? orderItem.Guid.ToString("N") : string.Empty}
				};

				return FormJsonResponse((OrderCustomer)hostess, additionalParameters);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult RedeemItemDiscount(int productId, int quantity, int hostRewardRuleId, Dictionary<string, string> productProperties = null)
		{
			try
			{
				var hostess = OrderContext.Order.AsOrder().GetHostess();
				var product = Inventory.GetProduct(productId);
				var orderItem = (OrderItem)OrderContext.Order.AsOrder().AddItem(hostess, product, quantity, Constants.OrderItemType.ItemDiscount, -1, -1, false, hostRewardRuleId: hostRewardRuleId);
				var isDynamicKit = product.IsDynamicKit();
				if (productProperties != null && productProperties.Any())
				{
					foreach (var kvp in productProperties)
					{
						orderItem.AddOrderItemProperty(kvp.Key, kvp.Value);
					}
				}
				OrderService.UpdateOrder(OrderContext);

				var additionalParameters = new Dictionary<string, object>
				{
					{"bundleGuid", isDynamicKit ? orderItem.Guid.ToString("N") : string.Empty},
					{"isBundle", isDynamicKit}
				};

				return FormJsonResponse(hostess, additionalParameters);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult RedeemExclusiveItem(int productId, int quantity, int hostRewardRuleId, Dictionary<string, string> productProperties = null)
		{
			try
			{
				bool reloadPage = false;

				if (!OrderContext.Order.AsOrder().HasConsultantOnOrder())
				{
					AddMyselfToParty();
					reloadPage = true;
				}

				var hostessRewardRule = SmallCollectionCache.Instance.HostessRewardRules.GetById(hostRewardRuleId);
				var consultant = CurrentParty.GetOrderCustomerForHostessRewardRule(hostessRewardRule);
				var hostess = OrderContext.Order.AsOrder().GetHostess();

				BasicResponse isValidated = consultant.ValidateHostessRewardItem(quantity, hostRewardRuleId, OrderContext.Order.AsOrder());
				if (isValidated.Success)
				{
					var product = Inventory.GetProduct(productId);
					var orderItem = OrderContext.Order.AsOrder().AddItem(consultant, product, quantity, Constants.OrderItemType.ExclusiveProduct, -1, -1, false, hostRewardRuleId: hostRewardRuleId);

					var isDynamicKit = product.IsDynamicKit();
					if (productProperties != null && productProperties.Any())
					{
						foreach (var kvp in productProperties)
						{
							((OrderItem)orderItem).AddOrderItemProperty(kvp.Key, kvp.Value);
						}
					}

					OrderService.UpdateOrder(OrderContext);

					var additionalParameters = new Dictionary<string, object>
					{
						{"remainingExclusiveProducts", RemainingExclusiveProducts},
						{"reloadPage", reloadPage},
						{"isBundle", isDynamicKit},
						{"bundleGuid", isDynamicKit ? ((OrderItem)orderItem).Guid.ToString("N") : string.Empty}
					};

					return FormJsonResponse(hostess, additionalParameters);
				}

				return Json(new { result = false, message = isValidated.Message });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult RedeemPercentOff(int productId, int quantity, int hostRewardRuleId, Dictionary<string, string> productProperties = null)
		{
			try
			{
				var hostess = OrderContext.Order.AsOrder().GetHostess();
				BasicResponse isValidated = hostess.ValidateHostessRewardItem(quantity, hostRewardRuleId, OrderContext.Order.AsOrder());

				if (isValidated.Success)
				{
					var product = Inventory.GetProduct(productId);
					var orderItem = (OrderItem)OrderContext.Order.AsOrder().AddItem(hostess, product, quantity, Constants.OrderItemType.PercentOff, -1, -1, false, hostRewardRuleId: hostRewardRuleId);
					var isDynamicKit = product.IsDynamicKit();
					if (productProperties != null && productProperties.Any())
					{
						foreach (var kvp in productProperties)
						{
							orderItem.AddOrderItemProperty(kvp.Key, kvp.Value);
						}
					}

					OrderService.UpdateOrder(OrderContext);

					var additionalParameters = new Dictionary<string, object>
					{
						{"remainingProductDiscounts", RemainingProductDiscounts},
						{"remainingBookingCredits", RemainingBookingCredits.Values.Sum()},
						{"remainingExclusiveProducts", RemainingExclusiveProducts},
						{"isBundle", isDynamicKit},
						{"bundleGuid", isDynamicKit ? orderItem.Guid.ToString("N") : string.Empty}
					};

					return FormJsonResponse(hostess, additionalParameters);
				}

				return Json(new { result = false, message = isValidated.Message });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult RedeemBookingCredit(int productId, int quantity, int hostRewardRuleId, Dictionary<string, string> productProperties = null)
		{
			try
			{
				var hostessRewardRule = SmallCollectionCache.Instance.HostessRewardRules.GetById(hostRewardRuleId);
				var hostess = CurrentParty.GetOrderCustomerForHostessRewardRule(hostessRewardRule);

				BasicResponse isValidated = hostess.ValidateHostessRewardItem(quantity, hostRewardRuleId, CurrentParty.Order);
				if (isValidated.Success)
				{
					var product = Inventory.GetProduct(productId);
					var orderItem = (OrderItem)OrderContext.Order.AsOrder().AddItem(hostess, product, quantity, Constants.OrderItemType.BookingCredit, -1, -1, false, hostRewardRuleId: hostRewardRuleId);
					var isDynamicKit = product.IsDynamicKit();
					if (productProperties != null && productProperties.Any())
					{
						foreach (var kvp in productProperties)
						{
							orderItem.AddOrderItemProperty(kvp.Key, kvp.Value);
						}
					}

					OrderService.UpdateOrder(OrderContext);

					var additionalParameters = new Dictionary<string, object>
					{
						{"remainingProductDiscounts", RemainingProductDiscounts},
						{"remainingBookingCredits", RemainingBookingCredits.Values.Sum()},
						{"remainingExclusiveProducts", RemainingExclusiveProducts},
						{"isBundle", isDynamicKit},
						{"bundleGuid", isDynamicKit ? orderItem.Guid.ToString("N") : string.Empty}
					};

					return FormJsonResponse(hostess, additionalParameters);
				}

				return Json(new { result = false, message = isValidated.Message });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		protected JsonResult FormJsonResponse(OrderCustomer customer)
		{
			return FormJsonResponse(customer, new Dictionary<string, object>());
		}

		protected JsonResult FormJsonResponse(OrderCustomer customer, Dictionary<string, object> additionalValues)
		{
			ViewBag.OrderItemMessagesDictionary = GetOrderItemMessagesDictionary(OrderContext.Order.AsOrder());

			var data = new Dictionary<string, object>
			{
				{ "result", true },
				{ "itemsInCart", customer.OrderItems.Count },
				{ "totals", GetTotals(customer.Guid.ToString("N")) },
				{ "orderItems", GetOrderItemsHtml(OrderContext.Order.AsOrder(), customer) },
				{ "orderCustomerId", customer.Guid.ToString("N") },
				{ "BundleOptionsSpanHTML",  GetDynamicBundleUpSale(customer) },
				{ "hasReachedMinimumSubtotal", CurrentParty.HasReachedMinimumPartySubtotal },
				{ "promotions", GetApplicablePromotions(OrderContext, customer) }
			};
			foreach (var key in additionalValues.Keys)
			{
				data[key] = additionalValues[key];
			}

			return Json(data);
		}

		#endregion

		[PartySetup]
		[FunctionFilter("Orders-Party Order", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult Index(int? partyId, string orderCustomerGuid = null)
		{
			bool createParty = false;

			if (partyId.HasValue && partyId.Value > 0)
			{
				if (CurrentParty == null || CurrentParty.PartyID != partyId.Value)
				{
					CurrentParty = Party.LoadFull(partyId.Value);
					if (!CurrentAccountCanAccessParty(CurrentParty))
					{
						return RedirectToSafePage();
					}
					OrderContext.Order = CurrentParty.Order;
					OrderService.UpdateOrder(OrderContext);
					SetHasChangesToFalseOnAllItems();
				}
			}
			else if (Request.UrlReferrer == null || !Request.UrlReferrer.LocalPath.Contains("DetermineStep"))
			{
				CurrentParty = null;
				OrderContext.Clear();
			}

			var hostInviteEmailTemplate = EmailTemplate.Search(new EmailTemplateSearchParameters
			{
				Active = true,
				PageIndex = 0,
				PageSize = 1,
				EmailTemplateTypeIDs = new List<short> { (short)Constants.EmailTemplateType.EvitesHostessInvite }
			}).FirstOrDefault();
			var hostInviteEmailTranslation = hostInviteEmailTemplate == null ? null : hostInviteEmailTemplate.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);
			ViewBag.HasHostInviteContent = hostInviteEmailTranslation != null && hostInviteEmailTranslation.Body.Contains("{{DistributorContent}}");

			OrderCustomer host = null;
			NetSteps.Data.Entities.Account hostAccount = null;
			Address hostAddress = null;
			var isPartyAtHosts = true;
			PartyShipTo? shipTo = null;

			Address consultantShippingAddress = null;
			NetSteps.Addresses.Common.Models.IAddress shippingAddress = null;
			if (CurrentParty != null)
			{
				if (orderCustomerGuid != null)
				{
					createParty = true;
					var customer = CurrentParty.Order.OrderCustomers.FirstOrDefault(oc => oc.Guid.ToString("N") == orderCustomerGuid);
					hostAccount = NetSteps.Data.Entities.Account.LoadFull(customer.AccountID);
					host = new OrderCustomer(hostAccount);
					hostAddress = GetDefaultAddress(hostAccount);
					shippingAddress = hostAccount.Addresses.FirstOrDefault(a => a.AddressTypeID == Constants.AddressType.Shipping.ToShort());
					shipTo = PartyShipTo.Host;
				}
				else
				{
					shippingAddress = CurrentParty.Order.GetDefaultShipment();

					host = CurrentParty.Order.GetHostess();
					if (host != null)
					{
						hostAccount = NetSteps.Data.Entities.Account.LoadFull(host.AccountID);
						hostAddress = GetDefaultAddress(hostAccount);
					}
					isPartyAtHosts = CurrentParty.Address.IsEqualTo(hostAddress, false);
					if (shippingAddress.IsEqualTo(hostAddress, false))
					{
						shipTo = PartyShipTo.Host;
					}

					if (shippingAddress != null)
					{
						//check if it's one of the consultant's addresses
						foreach (var address in CurrentAccount.Addresses.Where(a => a.AddressTypeID == Constants.AddressType.Shipping.ToInt()))
						{
							if (shippingAddress.IsEqualTo(address, false))
							{
								if (!shipTo.HasValue)
								{
									shipTo = PartyShipTo.Consultant;
								}
								consultantShippingAddress = address;
								break;
							}
						}
					}

					if (!shipTo.HasValue)
					{
						//if still not set, it's other
						shipTo = PartyShipTo.Other;
					}
				}
			}
			else
			{
				createParty = true;
			}

			ViewBag.Host = host;
			ViewBag.HostAccount = hostAccount;
			ViewBag.HostAddress = hostAddress;
			ViewBag.IsPartyAtHosts = isPartyAtHosts;
			ViewBag.ShipTo = shipTo;
			ViewBag.ConsultantShippingAddress = consultantShippingAddress;
			ViewBag.ShippingAddress = shippingAddress;

			var ownerAccount = CurrentAccount;
			var ownerAddress = ownerAccount.GetBestAccountAddress();

			ViewBag.Country = ownerAddress == null ? null : ownerAddress.GetCountryFromCache();

			if (createParty)
			{
				if (orderCustomerGuid == null)
				{
					return View(new Party() { StartDate = DateTime.Now });
				}

				return View(new Party() { StartDate = DateTime.Now, ParentPartyID = CurrentParty.PartyID });
			}

			return View(CurrentParty);
		}

		[NonAction]
		public virtual string GetDynamicBundleUpSale(OrderCustomer customer)
		{
			if (CurrentParty == null || customer.IsTooBigForBundling())
			{
				return string.Empty;
			}

			var possibleDynamicKitProducts = CurrentParty.Order.AsOrder().GetPotentialDynamicKitUpSaleProducts(customer, OrderContext.SortedDynamicKitProducts).ToList();
			var sb = new StringBuilder();
			var sbi = new StringBuilder();

			for (int i = 0; i < possibleDynamicKitProducts.Count(); i++)
			{
				Guid guid = Guid.NewGuid();
				var newID = guid.ToString("N");

				if (possibleDynamicKitProducts.Count() > 1 && i == 0)
				{
					sb.Append("<a href=\"#\" id=\"BundleTrigger" + newID + "\" class=\"jqModal bold\">");
					sb.Append(possibleDynamicKitProducts.Count() + " bundles available");
					sb.Append("</a>");

					sbi.Append("<div id=\"BundleModal" + newID + "\" class=\"jqmWindow LModal bundleModal\" style=\"display:none;\">" +
												 "<div class=\"mContent\">" +
												 "<h2>" + Translation.GetTerm("BundlesAvailableModal_Heading", "Bundles Available") + "</h2>" +
												 "<p class=\"mb10 bundleAvailableTerm\">" + Translation.GetTerm("BundlesAvailableModal_Text", "Click on an available bundle below to save on your order!") + "</p>" +
												 "<ul class=\"flatList bundleList\">");
					for (int j = 0; j < possibleDynamicKitProducts.Count(); j++)
					{
						var product = possibleDynamicKitProducts[j];
						sbi.Append("<li class=\"UI-lightBg brdrAll bundleOption\">" +
													"<input type=\"hidden\" class=\"dynamicKitProductSuggestion\" value=\"" + product.ProductID + "\" />" +
													"<a href=\"javascript:void(0);\" class=\"block pad5 brdrAll CreateBundle\">" + product.Translations.Name() + "</a></li>");
					}
					sbi.Append("</ul>" +
									"<p class=\"mt10\"><a href=\"javascript:void(0);\" class=\"jqmClose FL cancel close\">Close</a></p>" +
									"<span class=\"clr\"></span>" +
									"</div></div>");
					sbi.Append("<script type=\"text/javascript\">$(function() { $('#BundleModal" + newID + "').jqm({ trigger: '#BundleTrigger" + newID + "'," +
													"onShow: function (h) {h.w.fadeIn(); } }) });</script>");
				}
			}

			return sb.ToString() + sbi.ToString();
		}

		[PartySetup]
		public virtual ActionResult SavePartySetup(PartySetupModel model)
		{
			try
			{
				var addressFromUIModel = Create.New<IAddressesService>().GetAddressEntityFromUIModel(model.Host.AddressNew);
				addressFromUIModel.SetState(addressFromUIModel.State, addressFromUIModel.CountryID);
				addressFromUIModel.AddressTypeID = (int)Constants.AddressType.Main;

				Party party = model.PartyID.HasValue ?
					CurrentParty != null && CurrentParty.PartyID == model.PartyID.Value
						? CurrentParty
						: Party.LoadFull(model.PartyID.Value)
					: new Party();

				if (!CurrentAccountCanAccessParty(party))
				{
					return RedirectToSafePage();
				}

				party.StartEntityTracking();
				party.StartDate = model.PartyDate.AddTime(model.PartyTime);
				party.Name = model.PartyName;
				party.ShowOnPWS = model.ListOnPWS;

				party.ParentPartyID = model.ParentPartyID;
				party.UseEvites = model.UseEvites;
				party.EviteOrganizerEmail = model.EvitesEmail;

				var maxPartyDate = party.GetMaximumFuturePartyDate();
				if (model.PartyDate > maxPartyDate)
				{
					return Json(new { result = false, message = string.Format("Party date must be sooner than {0}", maxPartyDate) });
				}

				var hostInviteEmailTemplate = EmailTemplate.Search(new EmailTemplateSearchParameters
				{
					Active = true,
					PageIndex = 0,
					PageSize = 1,
					EmailTemplateTypeIDs = new List<short> { (short)Constants.EmailTemplateType.EvitesHostessInvite }
				}).FirstOrDefault();

				if (hostInviteEmailTemplate != null)
				{
					if (!string.IsNullOrEmpty(model.PersonalizedContent))
					{
						var token = party.EmailTemplateTokens.Any(ett => ett.Token == "DistributorContent" && ett.AccountID == CurrentAccount.AccountID) ? party.EmailTemplateTokens.First(ett => ett.Token == "DistributorContent" && ett.AccountID == CurrentAccount.AccountID) : new EmailTemplateToken();
						token.AccountID = CurrentAccount.AccountID;
						token.Token = "DistributorContent";
						token.Value = model.PersonalizedContent;
						party.EmailTemplateTokens.Add(token);
					}
				}

				if (model.PartyIsAtHosts)
				{
					if (party.Address == null)
					{
						party.Address = new Address();
					}
					Address.CopyPropertiesTo(addressFromUIModel, party.Address);
				}
				else
				{
					model.PartyAddress.SetState(model.PartyAddress.State, model.PartyAddress.CountryID);
					if (party.Address == null)
					{
						party.Address = model.PartyAddress;
					}
					else
					{
						Address.CopyPropertiesTo(model.PartyAddress, party.Address);
					}
				}
				party.Address.AddressTypeID = (int)Constants.AddressType.Party;
				party.Address.Validate();
				if (!party.Address.IsValid)
				{
					return Json(new
					{
						result = false,
						message = party.Address.GetValidationErrorMessage()
					});
				}
				var result = party.Address.ValidateAddressAccuracy();
				if (!result.Success)
				{
					return Json(new { result = false, message = result.Message });
				}

				Order order = party.Order ?? new Order { DateCreated = DateTime.Now };
				order.StartEntityTracking();
				order.ConsultantID = CurrentAccount.AccountID;
				order.OrderTypeID = Constants.OrderType.PartyOrder.ToShort();
				order.OrderStatusID = Constants.OrderStatus.Pending.ToShort();
				order.OrderPendingState = Constants.OrderPendingStates.Open;
				order.CurrencyID = SmallCollectionCache.Instance.Countries.GetById(party.Address.CountryID).CurrencyID;
				OrderContext.Order = order;

				NetSteps.Data.Entities.Account hostAccount;
				if (model.Host.AccountID.HasValue && NetSteps.Data.Entities.Account.NonProspectExists(model.Host.Email))
				{
					hostAccount = NetSteps.Data.Entities.Account.LoadFull(model.Host.AccountID.Value);

					OrderCustomer host = order.OrderCustomers.FirstOrDefault(oc => oc.AccountID == (model.Host.AccountID.Value));
					if (host == null)
					{
						host = order.AddNewCustomer(hostAccount);
					}

					//TODO: Conditionally update the account to reflect the new values from the form - DES
					order.SetHostess(host);
				}
				else
				{
					//Upgrade the prospect for this sponsor if one exists - DES
					var attemptedHostAccount = NetSteps.Data.Entities.Account.GetAccountByEmailAndSponsorID(model.Host.Email, CurrentAccount.AccountID);
					if (attemptedHostAccount == null)
					{
						hostAccount = new NetSteps.Data.Entities.Account
						{
							DateCreated = DateTime.Now,
							DefaultLanguageID = CurrentAccount.DefaultLanguageID,
							FirstName = model.Host.FirstName,
							LastName = model.Host.LastName,
							EmailAddress = model.Host.Email,
							SponsorID = CurrentAccount.AccountID,
							MarketID = CurrentSite.MarketID,
							AccountTypeID = (int)Constants.AccountType.RetailCustomer
						};
					}
					else
					{
						hostAccount = attemptedHostAccount;
					}

					hostAccount.StartTracking();
					if (hostAccount.AccountTypeID == (short)Constants.AccountType.Prospect)
					{
						hostAccount.AccountTypeID = (int)Constants.AccountType.RetailCustomer;
					}
					hostAccount.MainPhone = model.Host.PhoneNumber;
					//set the account status to active and set the enrollment date to now - Scott Wilson
					var countryForMarket = SmallCollectionCache.Instance.Countries.GetById(party.Address.CountryID);
					hostAccount.MarketID = countryForMarket != null ? countryForMarket.MarketID : CurrentSite.MarketID;
					hostAccount.Activate();

					/************** Save the Account ****************/
					hostAccount.Save();

					if (order.OrderCustomers.All(x => x.AccountID != hostAccount.AccountID))
					{
						var hostOrderCustomer = order.AddNewCustomer(hostAccount);
						order.SetHostess(hostOrderCustomer);
					}
				}

				if (hostAccount.User == null)
				{
					hostAccount.User = new User
					{
						Username = NetSteps.Data.Entities.Account.GenerateUsername(hostAccount),
						Password = NetSteps.Data.Entities.Account.GenerateRandomPassword(),
						UserTypeID = (int)Constants.UserType.Distributor,
						UserStatusID = (int)Constants.UserStatus.Active,
						DefaultLanguageID = ApplicationContext.Instance.CurrentLanguageID
					};
				}

				var hostAddress = hostAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Hostess);
				if (hostAddress == null)
				{
					hostAddress = new Address();
					hostAccount.Addresses.Add(hostAddress);
				}
				hostAddress.StartEntityTracking();
				hostAddress.AttachAddressChangedCheck();
				Address.CopyPropertiesTo(addressFromUIModel, hostAddress, false);
				hostAddress.AddressTypeID = (int)Constants.AddressType.Hostess;
				hostAddress.LookUpAndSetGeoCode();
				hostAddress.Validate();
				if (!hostAddress.IsValid)
				{
					return Json(new
					{
						result = false,
						message = hostAddress.GetValidationErrorMessage()
					});
				}
				result = hostAddress.ValidateAddressAccuracy();
				if (!result.Success)
				{
					return Json(new { result = false, message = result.Message });
				}

				//The host address doesn't get added to the party object graph, so we need to add this separately.

				hostAccount.Orders.Clear();

				switch (model.ShipTo)
				{
					case PartyShipTo.Host:
						order.UpdateOrderShipmentAddressAndDefaultShipping(addressFromUIModel);
						break;
					case PartyShipTo.Consultant:
						order.UpdateOrderShipmentAddressAndDefaultShipping(CurrentAccount.Addresses.FirstOrDefault(a => a.AddressID == model.ShipToAddressID.ToInt()));
						var shipment = order.GetDefaultShipment();
						shipment.FirstName = CurrentAccount.FirstName;
						shipment.LastName = CurrentAccount.LastName;
						break;
					case PartyShipTo.Other:
						model.ShippingAddress.SetState(model.ShippingAddress.State, model.ShippingAddress.CountryID);
						result = model.ShippingAddress.ValidateAddressAccuracy();
						if (!result.Success)
							return Json(new { result = false, message = result.Message });

						order.UpdateOrderShipmentAddressAndDefaultShipping(model.ShippingAddress);
						break;
				}

				party.Order = order;
				party.Save();
				hostAccount.Save();

				if (!model.IsBooking)
				{
					CurrentParty = Party.LoadFull(party.PartyID);
				}
				else
				{
					var parentParty = Party.LoadFull(model.ParentPartyID.Value);

					if (!CurrentAccountCanAccessParty(parentParty)) return this.RedirectToSafePage();

					// Mark the OrderCustomer as IsBookingCredit - JHE
					var customer = parentParty.Order.OrderCustomers.FirstOrDefault(oc => oc.AccountID == hostAccount.AccountID);
					if (customer != null)
					{
						customer.IsBookingCredit = true;
						parentParty.Save();
					}

					CurrentParty = parentParty;
				}

				if (model.UseEvites)
					SendHostInvitation(party, party.EviteOrganizerEmail);

				try
				{
					if (TempImages != null)
					{
						foreach (var tempImage in TempImages)
						{
							System.IO.File.Delete(tempImage);
						}
					}
				}
				catch (Exception)
				{
					//Oh well, we couldn't delete the temp images, we'll clean them up another way - DES
				}

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[PartySetup]
		[FunctionFilter("Orders-Party Order", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult CancelParty(int partyId)
		{
			Party party = Party.Load(partyId);

			int? orderId = null;
			try
			{
				Order order = Order.Load(party.OrderID);

				if (order.IsCancellable())
				{
					orderId = order.OrderID;
					order.OrderStatusID = (int)Constants.OrderStatus.Cancelled;
					order.Save();

					return Json(new { result = true });
				}

				return Json(new { result = false, message = Translation.GetTerm("ThisOrderIsNotCancellable", "This order is not cancellable.") });

			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId.ToIntNullable());
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		protected virtual string GetPartyOrderUrl(int partyId)
		{
			return String.Empty;
		}

		protected void RefreshPartyFrom(Party originalParty)
		{
			OrderService.UpdateOrder(OrderContext);
			CurrentParty.Save();
		}

		#endregion

		#region Cart

		protected virtual object GetApplicablePromotions(IOrderContext orderContext, OrderCustomer orderCustomer)
		{
			var inventoryService = Create.New<IInventoryService>();
			var customerAdjustments = orderContext.Order.AsOrder().OrderAdjustments.Where(adjustment =>
				adjustment.OrderLineModifications.Any(orderlineMod => ((OrderAdjustmentOrderLineModification)orderlineMod).OrderItem.OrderCustomer == orderCustomer) ||
				adjustment.OrderModifications.Any(orderMod => ((OrderAdjustmentOrderModification)orderMod).OrderCustomer == orderCustomer) ||
				adjustment.InjectedOrderSteps.Any(s => s.CustomerAccountID == orderCustomer.AccountID));
			var promotionAdjustments = customerAdjustments.Where(adjustment => adjustment.ExtensionProviderKey == PromotionProvider.ProviderKey);
			var adjustments = promotionAdjustments.Where(adjustment => adjustment.OrderAdjustmentOrderLineModifications.Any() || adjustment.OrderAdjustmentOrderModifications.Any() || adjustment.InjectedOrderSteps.Any());
			return adjustments.Select(adj =>
			{
				var giftStep = adj.InjectedOrderSteps.FirstOrDefault(os => os is IUserProductSelectionOrderStep && os.CustomerAccountID == orderCustomer.AccountID &&
					(os.Response == null || (os.Response is IUserProductSelectionOrderStepResponse && (os.Response as IUserProductSelectionOrderStepResponse).SelectedOptions.Count == 0)));
				var promotionOutOfStock = false;
				if (giftStep == null)
				{
					promotionOutOfStock = adj.OrderAdjustmentOrderModifications.Any(mod => mod.ModificationOperationID == (int)OrderAdjustmentOrderOperationKind.Message);
				}
				else
				{
					var productSelectionStep = (IUserProductSelectionOrderStep)giftStep;
					promotionOutOfStock = !(productSelectionStep.AvailableOptions.Any(option => inventoryService.GetProductAvailabilityForOrder(orderContext, option.ProductID, option.Quantity).CanAddNormally == option.Quantity));
				}
				return new { Description = adj.Description, StepID = giftStep == null ? null : giftStep.OrderStepReferenceID.ToString(), PromotionOutOfStock = promotionOutOfStock };
			});
		}

		public virtual ActionResult ApplyPromotionCode(string promotionCode, string orderCustomerId)
		{
			ICouponCode code = Create.New<ICouponCode>();
			try
			{
				if (string.IsNullOrWhiteSpace(promotionCode))
				{
					return JsonError(_errorInvalidPromotionCode(promotionCode));
				}

				var orderCustomerGuid = new Guid(orderCustomerId);
				var customer = ((Order)OrderContext.Order).OrderCustomers.Single(oc => oc.Guid == orderCustomerGuid);

				if (!OrderService.GetActivePromotionCodes(customer.AccountID).Contains(promotionCode))
				{
					return JsonError(Translation.GetTerm("Promotions_PromotionCodeNotFound", "Promotion Code Not Found or Already Used"));
				}

				if (OrderContext.CouponCodes.Any(c => c.AccountID == customer.AccountID && promotionCode.EqualsIgnoreCase(c.CouponCode)))
				{
					return JsonError(Translation.GetTerm("Promotions_PromotionCodeAlreadyApplied", "Promotion Code Already Applied"));
				}

				code.AccountID = customer.AccountID;
				code.CouponCode = promotionCode;
				OrderContext.CouponCodes.Add(code);
				OrderService.UpdateOrder(OrderContext);

				return FormJsonResponse(customer);
			}
			catch (Exception ex)
			{
				OrderContext.CouponCodes.Remove(code);
				var message = ex.Log(orderID: OrderContext.Order.OrderID, accountID: CurrentAccount.AccountID).PublicMessage;
				return JsonError(message);
			}
		}

		public virtual ActionResult GetGiftStepInfo(string stepId)
		{
			try
			{
				var allSteps = OrderContext.InjectedOrderSteps.Union(OrderContext.Order.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps));
				var step = (IUserProductSelectionOrderStep)allSteps.Single(os => os is IUserProductSelectionOrderStep && os.OrderStepReferenceID.ToString() == stepId);
				var inventoryService = Create.New<IInventoryService>();
				var options = step.AvailableOptions.Select(o =>
				{
					var product = Inventory.GetProduct(o.ProductID);
					int currencyID = OrderContext.Order.CurrencyID;
					return new GiftModel
					{
						Name = product.Name,
						Image = product.MainImage != null ? product.MainImage.FilePath.ReplaceFileUploadPathToken() : String.Empty,
						Description = product.GetShortDescriptionDisplay(),
						ProductID = product.ProductID,
						Value = product.GetPriceByPriceType((int)Constants.ProductPriceType.Retail, currencyID).ToString(currencyID),
						IsOutOfStock = inventoryService.GetProductAvailabilityForOrder(OrderContext, o.ProductID, o.Quantity).CanAddNormally != o.Quantity,
					};
				});

				var giftSelectionModel = new GiftSelectionModel(Url.Action("GetGiftStepInfo"), Url.Action("AddGifts"), callbackFunctionName: "onGiftAdded");
				giftSelectionModel.StepID = stepId;
				giftSelectionModel.MaxQuantity = step.MaximumOptionSelectionCount;

				var selections = step.Response == null ? Enumerable.Empty<IProductOption>() : ((IUserProductSelectionOrderStepResponse)step.Response).SelectedOptions;
				giftSelectionModel.SelectedOptions = selections.Select(p => options.First(o => o.ProductID == p.ProductID)).ToList();
				giftSelectionModel.AvailableOptions = options.ToList();

				return Json(new { result = true, GiftSelectionModel = giftSelectionModel });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult AddGifts(List<int> productIds, string stepId)
		{
			try
			{
				var response = Create.New<IUserProductSelectionOrderStepResponse>();
				if (productIds != null)
				{
					foreach (var productId in productIds)
					{
						var option = Create.New<IProductOption>();
						option.ProductID = productId;
						option.Quantity = 1;
						response.SelectedOptions.Add(option);
					}
				}
				var allSteps = OrderContext.InjectedOrderSteps.Union(OrderContext.Order.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps));
				var step = (IUserProductSelectionOrderStep)allSteps.Single(os => os is IUserProductSelectionOrderStep && os.OrderStepReferenceID.ToString() == stepId);
				step.Response = response;
				OrderService.UpdateOrder(OrderContext);
				var adjustment = OrderContext.Order.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps).Single(s => s.OrderStepReferenceID == stepId);
				var customer = (OrderCustomer)OrderContext.Order.OrderCustomers.Single(c => c.AccountID == adjustment.CustomerAccountID);

				return FormJsonResponse(customer);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		/// <summary>
		/// Returns the data to send to the client-side viewmodel during AJAX calls.
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		protected virtual IDictionary<string, object> GetOrderEntryModelData(IOrderContext orderContext)
		{
			Contract.Requires<ArgumentNullException>(orderContext != null);

			return LoadOrderEntryModelData(new DynamicDictionary(), orderContext).AsDictionary();
		}

		/// <summary>
		/// Loads the data bag for the client-side viewmodel.
		/// For consistency, this method should be used for
		/// both the initial page load and for AJAX calls.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="order"></param>
		protected virtual dynamic LoadOrderEntryModelData(dynamic data, IOrderContext orderContext)
		{
			Contract.Requires<ArgumentNullException>(orderContext != null);
			// Code contracts rewriter doesn't work with dynamics
			if (data == null)
			{
				throw new ArgumentNullException("options");
			}
			data.Subtotal = ((OrderCustomer)orderContext.Order.OrderCustomers[0]).Subtotal.ToString(orderContext.Order.CurrencyID);
			data.SubtotalAdjusted = orderContext.Order.OrderCustomers[0].AdjustedSubTotal.ToString(orderContext.Order.CurrencyID);
			data.OrderItemModels = GetOrderItemModels(orderContext.Order.AsOrder());
			data.ApplicablePromotions = GetApplicablePromotions(orderContext);
			data.FreeGiftModels = GetFreeGiftModels(orderContext.Order.AsOrder());

			return data;
		}

		protected virtual IEnumerable<object> GetFreeGiftModels(Order order)
		{
			OrderCustomer orderCustomer = OrderContext.Order.AsOrder().OrderCustomers[0];
			int addedItemOperationID = (int)OrderAdjustmentOrderLineOperationKind.AddedItem;
			var nonPromotionalItems = orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == addedItemOperationID));
			var promotionalItems = orderCustomer.ParentOrderItems.Except(nonPromotionalItems).ToList();
			var adjustments = promotionalItems.GroupBy(x => x.OrderAdjustmentOrderLineModifications.Single(y => y.ModificationOperationID == addedItemOperationID).OrderAdjustment);

			return adjustments.Select(grp => new
			{
				Description = grp.Key.Description,
				StepID = grp.Key.InjectedOrderSteps.Any() ? grp.Key.InjectedOrderSteps.First().OrderStepReferenceID : null,
				Selections = grp.Select(i => new { SKU = i.SKU, Name = i.ProductName, Quantity = i.Quantity })
			});
		}

		/// <summary>
		/// Returns order items, formatted for the client-side viewmodel.
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		protected virtual IList<IOrderItemModel> GetOrderItemModels(Order order)
		{
			Contract.Requires<ArgumentNullException>(order != null);
			Contract.Requires<ArgumentException>(order.OrderCustomers != null);
			Contract.Requires<ArgumentException>(order.OrderCustomers.Count > 0);

			return order.OrderCustomers[0].ParentOrderItems
				.Where(oi => !oi.OrderAdjustmentOrderLineModifications.Any(mod => mod.ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem))
				.Select(orderItem =>
				{
					var orderItemModel = Create.New<IOrderItemModel>();
					var orderItemProduct = Inventory.GetProduct(orderItem.ProductID.Value);
					var preAdjustmentUnitPrice = orderItem.GetPreAdjustmentPrice();
					var finalUnitPrice = orderItem.ItemPriceActual ?? orderItem.GetAdjustedPrice();

					orderItemModel.Guid = orderItem.Guid.ToString("N");
					orderItemModel.ProductID = orderItem.ProductID ?? 0;
					orderItemModel.SKU = orderItem.SKU;
					orderItemModel.ProductName = orderItemProduct.Translations.Name();

					orderItemModel.AdjustedUnitPrice = finalUnitPrice.ToString(order.CurrencyID);
					orderItemModel.OriginalUnitPrice = preAdjustmentUnitPrice.ToString(order.CurrencyID);

					orderItemModel.AdjustedTotal = (finalUnitPrice * orderItem.Quantity).ToString(order.CurrencyID);
					orderItemModel.OriginalTotal = (preAdjustmentUnitPrice * orderItem.Quantity).ToString(order.CurrencyID);

					orderItemModel.Quantity = orderItem.Quantity;

					orderItemModel.OriginalCommissionableTotal = (orderItem.GetPreAdjustmentPrice(orderItem.OrderCustomer.CommissionablePriceTypeID) * orderItem.Quantity).ToString(order.CurrencyID);
					orderItemModel.AdjustedCommissionableTotal = (orderItem.GetAdjustedPrice(orderItem.OrderCustomer.CommissionablePriceTypeID) * orderItem.Quantity).ToString(order.CurrencyID);

					// Hostess rewards show the discount amount next total.
					if (orderItem.IsHostReward)
					{
						// when hostess rewards are an order adjustment type we can refactor this.
						orderItemModel.AdjustedTotal = string.Format("{0} {1}",
							orderItemModel.AdjustedTotal,
							Translation.GetTerm("HostRewardDiscount", "(discounted {0})",
								(orderItem.Discount ?? (orderItem.DiscountPercent.HasValue ? (orderItem.ItemPrice * orderItem.Quantity) * orderItem.DiscountPercent.Value : 0)).ToString(order.CurrencyID)
							)
						);
					}

					orderItemModel.IsStaticKit = orderItemProduct.IsStaticKit();
					orderItemModel.IsDynamicKit = orderItemProduct.IsDynamicKit();
					if (orderItemModel.IsDynamicKit)
					{
						orderItemModel.IsDynamicKitFull = orderItem.ChildOrderItems.Sum(oi => oi.Quantity) >= orderItemProduct.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount);
					}
					orderItemModel.IsHostReward = orderItem.IsHostReward;
					orderItemModel.BundlePackItemsUrl = Url.Action("BundlePackItems", new { productId = orderItem.ProductID, bundleGuid = orderItem.Guid.ToString("N"), orderCustomerId = orderItem.OrderCustomer.Guid.ToString("N") });

					orderItemModel.KitItemsModel = Create.New<IKitItemsModel>();
					if (orderItemProduct.IsStaticKit() || orderItemProduct.IsDynamicKit())
					{
						orderItemModel.KitItemsModel.KitItemModels = orderItem.ChildOrderItems
							.Select(k =>
							{
								var kitItemModel = Create.New<IKitItemModel>();
								var kitItemProduct = Inventory.GetProduct(k.ProductID.Value);
								kitItemModel.ProductName = kitItemProduct.Translations.Name();
								kitItemModel.Quantity = k.Quantity;
								kitItemModel.SKU = kitItemProduct.SKU;
								return kitItemModel;
							}).ToList();
					}
					else
					{
						// Non-kits still need an empty list.
						orderItemModel.KitItemsModel.KitItemModels = new List<IKitItemModel>();
					}

					return orderItemModel;
				}).ToList();
		}

		protected virtual object GetApplicablePromotions(IOrderContext orderContext)
		{
			var promotionAdjustments = orderContext.Order.AsOrder().OrderAdjustments.Where(adjustment => adjustment.ExtensionProviderKey == PromotionProvider.ProviderKey);
			var adjustments = promotionAdjustments.Where(adjustment => adjustment.OrderAdjustmentOrderLineModifications.Any() || adjustment.OrderAdjustmentOrderModifications.Any() || adjustment.InjectedOrderSteps.Any());
			var inventoryService = Create.New<IInventoryService>();
			return adjustments.Select(adj =>
			{
				var giftStep = adj.InjectedOrderSteps.FirstOrDefault(os => os is IUserProductSelectionOrderStep &&
					(os.Response == null || (os.Response is IUserProductSelectionOrderStepResponse && (os.Response as IUserProductSelectionOrderStepResponse).SelectedOptions.Count == 0)));
				var promotionOutOfStock = false;
				if (giftStep == null)
				{
					promotionOutOfStock = adj.OrderAdjustmentOrderModifications.Any(mod => mod.ModificationOperationID == (int)OrderAdjustmentOrderOperationKind.Message);
				}
				else
				{
					var productSelectionStep = (IUserProductSelectionOrderStep)giftStep;
					promotionOutOfStock = !(productSelectionStep.AvailableOptions.Any(option => inventoryService.GetProductAvailabilityForOrder(orderContext, option.ProductID, option.Quantity).CanAddNormally == option.Quantity));
				}

				return new { Description = adj.Description, StepID = giftStep == null ? null : giftStep.OrderStepReferenceID.ToString(), PromotionOutOfStock = promotionOutOfStock };
			});
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult AddToCart(string orderCustomerId, int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = null, bool calculateTotals = true, Dictionary<string, string> itemProperties = null)
		{
			try
			{
				if (quantity == 0)
				{
					return Json(new
					{
						result = false,
						message = string.Format(Translation.GetTerm("TheItemCouldNotBeAddedInvalidQuantityOf", "The product could not be added. Invalid quantity of {0}."), "0")
					});
				}

				bool allowAdd = true;
				bool showOutOfStockMessage = false;
				string outOfStockMessage = string.Empty;
				var product = Inventory.GetProduct(productId);
				if (Product.CheckStock(productId, ((Order)OrderContext.Order).GetDefaultShipment()).IsOutOfStock)
				{
					switch (product.ProductBackOrderBehaviorID)
					{
						case (int)Constants.ProductBackOrderBehavior.AllowBackorder:
							break;
						case (int)Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer:
							allowAdd = true;
							showOutOfStockMessage = true;
							break;
						case (int)Constants.ProductBackOrderBehavior.ShowOutOfStockMessage:
							allowAdd = false;
							showOutOfStockMessage = true;
							outOfStockMessage = Translation.GetTerm("ProductOutOfStock", "Product out of stock.");
							break;
						case (int)Constants.ProductBackOrderBehavior.Hide:
							allowAdd = false;
							break;
					}
				}
				bool isDynamicKit = product.IsDynamicKit();
				bool isBundleGroupComplete = false;
				var orderCustomerGuid = new Guid(orderCustomerId);
				var customer = OrderContext.Order.AsOrder().OrderCustomers.Single(oc => oc.Guid == orderCustomerGuid);
				OrderItem bundleItem = null;
				Product bundleProduct = null;
				DynamicKitGroup group = null;

				Guid pGuid;
				if (dynamicKitGroupId.HasValue && !String.IsNullOrWhiteSpace(parentGuid) && Guid.TryParse(parentGuid, out pGuid))
				{
					bundleItem = customer.OrderItems.First(oc => oc.Guid == pGuid);
					bundleProduct = Inventory.GetProduct(bundleItem.ProductID.Value);
					group = bundleProduct.DynamicKits[0].DynamicKitGroups.First(g => g.DynamicKitGroupID == dynamicKitGroupId);
					int currentCount = bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId).Sum(oi => oi.Quantity);
					if ((currentCount + quantity) > group.MinimumProductCount)
					{
						return Json(new
						{
							result = false,
							message = string.Format(Translation.GetTerm("TheItemCouldNotBeAddedInvalidQuantityOf", "The product could not be added. Invalid quantity of {0}."), quantity)
						});
					}
				}

				IEnumerable<IOrderItem> addedItems = null;

				if (allowAdd)
				{
					var mod = Create.New<IOrderItemQuantityModification>();
					mod.ProductID = productId;
					mod.Quantity = quantity;
					mod.ModificationKind = OrderItemQuantityModificationKind.Add;
					mod.Customer = customer;

					if (bundleItem != null && bundleProduct != null && group != null)
					{
						OrderService.AddOrderItemsToOrderBundle(OrderContext, bundleItem, new IOrderItemQuantityModification[] { mod }, dynamicKitGroupId.Value);
						OrderService.UpdateOrder(OrderContext);
						int currentCount = bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId).Sum(oi => oi.Quantity);
						isBundleGroupComplete = currentCount == group.MinimumProductCount;
					}
					else
					{
						addedItems = AddOrUpdateOrderItems(new IOrderItemQuantityModification[] { mod });
					}

					if (addedItems != null && addedItems.Any() && itemProperties != null && itemProperties.Any())
					{
						var modProps = Create.New<IOrderItemPropertyModification>();
						modProps.Customer = customer;
						modProps.ExistingItem = addedItems.First();

						foreach (var kvp in itemProperties)
						{
							modProps.Properties.Add(kvp.Key, kvp.Value);
						}

						OrderService.UpdateOrderItemProperties(OrderContext, new IOrderItemPropertyModification[] { modProps });
						OrderService.UpdateOrder(OrderContext);
					}
				}

				var additionalParameters = new Dictionary<string, object>();
				additionalParameters.Add("allow", allowAdd);
				additionalParameters.Add("showOutOfStockMessage", showOutOfStockMessage);
				additionalParameters.Add("isBundle", isDynamicKit);
				additionalParameters.Add("bundleGuid", isDynamicKit ? customer.OrderItems.FirstOrDefault(i => i.ProductID == product.ProductID && !i.HasChildOrderItems).Guid.ToString("N") : string.Empty);
				additionalParameters.Add("productId", product.ProductID);
				additionalParameters.Add("groupItemsHtml", parentGuid.IsNullOrEmpty() ? string.Empty : GetGroupItemsHtml(parentGuid, orderCustomerId, dynamicKitGroupId.Value).ToString());
				additionalParameters.Add("isBundleGroupComplete", isBundleGroupComplete);
				additionalParameters.Add("childItemCount", parentGuid.IsNullOrEmpty() ? 0 : customer.OrderItems.GetByGuid(parentGuid).ChildOrderItems.Sum(c => c.Quantity));
				InsertCustomAdditionalParameters(additionalParameters);

				return FormJsonResponse(customer, additionalParameters);
			}
			catch (ProductShippingExcludedShippingException ex)
			{
				var excludedProducts = ex.ProductsThatHaveExcludedShipping.Select(prod => prod.Name);
				return Json(new { result = false, message = ex.PublicMessage, products = excludedProducts });
			}
			catch (Exception ex)
			{
				NetStepsException exception;
				if (!(ex is NetStepsBusinessException))
					exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				else
					exception = ex as NetStepsException;
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual void InsertCustomAdditionalParameters(Dictionary<string, object> additionalParameters)
		{ }

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult UpdateCart(string orderCustomerId, List<ProductQuantityContainer> orderItems)
		{
			if (orderItems == null)
			{
				return JsonError(_errorNoItemsInOrder);
			}

			var orderCustomerGuid = new Guid(orderCustomerId);
			var customer = OrderContext.Order.AsOrder().OrderCustomers.Single(oc => oc.Guid == orderCustomerGuid);
			try
			{
				var toUpdate = (from oi in orderItems
								from ei in customer.OrderItems
								where Guid.Parse(oi.OrderItemGuid) == ei.Guid
								select new { Quantity = oi.Quantity, ExistingItem = ei, ProductID = oi.ProductID });


				var changes = toUpdate
					.Select(item =>
					{
						var mod = Create.New<IOrderItemQuantityModification>();
						mod.ExistingItem = item.ExistingItem;
						mod.ProductID = item.ProductID;
						mod.Quantity = item.Quantity;
						mod.ModificationKind = OrderItemQuantityModificationKind.SetToQuantity;
						mod.Customer = customer;
						return mod;
					});

				(from oi in orderItems
				 from c in changes.Select(x => (OrderItem)x.ExistingItem).ToList()
				 where c.Guid == Guid.Parse(oi.OrderItemGuid) &&
				 c.Quantity != oi.Quantity
				 select c).ForEach(x => x.HasChanges = true);

				OrderService.UpdateOrderItemQuantities(OrderContext, changes);
				OrderService.UpdateOrder(OrderContext);
				var additionalParameters = new Dictionary<string, object>();

				InsertCustomAdditionalParameters(additionalParameters);

				return FormJsonResponse(customer, additionalParameters);
			}
			catch (Exception ex)
			{
				var message = ex.Log(orderID: OrderContext.Order.OrderID, accountID: CurrentAccount.AccountID).PublicMessage;
				return JsonError(message);
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult RemoveFromCart(string orderCustomerId, string orderItemGuid, string parentGuid = null)
		{
			try
			{
				var orderCustomerGuid = new Guid(orderCustomerId);
				var oiGuid = new Guid(orderItemGuid);
				Guid pGuid;
				if (String.IsNullOrWhiteSpace(parentGuid) || !Guid.TryParse(parentGuid, out pGuid))
				{
					pGuid = Guid.Empty;
				}

				var customer = OrderContext.Order.AsOrder().OrderCustomers.First(oc => oc.Guid == orderCustomerGuid);
				var item = customer.OrderItems.First(oi => ((OrderItem)oi).Guid == oiGuid);
				var itemTypeId = item.OrderItemTypeID;
				var dynamicKitGroupId = ((OrderItem)item).DynamicKitGroupID;

				var removeModification = Create.New<IOrderItemQuantityModification>();
				removeModification.Customer = customer;
				removeModification.ModificationKind = OrderItemQuantityModificationKind.Delete;
				removeModification.ProductID = item.ProductID.Value;
				removeModification.Quantity = 0;
				removeModification.ExistingItem = item;

				var changes = new IOrderItemQuantityModification[] { removeModification };
				OrderService.UpdateOrderItemQuantities(OrderContext, changes);

				bool cartRemoved = CleanupConsultantCart(CurrentParty, itemTypeId);

				OrderService.UpdateOrder(OrderContext);

				OrderItem bundleItem = null;

				if (pGuid != Guid.Empty)
				{
					bundleItem = customer.OrderItems.First(oc => oc.Guid == pGuid);
				}

				bool isBundleGroupComplete = false;
				if (bundleItem != null)
				{
					var bundleProduct = Inventory.GetProduct(bundleItem.ProductID.Value);
					var group = bundleProduct.DynamicKits[0].DynamicKitGroups.Where(g => g.DynamicKitGroupID == dynamicKitGroupId).FirstOrDefault();
					int currentCount = bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId).Sum(oi => oi.Quantity);
					isBundleGroupComplete = currentCount == group.MinimumProductCount;
				}

				var additionalParameters = new Dictionary<string, object>();
				additionalParameters.Add("remainingProductDiscounts", RemainingProductDiscounts);
				additionalParameters.Add("remainingExclusiveProducts", RemainingExclusiveProducts);
				additionalParameters.Add("remainingBookingCredits", RemainingBookingCredits.Values.Sum());
				//additionalParameters.Add("orderHostRewardsItems", GetHostRewardsOrderItemsHtml(CurrentParty.Order, customer));
				additionalParameters.Add("groupItemsHtml", parentGuid.IsNullOrEmpty() ? string.Empty : GetGroupItemsHtml(parentGuid, orderCustomerId, dynamicKitGroupId.Value).ToString());
				additionalParameters.Add("reloadPage", cartRemoved);
				InsertCustomAdditionalParameters(additionalParameters);

				return FormJsonResponse(customer, additionalParameters);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult Cart()
		{
			var currentTime = DateTime.Now;

			if (ApplicationContext.Instance.UseDefaultBundling)
			{
				var bundleUpSale = new Dictionary<string, string>();
				foreach (var customer in CurrentParty.Order.OrderCustomers)
				{
					bundleUpSale.Add(customer.Guid.ToString("N"), GetDynamicBundleUpSale(customer));
				}

				ViewBag.DynamicKitUpSaleHTML = bundleUpSale;
			}

			var bundleTime = (int)DateTime.Now.Subtract(currentTime).TotalMilliseconds;

			ViewBag.PartyOrderUrl = GetPartyOrderUrl(CurrentParty != null ? CurrentParty.PartyID : 0);
			var onlineOrders = Order.LoadChildOrdersFull(CurrentParty.Order.OrderID, (int)Constants.OrderType.OnlineOrder, (int)Constants.OrderType.PortalOrder);
			onlineOrders.SelectMany(o => o.OrderCustomers).SelectMany(oc => oc.OrderItems).ToList().ForEach(oi => ((OrderItem)oi).HasChanges = false);
			ViewBag.OnlineOrders = onlineOrders;
			OrderContext.Order = CurrentParty.Order;
			OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
			OrderService.UpdateOrder(OrderContext);

			var model = GetCartModel(CurrentParty);
			ViewBag.PartyHasChanges = this.PartyHasChanges();

			var operationTime = DateTime.Now.Subtract(currentTime);
			int hours = operationTime.Hours;
			int minutes = operationTime.Minutes;
			int milliseconds = (int)operationTime.TotalMilliseconds;

			return View(model);
		}

		protected virtual IDictionary<Guid, IList<string>> GetOrderItemMessagesDictionary(Order order)
		{
			return order.OrderCustomers
				.SelectMany(oc => oc.OrderItems)
				.ToDictionary(oi => oi.Guid, GetOrderItemMessages);
		}

		protected virtual IList<string> GetOrderItemMessages(OrderItem orderItem)
		{
			Contract.Requires<ArgumentNullException>(orderItem != null);

			return orderItem.OrderItemMessages
				.Select(m => m.Message)
				.ToList();
		}

		protected virtual ICartModel GetCartModel(Party party)
		{
			var model = Create.New<ICartModel>();
			LoadCartModelData(model.Data, party);
			model.CartElements = GetCartElements(party);
			model.Party = party;
			return model;
		}

		protected virtual void LoadCartModelData(dynamic data, Party party)
		{
			Contract.Requires<ArgumentNullException>(party != null);
			// Code contracts rewriter doesn't work with dynamics
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			data.CustomerCarts = GetCustomerCartModels(party);
		}

		protected virtual IList<object> GetCustomerCartModels(Party party)
		{
			Contract.Requires<ArgumentNullException>(party != null);
			Contract.Requires<ArgumentNullException>(party.Order != null);
			Contract.Requires<ArgumentNullException>(party.Order.OrderCustomers != null);

			return party.Order.OrderCustomers
				.Select(GetCustomerCartModel)
				.ToList();
		}

		protected virtual object GetCustomerCartModel(OrderCustomer orderCustomer)
		{
			Contract.Requires<ArgumentNullException>(orderCustomer != null);

			return new
			{
				orderCustomer.Guid
			};
		}

		/// <summary>
		/// Can be overridden to customize the elements on the Cart view.
		/// </summary>
		protected virtual IEnumerable<ICartElement> GetCartElements(Party party)
		{
			return new[]
			{
				GetCartMinimumAmountMessageElement(party),
				GetCartCollapseLinkElement(party),
				GetCartOutOfStockMessageElement(party),
				GetCustomerCartsElement(party),
				GetOnlineOrdersElement(party),
				GetGiftSelectionElement(party)
			};
		}

		protected virtual ICartElement GetCartMinimumAmountMessageElement(Party party)
		{
			var model = Create.New<ICartElement>();
			model.PartialViewName = "_CartMinimumAmountMessage";
			model.Model = party;

			return model;
		}

		protected virtual ICartElement GetCartCollapseLinkElement(Party party)
		{
			var model = Create.New<ICartElement>();
			model.PartialViewName = "_CartCollapseLink";
			model.Model = party;

			return model;
		}

		protected virtual ICartElement GetCartOutOfStockMessageElement(Party party)
		{
			var model = Create.New<ICartElement>();
			model.PartialViewName = "_CartOutOfStockMessage";
			model.Model = party;

			return model;
		}

		protected virtual ICartElement GetCustomerCartsElement(Party party)
		{
			var model = Create.New<ICartElement>();
			model.PartialViewName = "_CustomerCarts";
			model.Model = party;

			return model;
		}

		protected virtual ICartElement GetOnlineOrdersElement(Party party)
		{
			var model = Create.New<ICartElement>();
			model.PartialViewName = "_OnlineOrders";
			model.Model = party;

			return model;
		}

		protected virtual ICartElement GetGiftSelectionElement(Party party)
		{
			var model = Create.New<ICartElement>();
			model.PartialViewName = "GiftSelection";
			model.Model = new GiftSelectionModel(
				getStepInfoUrl: Url.Action("GetGiftStepInfo"),
				saveGiftSelectionUrl: Url.Action("AddGifts"),
				callbackFunctionName: "updateCartDisplay"
			);

			return model;
		}

		protected virtual IEnumerable<IOrderItem> AddOrUpdateOrderItems(IEnumerable<IOrderItemQuantityModification> changes)
		{
			try
			{
				var result = OrderService.UpdateOrderItemQuantities(OrderContext, changes);
				OrderService.UpdateOrder(OrderContext);

				return result;
			}
			catch (Exception ex)
			{

				NetStepsException exception;
				if (!(ex is NetStepsBusinessException))
					exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				else
					exception = ex as NetStepsException;
				throw exception;
			}
		}

		protected virtual void AddOrUpdateOrderItem(string orderCustomerId, Dictionary<int, int> products, bool overrideQuantity, string parentGuid = null, int? dynamicKitGroupId = null)
		{
			try
			{
				var customer = CurrentParty.Order.OrderCustomers.FirstOrDefault(oc => oc.Guid == Guid.Parse(orderCustomerId));
				if (customer == null)
					throw new Exception(Translation.GetTerm("InvalidOrderCustomerTheItemCouldNotBeAdded", "Invalid OrderCustomer. The product could not be added."));

				CurrentParty.Order.AddOrUpdateOrderItem(customer, products.Select(p => new OrderItemUpdateInfo { ProductID = p.Key, Quantity = p.Value }), overrideQuantity, parentGuid: parentGuid, dynamicKitGroupId: dynamicKitGroupId);
				// The order has been modified. We must calculate the totals again.

				OrderService.UpdateOrder(OrderContext);
			}
			catch (Exception ex)
			{
				NetStepsException exception;
				if (!(ex is NetStepsBusinessException))
				{
					exception = EntityExceptionHelper.GetAndLogNetStepsException(
						ex,
						Constants.NetStepsExceptionType.NetStepsApplicationException,
						(CurrentParty != null) ? CurrentParty.OrderID.ToIntNullable() : null,
						accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				}
				else
				{
					exception = ex as NetStepsException;
				}
				throw exception;
			}
		}

		protected virtual string GetOrderItemsHtml(Order order, OrderCustomer orderCustomer)
		{
			var model = new OrderItemsModel(orderCustomer);
			var rendered = RenderRazorPartialViewToString(this.CartItemPartialName, model);

			if (string.IsNullOrWhiteSpace(rendered))
			{
				rendered = string.Empty;
			}

			return rendered;
		}

		protected virtual string GetCustomOrderItemColumn(Order order, OrderCustomer customer, OrderItem orderItem)
		{
			return string.Empty;
		}

		protected virtual object GetTotals(string orderCustomerId)
		{
			Order order = CurrentParty.Order;
			if (order == null)
			{
				return null;
			}

			var customer = order.OrderCustomers.FirstOrDefault(oc => oc.Guid.ToString("N") == orderCustomerId);

			if (customer == null)
			{
				return null;
			}

			List<int> directShipOCIDs = order.OrderShipments.Where(os => os.IsDirectShipment && os.OrderCustomerID.HasValue).Select(os => os.OrderCustomerID.Value).ToList();
			decimal dsSH = order.OrderCustomers.Where(oc => directShipOCIDs.Contains(oc.OrderCustomerID)).Sum(oc => oc.AdjustedShippingTotal + oc.AdjustedHandlingTotal);
			return new
			{
				subtotal = customer.Subtotal.ToDecimal().ToString(order.CurrencyID),
				customerTotal = order.OrderPendingState != Constants.OrderPendingStates.Open ? customer.Total.ToDecimal().ToString(order.CurrencyID) : customer.Subtotal.ToString(order.CurrencyID),
				adjustedSubtotal = customer.AdjustedSubTotal.ToString(order.CurrencyID),
				commissionableTotal = customer.CommissionableTotal.ToDecimal().ToString(order.CurrencyID),
				taxTotal = customer.AdjustedTaxTotal.ToString(order.CurrencyID),
				shippingAndHandlingTotal = (customer.AdjustedShippingTotal + customer.AdjustedHandlingTotal).ToString(order.CurrencyID),
				grandTotal = order.OrderPendingState != Constants.OrderPendingStates.Open ? customer.Total.ToDecimal().ToString(order.CurrencyID) : customer.Subtotal.ToString(order.CurrencyID),
				hostOverage = order.HostessOverage.ToString(order.CurrencyID),
				hostCreditRemaining = order.GetRemainingHostessRewards().ToString(order.CurrencyID),
				partyHostCredit = order.HostessRewardsEarned.ToString(order.CurrencyID),
				partyShippingAndHandling = (order.PartyShipmentTotal + order.PartyHandlingTotal).ToString(order.CurrencyID),
				partyShipping = order.PartyShipmentTotal.ToString(order.CurrencyID),
				partyHandling = order.PartyHandlingTotal.ToString(order.CurrencyID),
				partyTax = order.TaxAmountTotalOverride.HasValue ? order.TaxAmountTotalOverride.ToString(order.CurrencyID) : order.TaxAmountTotal.ToString(order.CurrencyID),
				partySubtotal = order.Subtotal.ToString(order.CurrencyID),
				partyBalanceDue = order.Balance.AsymmetricRoundedNumber().ToString(order.CurrencyID),
				partyCommissionableTotal = order.CommissionableTotal.ToString(order.CurrencyID),
				partyGrandTotal = order.OrderPendingState != Constants.OrderPendingStates.Open ? order.GrandTotal.ToString(order.CurrencyID) : order.Subtotal.ToString(order.CurrencyID),
				directShippingAndHandling = dsSH.ToString(order.Currency),
				exclusiveProductsSubtotal = GetExclusiveProductsSubtotal()
			};
		}

		protected virtual string GetExclusiveProductsSubtotal()
		{
			decimal result = CurrentParty.Order.OrderCustomers
				.SelectMany(oc => oc.OrderItems
					.Where(x => x.OrderItemTypeID == (int)Constants.OrderItemType.ExclusiveProduct)
				).Sum(x => x.GetAdjustedPrice() * x.Quantity);

			return result.ToString(CurrentParty.Order.CurrencyID);
		}

		protected virtual object PartyTotals
		{
			get
			{
				Order order = CurrentParty.Order;
				if (order == null)
					return null;
				return new
				{
					partyHostCredit = order.HostessRewardsEarned.ToString(order.CurrencyID),
					partyShipping = order.PartyShipmentTotal.ToString(order.CurrencyID),
					partyHandling = order.HandlingTotal.ToString(order.CurrencyID),
					partyTax = order.TaxAmountTotalOverride.HasValue ? order.TaxAmountTotalOverride.ToString(order.CurrencyID) : order.TaxAmountTotal.ToString(order.CurrencyID),
					partySubtotal = order.Subtotal.ToString(order.CurrencyID),
					partyCommissionableTotal = order.CommissionableTotal.ToString(order.CurrencyID),
					partyBalanceDue = order.Balance.AsymmetricRoundedNumber().ToString(order.CurrencyID),
					partyGrandTotal = order.GrandTotal.ToString(order.CurrencyID)
				};
			}
		}

		protected bool CleanupConsultantCart(Party party, int orderItemTypeId)
		{
			bool removed = false;
			int consultantId = party.Order.ConsultantID;
			var consultantCustomer = party.Order.OrderCustomers.FirstOrDefault(c => c.AccountID == consultantId);
			if (consultantCustomer != null && !consultantCustomer.OrderItems.Any() && orderItemTypeId == (int)Constants.OrderItemType.ExclusiveProduct)
			{
				RemoveGuestFromParty(consultantCustomer.Guid.ToString("N"));
				party = Party.LoadFull(party.PartyID);
				removed = true;
			}

			return removed;
		}

		public ActionResult HasMetTotal()
		{
			return Json(CurrentParty == null || CurrentParty.HasMetTotal());
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult RemoveGuest(string orderCustomerId)
		{
			var originalParty = CurrentParty.Clone();
			try
			{
				var party = CurrentParty;
				var customer = party.Order.OrderCustomers.FirstOrDefault(oc => oc.Guid.ToString("N") == orderCustomerId);

				if (customer == null)
				{
					return Json(new
					{
						result = false,
						message = Translation.GetTerm("DWS_Party_RemoveGuest_GuestAlreadyRemoved", "The guest has already been removed.  Please refresh the page to see the changes.")
					});
				}

				if (customer.OrderItems.Any())
				{
					return Json(new
					{
						result = false,
						message = Translation.GetTerm("ItemsMustBeDeletedFromCartFirst", "Items must be deleted from cart first.")
					});
				}

				var removedMyself = RemoveGuestFromParty(orderCustomerId);

				return Json(new { result = true, removedMyself });
			}
			catch (Exception ex)
			{
				RefreshPartyFrom(originalParty); // Revert to original party object (before these modifications) to avoid a possibly corrupted object graph - JHE
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		private bool RemoveGuestFromParty(string orderCustomerId)
		{
			try
			{
				var customer = CurrentParty.Order.OrderCustomers.FirstOrDefault(oc => oc.Guid.ToString("N") == orderCustomerId);
				if (customer == null)
				{
					return false;
				}

				customer.OrderItems.RemoveAllAndMarkAsDeleted();
				customer.OrderShipments.RemoveAllAndMarkAsDeleted();
				customer.OrderPayments.RemoveAllAndMarkAsDeleted();
				customer.OrderPaymentResults.RemoveAllAndMarkAsDeleted();
				CurrentParty.Order.OrderCustomers.RemoveAndMarkAsDeleted(customer);
				CurrentParty.Order.Save();

				return customer.AccountID == CurrentAccount.AccountID;
			}
			catch (Exception e)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(e, Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw;
			}
		}

		#endregion

		#region Add Guests
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult AddGuests()
		{
			return View(CurrentParty);
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult GuestForm(int? guestCount, int? groupId, int? guestId)
		{
			if (groupId.HasValue)
			{
				var group = DistributionListCacheHelper.GetDistributionListByAccountID(CurrentAccountId).FirstOrDefault(g => g.DistributionListID == groupId.Value);
				if (group != null)
				{
					return PartialView(NetSteps.Data.Entities.Account.LoadBatchSlim(group.DistributionSubscribers.Where(s => s.Active && s.AccountID.HasValue).Select(s => s.AccountID.Value)));
				}
			}
			else if (guestId.HasValue)
			{
				ViewBag.Guest = PartyGuest.Load(guestId.Value);
				return PartialView();
			}
			ViewBag.GuestCount = guestCount.HasValue ? guestCount.Value : 1;
			return PartialView();
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult SaveGuests(IEnumerable<GuestModel> guests)
		{
			if (guests != null && guests.Any())
			{
				var duplicateguestEmails = guests.GroupBy(x => x.Email).Where(e => e.Count() > 1).Select(e => e).Distinct().ToList();
				var response = new StringBuilder();
				if (duplicateguestEmails.Any())
				{
					response.Append(Translation.GetTerm("AllGuestEmailsMustBeUnique", "All guest emails must be unique."));
					duplicateguestEmails.Each((s) => response.Append("<br/> ").Append(Translation.GetTerm("EmailIsListedMultipleTimes", "'{0}' is listed multiple times.<br/>", s.Key)));
					return Json(new { result = false, message = response.ToString() });
				}
				var reg = new Regex(@"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
				var invalidEmails = guests.Where(x => !reg.IsMatch(x.Email)).ToList();
				if (invalidEmails.Count > 0)
				{
					response.Append(Translation.GetTerm("TheFollowingEmailsAreInvalidPleaseCorrectThemAndResubmit", "The following emails are invalid. Please correct them and resubmit."));
					invalidEmails.Each(s => response.Append("<br/>").Append(s.Email).Append("<br/>"));
					return Json(new { result = false, message = response.ToString() });
				}
				//At this point we know that none of the guests are duplicates all of the inputed emails are correct
				//Filter out the guests that are not already currently in your party order. if you have anyone in your party order yet...
				var allEmailAddresses = CurrentParty.Order.OrderCustomers
						.Where(x => x.AccountInfo != null)
						.Select(x => x.AccountInfo.EmailAddress);
				var guestsYouWant = guests.Where(x => !allEmailAddresses.Contains(x.Email));

				try
				{
					if (guestsYouWant != null && guestsYouWant.Any())
					{
						foreach (var guest in guestsYouWant)
						{
							//Allow all existing non prospect accounts active or not to be able to be placed on a party order.
							var accountId = NetSteps.Data.Entities.Account.GetNonProspectAccountIDByEmail(guest.Email, null);
							OrderCustomer customer;
							if (accountId.HasValue)
							{
								if (CurrentParty.Order.OrderCustomers.All(x => x.AccountID != accountId))
								{
									customer = CurrentParty.Order.AddNewCustomer(accountId.Value);
								}
								else
								{
									string publicMessage = string.Format("Error Adding Guest: {0}{1} '{2}'. {3}",
											   Environment.NewLine,
											   "The following Account already exists as a Customer on the current party:",
											   guest.Email,
											   "Please remove this guest and try again.");
									return Json(new { result = false, message = publicMessage });
								}
							}
							else
							{
								var existingProspect = NetSteps.Data.Entities.Account.GetAccountByEmailAndSponsorID(guest.Email, CurrentAccount.AccountID, true);
								NetSteps.Data.Entities.Account newAccount;
								if (existingProspect == null)
								{
									newAccount = new NetSteps.Data.Entities.Account
									{
										DateCreated = DateTime.Now,
										DefaultLanguageID = CurrentAccount.DefaultLanguageID,
										MarketID = CoreContext.CurrentMarketId,
										FirstName = guest.FirstName,
										LastName = guest.LastName,
										EmailAddress = guest.Email,
										SponsorID = CurrentAccount.AccountID,
										AccountSourceID = (int)ConstantsGenerated.AccountSource.PartyGuest
									};
									//newAccount.AccountNumber = "Temp" + Guid.NewGuid().ToString("N"); //To avoid inserting 2 or more accounts with null account numbers - DES
									// 9654 - New party guests should have the sponsor ID set when they are invited - JM
								}
								else
								{
									newAccount = existingProspect;
								}

								newAccount.AccountTypeID = (int)Constants.AccountType.RetailCustomer;
								//set the account status to active and set the enrollment date to now - Scott Wilson
								newAccount.Activate();
								newAccount.MainPhone = guest.Phone;
								newAccount.User = CreateRetailAccountUser(newAccount);
								var result = newAccount.ValidateRecursive();
								if (!newAccount.IsValid)
								{
									string publicMessage = string.Format("Error Saving {0}: {1}{2}",
											"Account",
											Environment.NewLine,
											result.BrokenRulesList.ToString(brokenRule => string.Format("{0}{1}", brokenRule.Description.ToCleanString().Replace("UTC' ", "' "), Environment.NewLine)));
									return Json(new { result = false, message = publicMessage });
								}

								newAccount.Save();
								customer = CurrentParty.Order.AddNewCustomer(newAccount);
							}

							//Direct ship customer
							if (!string.IsNullOrEmpty(guest.Address1))
							{
								var shipment = new OrderShipment()
								{
									FirstName = guest.FirstName,
									LastName = guest.LastName,
									Attention = guest.Attention,
									Address1 = guest.Address1,
									Address2 = guest.Address2,
									Address3 = guest.Address3,
									City = guest.City,
									PostalCode = guest.PostalCode,
									County = guest.County,
									CountryID = guest.CountryID,
									OrderID = CurrentParty.OrderID,
									IsDirectShipment = true
								};
								shipment.SetState(guest.State, guest.CountryID);
								customer.OrderShipments.Add(shipment);
							}
						}
					}
				}
				catch (Exception ex)
				{
					var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
					return Json(new { result = false, message = exception.PublicMessage });
				}
			}
			return Json(new { result = true });
		}

		protected virtual User CreateRetailAccountUser(NetSteps.Data.Entities.Account account)
		{
			var user = new User();
			user.StartEntityTracking();
			user.Username = GetUsernameDefault(account);
			user.Password = NetSteps.Data.Entities.User.GeneratePassword();
			user.UserStatusID = Constants.UserStatus.Active.ToShort();
			user.UserTypeID = Constants.UserType.Distributor.ToShort();
			user.DefaultLanguageID = account.DefaultLanguageID;
			user.Roles.Add(SmallCollectionCache.Instance.Roles.GetById(Constants.Role.LimitedUser.ToInt()));
			user.Save();

			return user;
		}

		protected virtual string GetUsernameDefault(NetSteps.Data.Entities.Account account)
		{
			var username = account.EmailAddress;
			return username;
		}


		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult AddMyself()
		{
			try
			{
				AddMyselfToParty();

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		private void AddMyselfToParty()
		{
			NetSteps.Data.Entities.Account account = null;
			if (CurrentParty.Order.Consultant != null && CurrentParty.Order.Consultant.AccountID == CurrentAccount.AccountID)
				account = CurrentParty.Order.Consultant;
			else
				account = CurrentAccount;

			CurrentParty.Order.AddNewCustomer(account);

			CurrentParty.Save();
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult RemoveDirectShip(string orderCustomerId)
		{
			var party = CurrentParty;
			var order = party.Order;
			var customer = order.OrderCustomers.FirstOrDefault(oc => oc.Guid.ToString("N") == orderCustomerId);
			if (customer == null)
				return RedirectToAction("Cart");
			customer.OrderShipments.RemoveAllAndMarkAsDeleted();

			OrderService.UpdateOrder(OrderContext);
			CurrentParty = party;
			return RedirectToAction("Cart");
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult EditDirectShip(string orderCustomerId)
		{
			var customer = CurrentParty.Order.OrderCustomers.FirstOrDefault(oc => oc.Guid.ToString("N") == orderCustomerId);
			if (customer == null)
				return RedirectToAction("Cart");
			ViewBag.OrderCustomerId = orderCustomerId;
			ViewBag.Shipment = customer.OrderShipments.FirstOrDefault();
			return View(CurrentParty);
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult SaveDirectShip(string orderCustomerId, string attention,
			string address1, string address2, string address3, string city,
			string state, string county, string postalCode, int countryId)
		{
			try
			{
				var party = CurrentParty;
				var order = party.Order;
				var customer = order.OrderCustomers.FirstOrDefault(oc => oc.Guid.ToString("N") == orderCustomerId);
				var shipment = customer.OrderShipments.FirstOrDefault();
				if (shipment == null)
				{
					shipment = new OrderShipment();
					customer.OrderShipments.Add(shipment);
				}
				shipment.FirstName = customer.FirstName;
				shipment.LastName = customer.LastName;
				shipment.Attention = attention;
				shipment.Address1 = address1;
				shipment.Address2 = address2;
				shipment.Address3 = address3;
				shipment.City = city;
				shipment.PostalCode = postalCode;
				shipment.County = county;
				shipment.CountryID = countryId;
				shipment.SetState(state, countryId);
				shipment.OrderID = CurrentParty.OrderID;
				shipment.IsDirectShipment = true;

				//Set the shippingmethod or an exception is caused in the next page.
				if (!shipment.ShippingMethodID.HasValue || shipment.ShippingMethodID == 0)
				{
					var lowestPriceRate = ShippingCalculator.GetLeastExpensiveShippingMethod(customer, shipment);
					shipment.ShippingMethodID = lowestPriceRate.ShippingMethodID;
					OrderService.UpdateOrder(OrderContext);
				}
				CurrentParty = party;

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region Host Rewards

		public ActionResult ValidateHostRewards()
		{
			var response = CurrentParty.Order.GetHostess().ValidateHostessRewards();
			return Json(new { result = response.Success, message = response.Message });
		}

		protected void SetHasChangesToFalseOnAllItems()
		{
			foreach (var orderItem in this.OrderContext.Order.OrderCustomers.SelectMany(customer => customer.OrderItems))
			{
				((OrderItem)orderItem).HasChanges = false;
			}
		}

		protected bool PartyHasChanges()
		{
			foreach (var cust in OrderContext.Order.AsOrder().OrderCustomers)
			{
				foreach (var item in cust.OrderItems)
				{
					if (item.HasChanges == true)
					{
						return true;
					}
				}
			}

			return false;
			//var retVal = OrderContext.Order.AsOrder().OrderCustomers.Any(x => x.OrderItems.Any(y => y.HasChanges));
			//return retVal;
		}

		public virtual ActionResult Save()
		{
			try
			{
				RefreshPartyFrom(CurrentParty);
				SetHasChangesToFalseOnAllItems();
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}

			return Json(new { result = true });
		}

		public virtual ActionResult ValidateCart()
		{
			if (CurrentParty.Order == null)
			{
				return Json(new { result = false, message = Translation.GetTerm("Workstation_OrderIsNullOnParty", "Order is null on party") });
			}

			var response = ValidateBundles();
			if (response.Success)
			{
				response = HasEmptyCarts();
			}

			return Json(new { result = response.Success, message = response.Message });
		}

		public virtual BasicResponse HasEmptyCarts()
		{
			BasicResponse response = new BasicResponse() { Success = true };

			var orderCustomers = this.GetOrderCustomers();
			foreach (OrderCustomer customer in orderCustomers)
			{
				if (customer.OrderItems.Count == 0)
				{
					response.Success = false;
					response.Message = Translation.GetTerm("EmptyGuestCart", "There are 1 or more guests with empty carts.  Please remove these guests or add items to their cart to continue.");
					break;
				}
			}

			return response;
		}

		protected virtual IEnumerable<OrderCustomer> GetOrderCustomers()
		{
			var parentPartyHostess = CurrentParty.GetParentPartyHostess();
			return CurrentParty.Order.OrderCustomers
				.Where(o => o.OrderCustomerTypeID != Constants.OrderCustomerType.Hostess.ToInt()
					&& (parentPartyHostess == null || (parentPartyHostess != null && o.AccountID != parentPartyHostess.AccountID)))
				.ToList();
		}

		protected BasicResponse ValidateBundles()
		{
			BasicResponse response = new BasicResponse { Success = true };

			foreach (var orderItem in CurrentParty.Order.OrderCustomers.SelectMany(x => x.OrderItems))
			{
				var product = Inventory.GetProduct(orderItem.ProductID.Value);
				if (!orderItem.IsHostReward && !Order.IsDynamicKitValid(orderItem))
				{
					response.Success = false;
					response.Message = Translation.GetTerm("TheBundleYouTriedToOrderIsNotComplete", "The bundle you tried to order ({0}) is not complete.", product.Translations.Name());
				}

				if (Order.IsStaticKitValid(orderItem))
					continue;

				response.Success = false;
				response.Message = Translation.GetTerm("TheKitYouTriedToOrderIsNotComplete", "The kit you tried to order ({0}) is not complete.", product.Translations.Name());
			}

			return response;
		}

		protected virtual bool ShouldSkipShippingMethods()
		{
			return false;
		}

		public virtual ActionResult HostRewards()
		{
			if (OrderContext.Order == null)
				OrderContext.Order = CurrentParty.Order;

			var totalHostessRewards = Create.New<ITotalsCalculator>().SumHostessRewards(CurrentParty.Order);
			var hostessRewardRules = CurrentParty.GetApplicableHostessRewardRules().ToList();
			var productDiscounts = hostessRewardRules.Where(r => r.HostessRewardTypeID != Constants.HostessRewardType.BookingCredit.ToInt()
				&& r.HostessRewardTypeID != Constants.HostessRewardType.ExclusiveProduct.ToInt()
				&& r.Products.HasValue && r.Products.Value > 0).ToList();
			var itemDiscounts = hostessRewardRules.Where(r => r.HostessRewardTypeID == Constants.HostessRewardType.ItemDiscount.ToInt() && r.ProductDiscount.HasValue
				&& r.ProductDiscount.Value > 0).ToList();
			var hostCredit = hostessRewardRules.Where(r => r.HostessRewardTypeID == Constants.HostessRewardType.HostCredit.ToInt()).FirstOrDefault();
			var bookingCredit = hostessRewardRules.Where(r => r.HostessRewardTypeID == Constants.HostessRewardType.BookingCredit.ToInt()).FirstOrDefault();
			var exclusiveProducts = hostessRewardRules.Where(r => r.HostessRewardTypeID == Constants.HostessRewardType.ExclusiveProduct.ToInt()).ToList();
			var hostess = CurrentParty.Order.GetHostess();

			// remove items for which the person no longer qualifies
			var applicableHostessRewardRuleIDs = hostessRewardRules.Select(x => x.HostessRewardRuleID);
			var nonQualifyingRewardItems = hostess.OrderItems.Where(item => item.HostessRewardRuleID != null && !applicableHostessRewardRuleIDs.Contains(item.HostessRewardRuleID.Value)).ToList();
			foreach (var nonQualifyingRewardItem in nonQualifyingRewardItems)
			{
				OrderContext.Order.RemoveItem(nonQualifyingRewardItem);
			}

			// add free items
			var awardedItemRules = hostessRewardRules.Where(r => r.HostessRewardTypeID == Constants.HostessRewardType.FreeItem.ToInt() && r.ProductID != null).ToList();
			foreach (var awardedItemRule in awardedItemRules)
			{
				var existingItem = hostess.OrderItems.SingleOrDefault(existing => existing.HostessRewardRuleID != null && existing.HostessRewardRuleID.Value == awardedItemRule.HostessRewardRuleID);
				if (existingItem == null)
				{
					var inventoryService = Create.New<IInventoryService>();
					var inventoryCheckResponse = inventoryService.GetProductAvailabilityForOrder(OrderContext, awardedItemRule.ProductID.Value, 1);
					if (inventoryCheckResponse.CanAddNormally == 1 || inventoryCheckResponse.CanAddBackorder == 1)
					{
						var product = Inventory.GetProduct(awardedItemRule.ProductID.Value);
						if (product == null)
						{
							var message = String.Format("Hostess reward attempted to add item with product id {0} but it was not found.", awardedItemRule.ProductID);
							var exception = EntityExceptionHelper.GetAndLogNetStepsException(new Exception(), NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null);
							return Json(new { result = false, message = exception.PublicMessage });
						}
						CurrentParty.Order.AddItem(hostess, product, 1, Constants.OrderItemType.FreeItem, 0, 0, true, awardedItemRule.HostessRewardRuleID, null, null);
					}
				}
			}

			OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
			OrderService.UpdateOrder(OrderContext);

			int? qualifiedBookingCredits = null;
			if (bookingCredit != null)
			{
				qualifiedBookingCredits = bookingCredit.Products;
			}

			//There are no host rewards to redeem - DES
			if (CurrentParty.Order.HostessRewardsEarned == 0 && !productDiscounts.Any() && !itemDiscounts.Any() && bookingCredit == null && !exclusiveProducts.Any())
			{
				if (CurrentParty.Order.GetHostess() != null && !CurrentParty.Order.GetHostess().OrderItems.Any(x => x.IsHostReward))
				{
					return RedirectToAction("ShippingMethod");
				}
			}

			ViewBag.ExclusiveProducts = exclusiveProducts;
			ViewBag.BookingCredit = bookingCredit;
			ViewBag.QualifiedBookingCredits = qualifiedBookingCredits;
			ViewBag.ProductDiscounts = productDiscounts;
			ViewBag.ItemDiscounts = itemDiscounts;

			ViewBag.HostCreditCollection = new HostCreditCollection
			{
				HostCredit = new HostCreditItem
				{
					Title = "HostCredit",
					RemainingDictionary = new Dictionary<string, int> 
					{ 
					   {   
						CurrentParty.Order.GetRemainingHostessRewards().ToString(CurrentParty.Order.CurrencyID) ,
						hostCredit != null ? hostCredit.HostessRewardRuleID : 0
						}
					},
					CartPrefix = new PrefixHelper("hostCredit"),
					HeaderPrefix = new PrefixHelper("hostCredit"),
					CartSuffix = new SuffixHelper("HostCredit"),
					Rules = productDiscounts
				},
				ItemDiscounts = new HostCreditItem()
				{
					Rules = itemDiscounts,
					Title = Translation.GetTerm("HostessItemDiscounts", "Product Discounts"),
					HiddenClass = "itemDiscountHostRewardRuleId",
					CartPrefix = new PrefixHelper("itemDiscount"),
					HeaderPrefix = new PrefixHelper("itemDiscount"),
					CartSuffix = new SuffixHelper("ItemDiscount")
				},
				ProductDiscounts = new HostCreditItem()
				{
					Title = Translation.GetTerm("ProductDiscounts", "Product Discounts"),
					HeaderPrefix = new PrefixHelper("productDiscount"),
					HiddenClass = "hostRewardRuleId",
					CartPrefix = new PrefixHelper("percentOff"),
					CartSuffix = new SuffixHelper("PercentOff"),
					RemainingDictionary = RemainingExclusiveProducts,
					Rules = productDiscounts
				}
			};

			ViewBag.ExclusiveProductsSubtotal = GetExclusiveProductsSubtotal();
			ViewData["RemainingBookingCredits"] = RemainingBookingCredits.Values.Sum();
			ViewData["RemainingProductDiscounts"] = RemainingProductDiscounts;
			ViewData["RemainingExclusiveProducts"] = RemainingExclusiveProducts;
			ViewData["HostCreditRewardRuleID"] = hostCredit != null ? hostCredit.HostessRewardRuleID : 0;

			return View(CurrentParty);
		}

		protected virtual string BuildProductCell(string prependInfo, Product product, OrderItem orderItem, OrderCustomer orderCustomer)
		{
			string cellString = prependInfo;
			if (product.IsDynamicKit())
			{
				if (orderItem.IsHostReward)
				{
					cellString += "<a href=\"" +
					Url.Content("~/Orders/Party/BundlePackItems?productId=" + product.ProductID + "&bundleGuid=" + orderItem.Guid.ToString("N") + "&orderCustomerId=" +
						orderCustomer.Guid.ToString("N")) + "\">" + product.Translations.Name() +
					"</a>";
				}
				else
				{
					cellString += product.Translations.Name();
				}
			}
			else
			{
				cellString += product.Translations.Name();
			}

			if (product.IsDynamicKit() || product.IsStaticKit())
			{
				if (product.IsDynamicKit())
				{
					int requiredItemsInBundleCount = product.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount);
					if (orderItem.ChildOrderItems.Sum(oi => oi.Quantity) == requiredItemsInBundleCount)
					{
						cellString += "<span class=\"UI-icon icon-bundle-full\"></span>";
					}
					else
					{
						cellString += "<span class=\"UI-icon icon-bundle-add\"></span>";
					}
				}
				cellString += "<div class=\"bundlePackItemList\">";
				cellString += "<table cellspacing=\"0\" width=\"100%\">";
				cellString += "<tbody>";
				cellString += "<tr>";
				cellString += "<th></th>";
				cellString += "<th>" + Translation.GetTerm("SKU") + "</th>";
				cellString += "<th>" + Translation.GetTerm("Product") + "</th>";
				cellString += "<th>" + Translation.GetTerm("Quantity") + "</th>";
				cellString += "</tr>";
				foreach (OrderItem childItem in orderItem.ChildOrderItems)
				{
					Product childProduct = Inventory.GetProduct(childItem.ProductID.Value);
					cellString += "<tr>";
					cellString += "<td>";
					cellString += "<span class=\"UI-icon icon-bundle-arrow\"></span>";
					cellString += "</td>";
					cellString += "<td class=\"KitSKU\">" + childItem.SKU + "</td>";
					cellString += "<td>" + childProduct.Translations.Name() + "</td>";
					cellString += "<td>" + childItem.Quantity + "</td>";
					cellString += "</tr>";
				}
				cellString += "</tbody>";
				cellString += "</table>";
				cellString += "</div>";
			}

			return cellString;
		}

		protected virtual void AppendCustomCellAfterProductCell(StringBuilder builder, Product product)
		{
			// Extension point.
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult SearchRewardProducts(string query, int hostessRewardTypeID)
		{
			try
			{
				var types = CurrentParty.GetApplicableHostessRewardRules().Where(r => r.HostessRewardTypeID == hostessRewardTypeID).Select(r => r.HostessRewardTypeID).Distinct();
				var catalogsIDs = HostessRewardType.GetAvailableCatalogs(types);
				//if no catalogs found then don't return any products
				if (catalogsIDs.Count == 0)
					return Json(new List<int>());

				OrderShipment orderShipment = null;
				if (CurrentParty.Order != null)
				{
					orderShipment = CurrentParty.Order.OrderShipments.FirstOrDefault();
				}
				var outOfStockProductIDs = Product.GetOutOfStockProductIDs(orderShipment);
				var rewardProducts = GetRewardProducts(query, catalogsIDs, outOfStockProductIDs, orderShipment);
				return FormatProductList(rewardProducts);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		protected virtual JsonResult FormatProductList(List<Product> products)
		{
			var productList = products.Select(p => new
				{
					id = p.ProductID,
					text = p.SKU + " - " + p.Translations.Name(),
					needsBackOrderConfirmation = Product.CheckStock(p.ProductID).IsOutOfStock && p.ProductBackOrderBehaviorID == Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer.ToShort()
				}).ToList();
			return Json(productList);
		}

		protected virtual List<Product> GetRewardProducts(string query, List<int> catalogIDs, List<int> outOfStockProductIDs, OrderShipment orderShipment)
		{
			var productList = Inventory.SearchProducts(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID, query, CurrentAccount.AccountTypeID, catalogsToSearch: catalogIDs)
				.Where(p => (!outOfStockProductIDs.Contains(p.ProductID) || p.ProductBackOrderBehaviorID != (int)Constants.ProductBackOrderBehavior.Hide) && (p.WarehouseProducts.Count > 0) && (!p.IsVariantTemplate));
			return productList.ToList();
		}

		protected virtual Dictionary<string, int> RemainingExclusiveProducts
		{
			get
			{
				var hostess = CurrentParty.Order.GetHostess();
				var hostessRewardItems = CurrentParty.Order.GetHostessRewardOrderItems();
				var applicableHostessRewards = CurrentParty.GetApplicableHostessRewardRules().Where(r => r.HostessRewardTypeID == (int)Constants.HostessRewardType.ExclusiveProduct && r.Products.HasValue && r.Products.Value > 0).ToList();
				var items = applicableHostessRewards.ToDictionary(r => r.HostessRewardRuleID.ToString(), r => r.Products.ToInt() - hostessRewardItems.Where(oi => ((OrderItem)oi).OrderItemTypeID == (int)Constants.OrderItemType.ExclusiveProduct).Sum(oi => oi.Quantity));

				return items;
			}
		}

		protected virtual Dictionary<string, int> RemainingProductDiscounts
		{
			get
			{
				var hostess = CurrentParty.Order.GetHostess();
				var applicableHostessRewards = CurrentParty.GetApplicableHostessRewardRules().Where(r => r.HostessRewardTypeID != (int)Constants.HostessRewardType.BookingCredit && r.HostessRewardTypeID != (int)Constants.HostessRewardType.ExclusiveProduct && r.Products.HasValue && r.Products.Value > 0);

				return applicableHostessRewards.ToDictionary(r => r.HostessRewardRuleID.ToString(), r => r.Products.ToInt() - hostess.OrderItems.Where(oi => oi.OrderItemTypeID != (int)Constants.OrderItemType.BookingCredit && oi.OrderItemTypeID != (int)Constants.OrderItemType.ExclusiveProduct && oi.DiscountPercent == r.ProductDiscount).Sum(oi => oi.Quantity));
			}
		}

		protected virtual Dictionary<string, int> RemainingBookingCredits
		{
			get
			{
				var hostessRewardOrderItems = CurrentParty.Order.GetHostessRewardOrderItems();
				var hostessRewardRules = CurrentParty.GetApplicableHostessRewardRules().Where(r => r.HostessRewardTypeID == Constants.HostessRewardType.BookingCredit.ToInt()).ToList();
				var applicableHostessRewards = hostessRewardRules.FirstOrDefault();
				var creditsRemainingDictionary = new Dictionary<string, int>();
				if (applicableHostessRewards != null)
				{
					int creditsRemaining = applicableHostessRewards.Products.ToInt() - hostessRewardOrderItems.Where(oi => ((OrderItem)oi).HostessRewardRuleID == applicableHostessRewards.HostessRewardRuleID).Sum(oi => oi.Quantity);
					creditsRemainingDictionary.Add(applicableHostessRewards.HostessRewardRuleID.ToString(), creditsRemaining);
				}

				return creditsRemainingDictionary;
			}
		}

		public virtual ActionResult UpdateRewardQuantities(Dictionary<string, int> orderItems)
		{
			try
			{
				var hostess = CurrentParty.Order.GetHostess();
				bool hasInvalidItems = false;
				int invalidItemsCount = 0;
				string invalidItemsErrorMessage = string.Empty;

				if (orderItems != null)
				{
					foreach (var orderItem in orderItems)
					{
						var originalOrderItem = hostess.OrderItems.FirstOrDefault(oi => oi.Guid == Guid.Parse(orderItem.Key));
						var updateQuantity = orderItem.Value;
						if (originalOrderItem != null)
						{
							if (updateQuantity <= 0)
							{
								CurrentParty.Order.RemoveItem(originalOrderItem);
								continue;
							}

							int quantityDiff = updateQuantity - originalOrderItem.Quantity;
							BasicResponse isValid = hostess.ValidateHostessRewardItem(quantityDiff, originalOrderItem.HostessRewardRuleID, CurrentParty.Order);
							//Only update if we are actually changing the quantity to avoid unnecessary calculations - DES
							if (isValid.Success && quantityDiff != 0)
							{
								CurrentParty.Order.UpdateItem(hostess, originalOrderItem, updateQuantity);
								if (((OrderItem)originalOrderItem).OrderItemTypeID == (int)Constants.OrderItemType.HostCredit)
								{
									foreach (var hostCreditItem in hostess.OrderItems.Where(oi => oi.OrderItemTypeID == (int)Constants.OrderItemType.HostCredit && oi.Guid.ToString("N") != ((OrderItem)originalOrderItem).Guid.ToString("N")))
									{
										CurrentParty.Order.UpdateItem(hostess, hostCreditItem, hostCreditItem.Quantity);
									}
								}
							}
							else if (!isValid.Success)
							{
								hasInvalidItems = true;
								invalidItemsCount++;
								string newErrorMessage = string.Format("{0} {1} {2}", isValid.Message.Clone(), Translation.GetTerm("PleaseRemoveProduct", "Please remove product: "), ((OrderItem)originalOrderItem).SKU);
								if (invalidItemsCount > 1)
								{
									invalidItemsErrorMessage = invalidItemsErrorMessage + "<br />" + newErrorMessage;
								}
								else
								{
									invalidItemsErrorMessage = invalidItemsErrorMessage + newErrorMessage;
								}
							}
						}
					}

					OrderContext.Order = CurrentParty.Order;
					OrderService.UpdateOrder(OrderContext);
				}

				var additionalParameters = new Dictionary<string, object>();
				additionalParameters.Add("remainingProductDiscounts", RemainingProductDiscounts);
				additionalParameters.Add("remainingBookingCredits", RemainingBookingCredits.Values.Sum());
				additionalParameters.Add("remainingExclusiveProducts", RemainingExclusiveProducts);

				if (hasInvalidItems)
				{
					additionalParameters.Add("message", invalidItemsErrorMessage);
				}

				return FormJsonResponse(hostess, additionalParameters);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region Shipping Method
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult ShippingMethod()
		{
			ViewBag.CurrencyID = CurrentParty.Order.CurrencyID;
			ViewBag.DirectShipGuests = CurrentParty.Order.OrderCustomers.Where(oc => oc.OrderShipments != null && oc.OrderShipments.Count > 0).ToDictionary(oc => new Tuple<string, string>(oc.Guid.ToString("N"), oc.FullName), oc => oc.GetShippingMethods());

			try
			{
				ViewBag.ShippingMethods = CurrentParty.Order.GetShippingMethods();
			}
			catch (ProductShippingExcludedShippingException ex)
			{
				ViewBag.Products = ex.ProductsThatHaveExcludedShipping;
			}

			return View(CurrentParty);
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult SetShippingMethod(int partyShippingMethod, Dictionary<string, int> guests)
		{
			try
			{
				CurrentParty.Order.SetShippingMethod(partyShippingMethod);

				if (guests != null)
				{
					foreach (var guest in guests)
					{
						CurrentParty.Order.OrderCustomers.FirstOrDefault(oc => oc.Guid.ToString("N") == guest.Key).SetShippingMethod(guest.Value);
					}
				}

				OrderContext.Order = CurrentParty.Order;
				OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
				OrderService.UpdateOrder(OrderContext);
				CurrentParty.Save();

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region Payments

		public virtual ActionResult Payments()
		{
			ViewBag.ProductCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(CurrentAccount.AccountID);

			return View(CurrentParty);
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult ApplyPayment(string orderCustomerId, Constants.PaymentType paymentType, int? paymentMethodId,
			string accountNumber, string bankName, string nameOnCard, string cvv, DateTime? expirationDate, decimal amount, string nameOnAccount, string bankAccountNumber,
			string routingNumber, short? bankAccountTypeID, string billingZipcode)
		{
			try
			{
				var customer = CurrentParty.Order.OrderCustomers.FirstOrDefault(oc => !orderCustomerId.IsNullOrEmpty() && oc.Guid.ToString("N") == orderCustomerId);
				var shipment = customer == null || !customer.OrderShipments.Any() ? CurrentParty.Order.GetDefaultShipment() : customer.OrderShipments.First();
				IPayment payment;
				if (paymentMethodId.HasValue)
				{
					payment = CurrentAccount.AccountPaymentMethods.GetByAccountPaymentMethodID(paymentMethodId.Value);
				}
				else
				{
					switch (paymentType)
					{
						case Constants.PaymentType.Check:
							payment = new Payment()
							{
								DecryptedAccountNumber = accountNumber.RemoveNonNumericCharacters(),
								PaymentType = paymentType
							};
							break;
						case Constants.PaymentType.GiftCard:
							payment = new NonAccountPaymentMethod()
							{
								DecryptedAccountNumber = accountNumber,
								PaymentTypeID = paymentType.ToInt()
							};
							break;
						case Constants.PaymentType.CreditCard:
							if (string.IsNullOrEmpty(billingZipcode))
							{
								var exception = EntityExceptionHelper.GetAndLogNetStepsException(TermTranslation.LoadTermByTermNameAndLanguageID("Zipcode Required.", CurrentSite.Language.LanguageID), NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
								return Json(new { result = false, message = exception.PublicMessage });
							}
							payment = new Payment()
							{
								DecryptedAccountNumber = accountNumber.RemoveNonNumericCharacters(),
								CVV = cvv,
								ExpirationDate = expirationDate.Value.LastDayOfMonth(),
								NameOnCard = nameOnCard,
								BillingAddress = new Address()
								{
									CountryID = shipment.CountryID,
									PostalCode = billingZipcode.ToCleanString()
								}
							};
							break;
						case Constants.PaymentType.EFT:
							payment = new Payment()
							{
								BankName = bankName,
								DecryptedAccountNumber = bankAccountNumber.RemoveNonNumericCharacters(),
								RoutingNumber = routingNumber,
								NameOnCard = nameOnAccount,
								PaymentType = Constants.PaymentType.EFT,
								BankAccountTypeID = bankAccountTypeID,
								BillingAddress = new Address
								{
									CountryID = shipment.CountryID,
									PostalCode = shipment.PostalCode
								}
							};
							break;
						case Constants.PaymentType.ProductCredit:
							payment = new NonAccountPaymentMethod()
							{
								PaymentTypeID = (int)paymentType
							};
							break;
						default:
							throw new Exception(Translation.GetTerm("InvalidPaymentType", "Invalid payment type"));
					}
					payment.PaymentTypeID = (int)paymentType;
				}

				BasicResponseItem<OrderPayment> response = customer == null
					? CurrentParty.Order.ApplyPaymentToOrder(payment, amount)  
					: CurrentParty.Order.ApplyPaymentToCustomer(payment.PaymentTypeID, amount,payment.NameOnCard,payment, customer);

				if (!response.Success)
					return Json(new { result = false, message = response.Message });

				// Don't recalculate shipping and taxes when adding or removing a payment.
				OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
				OrderService.UpdateOrder(OrderContext);
				OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;

				return Json(new
				{
					result = true,
					totals = string.IsNullOrEmpty(orderCustomerId) ? PartyTotals : GetTotals(orderCustomerId),
					orderCustomerId = orderCustomerId,
					productCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(CurrentAccount.AccountID).ToString(CurrentParty.Order.CurrencyID),
					paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", CurrentParty)
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult RemovePayment(string paymentId, string orderCustomerId)
		{
			try
			{
				if (string.IsNullOrEmpty(orderCustomerId) || CurrentParty.Order.OrderCustomers.Any(oc => oc.Guid.ToString("N") == orderCustomerId))
				{
					var payment = string.IsNullOrEmpty(orderCustomerId) ? CurrentParty.Order.OrderPayments.FirstOrDefault(op => op.Guid.ToString("N") == paymentId) : CurrentParty.Order.OrderCustomers.FirstOrDefault(oc => oc.Guid.ToString("N") == orderCustomerId).OrderPayments.FirstOrDefault(op => op.Guid.ToString("N") == paymentId);
					if (payment != null)
					{
						if (payment.OrderPaymentStatusID == Constants.OrderPaymentStatus.Completed.ToInt())
						{
							return Json(new { result = false, message = Translation.GetTerm("ThisCreditCardHasAlreadyBeenAuthorizedAndCannotBeRemoved", "This credit card has already been authorized and cannot be removed.") });
						}
						else
						{
							if (string.IsNullOrEmpty(orderCustomerId))
								CurrentParty.Order.RemovePayment(payment);
							else
								CurrentParty.Order.OrderCustomers.FirstOrDefault(oc => oc.Guid.ToString("N") == orderCustomerId).RemovePayment(payment);
						}
					}

					// Don't recalculate shipping and taxes when adding or removing a payment.
					OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
					OrderService.UpdateOrder(OrderContext);
					OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;

					return Json(new
					{
						result = true,
						totals = string.IsNullOrEmpty(orderCustomerId) ? PartyTotals : GetTotals(orderCustomerId),
						productCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(CurrentAccount.AccountID).ToString(CurrentParty.Order.CurrencyID),
						paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", CurrentParty),
					});
				}

				return Json(new { result = false, message = Translation.GetTerm("CouldNotFindCustomer", "Could not find that customer.") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpPost]
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult LookupGiftCardBalance(string giftCardCode)
		{
			try
			{
				var gcService = Create.New<IGiftCardService>();
				Order order = CurrentParty.Order;
				decimal? balance = gcService.GetBalanceWithPendingPayments(giftCardCode, order);
				if (!balance.HasValue)
				{
					return Json(new { result = false, message = Translation.GetTerm("GiftCardNotFound", "Gift card not found") });
				}
				else
				{
					return Json(new { result = true, balance = balance.Value.ToString(order.CurrencyID), amountToApply = Math.Min(balance.Value, order.Balance ?? 0m) });
				}
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region Receipt
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult Submit()
		{
			try
			{
				var validationResponse = CurrentParty.ValidatePartyForSubmission();
				if (!validationResponse.Success)
				{
					return Json(new
					{
						result = false,
						message = validationResponse.Message.ToCleanString().Replace(Environment.NewLine, "<br/>"),
						paymentsGrid = RenderRazorPartialViewToString(this.PaymentsGridPartialName, CurrentParty),
						totals = PartyTotals,
						productCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(CurrentAccount.AccountID).ToString(CurrentParty.Order.CurrencyID),
					});
				}

				OrderContext.Order = CurrentParty.Order;
				OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
				var response = OrderService.SubmitOrder(OrderContext);
				if (!response.Success)
				{
					return Json(new
					{
						result = false,
						message = response.Message.ToCleanString().Replace(Environment.NewLine, "<br/>"),
						paymentsGrid = RenderRazorPartialViewToString(this.PaymentsGridPartialName, CurrentParty),
						totals = PartyTotals,
						productCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(CurrentAccount.AccountID).ToString(CurrentParty.Order.CurrencyID)
					});
				}
				EventScheduler.SchedulePartyCompletionEvents(CurrentParty.PartyID);
				EventScheduler.ScheduleOrderCompletionEvents(CurrentParty.OrderID);

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new
				{
					result = false,
					message = exception.PublicMessage,
					paymentsGrid = RenderRazorPartialViewToString(this.PaymentsGridPartialName, CurrentParty),
					totals = PartyTotals,
					productCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(CurrentAccount.AccountID).ToString(CurrentParty.Order.CurrencyID),
				});
			}
		}

		[PartySetup]
		public virtual ActionResult Receipt(int? orderId)
		{
			try
			{
				ViewBag.HasEnrollmentCredit = false;
				if (CurrentParty == null && orderId.HasValue)
				{
					CurrentParty = Party.LoadFullByOrderID(orderId.Value);
					if (!CurrentAccountCanAccessParty(CurrentParty)) return this.RedirectToSafePage();
				}

				if (!orderId.HasValue && CurrentParty != null)
				{
					orderId = CurrentParty.OrderID;
				}

				//This causes an error because the model cannot be null
				if (CurrentParty == null)
				{
					throw new Exception(Translation.GetTerm("Workstation_NoPartyLoaded", "No party loaded"));
				}

				ViewBag.OnlineOrders = Order.LoadChildOrdersFull(orderId.Value, (int)Constants.OrderType.OnlineOrder, (int)Constants.OrderType.PortalOrder);
				ViewBag.DisplayConsultantEnrollmentLink = DisplayConsultantEnrollmentLink();
				var hostess = CurrentParty.Order.GetHostess();
				if (hostess != null && NetSteps.Common.Configuration.ConfigurationManager.GetAppSetting<bool>("ShowPartyHostEnrollLink"))
				{
					decimal enrollmentCredit = _productCreditLedgerService.GetCurrentBalance(hostess.AccountID);
					if (enrollmentCredit > 0 && (hostess.AccountTypeID == Constants.AccountType.RetailCustomer.ToInt()
						|| hostess.AccountTypeID == Constants.AccountType.PreferredCustomer.ToInt()))
					{
						ViewBag.HasEnrollmentCredit = true;
						string PWSUrl = string.Empty;
						ViewBag.ConsultantHasPWS = NetSteps.Data.Entities.AccountExtensions.HasPWS(CurrentAccount, ref PWSUrl);
						if ((bool)ViewBag.ConsultantHasPWS)
						{
							string unencodedURL = string.Format("{0}Login?token={1}&returnUrl=Enroll/Landing%3Fupgrade%3DTrue%26accountTypeID%3D{2}%26countryID%3D{3}",
															PWSUrl.AppendForwardSlash().ConvertToSecureUrl(ConfigurationManager.ForceSSL),
															NetSteps.Data.Entities.Account.GetSingleSignOnToken(hostess.AccountID),
															Constants.AccountType.Distributor.ToInt(),
															CurrentAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Main).CountryID);

							ViewBag.EnrollmentSiteUrl = IEldResolverExtensions.EldEncode(unencodedURL);
						}
					}
				}

				var party = CurrentParty;
				CurrentParty = null; // So that the user can not hit the back button - JHE
				OrderContext.Clear();

				return View(this.ReceiptViewName, party);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId.ToIntNullable());
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual bool DisplayConsultantEnrollmentLink()
		{
			return false;
		}

		public virtual string GetConsultantEnrollmentLink(int accountID, int countryID)
		{
			return "";
		}

		#endregion

		#region Bundle

		public virtual ActionResult BundlePackItems(int productId, string bundleGuid, string orderCustomerId)
		{
			var product = Inventory.GetProduct(productId);
			var dynamicKit = new DynamicKit();
			var dynamicKitGroups = new NetSteps.Data.Entities.TrackableCollection<DynamicKitGroup>();
			var order = CurrentParty.Order;

			if (product.DynamicKits.Count != 0)
			{
				dynamicKit = product.DynamicKits[0];
				dynamicKitGroups = product.DynamicKits[0].DynamicKitGroups;
			}

			ViewBag.DynamicKit = dynamicKit;
			ViewBag.DynamicKitGroups = dynamicKitGroups;
			ViewBag.MaxItemsInBundle = dynamicKitGroups.Sum(g => g.MinimumProductCount);
			ViewBag.ProductId = productId;
			ViewBag.BundleGuid = bundleGuid;
			ViewBag.OrderCustomerId = orderCustomerId;

			if (order != null)
			{
				ViewBag.OrderItem = order.OrderCustomers.GetByGuid(orderCustomerId).OrderItems.GetByGuid(bundleGuid);
			}

			return View();
		}

		public ActionResult SaveBundle(string orderCustomerId, string bundleGuid)
		{
			try
			{
				var customer = CurrentParty.Order.OrderCustomers.First(oc => oc.Guid.ToString("N") == orderCustomerId);
				var orderItem = customer.OrderItems.GetByGuid(bundleGuid);

				Product kitProduct = null;
				if (orderItem.ProductID.HasValue)
				{
					kitProduct = Inventory.GetProduct(orderItem.ProductID.Value);
				}
				if (kitProduct == null)
				{
					return Json(new
					{
						result = false,
						message = "Could not find a product with that SKU."
					});
				}

				if (!Order.IsDynamicKitValid(orderItem))
				{
					return Json(new
					{
						result = false,
						message = Translation.GetTerm("TheBundleYouTriedToOrderIsNotComplete", "The bundle you tried to order ({0}) is not complete.", kitProduct.Translations.Name())
					});
				}

				var orderPayments = customer.OrderPayments.Where(p => p.OrderPaymentStatusID != ConstantsGenerated.OrderPaymentStatus.Completed.ToShort()).ToList();
				OrderPayment payment = null;
				if (orderPayments.Count > 0)
				{
					payment = orderPayments[0];
				}

				if (payment != null)
				{
					OrderService.UpdateOrder(OrderContext);
					if (payment.OrderPaymentStatusID != ConstantsGenerated.OrderPaymentStatus.Completed.ToShort())
					{
						payment.Amount = CurrentParty.Order.GrandTotal.ToDecimal();
					}
					customer.OrderPayments.Add(payment);
				}

				return Json(new
				{
					result = true,
					itemsInCart = customer.OrderItems.Count,
					total = CurrentParty.Order.Subtotal.ToString(CurrentParty.Order.CurrencyID),
					productName = kitProduct.Translations.Name(),
					image = kitProduct.MainImage == null ? string.Empty : kitProduct.MainImage.FilePath.ReplaceFileUploadPathToken()//,
					//orderItems = GetFormattedCartPreview().ToString()
				});
			}
			catch (Exception ex)
			{
				NetStepsException exception;
				if (!(ex is NetStepsBusinessException))
				{
					exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				}
				else
				{
					exception = ex as NetStepsException;
				}
				return Json(new
				{
					result = false,
					message = exception.PublicMessage
				});
			}

		}

		[NonAction]
		public virtual HtmlString GetGroupItemsHtml(string parentGuid, string orderCustomerId, int groupId)
		{
			if (CurrentParty.Order == null)
				return new HtmlString(string.Empty);

			var customer = CurrentParty.Order.OrderCustomers.First(oc => oc.Guid.ToString("N") == orderCustomerId);
			var builder = new StringBuilder();
			var orderItem = customer.OrderItems.GetByGuid(parentGuid);
			var product = Inventory.GetProduct(orderItem.ProductID.ToInt());
			var dynamicKit = product.DynamicKits[0];
			var groupItems = customer.OrderItems.Where(index => index.DynamicKitGroupID == groupId);

			foreach (var item in groupItems)
			{
				Product childProduct = Inventory.GetProduct(item.ProductID.Value);
				for (int q = 0; q < item.Quantity; q++)
				{
					TagBuilder span = new TagBuilder("span");
					span.AddCssClass("block");

					span.InnerHtml = new StringBuilder()
							.Append("<input type=\"hidden\" value=\"" + item.Guid.ToString("N") + "\" class=\"orderItemGuid\" />")
							.Append("<a href=\"javascript:void(0)\" class=\"UI-icon icon-x RemoveItem\"></a>&nbsp;" + childProduct.SKU + " " + childProduct.Translations.Name()).ToString();

					builder.Append(span.ToString());
				}
			}
			var results = builder.ToString();

			return new HtmlString(string.IsNullOrEmpty(results) ? string.Empty : results);
		}

		#endregion

		#region Host Invite
		[PartySetup]
		[ValidateInput(false)]
		[FunctionFilter("Orders-Party Order", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult SetupHostInvitePreview(string content)
		{
			try
			{
				TempData["content"] = content;
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[PartySetup]
		[FunctionFilter("Orders-Party Order", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult PreviewHostInvite()
		{
			var emailTemplate = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
			{
				Active = true,
				PageIndex = 0,
				PageSize = 1,
				EmailTemplateTypeIDs = new List<short>() { (short)Constants.EmailTemplateType.EvitesHostessInvite }
			}).FirstOrDefault();

			if (emailTemplate != null)
			{
				string content = TempData["content"].ToString();

				var translation = emailTemplate.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);

				if (translation != null)
				{
					return Content(translation.GetTokenReplacedMailMessage(
							new CompositeTokenValueProvider(
									new FakePartyTokenValueProvider(),
									new FakeOrderCustomerTokenValueProvider(),
									new PersonalizedContentTokenValueProvider(content, null))).HTMLBody, "text/html");
				}
			}

			return Content(Translation.GetTerm("NoPreviewAvailable", "No preview available."));
		}

		protected EmailTemplate GetHostInviteEmailTemplate()
		{
			return EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
			{
				Active = true,
				PageIndex = 0,
				PageSize = 1,
				EmailTemplateTypeIDs = new List<short>() { (short)Constants.EmailTemplateType.EvitesHostessInvite }
			}).FirstOrDefault();
		}

		protected static MailAccount _corporateMailAccount = null;
		protected static object _lock = new object();

		[NonAction]
		public virtual void SendHostInvitation(Party party, string hostEmailAddress = null)
		{
			var hostInviteEmailTemplate = EmailTemplate.Search(new EmailTemplateSearchParameters
			{
				Active = true,
				PageIndex = 0,
				PageSize = 1,
				EmailTemplateTypeIDs = new List<short>() { (short)Constants.EmailTemplateType.EvitesHostessInvite }
			}).FirstOrDefault();
			if (hostInviteEmailTemplate != null)
			{
				var translation =
					hostInviteEmailTemplate.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);
				if (translation == null)
				{
					return;
				}
				var host = party.Order.GetHostess();
				var token =
					party.EmailTemplateTokens.Any(
						ett => ett.Token == "DistributorContent" && ett.AccountID == CurrentAccount.AccountID)
						? party.EmailTemplateTokens.First(
							ett => ett.Token == "DistributorContent" && ett.AccountID == CurrentAccount.AccountID)
						: new EmailTemplateToken();
				var partyTokenValueProvider = Create.NewWithParams<PartyTokenValueProvider>(
					LifespanTracking.External, Param.Value(party));
				var message =
					translation.GetTokenReplacedMailMessage(
						new CompositeTokenValueProvider(
							new PartyTokenValueProvider(party),
							new OrderCustomerTokenValueProvider(host),
							new PersonalizedContentTokenValueProvider(token.Value, null)));
				if (_corporateMailAccount == null)
				{
					lock (_lock)
					{
						if (_corporateMailAccount == null)
						{
							_corporateMailAccount = MailAccount.LoadByAccountID(1);
						}
					}
				}

				message.To.Add(
					new NetSteps.Data.Entities.Mail.MailMessageRecipient(
						string.IsNullOrEmpty(hostEmailAddress) ? host.AccountInfo.EmailAddress : hostEmailAddress));
				if (MailMessage.SetReplyToEmailAddress())
				{
					message.ReplyToAddress = host.AccountInfo.EmailAddress;
				}
				message.Send(_corporateMailAccount, CurrentSite.SiteID);
			}
		}

		#endregion

		#region Address

		[PartySetup]
		public virtual ActionResult FormatAddress(string firstName, string lastName, string attention, string address1, string address2, string address3, string zip, string city, string county, string state, int country)
		{
			try
			{
				var address = new Address()
				{
					Attention = attention,
					Address1 = address1,
					Address2 = address2,
					Address3 = address3,
					PostalCode = zip,
					City = city,
					County = county,
					CountryID = country,
					FirstName = firstName,
					LastName = lastName
				};
				address.SetState(state, country);

				return Json(new
				{
					result = true,
					address = address.ToDisplay(false, true, true)
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		protected Address GetDefaultAddress(NetSteps.Data.Entities.Account account)
		{
			Address address = null;

			if (account.Addresses.Count > 0)
			{
				address = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Hostess);

				if (address == null)
				{
					address = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);
				}
				if (address == null)
				{
					address = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
				}
				if (address == null)
				{
					address = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Billing);
				}
				if (address == null)
				{
					address = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Autoship);
				}
				if (address == null)
				{
					address = account.Addresses[0];
				}
			}

			if (address == null)
			{
				address = new Address();
			}

			return address;
		}

		protected int GetDefaultCountryID(NetSteps.Data.Entities.Account account)
		{
			var address = GetDefaultAddress(account);
			int countryID;
			if (address != null && address.CountryID > 0)
			{
				countryID = address.CountryID;
			}
			else
			{
				countryID = (int)Constants.Country.UnitedStates;
			}

			return countryID;
		}

		#endregion

		#region DynamicKits

		public ActionResult CreateDynamicBundleUpSale(int productId, string orderCustomerGuid)
		{
			if (CurrentParty == null)
			{
				return Json(new
				{
					result = false,
					guid = string.Empty,
					message = Translation.GetTerm("PartyDoesNotExist", "The party does not exist.")
				});
			}

			Order order = CurrentParty.Order;

			var customer = order.OrderCustomers.GetByGuid(orderCustomerGuid);

			if (customer == null)
			{
				return Json(new
				{
					result = false,
					guid = string.Empty,
					message = Translation.GetTerm("InvalidCustomer", "Invalid Customer")
				});
			}

			Order clonedOrder = order.Clone();

			try
			{
				var product = Inventory.GetProduct(productId);
				string kitGuid = ConvertToDynamicKit(product, customer);

				if (kitGuid.IsNullOrEmpty())
				{
					return Json(new
					{
						result = false,
						guid = kitGuid,
						message = Translation.GetTerm("BundleCouldNotBeCreated", "The bundle could not be created.  Please try again.")
					});
				}

				return Json(new
				{
					result = true,
					guid = kitGuid,
					orderCustomerId = customer.OrderCustomerID
				});
			}
			catch (Exception ex)
			{
				//something went wrong in conversion to kit so revert
				CurrentParty.Order = clonedOrder;

				NetStepsException exception;
				if (!(ex is NetStepsBusinessException))
					exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				else
					exception = ex as NetStepsException;
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}



		[NonAction]
		private string ConvertToDynamicKit(Product kitProduct, OrderCustomer customer)
		{
			if (CurrentParty == null)
			{
				return string.Empty;
			}
			OrderContext.Order = CurrentParty.Order;

			if (kitProduct == null)
				return string.Empty;

			var parentGuid = OrderContext.Order.AsOrder().ConvertToDynamicKit(customer, kitProduct);
			var service = Create.New<IOrderService>();
			service.UpdateOrder(OrderContext);

			return parentGuid;
		}

		#endregion

		#region EvitesStats
		[PartySetup]
		[OutputCache(CacheProfile = "PagedGridData")]
		public virtual ActionResult EvitesStats(int partyId)
		{
			return PartialView(Party.LoadFull(partyId));
		}

		[PartySetup]
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult ResendHostInvitation(int partyId, string hostEmailAddress)
		{
			try
			{
				Party party = Party.LoadFull(partyId);

				SendHostInvitation(party, hostEmailAddress);
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		#endregion

		protected override ActionResult RedirectToSafePage()
		{
			CurrentParty = null;
			return base.RedirectToSafePage();
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (CurrentParty == null
				&& (filterContext.ActionDescriptor.GetCustomAttributes(typeof(PartySetupAttribute), false) == null
					|| filterContext.ActionDescriptor.GetCustomAttributes(typeof(PartySetupAttribute), false).Length == 0))
			{
				filterContext.Result = RedirectToAction("Index", "OrderHistory");
				return;
			}
			base.OnActionExecuting(filterContext);
		}

		[PartySetup]
		public override ActionResult NewOrder()
		{
			CurrentParty = null;
			return base.NewOrder();
		}
	}
}
