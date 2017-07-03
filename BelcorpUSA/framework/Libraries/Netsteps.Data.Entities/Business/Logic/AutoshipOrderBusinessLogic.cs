using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Reflection;
using NetSteps.Data.Common.Models;
using NetSteps.Data.Entities.Business.HelperObjects;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Common.Context;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class AutoshipOrderBusinessLogic
	{
		public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }
		protected IOrderService OrderService { get { return Create.New<IOrderService>(); } }

		public override List<string> ValidatedChildPropertiesSetByParent(Repositories.IAutoshipOrderRepository repository)
		{
			return new List<string>()
			{
				"AutoshipOrderID",
				"TemplateOrderID",
				"OrderID",
				"OrderCustomerID",
                "OrderItemID",
                "OrderAdjustmentID",
                "OrderShipmentID",
                "OrderItemMessageID",
			};
		}

		/// <summary>
		/// Returns the first run date for a new autoship order.
		/// </summary>
		public virtual DateTime CalculateFirstRunDate(
			AutoshipOrder autoshipOrder)
		{
			if (autoshipOrder == null)
			{
				throw new ArgumentNullException("autoshipOrder");
			}

			var currentDate = DateTime.Today;
			var autoshipSchedule = SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipOrder.AutoshipScheduleID);
			var intervalType = SmallCollectionCache.Instance.IntervalTypes.GetById(autoshipSchedule.IntervalTypeID);
			var availableDays = SmallCollectionCache.Instance.AutoshipScheduleDays
				.Where(x => x.AutoshipScheduleID == autoshipOrder.AutoshipScheduleID)
				.Select(x => (int)x.Day);
			var preferredDay = autoshipOrder.Day > 0 && autoshipOrder.Day <= 31
				? autoshipOrder.Day
				: currentDate.Day;

			return CalculateFirstRunDate(
				currentDate,
				autoshipSchedule,
				intervalType,
				availableDays,
				preferredDay
			);
		}

		public virtual DateTime CalculateFirstRunDate(
			DateTime currentDate,
			AutoshipSchedule autoshipSchedule,
			IntervalType intervalType,
			IEnumerable<int> availableDays,
			int preferredDay)
		{
			if (autoshipSchedule == null)
			{
				throw new ArgumentNullException("autoshipSchedule");
			}
			if (intervalType == null)
			{
				throw new ArgumentNullException("intervalType");
			}
			if (availableDays == null)
			{
				throw new ArgumentNullException("availableDays");
			}
			if (preferredDay < 1 || preferredDay > 31)
			{
				throw new ArgumentException("preferredDay must be between 1 and 31.", "preferredDay");
			}

			// Default behavior is for all new autoships to run in the next interval.
			return GetNearestAvailableAutoshipDateInNextInterval(currentDate, intervalType, availableDays, preferredDay);
		}

		public virtual DateTime CalculateNextRunDateOnSuccess(
			DateTime currentDate,
			AutoshipOrder autoshipOrder)
		{
			if (autoshipOrder == null)
			{
				throw new ArgumentNullException("autoshipOrder");
			}

			var autoshipSchedule = SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipOrder.AutoshipScheduleID);
			var intervalType = SmallCollectionCache.Instance.IntervalTypes.GetById(autoshipSchedule.IntervalTypeID);
			var availableDays = SmallCollectionCache.Instance.AutoshipScheduleDays
				.Where(x => x.AutoshipScheduleID == autoshipOrder.AutoshipScheduleID)
				.Select(x => (int)x.Day);
			var preferredDay = autoshipOrder.Day > 0 && autoshipOrder.Day <= 31
				? autoshipOrder.Day
				: currentDate.Day;

			return CalculateNextRunDateOnSuccess(
				currentDate,
				autoshipSchedule,
				intervalType,
				availableDays,
				preferredDay
			);
		}

		public virtual DateTime CalculateNextRunDateOnSuccess(
			DateTime currentDate,
			AutoshipSchedule autoshipSchedule,
			IntervalType intervalType,
			IEnumerable<int> availableDays,
			int preferredDay)
		{
			if (autoshipSchedule == null)
			{
				throw new ArgumentNullException("autoshipSchedule");
			}
			if (intervalType == null)
			{
				throw new ArgumentNullException("intervalType");
			}
			if (availableDays == null)
			{
				throw new ArgumentNullException("availableDays");
			}
			if (preferredDay < 1 || preferredDay > 31)
			{
				throw new ArgumentException("preferredDay must be between 1 and 31.", "preferredDay");
			}

			return GetNearestAvailableAutoshipDateInNextInterval(currentDate, intervalType, availableDays, preferredDay);
		}

		public virtual DateTime CalculateNextRunDateOnFailure(
			DateTime currentDate,
			AutoshipOrder autoshipOrder)
		{
			if (autoshipOrder == null)
			{
				throw new ArgumentNullException("autoshipOrder");
			}

			var autoshipSchedule = SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipOrder.AutoshipScheduleID);
			var intervalType = SmallCollectionCache.Instance.IntervalTypes.GetById(autoshipSchedule.IntervalTypeID);
			var availableDays = SmallCollectionCache.Instance.AutoshipScheduleDays
				.Where(x => x.AutoshipScheduleID == autoshipOrder.AutoshipScheduleID)
				.Select(x => (int)x.Day);
			var preferredDay = autoshipOrder.Day > 0 && autoshipOrder.Day <= 31
				? autoshipOrder.Day
				: currentDate.Day;
			var totalAttemptsThisInterval = AutoshipLog.GetTotalAttemptsInInterval(currentDate, autoshipOrder.TemplateOrderID, intervalType);
			bool hasSucceededThisInterval = autoshipOrder.DateLastCreated.HasValue
											&& autoshipOrder.DateLastCreated.Value >= intervalType.GetStartOfInterval(currentDate);
			return CalculateNextRunDateOnFailure(
			currentDate,
			autoshipSchedule,
			intervalType,
			availableDays,
			preferredDay,
			totalAttemptsThisInterval,
			hasSucceededThisInterval
			);
		}

		public virtual DateTime CalculateNextRunDateOnFailure(
			DateTime currentDate,
			AutoshipSchedule autoshipSchedule,
			IntervalType intervalType,
			IEnumerable<int> availableDays,
			int preferredDay,
			int totalAttemptsThisInterval,
			bool hasSucceededThisInterval)
		{
			if (autoshipSchedule == null)
			{
				throw new ArgumentNullException("autoshipSchedule");
			}
			if (intervalType == null)
			{
				throw new ArgumentNullException("intervalType");
			}
			if (availableDays == null)
			{
				throw new ArgumentNullException("availableDays");
			}
			if (preferredDay < 1 || preferredDay > 31)
			{
				throw new ArgumentException("preferredDay must be between 1 and 31.", "preferredDay");
			}

			// Check for max attempts
			if (hasSucceededThisInterval
			|| totalAttemptsThisInterval >= autoshipSchedule.MaximumAttemptsPerInterval)
			{
				// Skip to the next interval
				return GetNearestAvailableAutoshipDateInNextInterval(currentDate, intervalType, availableDays, preferredDay);
			}
			else
			{
				var startOfNextInterval = intervalType.GetStartOfNextInterval(currentDate);
				var retryDate = currentDate.AddDays(1);
				// Check for end of interval
				if (retryDate.Date >= startOfNextInterval.Date)
				{
					// Skip to the next interval
					return GetNearestAvailableAutoshipDateInNextInterval(currentDate, intervalType, availableDays, preferredDay);
				}
				else
				{
					// Use the retry date
					return retryDate.Date;
				}
			}
		}

		public virtual void SetOrderShipmentForNewOrderIfAvailable(Order order, Order newOrder, Account account)
		{
			OrderShipment templateShipment = order.GetDefaultShipmentNoDefault();
			if (templateShipment.IsNotNull())
			{
				IAddress sourceAddress = null;

				sourceAddress = SourceShippingAddressOrTemplateAddress(account, templateShipment);

				OrderShipment newShipment = newOrder.GetDefaultShipmentNoDefault();

				if (newShipment == null)
				{
					newShipment = new OrderShipment();
					newShipment.StartEntityTracking();
				}
				newShipment.OrderShipmentStatusID = Constants.OrderShipmentStatus.Pending.ToShort();
				newOrder.OrderShipments.Add(newShipment);

				Address.CopyPropertiesTo(sourceAddress, newShipment);

				// Set ShippingMethodID, even if it is null.
				newShipment.ShippingMethodID = templateShipment.ShippingMethodID;

				newShipment.SourceAddressID = templateShipment.SourceAddressID;
			}
		}

		public virtual void SetOrderPaymentForNewOrderIfAvailable(Order newOrder, OrderCustomer orderCustomer, Account account)
		{
			OrderPayment templatePayment = orderCustomer.OrderPayments.FirstOrDefault();
			if (templatePayment == null)
			{
				throw new ApplicationException("No Order Payment.");
			}

			IPayment sourcePayment = null;
			IAddress sourcePaymentAddress = null;

			// Use the AccountPaymentMethod (if available) for the child order's OrderPayment
			// This will ensure we have the most current payment info
			if (templatePayment.SourceAccountPaymentMethodID.HasValue)
			{
				var sourceAccountPaymentMethod = Account.LoadPaymentMethodAndVerifyAccount(
					templatePayment.SourceAccountPaymentMethodID.Value,
					account.AccountID
				);
				sourcePayment = sourceAccountPaymentMethod;
				sourcePaymentAddress = sourceAccountPaymentMethod;
			}
			else
			{
				// Else use the OrderPayment from the template order
				sourcePayment = templatePayment;
				sourcePaymentAddress = templatePayment;
			}

			OrderPayment newPayment = new OrderPayment();
			Reflection.CopyPropertiesDynamic<IPayment, IPayment>(sourcePayment, newPayment);
			Address.CopyPropertiesTo(sourcePaymentAddress, newPayment);
			newPayment.OrderPaymentStatusID = Constants.OrderPaymentStatus.Pending.ToShort();
			newPayment.SourceAccountPaymentMethodID = templatePayment.SourceAccountPaymentMethodID;
			newPayment.CurrencyID = newOrder.CurrencyID;
			newPayment.Amount = newOrder.GrandTotal.ToDecimal();

			newOrder.OrderCustomers[0].OrderPayments.Add(newPayment);
		}

		public virtual IAddress SourceShippingAddressOrTemplateAddress(Account account, OrderShipment templateShipment)
		{
			IAddress sourceAddress;
			if (templateShipment.SourceAddressID.HasValue)
			{
				// This will throw an exception if the specified address is not found or is not linked to the specified account.
				var sourceAccountAddress = Account.LoadAddressAndVerifyAccount(
					templateShipment.SourceAddressID.Value,
					account.AccountID
					);

				sourceAddress = sourceAccountAddress;
			}
			else
			{
				sourceAddress = templateShipment;
			}

			return sourceAddress;
		}

		public virtual Order GenerateChildOrderFromTemplate(Order order)
		{
			try
			{
				if (order.OrderCustomers.Count != 1)
					throw new NetStepsException("Autoship order must have only one order customer.");

				OrderCustomer orderCustomer = order.OrderCustomers[0];
				Account account = Account.Load(orderCustomer.AccountID);

				Order newOrder = new Order(account);
				newOrder.SiteID = ConfigurationManager.GetAppSetting<int?>(ConfigurationManager.VariableKey.NSCoreSiteID); // TODO: Is this correct? - JHE
				newOrder.DateCreated = DateTime.Now;
				newOrder.CurrencyID = order.CurrencyID;
				newOrder.ParentOrderID = order.OrderID;
				newOrder.OrderPendingState = Constants.OrderPendingStates.Quote;

				var parentOrderType = SmallCollectionCache.Instance.OrderTypes.GetById(order.OrderTypeID);
				if (parentOrderType.IsTemplate)
					newOrder.OrderTypeID = GetDefaultOrderTypeID(account.AccountTypeID, templateOrderTypeID: parentOrderType.OrderTypeID);
				if (SmallCollectionCache.Instance.OrderTypes.GetById(newOrder.OrderTypeID).IsTemplate)
					throw new NetStepsException("A child order generated from a template cannot also be a template");

				newOrder.SetConsultantID(account);

				ValidateOrderTypeByAccountType(newOrder.OrderTypeID, account.AccountTypeID);

				// Add Order Items - JHE (Test this)
				List<OrderItemUpdateInfo> productUpdates = new List<OrderItemUpdateInfo>();
				foreach (var orderItem in orderCustomer.OrderItems)
				{
					productUpdates.Add(new OrderItemUpdateInfo() { ProductID = orderItem.ProductID.ToInt(), Quantity = orderItem.Quantity });
				}
				newOrder.AddOrUpdateOrderItem(orderCustomer, productUpdates, false);

				SetOrderShipmentForNewOrderIfAvailable(order, newOrder, account);

				// Force re-calculation now that we have a shipping address to update shipping rates and Taxes - JHE
				var orderContext = Create.New<IOrderContext>();
				orderContext.Order = newOrder;
				OrderService.UpdateOrder(orderContext);

				// Set Payment Method
				SetOrderPaymentForNewOrderIfAvailable(newOrder, orderCustomer, account);
				OrderService.UpdateOrder(orderContext);

				return newOrder;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Gets the default orderTypeID for ChildOrders of Template orders is templateOrderTypeID is specified, else get the orderTypeID for 
		/// Template Orders if isTemplate == true; Else just returns the default of a regular order based on AccountType. - JHE
		/// </summary>
		/// <param name="accountTypeID"></param>
		/// <param name="isTemplate"></param>
		/// <param name="templateOrderTypeID"></param>
		/// <returns></returns>
		public virtual short GetDefaultOrderTypeID(short accountTypeID, bool isTemplate = false, int? templateOrderTypeID = null)
		{
			OrderType templateOrderType = null;
			if (templateOrderTypeID != null)
			{
				templateOrderType = SmallCollectionCache.Instance.OrderTypes.GetById(templateOrderTypeID.ToShort());
				if (templateOrderType != null)
					isTemplate = templateOrderType.IsTemplate;
			}

			if (isTemplate)
			{
				if (templateOrderTypeID != null)
				{
					return GetDefaultGeneratedAutoshipOrderTypeID(accountTypeID, templateOrderTypeID);
				}
				else
				{
					if (accountTypeID == Constants.AccountType.Distributor.ToShort())
						return Constants.OrderType.AutoshipTemplate.ToShort();
					else if (accountTypeID == Constants.AccountType.PreferredCustomer.ToShort())
						return Constants.OrderType.AutoshipTemplate.ToShort();
					else
						throw new Exception("Invalid Template accountTypeID. Unable to set OrderType.");
				}
			}
			else
			{
				if (accountTypeID == Constants.AccountType.Distributor.ToInt())
					return Constants.OrderType.OnlineOrder.ToShort();
				else if (accountTypeID == Constants.AccountType.PreferredCustomer.ToInt())
					return Constants.OrderType.OnlineOrder.ToShort();
				else
					return Constants.OrderType.OnlineOrder.ToShort();
			}
		}

		public virtual short GetDefaultGeneratedAutoshipOrderTypeID(short accountTypeID, int? templateOrderTypeID = null)
		{
			if (templateOrderTypeID != null)
			{
				if (templateOrderTypeID == Constants.OrderType.AutoshipTemplate.ToInt())
					return Constants.OrderType.AutoshipOrder.ToShort();
				else
					throw new Exception("Invalid Template orderType. Unable to set child OrderType.");
			}
			else
			{
				if (accountTypeID == Constants.AccountType.Distributor.ToInt())
					return Constants.OrderType.AutoshipOrder.ToShort();
				else if (accountTypeID == Constants.AccountType.PreferredCustomer.ToInt())
					return Constants.OrderType.AutoshipOrder.ToShort();
				else
					throw new Exception("Invalid Template accountTypeID. Unable to set OrderType.");
			}
		}

		public virtual void ValidateOrderTypeByAccountType(short orderTypeID, short accountTypeID)
		{
			switch (orderTypeID)
			{
				case (int)Constants.OrderType.OnlineOrder:
					break;
				case (int)Constants.OrderType.OverrideOrder:
					break; //Do nothing
				default:
					if (accountTypeID == (int)Constants.AccountType.NotSet || accountTypeID == (int)Constants.AccountType.RetailCustomer)
						throw new ApplicationException("Retail Order type and Account type combination is invalid.");
					break;
			}
		}

		public override void CleanDataBeforeSave(Repositories.IAutoshipOrderRepository repository, AutoshipOrder entity)
		{
			if (entity.NextRunDate != null)
				entity.NextRunDate = entity.NextRunDate.ToDateTime().Date;
		}

		//// TODO: Finish this method - JHE
		//public virtual void ProcessAutoshipCancellationsForTerminatedAccounts()
		//{
		//    //LogMessage("Beginning Processing Autoship Cancellations For Terminated or Failed Accounts");

		//    //SqlCommand cmd = DataAccess.SetCommand("usp_autoship_cancel_for_terminated_accounts");
		//    //DataAccess.ExecuteNonQuery(cmd);
		//}


		public virtual void LogAutoshipGenerationResults(
			int autoshipBatchID,
			int templateOrderId,
			int? newOrderId,
			bool succeeded,
			string results,
			DateTime runDate)
		{
			AutoshipLog autoshipLog = new AutoshipLog();
			autoshipLog.StartEntityTracking();

			autoshipLog.AutoshipBatchID = autoshipBatchID;
			autoshipLog.TemplateOrderID = templateOrderId;
			autoshipLog.Succeeded = succeeded;
			autoshipLog.Results = results;
			autoshipLog.NewOrderID = newOrderId.ToIntNullable();
			autoshipLog.RunDate = runDate;

			autoshipLog.Save();
		}

		public virtual AutoshipOrder GenerateTemplateFromSchedule(int scheduleId, Account account, int marketId, int? siteId = null, bool saveAndChargeNewOrder = true)
		{
			IAddress shippingAddress = account.Addresses.GetDefaultByTypeID(Generated.ConstantsGenerated.AddressType.Shipping);
			if (saveAndChargeNewOrder && shippingAddress == default(Address))
			{
				throw new NetStepsBusinessException(new Exception("Please add a shipping address to this account."))
				{
					PublicMessage = Translation.GetTerm("PleaseAddaShippingAddressToThisAccount", "Please add a shipping address to this account.")
				};
			}

			IPayment paymentMethod = account.AccountPaymentMethods.Count(apm => apm.IsDefault) > 0 ? account.AccountPaymentMethods.First(apm => apm.IsDefault) : account.AccountPaymentMethods.FirstOrDefault();
			if (saveAndChargeNewOrder && paymentMethod == default(AccountPaymentMethod))
			{
				throw new NetStepsBusinessException(new Exception("Please add a payment method to this account."))
				{
					PublicMessage = Translation.GetTerm("PleaseAddaPaymentMethodToThisAccount", "Please add a payment method to this account.")
				};
			}

			return GenerateTemplateFromSchedule(scheduleId, account, marketId, shippingAddress, paymentMethod, siteId, saveAndChargeNewOrder: saveAndChargeNewOrder);
		}

		public virtual AutoshipOrder GenerateTemplateFromSchedule(int scheduleId, Account account, int marketId, IAddress shippingAddress, IPayment paymentMethod, int? siteId = null, bool saveAndChargeNewOrder = true)
		{
			try
			{
				if (saveAndChargeNewOrder && shippingAddress == default(Address))
				{
					throw new NetStepsBusinessException(new Exception("Please add a shipping address to this account."))
					{
						PublicMessage = Translation.GetTerm("PleaseAddaShippingAddressToThisAccount", "Please add a shipping address to this account.")
					};
				}
				if (saveAndChargeNewOrder && paymentMethod == default(AccountPaymentMethod))
				{
					throw new NetStepsBusinessException(new Exception("Please add a payment method to this account."))
					{
						PublicMessage = Translation.GetTerm("PleaseAddaPaymentMethodToThisAccount", "Please add a payment method to this account.")
					};
				}

				AutoshipSchedule schedule = AutoshipSchedule.LoadFull(scheduleId);
				var autoshipScheduleDays = SmallCollectionCache.Instance.AutoshipScheduleDays.Where(a => a.AutoshipScheduleID == schedule.AutoshipScheduleID);

				var autoshipOrder = new AutoshipOrder();
				autoshipOrder.StartTracking();
				autoshipOrder.AccountID = account.AccountID;
				autoshipOrder.AutoshipScheduleID = scheduleId;
				autoshipOrder.StartDate = DateTime.Today;
				autoshipOrder.NextRunDate = CalculateFirstRunDate(autoshipOrder);
				autoshipOrder.Day = autoshipOrder.NextRunDate.Value.Day;

				var order = new NetSteps.Data.Entities.Order(account);
				order.OrderTypeID = schedule.OrderTypeID;
				order.OrderStatusID = NetSteps.Data.Entities.Constants.OrderStatus.Pending.ToShort();
				order.SiteID = siteId;
				order.DateCreated = DateTime.Now;
				order.CurrencyID = SmallCollectionCache.Instance.Markets.GetById(marketId).GetDefaultCurrencyID();
				autoshipOrder.Order = order;
				var orderContext = Create.New<IOrderContext>();
				
				
				if (schedule.AutoshipScheduleProducts != null)
				{
					foreach (AutoshipScheduleProduct product in schedule.AutoshipScheduleProducts)
					{
						order.AddItem(product.ProductID, product.Quantity);
					}
				}

				var autoshipOrders = order.AutoshipOrders.ToList();
				order.AutoshipOrders.Clear();

				order.Save();

				order.AutoshipOrders.AddRange(autoshipOrders);
				orderContext.Order = autoshipOrder.Order;

				if (saveAndChargeNewOrder)
				{
					autoshipOrder.Order.UpdateOrderShipmentAddressAndDefaultShipping(shippingAddress);
					autoshipOrder.Order.OrderPendingState = Constants.OrderPendingStates.Quote;
					OrderService.UpdateOrder(orderContext);
                    int PaymentTypeID = 0;
					//autoshipOrder.Order.ApplyPaymentToCustomer(PaymentTypeID, autoshipOrder.Order.GrandTotal.ToDecimal());
					autoshipOrder.Order.OrderPendingState = Constants.OrderPendingStates.Open;
					OrderService.UpdateOrder(orderContext);
					autoshipOrder.Order.OrderPendingState = Constants.OrderPendingStates.Quote;
					autoshipOrder.Save();
					var submitResponse = OrderService.SubmitOrder(orderContext);
					if (!submitResponse.Success)
					{
						throw new Exception("Error submitting autoship order: " + submitResponse.Message);
					}
				}
				else
				{
					OrderService.UpdateOrder(orderContext);
				}

				return autoshipOrder;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
			}
		}

		public virtual AutoshipOrder LoadByAccountIDAndAutoshipScheduleID(Repositories.IAutoshipOrderRepository repository, int accountID, int autoshipScheduleID)
		{
			try
			{
				var autoshipOrder = repository.LoadByAccountIDAndAutoshipScheduleID(accountID, autoshipScheduleID);
				if (autoshipOrder != null)
				{
					autoshipOrder.StartEntityTracking();
					autoshipOrder.IsLazyLoadingEnabled = true;
				}
				return autoshipOrder;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual AutoshipOrder LoadFullByAccountIDAndAutoshipScheduleID(Repositories.IAutoshipOrderRepository repository, int accountID, int autoshipScheduleID)
		{
			try
			{
				var autoshipOrder = repository.LoadFullByAccountIDAndAutoshipScheduleID(accountID, autoshipScheduleID);
				if (autoshipOrder != null)
				{
					autoshipOrder.StartEntityTracking();
					autoshipOrder.IsLazyLoadingEnabled = true;
				}
				return autoshipOrder;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		public virtual List<AutoshipOrder> LoadAllFullByAccountID(Repositories.IAutoshipOrderRepository repository, int accountID)
		{
			try
			{
				var autoshipOrders = repository.LoadAllFullByAccountID(accountID);
				if (autoshipOrders != null && autoshipOrders.Count > 0)
				{
					foreach (var autoshipOrder in autoshipOrders)
					{
						autoshipOrder.StartEntityTracking();
						autoshipOrder.IsLazyLoadingEnabled = true;
					}
				}
				return autoshipOrders;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		public virtual AutoshipOrder LoadFullByOrderID(Repositories.IAutoshipOrderRepository repository, int orderID)
		{
			try
			{
				var autoshipOrder = repository.LoadFullByOrderID(orderID);
				if (autoshipOrder != null)
				{
					autoshipOrder.StartEntityTracking();
					autoshipOrder.IsLazyLoadingEnabled = true;
				}
				return autoshipOrder;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual List<AutoshipProcessInfo> GetAutoshipTemplatesByNextDueDateByScheduleID(Repositories.IAutoshipOrderRepository repository, int autoshipScheduleID, DateTime nextDueDate)
		{
			try
			{
				return repository.GetAutoshipTemplatesByNextDueDateByAutoshipScheduleID(autoshipScheduleID, nextDueDate);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual PaginatedList<AutoshipBatchReportData> GetAutoshipRunReport(Repositories.IAutoshipOrderRepository repository, AutoshipOrderSearchParameters searchParameters)
		{
			try
			{
				return repository.GetAutoshipRunReport(searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual PaginatedList<AutoshipLogReportData> LoadAutoshipLogsByAutoshipBatchID(Repositories.IAutoshipOrderRepository repository, AutoshipLogSearchParameters searchParameters)
		{
			try
			{
				return repository.LoadAutoshipLogsByAutoshipBatchID(searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual bool IsSiteSubscriptionTemplate(AutoshipOrder autoshipOrder)
		{
			try
			{
				var autoshipSchedule = SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipOrder.AutoshipScheduleID);
				if (autoshipSchedule != null)
					return (Constants.AutoshipScheduleType)autoshipSchedule.AutoshipScheduleTypeID == Generated.ConstantsGenerated.AutoshipScheduleType.SiteSubscription;
				return false;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual bool HasFixedTemplateItems(AutoshipOrder autoshipOrder)
		{
			try
			{
				return SmallCollectionCache.Instance.AutoshipScheduleProducts
					.Any(x => x.AutoshipScheduleID == autoshipOrder.AutoshipScheduleID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual void SyncFixedOrderItems(
			AutoshipOrder autoshipOrder,
			bool removePendingPayments = true)
		{
			if (autoshipOrder == null)
			{
				throw new ArgumentNullException("autoshipOrder");
			}

			var autoshipScheduleProducts = SmallCollectionCache.Instance.AutoshipScheduleProducts
				.Where(x => x.AutoshipScheduleID == autoshipOrder.AutoshipScheduleID);

			SyncFixedOrderItems(
				autoshipOrder,
				autoshipScheduleProducts,
				removePendingPayments: removePendingPayments
			);
		}

		public virtual void SyncFixedOrderItems(
			AutoshipOrder autoshipOrder,
			IEnumerable<AutoshipScheduleProduct> autoshipScheduleProducts,
			bool removePendingPayments = true)
		{
			if (autoshipOrder == null)
			{
				throw new ArgumentNullException("autoshipOrder");
			}
			if (autoshipOrder.Order == null)
			{
				throw new ArgumentException("autoshipOrder is missing an Order.");
			}
			if (autoshipOrder.Order.OrderCustomers.Count != 1)
			{
				throw new ArgumentException("autoshipOrder must have a single Order Customer.");
			}
			if (autoshipScheduleProducts == null)
			{
				throw new ArgumentNullException("autoshipScheduleProducts");
			}
			if (!autoshipScheduleProducts.Any())
			{
				// No products means this is not a fixed order, just return.
				return;
			}

			// Remove order items with incorrect ProductIDs
			var autoshipScheduleProductIDs = autoshipScheduleProducts.Select(x => x.ProductID).ToList();
			foreach (var orderItemToRemove in autoshipOrder.Order.OrderCustomers[0].OrderItems
				.Where(x => x.ProductID == null || !autoshipScheduleProductIDs.Contains(x.ProductID.Value))
				.ToList())
			{
				autoshipOrder.Order.RemoveItem(autoshipOrder.Order.OrderCustomers[0], orderItemToRemove);
			}

			// Add/Update quantities on correct ProductIDs
			autoshipOrder.Order.AddOrUpdateOrderItem(autoshipOrder.Order.OrderCustomers[0],
				autoshipScheduleProducts.Select(x => new OrderItemUpdateInfo { ProductID = x.ProductID, Quantity = x.Quantity }),
				true,
				removePendingPayments: removePendingPayments
			);
		}

		public virtual string GetWarningMessageForAutoshipChange(Repositories.IAutoshipOrderRepository repository, AutoshipOrder autoshipOrder)
		{
			try
			{
				if(autoshipOrder == null || autoshipOrder.Order == null || autoshipOrder.Order.OrderCustomers == null)
				{
					return string.Empty;
				}

				var totalOrderItems = autoshipOrder.Order.OrderCustomers.Sum(oc => oc.OrderItems.Count());
				if(totalOrderItems == 0)
				{
					return Translation.GetTerm("Warning:ThereAreNoItemsOnThisAutoshipOrder", "Warning: There are no items on this Autoship order.");
				}

				var totalCv = autoshipOrder.Order.OrderCustomers.Sum(oc => oc.OrderItems.Sum(oi => oi.CommissionableTotalOverride != null ? oi.CommissionableTotalOverride.Value : oi.CommissionableTotal));
				if(totalCv < 80)
				{
					return Translation.GetTerm("Warning:TheTotalCvisLessThan80cv", "Warning: The total CV is less than 80CV.");
				}
				
				return string.Empty;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Returns the most recent consecutive order count, for autoship orders.
		/// </summary>
		public virtual int CalculateConsecutiveOrders(
			AutoshipOrder autoshipOrder)
		{
			if (autoshipOrder == null)
			{
				throw new ArgumentNullException("autoshipOrder");
			}

			return CalculateConsecutiveOrders(
				Order.GetCompletedOrderDates(parentOrderID: autoshipOrder.TemplateOrderID)
			);
		}

		/// <summary>
		/// Returns the most recent consecutive order count, for autoship orders.
		/// </summary>
		public virtual int CalculateConsecutiveOrders(
			IEnumerable<DateTime> completedOrderDates)
		{
			if (completedOrderDates == null)
			{
				throw new ArgumentNullException("completedOrderDates");
			}

			if (!completedOrderDates.Any())
			{
				return 0;
			}

			return completedOrderDates
				.Select(x => x.TotalMonths())
				.Distinct()
				.OrderByDescending(x => x)
				.SplitWhere((first, second) => first != second + 1)
				.First()
				.Count();
		}


		public virtual DateTime GetAutoshipReminderDaysOffSet(IEnumerable<AutoshipSchedule> autoshipSchedules)
		{
			try
			{
				if (autoshipSchedules != null && autoshipSchedules.Any())
				{
					// AutoshipSchedules in here will have value for AutoshipReminderDayOffSet
					int daysOffSet = autoshipSchedules.First(x => x.AutoshipReminderDayOffSet.HasValue).AutoshipReminderDayOffSet.Value;

					return DateTime.Now.AddDays(daysOffSet);
				}
				return default(DateTime);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}

		}

		/// <summary>
		/// Send reminder emails for autoship orders that are due to run after (daysOffSet) certain number of days.
		/// </summary>
		public void SendAutoshipReminderAutoResponder(IEnumerable<AutoshipOrder> autoshipOrders)
		{
			try
			{
				foreach (var autoshipOrder in autoshipOrders)
				{
					DomainEventQueueItem.AddAutoshipReminderEventToQueue(autoshipOrder.TemplateOrderID);

					DateTime? nextRunDate = autoshipOrder.NextRunDate;
					autoshipOrder.AutoshipReminderNextRunDate = nextRunDate;
					autoshipOrder.Save();
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public PaginatedList<AutoshipOrder> QueueAutoshipReminders(IAutoshipOrderRepository repository)
		{
			try
			{
				PaginatedList<AutoshipOrder> list = new PaginatedList<AutoshipOrder>();


				var autoshipSchedules =
					SmallCollectionCache.Instance.AutoshipSchedules.Where(a => a.Active && a.AutoshipReminderDayOffSet.HasValue).ToList();

				// Get the day offset value - all autoshipSchedules will have a value for AutoshipReminderDayOffSet.
				DateTime autoshipReminderDate = GetAutoshipReminderDaysOffSet(autoshipSchedules);

				// Only run this if there is a value for autoship reminder days offset.
				if (autoshipReminderDate != default(DateTime))
				{
					IEnumerable<int> autoshipScheduleIDs = autoshipSchedules.Select(x => x.AutoshipScheduleID);

					list = repository.QueueAutoshipReminders(autoshipReminderDate, autoshipScheduleIDs);

					if (list != null)
					{
						foreach (var item in list)
						{
							item.StartTracking();
							item.IsLazyLoadingEnabled = true;
						}
					}
				}

				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}



		protected virtual DateTime GetNearestAvailableAutoshipDateInNextInterval(
			DateTime currentDate,
			IntervalType intervalType,
			IEnumerable<int> availableDays,
			int preferredDay)
		{
			if (intervalType == null)
			{
				throw new ArgumentNullException("intervalType");
			}
			if (availableDays == null)
			{
				throw new ArgumentNullException("availableDays");
			}
			if (preferredDay < 1 || preferredDay > 31)
			{
				throw new ArgumentException("preferredDay must be between 1 and 31.", "preferredDay");
			}


			if (intervalType.IsWeekly)
            {
                return intervalType.GetStartOfNextInterval(currentDate);
            }
            else if (intervalType.IsMonthly)
            {
                var nextIntervalDate = intervalType.GetStartOfNextInterval(currentDate);
                var targetDate = new DateTime(
                       nextIntervalDate.Year,
                       nextIntervalDate.Month,
                       Math.Min(preferredDay, DateTime.DaysInMonth(nextIntervalDate.Year, nextIntervalDate.Month))
                );
                return GetNearestAvailableAutoshipDateInMonth(availableDays, targetDate);
            }
            else if (intervalType.IsAnnual)
            {
                var nextIntervalDate = intervalType.GetStartOfNextInterval(currentDate);
                // Because this is an annual interval type, nextIntervalDate will come back as January 1st, but we use
                // the month from currentDate instead to ensure that the autoship is scheduled a full year from now.
                var targetDate = new DateTime(
                       nextIntervalDate.Year,
                       currentDate.Month,
                       Math.Min(preferredDay, DateTime.DaysInMonth(nextIntervalDate.Year, currentDate.Month))
                );
                return GetNearestAvailableAutoshipDateInMonth(availableDays, targetDate);
            }
            else
            {
                throw new ArgumentException("Unknown interval type.", "intervalType");
            }
        }

		protected virtual DateTime GetNearestAvailableAutoshipDateInMonth(
			IEnumerable<int> availableDays,
			DateTime targetDate)
		{
			if (availableDays == null)
			{
				throw new ArgumentNullException("availableDays");
			}

			int daysInMonth = DateTime.DaysInMonth(targetDate.Year, targetDate.Month);

			// If no days are available, then ALL days are available.
			if (!availableDays.Any())
			{
				return targetDate;
			}

			var availableDaysInMonth = availableDays.Where(x => x <= daysInMonth);
			// If no days are available in THIS month, return the last day.
			if (!availableDaysInMonth.Any())
			{
				return new DateTime(targetDate.Year, targetDate.Month, daysInMonth);
			}

			int nearestAvailableDay = availableDaysInMonth.Any(x => x >= targetDate.Day)
				// Return the closest available day ON OR AFTER the target day.
				? availableDaysInMonth.Where(x => x >= targetDate.Day).OrderBy(x => x).First()
				// Else return the closest available day BEFORE the target day.
				: availableDaysInMonth.OrderByDescending(x => x).First();

			return new DateTime(targetDate.Year, targetDate.Month, nearestAvailableDay);
		}

		public virtual List<AutoshipScheduleOverviewData> GetOverviews(
			IAutoshipOrderRepository repository,
			int accountID,
			short accountTypeID)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}

			var autoshipScheduleIDs = SmallCollectionCache.Instance.AutoshipSchedules
				.Where(s => s.Active && s.AccountTypes.Any(at => at.AccountTypeID == accountTypeID))
				.Select(s => s.AutoshipScheduleID);

			var autoshipOverviews = repository.LoadOverviews(
				accountID,
				autoshipScheduleIDs,
				includeAllActiveOrders: true,
				includeOrderItemData: true
			);

			// Get autoship schedules
			var autoshipSchedules = SmallCollectionCache.Instance.AutoshipSchedules
				.Where(x => autoshipScheduleIDs.Contains(x.AutoshipScheduleID)
					// Also include autoship schedules for any autoship orders returned by the query
					|| autoshipOverviews.Any(ao => ao.AutoshipScheduleID == x.AutoshipScheduleID))
				.ToList();

			// Get product info for fixed autoship schedules (if needed).
			var autoshipScheduleProducts = autoshipSchedules
				.Where(x => autoshipOverviews.Any(ao => ao.AutoshipScheduleID == x.AutoshipScheduleID))
				.ToDictionary(
					x => x.AutoshipScheduleID,
					x => SmallCollectionCache.Instance.AutoshipScheduleProducts
						.Where(asp => asp.AutoshipScheduleID == x.AutoshipScheduleID)
						.Select(asp => new AutoshipOverviewData.OrderItemData
						{
							Quantity = asp.Quantity,
                            SKU = Inventory.GetProduct(asp.ProductID).SKU
						})
						.ToList()
				);

			autoshipOverviews.ForEach(x =>
			{
				// Calculate Active status for each overview    
				string statusText;
				x.Active = IsAutoshipOrderActive(
					DateTime.Today,
					x.AccountStatusID,
					x.AutoshipScheduleID,
					x.OrderTypeID,
					x.OrderStatusID,
					x.OrderItems.Count(),
					x.StartDate,
					x.EndDate,
					x.NextRunDate,
					out statusText
				);
				x.StatusText = statusText;

				// Replace order items on overviews with fixed items.
				if (autoshipScheduleProducts[x.AutoshipScheduleID].Any())
				{
					x.OrderItems = autoshipScheduleProducts[x.AutoshipScheduleID]
						.ToList();
				}
			});

			var autoshipScheduleOverviews = autoshipSchedules
				.Select(x => new AutoshipScheduleOverviewData
				{
					AutoshipScheduleID = x.AutoshipScheduleID,
					AutoshipScheduleTypeID = x.AutoshipScheduleTypeID,
					Active = x.Active,
					LocalizedName = x.GetTerm(),
					AutoshipOverviews = autoshipOverviews
						.Where(ao => ao.AutoshipScheduleID == x.AutoshipScheduleID)
						.ToList(),
					IsTemplateEditable = x.IsTemplateEditable,
					IsEnrollable = x.IsEnrollable
				})
				.ToList();

			return autoshipScheduleOverviews;
		}

		public virtual bool IsAutoshipOrderActive(
			DateTime runDate,
			short accountStatusID,
			int autoshipScheduleID,
			short orderTypeID,
			short orderStatusID,
			int orderItemsCount,
			DateTime? startDate,
			DateTime? endDate,
			DateTime? nextRunDate,
			out string statusText)
		{
			// Schedule
			if (!SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipScheduleID).Active)
			{
				statusText = _autoshipStatusScheduleInactiveString;
				return false;
			}

			// Account
			if (!SmallCollectionCache.Instance.AccountStatuses.GetById(accountStatusID).ReportAsActive)
			{
				statusText = _autoshipStatusAccountInactiveString;
				return false;
			}

			// Order
			if (!SmallCollectionCache.Instance.OrderTypes.GetById(orderTypeID).IsTemplate)
			{
				statusText = _autoshipStatusInvalidOrderTypeString;
				return false;
			}
			if (orderStatusID == (short)Constants.OrderStatus.Pending)
			{
				statusText = _autoshipStatusOrderPendingString;
				return false;
			}
			if (orderStatusID != (short)Constants.OrderStatus.Paid)
			{
				statusText = _autoshipStatusOrderCancelledString;
				return false;
			}
			if (orderItemsCount == 0)
			{
				statusText = _autoshipStatusOrderHasNoItems;
				return false;
			}

			// Dates
			if (startDate != null && startDate.Value.Date > runDate.Date)
			{
				statusText = _autoshipStatusNotStartedString;
				return false;
			}
			if (endDate != null && endDate.Value.Date <= runDate.Date)
			{
				statusText = _autoshipStatusEndedString;
				return false;
			}
			if (nextRunDate == null)
			{
				statusText = _autoshipStatusInvalidNextRunDateString;
				return false;
			}

			statusText = string.Format(_autoshipStatusActiveString, nextRunDate);
			return true;
		}

		public virtual List<AutoshipOrder> LoadAllByScheduleID(IAutoshipOrderRepository repository, int scheduleID)
		{
			return repository.LoadAllByScheduleID(scheduleID);
		}

		public virtual List<AutoshipOrder> LoadBatchFullByAccountID(IAutoshipOrderRepository repository, List<int> accountIDs)
		{
			return repository.LoadBatchFullByAccountID(accountIDs);
		}

		#region Strings
		protected virtual string _autoshipStatusScheduleInactiveString { get { return Translation.GetTerm("AutoshipStatus_ScheduleInactive", "Schedule Inactive"); } }
		protected virtual string _autoshipStatusAccountInactiveString { get { return Translation.GetTerm("AutoshipStatus_AccountInactive", "Account Inactive"); } }
		protected virtual string _autoshipStatusInvalidOrderTypeString { get { return Translation.GetTerm("AutoshipStatus_InvalidOrderType", "Invalid Order Type"); } }
		protected virtual string _autoshipStatusOrderPendingString { get { return Translation.GetTerm("AutoshipStatus_OrderPending", "Order Pending"); } }
		protected virtual string _autoshipStatusOrderCancelledString { get { return Translation.GetTerm("AutoshipStatus_OrderCancelled", "Order Cancelled"); } }
		protected virtual string _autoshipStatusOrderHasNoItems { get { return Translation.GetTerm("AutoshipStatus_OrderHasNoItems", "Order Has No Items"); } }
		protected virtual string _autoshipStatusNotStartedString { get { return Translation.GetTerm("AutoshipStatus_NotStarted", "Not Started"); } }
		protected virtual string _autoshipStatusEndedString { get { return Translation.GetTerm("AutoshipStatus_Ended", "Ended"); } }
		protected virtual string _autoshipStatusInvalidNextRunDateString { get { return Translation.GetTerm("AutoshipStatus_InvalidNextRunDate", "Invalid Next Run Date"); } }
		protected virtual string _autoshipStatusActiveString { get { return Translation.GetTerm("AutoshipStatus_Active", "Due {0:d}"); } }
		#endregion
	}
}
