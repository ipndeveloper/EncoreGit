using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.OrderPackages;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Security;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Controls.Models;
using nsDistributor.Models.Account;
using NetSteps.Commissions.Common;
using NetSteps.Data.Entities.Business.Logic;

namespace nsDistributor.Controllers
{
	public class AccountController : BaseController
	{
		private ICommissionsService _commissionsService = Create.New<ICommissionsService>();
		private IProductCreditLedgerService _productCreditLedgerService = Create.New<IProductCreditLedgerService>();

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!IsLoggedIn && filterContext.ActionDescriptor.ActionName != "GetAddressControl")
			{
				if (Request.IsAjaxRequest())
				{
					filterContext.Result = Json(new { result = false, message = Translation.GetTerm("SessionTimedOut", "Your session has timed out.") });
				}
				else
				{
					filterContext.Result = Redirect("~/Login?ReturnUrl=Account");
				}
				return;
			}
			base.OnActionExecuting(filterContext);
		}

		protected virtual bool IsCurrentAccountNull()
		{
			return CoreContext.CurrentAccount.IsNull();
		}

		public virtual ActionResult Index()
		{
			var account = GetCurrentAccount();
            if (Convert.ToInt32(Session["CategoryId"]) > 0)
            {
                //Shop/Category/36
                int Id = Convert.ToInt32(Session["CategoryId"]);
                return Redirect("~/Shop/Category/" + Id);
            }
			// OnActionExecuting should prevent account from being null, but we should
			// check it anyway in case OnActionExecuting is ever changed / overridden.
			if (IsCurrentAccountNull())
			{
				return Redirect("~/Login?ReturnUrl=Account");
			}

			// TODO: Make a faster query for this.
			bool hasAutoshipOrders = AutoshipOrder
				.LoadAllFullByAccountID(account.AccountID)
				.Any(a =>
					a.Order.OrderStatusID == (int)Constants.OrderStatus.Paid
					&& SmallCollectionCache.Instance.AutoshipSchedules.GetById(a.AutoshipScheduleID).AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.Normal);

			bool showUpgrade = GetShowUpgrade(account);
			string upgradeUrl = GetUpgradeUrl(account);

			string accountType = SmallCollectionCache.Instance.AccountTypes.GetById(account.AccountTypeID).GetTerm();
			bool showUsername = GetAuthUIService().GetConfiguration().ShowUsernameFormFields;

			var model = new AccountIndexViewModel()
				.LoadResources(
					hasAutoshipOrders,
					showUpgrade,
					upgradeUrl,
					accountType,
					showUsername
				);

			return View(model);
		}

		[OutputCache(CacheProfile = "PagedGridData")]
		public virtual ActionResult GetOrders(int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection)
		{
			try
			{
				var cache = Create.New<IOrderSearchCache>();
				var orders = cache.Search(new OrderSearchParameters()
				{
					PageIndex = page,
					PageSize = pageSize,
					OrderBy = orderBy,
					OrderByDirection = orderByDirection,
					CustomerAccountID = CoreContext.CurrentAccount.AccountID
				});

				StringBuilder builder = new StringBuilder();
				int count = 0;
				bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
				var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
				var distributor = isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1));
				foreach (var order in orders)
				{
					var trackingLinks = string.Join(Environment.NewLine, AllTrackingNumberUrlLinksForOrder(order));

					builder.Append("<tr class=\"GridRow").Append(count % 2 == 0 ? "" : " Alt").Append("\">")
						.AppendLinkCell("~" + distributor + "/Account/OrderReceipt/" + order.OrderID, order.OrderNumber) //CGI(CMR)-17/10/2014
						.AppendCell(order.OrderStatus)
						.AppendLinkCell("~" + distributor + "/Cart?OrderId=" + order.OrderID, order.OrderStatusID == (int)Constants.OrderStatus.Pending ? "Resume Order" : " ", "ResumeOrderLink")
						.AppendCell(order.CompleteDate.HasValue ? order.CompleteDate.Value.ToString(BaseController.CurrentCulture) : "N/A");
					if (!string.IsNullOrEmpty(order.TrackingNumber))
						//builder.AppendLinkCell(GetTrackingUrl(order.ShippingMethodId, order.TrackingNumber), order.TrackingNumber, target: "_blank");
                        builder.AppendCell(trackingLinks);
					else
						builder.AppendCell("N/A");
					builder.AppendCell(order.GrandTotal.ToString(order.CurrencyID))
						.Append("</tr>");
					++count;
				}

				return Json(new { result = true, totalPages = orders.TotalPages, page = orders.TotalCount == 0 ? "<tr><td colspan=\"4\">" + Translation.GetTerm("NoOrdersPlaced", "You have not placed any orders.") + "</td></tr>" : builder.ToString() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		protected virtual IEnumerable<string> AllTrackingNumberUrlLinksForOrder(OrderSearchData order)
		{
			var packageHelper = Create.New<IOrderPackageInfoHelper>();

			var orderPackageInfoModel = packageHelper.GetOrderPackageInfoList(order.OrderID);

			return orderPackageInfoModel.Select(p => TrackingNumberUrlLink(p)).ToArray();
		}

		private static string TrackingNumberUrlLink(OrderPackageInfoModel package)
		{
			string href = !String.IsNullOrWhiteSpace(package.TrackingUrl.ToCleanString())
							 ? package.TrackingUrl.ToCleanString()
							 : string.Format(package.BaseTrackUrl.ToCleanString(), package.TrackingNumber.ToCleanString());
            //CGI(CMR)-16/10/2014-Inicio
            href = ConfigurationManager.AppSettings["TrackingUrl"];
            href = !String.IsNullOrWhiteSpace(href) ? href.Replace("{Number}", package.TrackingNumber).Replace("#", "&") : string.Empty;
            //CGI(CMR)-16/10/2014-Fin

			if (!String.IsNullOrWhiteSpace(href))
			{
				TagBuilder tag = new TagBuilder("a") { InnerHtml = package.TrackingNumber };
				tag.MergeAttribute("target", "_blank");
				tag.MergeAttribute("rel", "external");
				tag.MergeAttribute("href", href);
				return tag.ToString();
			}

			return string.Empty;
		}

		public virtual ActionResult SaveSettings(string firstName, string lastName, string email, string username)
		{
			try
			{
				var account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
				if (Account.AccountExists(email, account.EmailAddress))
				{
					return Json(new { result = false, message = Translation.GetTerm("EmailInUse", "That email address is already in use by another account.") });
				}
				account.FirstName = firstName;
				account.LastName = lastName;
				account.EmailAddress = email;
				if (username != null) //if(GetAuthUIService().GetConfiguration().LoginCredentialType == LoginCredentialTypes.Username)
				{
					account.User.Username = username;
				}
				account.Save();
				CoreContext.CurrentAccount = account;

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
		{
			try
			{
				var account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
				if (SimpleHash.VerifyHash(oldPassword, SimpleHash.Algorithm.SHA512, account.User.PasswordHash))
				{
					var response = NetSteps.Data.Entities.User.NewPasswordIsValid(newPassword, confirmPassword);
					if (response.Success)
					{
						account.User.Password = newPassword;
						account.Save();
						CoreContext.CurrentAccount = account;
					}
					else
					{
						return Json(new { result = false, message = response.Message });
					}
				}
				else
				{
					return Json(new { result = false, message = Translation.GetTerm("IncorrectPassword", "That password is incorrect, please try again.") });
				}

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		#region Shipping and Billing Profiles
		public virtual ActionResult ShippingAndBillingProfiles()
		{
			return View(CoreContext.CurrentAccount);
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult GetAddressControl(int? countryId, int? addressId, string prefix, bool showCountrySelect = true)
		{
			try
			{
				Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
				bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
				var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
				var distributor = isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1));
				Address address = addressId.HasValue && addressId.Value > 0 ? Address.Load(addressId.Value) : new Address();
				AddressModel model = new AddressModel()
				{
					Address = address,
					LanguageID = CoreContext.CurrentLanguageID,
					ShowCountrySelect = showCountrySelect,
					Country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == countryId),
					ChangeCountryURL = "~" + distributor + "/Account/GetAddressControl",
					Prefix = prefix
				};

				if (countryId != null)
					model.Country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == countryId);
				else if (addressId != null)
					model.Country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == address.CountryID);

				return PartialView("Address", model);
				//return Content(RenderPartialToString("Address", model));
				//return Content(AddressControl.RenderAddress(address, CoreContext.CurrentLanguageID, SmallCollectionCache.Instance.Countries.First(c => c.CountryID == countryId)));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		#region Addresses
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult EditShippingProfile(int? id)
		{
			try
			{
				return View(id.HasValue ? CoreContext.CurrentAccount.Addresses.GetByAddressID(id.Value) : new Address());
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		public virtual ActionResult SetDefaultAddress(int addressId)
		{
			try
			{
				Account account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
				account.StartEntityTracking();

				Address address = account.Addresses.FirstOrDefault(a => a.AddressID == addressId);
				if (address == null)
					throw EntityExceptionHelper.GetAndLogNetStepsException("That address is not associated with the current account.");
				if (!address.IsDefault)
				{
					address.IsDefault = true;
				}

				foreach (var addy in account.Addresses.GetAllByTypeID((Constants.AddressType)address.AddressTypeID).Where(a => a.IsDefault && a.AddressID != addressId))
				{
					addy.IsDefault = false;
				}

				account.Save();
				CoreContext.CurrentAccount = account;

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public virtual ActionResult SaveAddress(int? addressId, string attention, string address1, string address2,
			string address3, string postalCode, string city, string state, string county, int countryId, string profileName, string street)
		{
			try
			{
				Address address = null;
				var account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
				account.StartEntityTracking();
				if (addressId.HasValue && addressId > 0)
				{
					address = account.Addresses.GetByAddressID(addressId.Value);
				}
				else
				{
					address = new Address();
					account.Addresses.Add(address);
				}

				address.StartEntityTracking();
				address.AttachAddressChangedCheck();
				address.ProfileName = profileName.ToCleanString();
				address.Attention = attention.ToCleanString();
				address.Address1 = address1.ToCleanString();
				address.Address2 = address2.ToCleanString();
				address.Address3 = address3.ToCleanString();
				address.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Shipping.ToShort();
				address.City = city.ToCleanString();
				//address.StateProvinceID = (state.IsValidInt() && state.ToInt() > 0) ? state.ToInt() : (int?)null;
                //address.State = (state.IsValidInt() && state.ToInt() > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(state.ToInt()).StateAbbreviation : state.ToCleanString();
                address.SetState(state, countryId);
				address.County = county.ToCleanString();
				address.PostalCode = postalCode.ToCleanString();
				address.CountryID = countryId;
				address.LookUpAndSetGeoCode();
                address.Street = street;

				address.Validate();
				if (!address.IsValid)
				{
					return Json(new
					{
						result = false,
						message = address.GetValidationErrorMessage()
					});
				}
				var result = address.ValidateAddressAccuracy();
				if (!result.Success)
					return Json(new { result = false, message = result.Message });

				//account.StopEntityTracking();
				//account.OrderCustomers.Clear();
				//account.StartEntityTracking();

				account.Save();

                UpdateAddressStreet(account, street, address.AddressID);

				CoreContext.CurrentAccount = account;

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult DeleteAddress(int addressId)
		{
			try
			{
				var account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
				if (account.Addresses.Any(a => a.AddressID == addressId))
				{
					account.StartEntityTracking();
					var address = account.Addresses.First(a => a.AddressID == addressId);
					account.Addresses.RemoveAndMarkAsDeleted(address);
					account.Save();
					CoreContext.CurrentAccount = account;
					return Json(new { result = true });
				}
				else
					return Json(new { result = false, message = Translation.GetTerm("AddressNotFoundOnCurrentAccount", "Address not found on current Account.") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region Payment Methods
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult EditBillingProfile(int? id)
		{
			try
			{
				AccountPaymentMethod accountPaymentMethod = null;
				Account account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
				if (id.HasValue && id.Value > 0)
					accountPaymentMethod = account.AccountPaymentMethods.GetByAccountPaymentMethodID(id.Value);
				else
					accountPaymentMethod = new AccountPaymentMethod();

				if (accountPaymentMethod.BillingAddress == null)
					accountPaymentMethod.BillingAddress = new Address()
					{
						AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort()
					};

				CoreContext.CurrentAccount = account;

				return View(accountPaymentMethod);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult SetDefaultPaymentMethod(int paymentMethodId)
		{
			try
			{
				var account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
				account.StartEntityTracking();

				foreach (var pm in account.AccountPaymentMethods)
				{
					pm.IsDefault = pm.AccountPaymentMethodID == paymentMethodId ? true : false;
				}

				account.Save();
				CoreContext.CurrentAccount = account;

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpPost]
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult SavePaymentMethodEFT(int? paymentMethodId, string nameOnCard, string bankName, string accountNumber, string routingNumber, short bankAccountTypeID, bool? useDefaultShippingAddress,
            string attention, string address1, string address2, string address3, string zip, string city, string county, string state, int? countryId, string profilename, string street)
		{
			try
			{
				Account account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
				account.StartEntityTracking();
				AccountPaymentMethod paymentMethod = null;
				if (paymentMethodId.HasValue && paymentMethodId.Value > 0)
				{
					paymentMethod = account.AccountPaymentMethods.First(a => a.AccountPaymentMethodID == paymentMethodId.Value);
				}
				else
				{
					paymentMethod = new AccountPaymentMethod();
					account.AccountPaymentMethods.Add(paymentMethod);
				}
				paymentMethod.StartEntityTracking();

				paymentMethod.BankName = bankName;
				//Set paymentMethod information.
				paymentMethod.PaymentTypeID = (int)NetSteps.Data.Entities.Constants.PaymentType.EFT;
				if (!accountNumber.Contains("*"))
					paymentMethod.DecryptedAccountNumber = accountNumber.RemoveNonNumericCharacters();
				paymentMethod.NameOnCard = nameOnCard.ToCleanString();
				paymentMethod.RoutingNumber = routingNumber.ToCleanString();
				paymentMethod.ProfileName = profilename.ToCleanString();
				paymentMethod.BankAccountTypeID = bankAccountTypeID;

				Address billingAddress = null;
				if (useDefaultShippingAddress.HasValue && useDefaultShippingAddress.Value)
				{
					var defaultShippingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
					billingAddress = new Address();
					Address.CopyPropertiesTo(defaultShippingAddress, billingAddress);
					billingAddress.AddressTypeID = (int)Constants.AddressType.Billing;

					paymentMethod.BillingAddress = billingAddress;
					account.Addresses.Add(billingAddress);
				}
				else
				{
					if (paymentMethod.BillingAddressID.HasValue && paymentMethod.BillingAddressID > 0)
					{
						billingAddress = account.Addresses.GetByAddressID(paymentMethod.BillingAddressID.Value);
					}
					else
					{
						billingAddress = new Address();
						account.Addresses.Add(billingAddress);

						if (paymentMethod.BillingAddress != null && paymentMethod.BillingAddress.AddressID == 0)
							paymentMethod.BillingAddress = null;
					}

					billingAddress.StartEntityTracking();
					billingAddress.AttachAddressChangedCheck();
					billingAddress.ProfileName = profilename.ToCleanString();
					billingAddress.Attention = attention.ToCleanString();
					billingAddress.Address1 = address1.ToCleanString();
					billingAddress.Address2 = address2.ToCleanString();
					billingAddress.Address3 = address3.ToCleanString();
					billingAddress.City = city.ToCleanString();
                    billingAddress.County = county.ToCleanString();
					//billingAddress.StateProvinceID = (state.IsValidInt() && state.ToInt() > 0) ? state.ToInt() : (int?)null;
					//billingAddress.State = (state.IsValidInt() && state.ToInt() > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(state.ToInt()).StateAbbreviation : state.ToCleanString();
                    billingAddress.SetState(state, Convert.ToInt32(countryId));
					billingAddress.PostalCode = zip.ToCleanString();
					billingAddress.CountryID = countryId.Value;
					billingAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort();
                    billingAddress.Street = street;
					billingAddress.LookUpAndSetGeoCode();
					billingAddress.Validate();
					if (!billingAddress.IsValid)
					{
						return Json(new
						{
							result = false,
							message = billingAddress.GetValidationErrorMessage()
						});
					}
					var result = billingAddress.ValidateAddressAccuracy();
					if (!result.Success)
						return Json(new { result = false, message = result.Message });

					paymentMethod.BillingAddress = billingAddress;
				}

				account.Save();

                UpdateAddressStreet(account, street, billingAddress.AddressID);

				CoreContext.CurrentAccount = account;

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpPost]
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult SavePaymentMethod(int? paymentMethodId, string nameOnCard, string accountNumber, DateTime expDate, bool? useDefaultShippingAddress,
            string attention, string address1, string address2, string address3, string zip, string city, string county, string state, int? countryId, string profilename, string street)
		{
			try
			{
				Account account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
				account.StartEntityTracking();
				AccountPaymentMethod paymentMethod = null;
				if (paymentMethodId.HasValue && paymentMethodId.Value > 0)
				{
					paymentMethod = account.AccountPaymentMethods.First(a => a.AccountPaymentMethodID == paymentMethodId.Value);
				}
				else
				{
					paymentMethod = new AccountPaymentMethod();
					account.AccountPaymentMethods.Add(paymentMethod);
				}
				paymentMethod.StartEntityTracking();

				paymentMethod.ProfileName = profilename.ToCleanString();
				paymentMethod.PaymentTypeID = (int)NetSteps.Data.Entities.Constants.PaymentType.CreditCard;
				if (!accountNumber.Contains("*"))
					paymentMethod.DecryptedAccountNumber = accountNumber.RemoveNonNumericCharacters();
				paymentMethod.NameOnCard = nameOnCard.ToCleanString();
				paymentMethod.ExpirationDateUTC = expDate.LastDayOfMonth();

				Address billingAddress = null;
				if (useDefaultShippingAddress.HasValue && useDefaultShippingAddress.Value)
				{
					var defaultShippingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
					billingAddress = new Address();
					Address.CopyPropertiesTo(defaultShippingAddress, billingAddress);
					billingAddress.AddressTypeID = (int)Constants.AddressType.Billing;

					paymentMethod.BillingAddress = billingAddress;
					account.Addresses.Add(billingAddress);
				}
				else
				{
					if (paymentMethod.BillingAddressID.HasValue && paymentMethod.BillingAddressID > 0)
					{
						billingAddress = account.Addresses.GetByAddressID(paymentMethod.BillingAddressID.Value);
					}
					else
					{
						billingAddress = new Address();
						account.Addresses.Add(billingAddress);

						if (paymentMethod.BillingAddress != null && paymentMethod.BillingAddress.AddressID == 0)
							paymentMethod.BillingAddress = null;
					}

					billingAddress.StartEntityTracking();
					billingAddress.AttachAddressChangedCheck();
					billingAddress.ProfileName = profilename.ToCleanString();
					billingAddress.Attention = attention.ToCleanString();
					billingAddress.Address1 = address1.ToCleanString();
					billingAddress.Address2 = address2.ToCleanString();
					billingAddress.Address3 = address3.ToCleanString();
					billingAddress.City = city.ToCleanString();
                    billingAddress.County = county.ToCleanString();
					//billingAddress.StateProvinceID = (state.IsValidInt() && state.ToInt() > 0) ? state.ToInt() : (int?)null;
					//billingAddress.State = (state.IsValidInt() && state.ToInt() > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(state.ToInt()).StateAbbreviation : state.ToCleanString();
                    billingAddress.SetState(state, Convert.ToInt32(countryId));
					billingAddress.PostalCode = zip.ToCleanString();
					billingAddress.CountryID = countryId.Value;
					billingAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort();
                    billingAddress.Street = street;
					billingAddress.LookUpAndSetGeoCode();
					billingAddress.Validate();
					if (!billingAddress.IsValid)
					{
						return Json(new
						{
							result = false,
							message = billingAddress.GetValidationErrorMessage()
						});
					}
					var result = billingAddress.ValidateAddressAccuracy();
					if (!result.Success)
						return Json(new { result = false, message = result.Message });

					paymentMethod.BillingAddress = billingAddress;
				}

				//account.StopEntityTracking();
				//account.OrderCustomers.Clear();
				//account.StartEntityTracking();

				account.Save();

                UpdateAddressStreet(account, street, billingAddress.AddressID);

				CoreContext.CurrentAccount = account;

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

        private void UpdateAddressStreet(Account account, string street, int addressID)
        {
            Address direccion = new Address();
            direccion = account.Addresses.Where(donde => donde.AddressID == addressID).FirstOrDefault();
            if (direccion != null)
            {
                direccion.Street = street;
                AddressBusinessLogic business = new AddressBusinessLogic();
                business.UpdateAddressStreet(direccion);
            }
        }

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult DeletePaymentMethod(int paymentMethodId)
		{
			try
			{
				var account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
				if (account.AccountPaymentMethods.Any(a => a.AccountPaymentMethodID == paymentMethodId))
				{
					account.DeleteAccountPaymentMethod(paymentMethodId);
					CoreContext.CurrentAccount = account;
					return Json(new { result = true });
				}
				else
					return Json(new { result = false, message = Translation.GetTerm("PaymentmethodNotFoundOnCurrentAccount", "Payment Method not found on current Account.") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion
		#endregion

		public virtual ActionResult OrderReceipt(int id)
		{
			var order = Order.LoadFull(id);
			if (!order.OrderCustomers.Any(c => c.AccountID == CoreContext.CurrentAccount.AccountID))
				return RedirectToAction("Index");
			ViewBag.CartModel = new CheckoutController().GetCartModelData(order); //this should be pulled in to a UI service somewhere...
			return View("Receipt", new nsDistributor.Models.Checkout.CheckoutReceiptModel() { CartModel = ViewBag.CartModel, Order = order, ContinueShopping = false });
		}

		public virtual ActionResult Gallery()
		{
			return View("UserGallery/Gallery", CoreContext.CurrentAccount.FileResources.ToList());
		}

		public virtual ActionResult UploadImage(string qqFile)
		{
			if (qqFile == null)
			{
				return Content("{success:false}");
			}
			return UploadImage(qqFile, null);
		}

		public virtual ActionResult UploadImageIE(HttpPostedFileWrapper qqFile)
		{
			return UploadImage(null, qqFile);
		}

		protected virtual ActionResult UploadImage(string qqFile, HttpPostedFileWrapper wrapper)
		{
			Account account = GetCurrentAccount();
			account.RemoveAllOrdersFromObjectGraph();
			string accountPart = "Accounts\\" + account.AccountID;
			string thePath = ConfigurationManager.GetAbsoluteFolder(accountPart);
			FileInfo info;
			if (wrapper != null)
			{
				info = GetNonDuplicateFilePath(thePath, Path.GetFileName(wrapper.FileName));
				wrapper.SaveAs(info.FullName);
			}
			else
			{
				info = GetNonDuplicateFilePath(thePath, qqFile);
				Request.SaveAs(info.FullName, false);
			}

			FileResource newImage = new FileResource
			{
				FileResourceTypeID = (int)Constants.FileResourceType.Image,
				FileResourcePath = ConfigurationManager.TOKEN + Path.Combine(accountPart, info.Name),
				ModifiedByUserID = account.UserID,
				DateCreatedUTC = DateTime.UtcNow,
			};
			account.FileResources.Add(newImage);
			account.Save();

			SetCurrentAccountFromAccount(account);
			return Content("{success:true}");
		}

		public virtual FileInfo GetNonDuplicateFilePath(string dir, string fileName)
		{
			const string pathFormat = "{0}{1}{2}";
			var fileInfo = new FileInfo(dir + fileName);
			var counter = 1;
			while (fileInfo.Exists)
			{
				var proposedFilePath = string.Format(pathFormat, dir + fileName.RemoveFileExtension(), counter, fileInfo.Extension);
				fileInfo = new FileInfo(proposedFilePath);
				counter++;
			}
			return fileInfo;
		}

		public virtual ActionResult DeleteImage(int resourceID)
		{
			Account account = GetCurrentAccount();
			try
			{
				FileResource resource = account.FileResources.FirstOrDefault(x => x.FileResourceID == resourceID);
				if (resource != null)
				{
					System.IO.File.Delete(resource.FileResourcePath.ReplaceFileUploadPathToken().WebUploadPathToAbsoluteUploadPath());
					account.FileResources.RemoveAndMarkAsDeleted(resource);
					account.Save();
				}
				SetCurrentAccountFromAccount(account);
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (account != null) ? account.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult GetImageHtml(string fileName)
		{
			var account = GetCurrentAccount();
			try
			{
				var source = account.FileResources.Where(x =>
					x.FileResourcePath.Contains(fileName.RemoveFileExtension()) && x.FileResourcePath.
					EndsWith(fileName.GetFileExtention())).OrderByDescending(x => x.FileResourceID);
				return PartialView("UserGallery/ImageLibraryItem", source.First());
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (account != null) ? account.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual Account GetCurrentAccount()
		{
			return CoreContext.CurrentAccount;
		}

		protected virtual void SetCurrentAccountFromAccount(Account account)
		{
			CoreContext.CurrentAccount = account;
		}

		#region Product Credit Ledger
		public virtual ActionResult ProductCredit()
		{
			return View();
		}

		public virtual ActionResult GetProductCredit(int page, int pageSize)
		{
			try
			{
				var accountLedgers = _productCreditLedgerService.RetrieveLedger(CoreContext.CurrentAccount.AccountID);
				var entries = accountLedgers.OrderByDescending(le => le.EffectiveDate).ThenByDescending(le => le.EntryId);
				if (entries.Count() > 0)
				{
					StringBuilder builder = new StringBuilder();

					// TODO: AccountLedger should have a CurrencyID for formatting currency, right? - JHE
					int count = 0;
					foreach (var entry in entries.Skip(page * pageSize).Take(pageSize))
					{
						// The commissions team removed the CurrencyTypes table from the commissions database.
						// The Ledger tables now point to the Core database's Currencies table. 5/21/2013
						var currency = SmallCollectionCache.Instance.Currencies.FirstOrDefault(c => c.CurrencyID == entry.CurrencyTypeId);

						builder.Append("<tr").Append(count % 2 == 1 ? " class=\"Alt\"" : "").Append(">")
							.AppendCell(entry.EntryDescription)
							.AppendCell(entry.EntryReason.TermName)
							.AppendCell(entry.EntryKind.TermName)
							.AppendCell(entry.EffectiveDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
							.AppendCell(entry.BonusTypeId.HasValue ? _commissionsService.GetBonusKind(entry.BonusTypeId.Value).Name : "")
							.AppendCell(entry.EntryAmount.ToString("C", currency.Culture))
							.AppendCell(entry.EndingBalance.ToDecimal().ToString("C", currency.Culture))
							.Append("</td></tr>");
						++count;
					}
					return Json(new { totalPages = Math.Ceiling(entries.Count() / (double)pageSize), page = builder.ToString() });
				}
				return Json(new { totalPages = 0, page = "<tr><td colspan=\"7\">" + Translation.GetTerm("NoLedgerEntries", "No ledger entries.") + "</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		/// <summary>
		/// Returns true if the given account can be upgraded.
		/// </summary>
		protected virtual bool GetShowUpgrade(
			Account account)
		{
			return account.AccountTypeID != (short)Constants.AccountType.Distributor;
		}

		/// <summary>
		/// Returns true if the given account can be upgraded.
		/// </summary>
		protected virtual string GetUpgradeUrl(
			Account account)
		{
			string upgradeUrl = string.Empty;
			if (GetShowUpgrade(account))
			{

				if (account.AccountTypeID == (short)Constants.AccountType.PreferredCustomer)
				{
					upgradeUrl = "~/Enroll?upgrade=True&accountTypeID=" + (short)Constants.AccountType.Distributor;
				}
				else if (account.AccountTypeID == (short)Constants.AccountType.RetailCustomer)
				{
					upgradeUrl = "~/Enroll?upgrade=True";
				}
			}
			return upgradeUrl;
		}
	}
}
