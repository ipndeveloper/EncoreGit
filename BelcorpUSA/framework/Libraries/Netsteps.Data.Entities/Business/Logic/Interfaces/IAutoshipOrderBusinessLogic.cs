using System;
using System.Collections.Generic;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	public partial interface IAutoshipOrderBusinessLogic
	{
		/// <summary>
		/// Returns the first run date for a new autoship order.
		/// </summary>
		DateTime CalculateFirstRunDate(AutoshipOrder autoshipOrder);
		DateTime CalculateFirstRunDate(DateTime currentDate, AutoshipSchedule autoshipSchedule, IntervalType intervalType, IEnumerable<int> availableDays, int preferredDay);
		DateTime CalculateNextRunDateOnSuccess(DateTime currentDate, AutoshipOrder autoshipOrder);
		DateTime CalculateNextRunDateOnSuccess(DateTime currentDate, AutoshipSchedule autoshipSchedule, IntervalType intervalType, IEnumerable<int> availableDays, int preferredDay);
		DateTime CalculateNextRunDateOnFailure(DateTime currentDate, AutoshipOrder autoshipOrder);
		DateTime CalculateNextRunDateOnFailure(DateTime currentDate, AutoshipSchedule autoshipSchedule, IntervalType intervalType, IEnumerable<int> availableDays, int preferredDay, int totalAttemptsThisInterval, bool hasSucceededThisInterval);
		Order GenerateChildOrderFromTemplate(Order order);
		short GetDefaultOrderTypeID(short accountTypeID, bool isTemplate = false, int? templateOrderTypeID = null);
		void ValidateOrderTypeByAccountType(short orderTypeID, short accountTypeID);
		void LogAutoshipGenerationResults(int autoshipBatchID, int templateOrderId, int? newOrderId, bool succeeded, string results, DateTime runDate);
		AutoshipOrder GenerateTemplateFromSchedule(int scheduleId, Account account, int marketId, int? siteId = null, bool saveAndChargeNewOrder = true);
		AutoshipOrder GenerateTemplateFromSchedule(int scheduleId, Account account, int marketId, IAddress shippingAddress, IPayment paymentMethod, int? siteId = null, bool saveAndChargeNewOrder = true);

		AutoshipOrder LoadByAccountIDAndAutoshipScheduleID(Repositories.IAutoshipOrderRepository repository, int accountID, int autoshipScheduleID);
		AutoshipOrder LoadFullByAccountIDAndAutoshipScheduleID(Repositories.IAutoshipOrderRepository repository, int accountID, int autoshipScheduleID);
		List<AutoshipOrder> LoadAllFullByAccountID(Repositories.IAutoshipOrderRepository repository, int accountID);
		AutoshipOrder LoadFullByOrderID(Repositories.IAutoshipOrderRepository repository, int orderID);
		List<AutoshipProcessInfo> GetAutoshipTemplatesByNextDueDateByScheduleID(Repositories.IAutoshipOrderRepository repository, int autoshipScheduleID, DateTime nextDueDate);
		PaginatedList<AutoshipBatchReportData> GetAutoshipRunReport(Repositories.IAutoshipOrderRepository repository, AutoshipOrderSearchParameters searchParameters);
		PaginatedList<AutoshipLogReportData> LoadAutoshipLogsByAutoshipBatchID(Repositories.IAutoshipOrderRepository repository, AutoshipLogSearchParameters searchParameters);

		bool IsSiteSubscriptionTemplate(AutoshipOrder autoshipOrder);
		bool HasFixedTemplateItems(AutoshipOrder autoshipOrder);
		void SyncFixedOrderItems(AutoshipOrder autoshipOrder, bool removePendingPayments = true);
		void SyncFixedOrderItems(AutoshipOrder autoshipOrder, IEnumerable<AutoshipScheduleProduct> autoshipScheduleProducts, bool removePendingPayments = true);

		string GetWarningMessageForAutoshipChange(Repositories.IAutoshipOrderRepository repository, AutoshipOrder autoshipOrder);
		short GetDefaultGeneratedAutoshipOrderTypeID(short accountTypeID, int? templateOrderTypeID = null);
		int CalculateConsecutiveOrders(AutoshipOrder autoshipOrder);
		int CalculateConsecutiveOrders(IEnumerable<DateTime> completedOrderDates);

		void SendAutoshipReminderAutoResponder(IEnumerable<AutoshipOrder> autoshipOrders);
		PaginatedList<AutoshipOrder> QueueAutoshipReminders(Repositories.IAutoshipOrderRepository repository);
		DateTime GetAutoshipReminderDaysOffSet(IEnumerable<AutoshipSchedule> autoshipSchedules);
		List<AutoshipScheduleOverviewData> GetOverviews(IAutoshipOrderRepository repository, int accountID, short accountTypeID);
		List<AutoshipOrder> LoadAllByScheduleID(IAutoshipOrderRepository repository, int scheduleID);
		List<AutoshipOrder> LoadBatchFullByAccountID(IAutoshipOrderRepository repository, List<int> accountIDs);
	}
}
