using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Orders.Helpers;
using DistributorBackOffice.Areas.Orders.Models.Fundraisers;
using DistributorBackOffice.Models;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls;
using NetSteps.Web.Mvc.Helpers;


namespace DistributorBackOffice.Areas.Orders.Controllers
{
	public class FundRaisersController : AbstractPartyAndFundraiserController
	{
		public IOrderTypeRepository OrderTypeRepository { get { return Create.New<IOrderTypeRepository>(); } }

		[PartySetup]
		public override ActionResult Index(int? partyId, string orderCustomerGuid = null)
		{
			var currentAction = DetermineStep(partyId);
			return currentAction;
		}

		[PartySetup]
		public ActionResult Setup(int? id, Guid? orderCustomerGuid = null)
		{
			var defaultCountryID = GetDefaultCountryID(CurrentAccount);
			var model = InitializeDefaultsForSetupModel(defaultCountryID);
			model = SetupCurrentParty(id, model);

			if (!CurrentAccountCanAccessParty(CurrentParty)) return this.RedirectToSafePage();

			if (CurrentParty.OrderID > 0)
			{
				if (orderCustomerGuid.HasValue)
				{
					var customer = CurrentParty.Order.OrderCustomers.FirstOrDefault(oc => oc.Guid == orderCustomerGuid.Value);
					model.HostAccount = NetSteps.Data.Entities.Account.LoadFull(customer.AccountID);
					model.Host = new OrderCustomer(model.HostAccount);

					var hostAddresses = Address.LoadByAccountId(model.HostAccount.AccountID);
					model.HostAddress = GetDefaultAddress(model.HostAccount).ToBasicAddressModel();

					var shippingAddress = model.HostAccount.Addresses.FirstOrDefault(a => a.AddressTypeID == Constants.AddressType.Shipping.ToShort());
					if (shippingAddress != null)
						model.ShippingAddress = shippingAddress.ToBasicAddressModel();

					model.ShipTo = PartyShipTo.Host;
				}
				else
				{
					var defaultShipment = CurrentParty.Order.GetDefaultShipment();
					var host = CurrentParty.Order.GetHostess();

					if (host != null)
					{
						model.HostAccount = NetSteps.Data.Entities.Account.LoadFull(host.AccountID);
						model.HostAddress = GetDefaultAddress(model.HostAccount).ToBasicAddressModel();

						model.AccountId = model.HostAccount.AccountID;
						model.FirstName = model.HostAccount.FirstName;
						model.LastName = model.HostAccount.LastName;
						model.Email = model.HostAccount.EmailAddress;

						model.PhoneNumber = new PhoneNumber(model.HostAccount.MainPhone);
					}

					model.Party = CurrentParty;
					model.PartyAddress = (CurrentParty != null && CurrentParty.Address != null) ? CurrentParty.Address.ToBasicAddressModel() : null;

					model.IsFundraiserAtHosts = CurrentParty.Address.IsEqualTo(model.HostAddress.ToAddress(), includeFirstAndLastNames: false);

					if (defaultShipment.IsEqualTo(model.HostAddress.ToAddress(), includeFirstAndLastNames: false))
					{
						model.ShipTo = PartyShipTo.Host;
					}
				}
			}

			return View(model);
		}

