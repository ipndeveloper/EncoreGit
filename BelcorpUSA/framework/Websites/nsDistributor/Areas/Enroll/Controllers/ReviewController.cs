using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using nsDistributor.Areas.Enroll.Models.Review;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Common.Context;

namespace nsDistributor.Areas.Enroll.Controllers
{
	public class ReviewController : EnrollStepBaseController
	{
		#region Actions
		public virtual ActionResult Index()
		{
			try
			{
				// Prevent autoship from being shown/charged if it was skipped.
				if (_enrollmentContext.SkipAutoshipOrder)
				{
					DeleteAutoshipOrder();
				}

				// Make sure all calculations are current.
				var shippingAddress = GetEnrollingAccount().Addresses
					 .GetDefaultByTypeID(Constants.AddressType.Shipping);

				// This calculates the totals for each of the orders.
				// For the love of all that is holy, don't call CalculateTotals again.
				UpdateAllOrderShipmentAddresses((Address)shippingAddress);

				var model = new IndexModel();

				Index_LoadResources(model);
				if(_enrollmentContext.InitialOrder != null)
				{
					_enrollmentContext.InitialOrder.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
				}
				if(_enrollmentContext.AutoshipOrder != null)
				{
					
				}

				return View(model);
			}
			catch (Exception ex)
			{
				AddErrorToTempData(ex.Log().PublicMessage);
				return RedirectToStep(_enrollmentContext.EnrollmentConfig.Steps.PreviousItem);
			}
		}

