using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DistributorBackOffice.Areas.Account.Models.Landing;
using DistributorBackOffice.Models.Home;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Security;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Business.Logic;

namespace DistributorBackOffice.Areas.Account.Controllers
{
	public class LandingController : BaseAccountsController
	{
		[FunctionFilter("Accounts", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult Index()
		{
			ViewData["EncAccount"] = HttpUtility.UrlEncode(Encryption.EncryptTripleDES(CurrentAccount.AccountNumber, Encryption.SingleSignOnSalt));

			Func<int, int, AutoshipOrder> autoship = (a, b) => Autoships.FirstOrDefault(x => x.AutoshipScheduleID == a && x.Order.OrderStatusID == b);

			AccountModel viewModel = new AccountModel();
			viewModel.AutoshipOrders = new List<AutoshipOrderViewModel>();

			viewModel.Account = CurrentAccount;

			foreach (var autoshipSchedule in Schedules.Where(a => a.Active == true))
			{
				var autoshipOrderModel = new AutoshipOrderViewModel(autoshipSchedule);

				var paidTemplate = autoship(autoshipSchedule.AutoshipScheduleID, (int)Constants.OrderStatus.Paid);

				if (paidTemplate != null)
				{
					autoshipOrderModel.AutoshipOrder = paidTemplate;
					autoshipOrderModel.IsValid = true;
					autoshipOrderModel.IsCanceled = false;
					autoshipOrderModel.OrderItems = paidTemplate.Order.OrderCustomers[0].OrderItems;
					autoshipOrderModel.Site = GetSite(paidTemplate, autoshipSchedule);
				}

				if (autoshipSchedule != null)
				{
					autoshipOrderModel.IsTemplateEditable = autoshipSchedule.IsTemplateEditable;
					autoshipOrderModel.IsEnrollable = autoshipSchedule.IsEnrollable;
				}

				viewModel.AutoshipOrders.Add(autoshipOrderModel);
			}
            Session["Edit"] = "3";
            viewModel.Sponsor = AccountSponsorBusinessLogic.Instance.GetSponsorBasicInfo(viewModel.Account.AccountID);

			return View(viewModel);
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult GetAddresses()
		{
			try
			{
				var shippingAddresses = CurrentAccount.Addresses.Where(a => a.AddressTypeID == Constants.AddressType.Shipping.ToInt()).OrderByDescending(a => a.IsDefault).ThenBy(a => a.AddressID).ToList();
				var defaultAddress = CurrentAccount.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Shipping);

				var defaultAddressText = Translation.GetTerm("N/A", "N/A");
				if (defaultAddress != null)
					defaultAddressText = string.Format("<a href=\"javascript:void(0);\" onclick=\"editAddressCustom({1});\">{0}</a>", defaultAddress.ToDisplay(), defaultAddress.AddressID);
				return Json(new { result = true, addresses = GetAddressData(shippingAddresses), defaultAddressHtml = defaultAddressText });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (OrderContext != null && OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		protected virtual List<object> GetAddressData(List<Address> addresses)
		{
			List<object> returnAddresses = new List<object>();
			foreach (var address in addresses)
			{
				string profileName = address.ProfileName + (address.IsDefault ? " (" + Translation.GetTerm("default") + ")" : "");
				if (string.IsNullOrEmpty(profileName))
					profileName = SmallCollectionCache.Instance.AddressTypes.GetById(address.AddressTypeID).GetTerm();

				returnAddresses.Add(new
				{
					addressId = address.AddressID,
					profileName = profileName,
					address1 = address.Address1,
					isDefault = address.IsDefault
				});
			}
			return returnAddresses;
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult GetPaymentMethods()
		{
			try
			{
				var accountPaymentMethods = CurrentAccount.AccountPaymentMethods.OrderByDescending(pm => pm.IsDefault).ToList();
				var defaultPaymentMethod = CurrentAccount.AccountPaymentMethods.FirstOrDefault(pm => pm.IsDefault);

				var defaultPaymentMethodText = Translation.GetTerm("N/A", "N/A");
				if (defaultPaymentMethod != null)
					defaultPaymentMethodText = string.Format("<a href=\"javascript:void(0);\" onclick=\"editPaymentMethodCustom({1});\">{0}</a>", defaultPaymentMethod.ToDisplay(CoreContext.CurrentCultureInfo), defaultPaymentMethod.AccountPaymentMethodID);
				return Json(new { result = true, paymentMethods = GetPaymentMethodsData(accountPaymentMethods), defaultPaymentMethodHtml = defaultPaymentMethodText });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (OrderContext != null && OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		protected virtual List<object> GetPaymentMethodsData(List<AccountPaymentMethod> accountPaymentMethods)
		{
			List<object> returnAddresses = new List<object>();
			foreach (var paymentMethod in accountPaymentMethods)
			{
				string profileName = paymentMethod.ProfileName + (paymentMethod.IsDefault ? " (" + Translation.GetTerm("default") + ")" : "");
				if (string.IsNullOrEmpty(profileName))
					profileName = SmallCollectionCache.Instance.PaymentTypes.GetById(paymentMethod.PaymentTypeID).GetTerm();

				returnAddresses.Add(new
				{
					accountPaymentMethodId = paymentMethod.AccountPaymentMethodID,
					profileName = profileName,
					address1 = paymentMethod.BillingAddress != null ? paymentMethod.BillingAddress.Address1 : string.Empty,
					isDefault = paymentMethod.IsDefault
				});
			}
			return returnAddresses;
		}

		[NonAction]
		public virtual Site GetSite(AutoshipOrder autoshipOrder, AutoshipSchedule schedule)
		{
			return schedule.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.SiteSubscription
					   ? Site.LoadByAutoshipOrderID(autoshipOrder.AutoshipOrderID)
					   : null;
		}
	}
}
