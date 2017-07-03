using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities
{
	public partial class AutoshipOrder : IAutoshipOrder
	{
		private IOrderService _orderService;
		public IOrderService OrderService
		{
			get
			{
				if (_orderService == null)
				{
					_orderService = Create.New<IOrderService>();
				}
				return _orderService;
			}
		}

		/// <summary>
		/// Related entities that can be included by the 'Load' methods (see <see cref="LoadRelationsExtensions"/>).
		/// WARNING: Changes to this list will affect various 'Load' methods, be careful.
		/// </summary>
		[Flags]
		public enum Relations
		{
			// These are bit flags so they must be numbered appropriately (eg. 0, 1, 2, 4, 8, 16)
			// Use bit-shifting for convenience (eg. 0, 1 << 0, 1 << 1, 1 << 2)
			None = 0,
			Account = 1 << 0,
			OrderLoadFull = 1 << 1,
			Sites = 1 << 2,

			/// <summary>
			/// The default relations used by the 'LoadFull' methods.
			/// </summary>
			LoadFull =
				Account
				| OrderLoadFull,

			/// <summary>
			/// Used by the 'LoadForAudit' method.
			/// </summary>
			LoadForAudit =
				Account
				| OrderLoadFull
				| Sites,
		};

		private AccountSlimSearchData _accountInfo;
		public AccountSlimSearchData AccountInfo
		{
			get
			{
				if (_accountInfo == null && this.AccountID > 0)
					_accountInfo = Account.LoadSlim(this.AccountID);
				return _accountInfo;
			}
		}

		/// <summary>
		/// Returns the first run date for a new autoship order.
		/// </summary>
		public DateTime CalculateFirstRunDate()
		{
			return BusinessLogic.CalculateFirstRunDate(this);
		}

		public DateTime CalculateNextRunDateOnSuccess(DateTime currentDate)
		{
			return BusinessLogic.CalculateNextRunDateOnSuccess(currentDate, this);
		}

		public DateTime CalculateNextRunDateOnFailure(DateTime currentDate)
		{
			return BusinessLogic.CalculateNextRunDateOnFailure(currentDate, this);
		}

		public int CalculateConsecutiveOrders()
		{
			return BusinessLogic.CalculateConsecutiveOrders(this);
		}

		public static AutoshipOrder LoadByAccountIDAndAutoshipScheduleID(int accountID, int autoshipScheduleID)
		{
			try
			{
				return BusinessLogic.LoadByAccountIDAndAutoshipScheduleID(Repository, accountID, autoshipScheduleID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static AutoshipOrder LoadFullByAccountIDAndAutoshipScheduleID(int accountID, int autoshipScheduleID)
		{
			try
			{
				return BusinessLogic.LoadFullByAccountIDAndAutoshipScheduleID(Repository, accountID, autoshipScheduleID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<AutoshipOrder> LoadAllFullByAccountID(int accountID)
		{
			try
			{
				return BusinessLogic.LoadAllFullByAccountID(Repository, accountID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static PaginatedList<AutoshipOrder> QueueAutoshipReminders()
		{
			try
			{
				return BusinessLogic.QueueAutoshipReminders(Repository);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void SendAutoshipReminderAutoResponder(IEnumerable<AutoshipOrder> autoshipOrders)
		{
			try
			{
				BusinessLogic.SendAutoshipReminderAutoResponder(autoshipOrders);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static AutoshipOrder LoadFullByOrderID(int orderID)
		{
			try
			{
				return BusinessLogic.LoadFullByOrderID(Repository, orderID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		public Order GenerateChildOrderFromTemplate()
		{
			try
			{
				return BusinessLogic.GenerateChildOrderFromTemplate(this.Order);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public override void Save()
		{
			try
			{
				if (this.Order == null && this.TemplateOrderID == 0)
					throw new Exception("Error saving AutoshipOrder. The Order on AutoshipOrder must be set.");

				bool newAutoship = this.TemplateOrderID == 0;

				base.Save();

				if (this.Order != null)
				{
					if (this.Order.CurrencyID == 0)
					{
						OrderService.SetCurrencyID(Order);
					}

					if (newAutoship || string.IsNullOrEmpty(this.Order.OrderNumber))
					{
						OrderService.GenerateAndSetNewOrderNumber(Order);

						base.Save();
					}
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		public static List<AutoshipProcessInfo> GetAutoshipTemplatesByNextDueDateByScheduleID(int autoshipScheduleID, DateTime nextDueDate)
		{
			try
			{
				return BusinessLogic.GetAutoshipTemplatesByNextDueDateByScheduleID(Repository, autoshipScheduleID, nextDueDate);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void LogAutoshipGenerationResults(int autoshipBatchID, int templateOrderId, int? newOrderId, bool succeeded, string results, DateTime runDate)
		{
			try
			{
				BusinessLogic.LogAutoshipGenerationResults(autoshipBatchID, templateOrderId, newOrderId, succeeded, results, runDate);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		public static PaginatedList<AutoshipBatchReportData> GetAutoshipRunReport(AutoshipOrderSearchParameters searchParameters)
		{
			try
			{
				return BusinessLogic.GetAutoshipRunReport(Repository, searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static PaginatedList<AutoshipLogReportData> LoadAutoshipLogsByAutoshipBatchID(AutoshipLogSearchParameters searchParameters)
		{
			try
			{
				return BusinessLogic.LoadAutoshipLogsByAutoshipBatchID(Repository, searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static AutoshipOrder GenerateTemplateFromSchedule(int scheduleId, Account account, int marketId, bool saveAndChargeNewOrder = true)
		{
			try
			{
				return BusinessLogic.GenerateTemplateFromSchedule(scheduleId, account, marketId, saveAndChargeNewOrder: saveAndChargeNewOrder);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static AutoshipOrder GenerateTemplateFromSchedule(int scheduleId, Account account, int marketId, NetSteps.Addresses.Common.Models.IAddress shippingAddress, IPayment paymentMethod, bool saveAndChargeNewOrder = true)
		{
			try
			{
				return BusinessLogic.GenerateTemplateFromSchedule(scheduleId, account, marketId, shippingAddress, paymentMethod, saveAndChargeNewOrder: saveAndChargeNewOrder);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public bool IsSiteSubscriptionTemplate()
		{
			try
			{
				return BusinessLogic.IsSiteSubscriptionTemplate(this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		public bool HasFixedTemplateItems()
		{
			try
			{
				return BusinessLogic.HasFixedTemplateItems(this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		public void SyncFixedOrderItems(bool removePendingPayments = true)
		{
			try
			{
				BusinessLogic.SyncFixedOrderItems(this, removePendingPayments: removePendingPayments);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public string GetWarningMessageForAutoshipChange()
		{
			try
			{
				return BusinessLogic.GetWarningMessageForAutoshipChange(Repository, this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static short GetDefaultAutoshipTemplateOrderTypeID(short accountTypeID)
		{
			try
			{
				return BusinessLogic.GetDefaultOrderTypeID(accountTypeID, true, null);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static short GetDefaultGeneratedAutoshipOrderTypeID(short accountTypeID, int? templateOrderTypeID = null)
		{
			try
			{
				return BusinessLogic.GetDefaultGeneratedAutoshipOrderTypeID(accountTypeID, templateOrderTypeID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public void SetScheduleDay(int autoshipDay)
		{
			this.Day = autoshipDay;
		}

		public static List<AutoshipScheduleOverviewData> GetOverviews(int accountID, short accountTypeID)
		{
			return BusinessLogic.GetOverviews(Repository, accountID, accountTypeID);
		}

		public static List<AutoshipOrder> LoadAllByScheduleID(int scheduleID)
		{
			try
			{
				return BusinessLogic.LoadAllByScheduleID(Repository, scheduleID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<AutoshipOrder> LoadBatchFullByAccountID(List<int> accountIDs)
		{
			try
			{
				return BusinessLogic.LoadBatchFullByAccountID(Repository, accountIDs);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
	}
}