		[HttpPost, ValidateAntiForgeryToken]
		public virtual ActionResult Index(IndexModel model)
		{
			var account = (Account)_enrollmentContext.EnrollingAccount;
			var initialOrder = _enrollmentContext.InitialOrder.AsOrder();
			var autoshipOrder = (AutoshipOrder)_enrollmentContext.AutoshipOrder;
			var subscriptionAutoshipOrder = (AutoshipOrder)_enrollmentContext.SiteSubscriptionAutoshipOrder;

			account.AccountTypeID = (short)_enrollmentContext.AccountTypeID;

			// TODO: Lots of validation here!

			if (!ModelState.IsValid)
			{
				Index_LoadResources(model);
				return View(model);
			}

			// NOTE: Any changes to this method must be thoroughly code-reviewed.
			// Most enrollments will have an initial order that needs to succeed in order for the enrollment to be considered "complete".
			// Once the initial order is successfully submitted, that is the "POINT OF NO RETURN", which means no matter what happens,
			// we HAVE to proceed to the next step (receipt). Even if there are other orders being submitted (i.e. autoships), they cannot
			// prevent the user from going to the next step. Everything must be wrapped in a try/catch from this point on.

			// If charging for an autoship or subscription immediately, add those items to the initial order (to bill for everything at once)
			//
			// Some clients process the autoship or subscription immediately
			IEnumerable<dynamic> autoshipOrderItems = null;
			IEnumerable<dynamic> subscriptionOrderItems = null;
			if (_enrollmentContext.AddAutoshipToInitialOrder && autoshipOrder != null)
				CopyAutoshipItemsToInitialOrder(account, autoshipOrder, ref initialOrder, ref autoshipOrderItems);

			if (_enrollmentContext.AddSubscriptionToInitialOrder && subscriptionAutoshipOrder != null)
				CopyAutoshipItemsToInitialOrder(account, subscriptionAutoshipOrder, ref initialOrder, ref subscriptionOrderItems);

			// Submit initial order
			try
			{
				if (initialOrder != null)
				{
					SubmitInitialOrder(initialOrder, account);
				}
			}
			catch (Exception ex)
			{
				AddErrorToViewData(string.Format("{0} {1}", _errorSubmittingInitialOrderString, ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage));

				bool shouldCalculateTotals = false;

				// Remove orderitems pulled from autoships and subscriptions - they will be re-added if they try to submit again
				if (autoshipOrderItems != null)
				{
					RemoveAutoshipItemsFromInitialOrder(initialOrder, autoshipOrderItems);
					shouldCalculateTotals = true;
				}

				if (subscriptionOrderItems != null)
				{
					RemoveAutoshipItemsFromInitialOrder(initialOrder, subscriptionOrderItems);
					shouldCalculateTotals = true;
				}

				if (shouldCalculateTotals && initialOrder != null)
				{
					initialOrder = this.TotalOrder(initialOrder);
				}

				if (initialOrder != null && (!initialOrder.OrderCustomers.Any() || !initialOrder.OrderCustomers[0].OrderItems.Any()))
					_enrollmentContext.InitialOrder = null;

				Index_LoadResources(model);
				return View(model);
			}

			// POINT OF NO RETURN
			// Initial order has been submitted. No matter what errors occur, we have to go to the next step.

			try
			{
				_enrollmentContext.EnrollmentComplete = true;

				this.IncrementConsultantsLeadCount(_enrollmentContext.SponsorID ?? 0);
				Account.OnEnrollmentCompleted(_enrollmentContext);
				Account.OnEnrollmentCompleted(initialOrder);
			}
			catch (Exception ex)
			{
				// Just log and continue
				ex.Log(accountID: account != null ? (int?)account.AccountID : null);
			}

			// Activate account
			try
			{
				account.Activate(false);
				Account.AssignRolesByAccountType(account);
				Account.AssignRolesByAccountType(account);
				account.Save();
			}
			catch (Exception ex)
			{
				AddErrorToTempData(string.Format("{0} {1}", _errorActivatingAccountString, ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage));
			}

			// Activate user (if they are new)
			try
			{
				if (account.User != null
					 && account.User.UserStatusID == (short)Constants.UserStatus.Inactive)
				{
					ActivateUser(account);
					UpdateUsername(account);
					account.Save();
				}
			}
			catch (Exception ex)
			{
				AddErrorToTempData(string.Format("{0} {1}", _errorActivatingAccountString, ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage));
			}

			// Submit autoship order
			try
			{
				// Some clients automatically save the initial order as an autoship
				if (autoshipOrder == null
					 && _enrollmentContext.EnrollmentConfig.EnrollmentOrder.SaveAsAutoshipOrder)
				{
					autoshipOrder = CreateAutoshipOrderFromInitialOrder(initialOrder, account);
					autoshipOrder.Save();
				}
				else if (autoshipOrder != null)
				{
					autoshipOrder.Order.OrderCustomers[0].RemoveAllPayments();
				}

				if (autoshipOrder != null)
				{
					SubmitAutoshipOrder(autoshipOrder, account);
				}
			}
			catch (Exception ex)
			{
				AddErrorToTempData(string.Format("{0} {1}", _errorSubmittingAutoshipOrderString, ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage));
			}

			// Submit subscription autoship order
			try
			{
				if (subscriptionAutoshipOrder != null)
				{
					SubmitAutoshipOrder(subscriptionAutoshipOrder, account);
				}
			}
			catch (Exception ex)
			{
				AddErrorToTempData(string.Format("{0} {1}", _errorSubmittingAutoshipOrderString, ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage));
			}

			// Create mail account
			try
			{
				CreateMailAccount(account);
			}
			catch (Exception ex)
			{
				AddErrorToTempData(string.Format("{0} {1}", _errorActivatingMailAccountString, ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage));
			}

			// Create PWS
			try
			{
				if (!string.IsNullOrWhiteSpace(_enrollmentContext.SiteSubscriptionUrl))
				{
					CreatePWS(account);
				}
			}
			catch (Exception ex)
			{
				AddErrorToTempData(string.Format("{0} {1}", _errorActivatingPWSString, ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage));
			}

			// First domain event
			try
			{
				if (initialOrder != null)
				{
					DomainEventQueueItem.AddEnrollmentCompletedEventToQueue(initialOrder.OrderID, account.AccountTypeID, account.AccountID);
				}
				else
				{
					DomainEventQueueItem.AddEnrollmentCompletedEventToQueue(account.AccountID, account.AccountTypeID);
				}
			}
			catch (Exception ex)
			{
				// Just log and continue
				ex.Log(accountID: account != null ? (int?)account.AccountID : null);
			}

			// Second domain event
			try
			{
				if (_enrollmentContext.AccountTypeID == (short)Constants.AccountType.Distributor)
				{
					if (initialOrder != null)
					{
						DomainEventQueueItem.AddDistributorJoinsDownlineEventToQueue(initialOrder.OrderID, account.AccountID);
					}
					else
					{
						// TODO: Enable DistributorJoinsDownlineEvent without an OrderID.
					}
				}
			}
			catch (Exception ex)
			{
				// Just log and continue
				ex.Log(accountID: account != null ? (int?)account.AccountID : null);
			}

			// Call OnEnrollmentCompleted() for overrides to perform additional actions.
			try
			{
				OnEnrollmentCompleted(account);
			}
			catch (Exception ex)
			{
				// Just log and continue
				ex.Log(accountID: account != null ? (int?)account.AccountID : null);
			}

			// Update the OrderContext.Order with the newly-created account.
			UpdateOrderContextAccount(account);

			return RedirectToStep(_enrollmentContext.EnrollmentConfig.Steps.NextItem);
		}
		#endregion