		[HttpPost, PartySetup]
		public ActionResult Setup(int? id, SetupModel model)
		{
			var party = id.HasValue ? Party.LoadFull(id.Value) : new Party();

			if (!CurrentAccountCanAccessParty(party)) return this.RedirectToSafePage();

			var maxPartyDate = party.GetMaximumFuturePartyDate();
			var startDate = model.StartDate.AddTime(model.StartTime);
			if (startDate > maxPartyDate)
			{
				ModelState.AddModelError("", string.Format("Fundraiser date must be sooner than {0}", maxPartyDate));
			}

			if (party.Address == null)
			{
				party.Address = new Address();
			}

			Address.CopyPropertiesTo(model.HostAddress.ToAddress(), party.Address);
			party.Address.AddressTypeID = (int)Constants.AddressType.Party;
			party.Address.Validate();

			if (!party.Address.IsValid)
			{
				ModelState.AddModelError("", party.Address.GetValidationErrorMessage());
			}

			var result = party.Address.ValidateAddressAccuracy();
			if (!result.Success)
			{
				ModelState.AddModelError("PartyAddress", result.Message);
			}

			if (ModelState.IsValid)
			{
				party.StartTracking();
				party.StartDate = startDate;
				party.ShowOnPWS = false;
				party.Name = model.Party.Name;
				party.EviteOrganizerEmail = model.EvitesOrganizersEmail;

				var hostInviteEmailTemplate = GetHostInviteEmailTemplate();
				if (hostInviteEmailTemplate != null)
				{
					// Personalized Content
				}

				var order = party.Order ?? new Order() { DateCreated = DateTime.Now };
				order.StartEntityTracking();
				order.ConsultantID = CurrentAccount.AccountID;
				order.OrderTypeID = (short)Constants.OrderType.FundraiserOrder;
				order.OrderStatusID = (short)Constants.OrderStatus.Pending;
				order.CurrencyID = SmallCollectionCache.Instance.Countries.GetById(party.Address.CountryID).CurrencyID;

				NetSteps.Data.Entities.Account hostAccount;
				if (model.AccountId > 0 && NetSteps.Data.Entities.Account.NonProspectExists(model.Email))
				{
					hostAccount = NetSteps.Data.Entities.Account.LoadFull(model.AccountId);
				}
				else
				{
					var attemptedHostAccount = NetSteps.Data.Entities.Account.GetAccountByEmailAndSponsorID(model.Email, CurrentAccount.AccountID);
					if (attemptedHostAccount == null)
					{
						var countryForMarket = SmallCollectionCache.Instance.Countries.GetById(party.Address.CountryID);
						hostAccount = new NetSteps.Data.Entities.Account()
							 {
								 DateCreated = DateTime.Now,
								 DefaultLanguageID = CurrentAccount.DefaultLanguageID,
								 FirstName = model.FirstName,
								 LastName = model.LastName,
								 EmailAddress = model.Email,
								 SponsorID = CurrentAccount.AccountID,
								 MarketID = countryForMarket != null ? countryForMarket.MarketID : CurrentSite.MarketID
							 };
					}
					else
					{
						hostAccount = attemptedHostAccount;
					}

					hostAccount.StartTracking();
					hostAccount.MainPhone = model.PhoneNumber.ToString();
					hostAccount.AccountTypeID = (int)Constants.AccountType.RetailCustomer;
					hostAccount.Activate();
					hostAccount.Save();
				}

				var host = order.OrderCustomers.FirstOrDefault(oc => oc.AccountID == hostAccount.AccountID) ?? order.AddNewCustomer(hostAccount);
				order.SetHostess(host);

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

				Address.CopyPropertiesTo(model.HostAddress.ToAddress(), hostAddress);
				hostAddress.AddressTypeID = (int)Constants.AddressType.Hostess;
				hostAddress.LookUpAndSetGeoCode();
				hostAddress.Validate();

				result = hostAddress.ValidateAddressAccuracy();
				if (!result.Success)
				{
					ModelState.AddModelError("HostAddress", result.Message);
				}

				if (!hostAddress.IsValid)
				{
					ModelState.AddModelError("HostAddress", hostAddress.GetValidationErrorMessage());
				}

				var orderType = OrderTypeRepository.FirstOrDefault(ot => ot.Name.Contains("FundRaiser"));
				if (orderType != null)
				{
					order.OrderTypeID = orderType.OrderTypeID;
				}
				else
				{
					ModelState.AddModelError("", "Fundraiser Order Type doesn't exist as an Order Type.");
				}

				if (!ModelState.IsValid)
				{
					return View(model);
				}

				hostAccount.Orders.Clear();

				if (model.ShippingAddressId.HasValue)
				{
					var shippingAddress = Address.LoadFull(model.ShippingAddressId.Value);
					order.UpdateOrderShipmentAddressAndDefaultShipping(shippingAddress);
				}
				else
				{
					order.UpdateOrderShipmentAddressAndDefaultShipping(model.ShippingAddress.ToAddress());
				}

				if (!ModelState.IsValid)
				{
					return View(model);
				}

				party.Order = order;

				party.Save();
				hostAccount.Save();

				CurrentParty = Party.LoadFull(party.PartyID);

				if (party.UseEvites)
				{
					SendHostInvitation(party, party.EviteOrganizerEmail);
				}

				return RedirectToAction("Cart");
			}

			return View(model);
		}

