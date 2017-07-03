using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IAutoshipOrderRepository
    {
        AutoshipOrder LoadByAccountIDAndAutoshipScheduleID(int accountID, int autoshipScheduleID);
        AutoshipOrder LoadFullByAccountIDAndAutoshipScheduleID(int accountID, int autoshipScheduleID);
        List<AutoshipOrder> LoadAllFullByAccountID(int accountID);
        AutoshipOrder LoadFullByOrderID(int orderID);
        List<AutoshipProcessInfo> GetAutoshipTemplatesByNextDueDateByAutoshipScheduleID(int autoshipScheduleID, DateTime nextDueDate);

        PaginatedList<AutoshipBatchReportData> GetAutoshipRunReport(AutoshipOrderSearchParameters searchParameters);
        PaginatedList<AutoshipLogReportData> LoadAutoshipLogsByAutoshipBatchID(AutoshipLogSearchParameters searchParameters);

        PaginatedList<AutoshipOrder> QueueAutoshipReminders(DateTime daysOffSet, IEnumerable<int> autoshipScheduleIDs);
        List<AutoshipOverviewData> LoadOverviews(int accountID, IEnumerable<int> autoshipScheduleIDs, bool includeAllActiveOrders = false, bool includeOrderItemData = false);
        List<AutoshipOrder> LoadAllByScheduleID(int scheduleID);
        List<AutoshipOrder> LoadBatchFullByAccountID(List<int> accountIDs);
    }
}