		#region Helpers
		protected virtual void Index_LoadResources(IndexModel model)
		{
			bool showInitialOrder = true;
			bool showAutoshipOrder = !_enrollmentContext.EnrollmentConfig.Autoship.Hidden;
			bool showSubscriptionAutoshipOrder = !_enrollmentContext.EnrollmentConfig.Autoship.Hidden;
			bool showDisbursementProfiles = !_enrollmentContext.EnrollmentConfig.DisbursementProfiles.Hidden;

			model.LoadResources(
				 ShouldShowSponsorEditLink(),
				 ShouldShowSponsor(),
				 _enrollmentContext,
				 FormatPWSUrl(_enrollmentContext.SiteSubscriptionUrl),
				 !_enrollmentContext.EnrollmentConfig.BasicInfo.SetShippingAddressFromMain,
				 showInitialOrder,
				 showAutoshipOrder && ShouldShowAutoshipOrders(),
				 showSubscriptionAutoshipOrder && ShouldShowAutoshipOrders(),
				 showDisbursementProfiles && ShouldShowDisbursementProfiles(),
				 GetEditInitialOrderController(),
				 GetEditInitialOrderAction(),
				 GetEditAutoshipController(),
				 GetEditAutoshipAction(),
				 GetEditDisbursementProfiles()
			);
		}

		protected virtual bool ShouldShowDisbursementProfiles()
		{
			return true;
		}

		protected virtual bool ShouldShowSponsor()
		{
			return true;
		}

		protected virtual bool ShouldShowAutoshipOrders()
		{
			return true;
		}

		protected virtual bool ShouldShowSponsorEditLink()
		{
			try
			{
				if (_enrollmentContext.EnrollmentConfig.Steps.Any(x => x.Controller == "Sponsor"))
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				ex.Log();
			}
			return false;
		}

		protected virtual string GetEditInitialOrderController()
		{
			if (_enrollmentContext.EnrollmentConfig.Steps.Any(x => x.Controller == "Products"))
			{
				return "Products";
			}

			return string.Empty;
		}

		protected virtual string GetEditInitialOrderAction()
		{
			var productsStep = _enrollmentContext.EnrollmentConfig.Steps.FirstOrDefault(x => x.Controller == "Products");
			if (productsStep == null)
			{
				return string.Empty;
			}

			var productsSections = productsStep.Sections;
			if (productsSections != null)
			{
				if (productsSections.Any(x => x.Action == "EnrollmentKits"))
				{
					return "EnrollmentKits";
				}
				if (productsSections.Any(x => x.Action == "EnrollmentItems"))
				{
					return "EnrollmentItems";
				}
			}

			return string.Empty;
		}