		[FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult SearchProducts(string query, bool includeDynamicKits = true, string param = "")
		{
			try
			{
				var catalogs = CatalogRepository.Where(c => c.CatalogTypeID == (short)Constants.CatalogType.Fundraiser).Select(c => c.CatalogID);

				var products = Inventory.SearchProducts(ApplicationContext.Instance.StoreFrontID, query, CurrentAccount.AccountTypeID, catalogs).AsEnumerable();
				products = Inventory.ExcludeInvalidProducts(products, accountTypeId: CurrentAccount.AccountTypeID).Where(p => !p.IsVariantTemplate);

				return Json(CustomFilterProducts(products, query, includeDynamicKits).Select(p => new
				{
					id = p.ProductID,
					text = p.SKU + " - " + p.Translations.Name(),
					isDynamicKit = p.IsDynamicKit(),
					needsBackOrderConfirmation = Inventory.IsOutOfStock(p) && p.ProductBackOrderBehaviorID == Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer.ToInt()
				}));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext != null && OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		protected virtual IEnumerable<Product> CustomFilterProducts(IEnumerable<Product> products, string query, bool includeDynamicKits = true)
		{
			return products;
		}

		IEnumerable<Product> FilterProductsByFundraiserCatalog(IEnumerable<Product> products)
		{
			IList<int> productIds = new List<int>();
			var catalogs = CatalogRepository.Where(c => c.CatalogTypeID == (short)Constants.CatalogType.Fundraiser);

			foreach (var catalog in catalogs.Select(c => Catalog.LoadFull(c.CatalogID)))
				productIds.AddRange(catalog.CatalogItems.Select(c => c.ProductID));

			return products.Where(c => productIds.Contains(c.ProductID));
		}

		SetupModel InitializeDefaultsForSetupModel(int countryID)
		{
			var model = new SetupModel
			{
				HostInviteEmailTemplate = GetHostInviteEmailTemplate()
			};
			model.HostAddress.CountryID = countryID;
			model.PartyAddress.CountryID = countryID;
			model.ShippingAddress.CountryID = countryID;

			return model;
		}

		SetupModel SetupCurrentParty(int? partyId, SetupModel model)
		{
			if (CurrentParty != null)
				if (!(partyId.HasValue && partyId.Value == 0))
					model.Party = CurrentParty;

			if (partyId.HasValue && partyId.Value > 0)
			{
				model.Party = Party.LoadFull(partyId.Value);
				CurrentParty = model.Party;
			}
			else
				CurrentParty = new Party();

			return model;
		}

		[PartySetup]
		public virtual ActionResult DetermineStep(int? partyId)
		{
			if (partyId.HasValue && partyId > 0)
			{
				CurrentParty = Party.LoadFull(partyId.Value);
				if (!CurrentAccountCanAccessParty(CurrentParty)) return this.RedirectToSafePage();
				OrderContext.Order = CurrentParty.Order;
				SetHasChangesToFalseOnAllItems();
				OrderService.UpdateOrder(OrderContext);
			}
			else
			{
				CurrentParty = null;
			}

			var actionResult = RedirectToAction("Setup", new { id = partyId });

			if (CurrentParty == null)
				return actionResult;

			//Check If Hostess Exists
			if (CurrentParty.Order.GetHostess() == null)
				return RedirectToAction("Setup");

			//the party was submitted - DES
			if (CurrentParty.Order.OrderStatusID != (int)Constants.OrderStatus.Pending && CurrentParty.Order.OrderStatusID != (int)Constants.OrderStatus.PartiallyPaid
					  && CurrentParty.Order.OrderStatusID != (int)Constants.OrderStatus.CreditCardDeclined)
				return RedirectToAction("Receipt");

			//Any payments means that we were on the payment step - DES
			if (CurrentParty.Order.OrderPayments.Count > 0 || CurrentParty.Order.OrderCustomers.Any(oc => oc.OrderPayments.Count > 0))
				return RedirectToAction("Payments");

			//Any order customers means that we were on the cart step - DES
			if (CurrentParty.Order.OrderCustomers.Count > 0)
				return RedirectToAction("Cart");

			return actionResult;
		}
	}
}