		protected virtual string GetEditAutoshipController()
		{
			if (_enrollmentContext.EnrollmentConfig.Steps.Any(x => x.Controller == "Products"))
			{
				return "Products";
			}

			return string.Empty;
		}

		protected virtual string GetEditAutoshipAction()
		{
			var productsStep = _enrollmentContext.EnrollmentConfig.Steps.FirstOrDefault(x => x.Controller == "Products");
			if (productsStep == null)
			{
				return string.Empty;
			}

			var productsSections = productsStep.Sections;
			if (productsSections != null)
			{
				if (productsSections.Any(x => x.Action == "AutoshipBundles"))
				{
					return "AutoshipBundles";
				}
				if (productsSections.Any(x => x.Action == "AutoshipItems"))
				{
					return "AutoshipItems";
				}
			}

			return string.Empty;
		}

		protected virtual string GetEditDisbursementProfiles()
		{
			var step = _enrollmentContext.EnrollmentConfig.Steps.FirstOrDefault(x => x.Controller == "AccountInfo");
			if (step == null)
			{
				return string.Empty;
			}

			var sections = step.Sections;
			if (sections != null)
			{
				var section = sections.FirstOrDefault(x => x.Name == "Disbursement Profiles");
				if (section != null)
				{
					return section.Action;
				}
			}

			return string.Empty;
		}

		protected virtual void ApplyPayments(Order order, Account account)
		{

			var accountPaymentMethod = GetAccountPaymentMethod(account);
			if (accountPaymentMethod == null)
			{
				throw new Exception(_errorOrderMissingInfoString);
			}

			order.OrderCustomers[0].RemoveAllPayments();

            var applyPaymentResponse = order.ApplyPaymentToCustomer(accountPaymentMethod.PaymentTypeID,order.GrandTotal.Value,accountPaymentMethod.NameOnCard,accountPaymentMethod);
			if (!applyPaymentResponse.Success)
			{
				throw new Exception(applyPaymentResponse.Message);
			}
		}

		protected virtual void SubmitInitialOrder(Order initialOrder, Account account)
		{
			SubmitOrder(initialOrder, account);
		}

		protected virtual void SubmitAutoshipOrder(AutoshipOrder autoshipOrder, Account account)
		{
			SubmitOrder(autoshipOrder.Order, account);
		}

		protected virtual void SubmitOrder(Order order, Account account)
		{
			if (order == null || !order.OrderCustomers.Any() || !order.OrderCustomers[0].OrderItems.Any())
			{
				throw new Exception(_errorOrderMissingInfoString);
			}

			if (order.GrandTotal == null)
			{
				throw new Exception(_errorOrderMissingInfoString);
			}

			var orderContext = Create.New<IOrderContext>();
			orderContext.Order = order;
			ApplyPayments(order, account);
			orderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
			OrderService.UpdateOrder(orderContext);
			orderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;

			// We always call this just before submitting an order
			order.SetConsultantID(account);

			var submitOrderResponse = OrderService.SubmitOrder(orderContext);
			if (!submitOrderResponse.Success)
			{
				throw new Exception(submitOrderResponse.Message);
			}
			else
			{
				//updates customer upgrade enrollment date to enrollment purchase instead of first customer purchase
				account.EnrollmentDate = order.CompleteDate;
				account.EnrollmentDateUTC = order.CompleteDateUTC;
			}
		}

		protected virtual void ActivateAccount(Account account)
		{
			if (_enrollmentContext.IsUpgrade)
			{
				account.AccountTypeID = (short)_enrollmentContext.AccountTypeID;
			}

			// Always call Activate() regardless of new/upgrade to ensure the correct EnrollmentDate is set.
			account.Activate(ShouldOverwriteExistingEnrollmentDate(account));
		}

		/// <summary>
		/// Sets the user status to active.
		/// </summary>
		protected virtual void ActivateUser(Account account)
		{
			if (account.User == null)
			{
				return;
			}

			account.User.UserStatusID = (short)Constants.UserStatus.Active;
		}

		/// <summary>
		/// Sets the username.
		/// </summary>
		protected virtual void UpdateUsername(Account account)
		{
			if (account.User == null)
			{
				return;
			}

			// Username always starts out as the account number to prevent conflicts,
			// but it can be changed once the enrollment is completed.
			account.User.Username = GetUsername(account);
		}

		/// <summary>
		/// Returns the new username for the enrolling account. Default is the account number for now.
		/// Some clients use the email address, but they should check for uniqueness first.
		/// </summary>
		protected virtual string GetUsername(Account account)
		{
			var useEmailAsUsername = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UseEmailAsUsername, false);
			if (useEmailAsUsername)
				return GetEmailUsername(account);
			else
				return account.AccountNumber;
		}

		protected virtual string GetEmailUsername(Account account)
		{
			if (Account.NonProspectExists(account.EmailAddress, account.AccountID))
				return account.AccountNumber; // the selected email is not available, but we are past the point of no return - use the account number for now so they don't get locked out

			if (!NetSteps.Data.Entities.User.IsUsernameAvailable(0, account.EmailAddress))
			{
				var existingUser = NetSteps.Data.Entities.User.GetByUsername(account.EmailAddress);
				existingUser.StartEntityTracking();
				existingUser.Username += "_%_" + Guid.NewGuid().ToString();
				existingUser.Save();
			}

			return account.EmailAddress;
		}

		/// <summary>
		/// Indicates if the account's EnrollmentDate should be overwritten if it already exists. Default is false for imported accounts, else true.
		/// </summary>
		protected virtual bool ShouldOverwriteExistingEnrollmentDate(Account account)
		{
			Contract.Requires<ArgumentNullException>(account != null);

			if (account.AccountStatusID == (short)Constants.AccountStatus.Imported)
			{
				return false;
			}

			return true;
		}

		protected virtual void CreateMailAccount(Account account)
		{
			// Build email address
			string emailAddress = GetMailAccountMailAddress(account);

			// Get or create mail account
			var mailAccount = MailAccount.LoadByAccountID(account.AccountID)
				 ?? new MailAccount { AccountID = account.AccountID };
			mailAccount.StartEntityTracking();

			// Set values
			mailAccount.EmailAddress = emailAddress;
			mailAccount.Active = true;
			mailAccount.Save();
		}

		public virtual string GetMailAccountMailAddress(Account account)
		{
			var mailbox = string.IsNullOrWhiteSpace(_enrollmentContext.SiteSubscriptionUrl)
				 ? account.AccountNumber
				 : _enrollmentContext.SiteSubscriptionUrl;
			string emailAddress = FormatEmailAddress(mailbox);
			return emailAddress;
		}

		protected virtual void CreatePWS(Account account)
		{
			int marketID = _enrollmentContext.MarketID;
			string url = FormatPWSUrl(_enrollmentContext.SiteSubscriptionUrl);
			int? websiteAutoshipScheduleID = _enrollmentContext.EnrollmentConfig.Website.AutoshipScheduleID;

			// PWS requires its own autoship order
			AutoshipOrder pwsAutoship = null;
			if (websiteAutoshipScheduleID.HasValue)
			{
				pwsAutoship = AutoshipOrder.GenerateTemplateFromSchedule(
					 websiteAutoshipScheduleID.Value,
					 account,
					 marketID
				);
			}

			// Check if PWS already exists
			var pws = Site.LoadByAccountID(account.AccountID).FirstOrDefault();
			if (pws == null)
			{
				// Create PWS
				var baseSite = Site.LoadBaseSiteForNewPWS(marketID);
				baseSite.CreateChildSite(
					 account,
					 marketID,
					 pwsAutoship != null ? (int?)pwsAutoship.AutoshipOrderID : null,
					 urls: new[] { url },
					 saveNewSite: true
				);
			}
			else
			{
				// Update SiteUrl
				var pwsUrl = pws.SiteUrls.OrderByDescending(x => x.IsPrimaryUrl).FirstOrDefault()
					 ?? new SiteUrl { SiteID = pws.SiteID };
				pwsUrl.StartEntityTracking();

				pwsUrl.IsPrimaryUrl = true;
				pwsUrl.LanguageID = account.DefaultLanguageID;
				pwsUrl.Url = url;
				pwsUrl.Save();
			}
		}

		protected virtual void CopyAutoshipItemsToInitialOrder(Account account, AutoshipOrder autoship, ref Order initialOrder, ref IEnumerable<dynamic> orderItemsTracker)
		{
			if (initialOrder == null)
			{
				_enrollmentContext.InitialOrder = initialOrder = CreateInitialOrder(account);
				UpdateOrderShipmentAddress(initialOrder, account);
			}

			var subscriptionOrderCustomer = autoship.Order.OrderCustomers.First();
			orderItemsTracker = subscriptionOrderCustomer.OrderItems.Select(oi => new { ProductId = oi.ProductID.Value, Qty = oi.Quantity }).ToList();
			foreach (var item in orderItemsTracker)
				initialOrder.AddItem(item.ProductId, item.Qty);

			// We've made changes to the order and the totals need to be calculated again.
			initialOrder = this.TotalOrder(initialOrder);
			initialOrder.Save();
		}

		protected virtual void RemoveAutoshipItemsFromInitialOrder(Order initialOrder, IEnumerable<dynamic> orderItemsTracker)
		{
			foreach (var item in orderItemsTracker)
			{
				var orderItem = initialOrder.OrderCustomers.First().OrderItems.First(oi => oi.ProductID == item.ProductId && oi.Quantity == item.Qty);
				initialOrder.RemoveItem(orderItem.OrderItemID);
			}

			initialOrder.Save();
		}

		protected virtual void UpdateOrderContextAccount(Account account)
		{
			OrderContext.Order.AsOrder().UpdateCustomer(account);

			foreach (var step in OrderContext.InjectedOrderSteps)
			{
				step.CustomerAccountID = account.AccountID;
			}
		}

		/// <summary>
		/// Allows overrides to perform additional actions after enrollment is completed.
		/// </summary>
		protected virtual void OnEnrollmentCompleted(Account account) { }
		#endregion

		#region Strings
		protected virtual string _errorSubmittingInitialOrderString { get { return Translation.GetTerm("ErrorSubmittingInitialOrder", "There was an error saving your order. Please try again."); } }
		protected virtual string _errorSubmittingAutoshipOrderString { get { return Translation.GetTerm("ErrorSubmittingAutoshipOrder", "There was an error saving your autoship order. Please contact customer service."); } }
		protected virtual string _errorOrderMissingInfoString { get { return Translation.GetTerm("ErrorOrderMissingInfo", "The order is missing required information."); } }
		protected virtual string _errorActivatingAccountString { get { return Translation.GetTerm("ErrorActivatingAccount", "There was an error activating your account. Please contact customer service."); } }
		protected virtual string _errorUpdatingUsernameString { get { return Translation.GetTerm("ErrorUpdatingUsername", "There was an error saving your username. Please contact customer service."); } }
		protected virtual string _errorActivatingPWSString { get { return Translation.GetTerm("ErrorActivatingPWS", "There was an error activating your personal website. Please contact customer service."); } }
		protected virtual string _errorActivatingMailAccountString { get { return Translation.GetTerm("ErrorActivatingMailAccount", "There was an error activating your email account. Please contact customer service."); } }
		//protected virtual string _errorSettingUpRolesString { get { return Translation.GetTerm("ErrorSettingUpRoles", "There was an error saving user roles. Please contact customer service."); } }
		#endregion
	}
}
