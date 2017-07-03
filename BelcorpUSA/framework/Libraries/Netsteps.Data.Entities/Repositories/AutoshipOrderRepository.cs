using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AutoshipOrderRepository
    {
		private Expression<Func<AutoshipOrder, bool>> GetByAccountIDAndAutoshipScheduleIDPredicate(int accountID, int autoshipScheduleID)
		{
			return ao =>
				ao.AccountID == accountID
				&& ao.AutoshipScheduleID == autoshipScheduleID
				// Only return "Paid" orders because "Paid" is the only valid status for
				// an active autoship. Every other status is considered "Cancelled".
				&& ao.Order.OrderStatusID == (int)Constants.OrderStatus.Paid;
		}

        public AutoshipOrder LoadByAccountIDAndAutoshipScheduleID(int accountID, int autoshipScheduleID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.AutoshipOrders.FirstOrDefault(
						GetByAccountIDAndAutoshipScheduleIDPredicate(accountID, autoshipScheduleID)
                    );
                }
            });
        }

        public AutoshipOrder LoadFullByAccountIDAndAutoshipScheduleID(int accountID, int autoshipScheduleID)
        {
            return FirstOrDefaultFull(
				GetByAccountIDAndAutoshipScheduleIDPredicate(accountID, autoshipScheduleID)
            );
        }

        public List<AutoshipOrder> LoadAllFullByAccountID(int accountID)
        {
            return WhereFull(
                ao => ao.AccountID == accountID
            );
        }

        public PaginatedList<AutoshipOrder> QueueAutoshipReminders(DateTime daysOffSet, IEnumerable<int> autoshipScheduleIDs)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var results = new PaginatedList<AutoshipOrder>();

                    IQueryable<AutoshipOrder> reminders = from a in AutoshipOrdersToRun(context, daysOffSet, autoshipScheduleIDs)
                                                          where a.AutoshipReminderNextRunDate == null ||
                                                                    a.NextRunDate != a.AutoshipReminderNextRunDate
                                                          select a;


                    results.AddRange(reminders.ToList());

                    return results;
                }
            });
        }

        public AutoshipOrder LoadFullByOrderID(int orderID)
        {
            return FirstOrDefaultFull(
                ao => ao.TemplateOrderID == orderID
            );
        }

        private IQueryable<AutoshipOrder> AutoshipOrdersToRun(NetStepsEntities context, DateTime runDate, IEnumerable<int> autoshipScheduleIDs)
        {
            IQueryable<AutoshipOrder> query = null;

            if (context != null)
            {
                // Get 12:00am
                runDate = runDate.Date;
                // Get 12:00am the next day
                var dayAfterRunDate = runDate.AddDays(1);

                query = from a in context.AutoshipOrders
                        where
                             a.Order.OrderStatusID == (int)Constants.OrderStatus.Paid
                            && a.Order.OrderType.IsTemplate == true
                            && a.Account.AccountStatus.ReportAsActive
                            && a.NextRunDate < dayAfterRunDate
                            && (a.StartDate == null || a.StartDate < dayAfterRunDate)
                            && (a.EndDate == null || a.EndDate > runDate)
                            && autoshipScheduleIDs.Contains(a.AutoshipScheduleID)
                        select a;
            }

            return query;
        }

        /// <summary>
        /// Gets autoship orders that are scheduled to run on the specified run date.
        /// </summary>
        public List<AutoshipProcessInfo> GetAutoshipTemplatesByNextDueDateByAutoshipScheduleID(int autoshipScheduleID, DateTime runDate)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    List<AutoshipProcessInfo> results = new List<AutoshipProcessInfo>();

                    var query = from a in AutoshipOrdersToRun(context, runDate, new[] { autoshipScheduleID })
                                select new AutoshipProcessInfo
                                           {
                                               AutoshipOrderID = a.AutoshipOrderID,
                                               AutoshipScheduleID = a.AutoshipScheduleID,
                                               TemplateOrderID = a.TemplateOrderID,
                                               TemplateOrderNumber = a.Order.OrderNumber,
                                               AccountID = a.AccountID,
                                               AccountNumber = a.Account.AccountNumber,
                                               FirstName = a.Account.FirstName,
                                               LastName = a.Account.LastName
                                           };

                    results.AddRange(query);
                    return results;
                }
            });
        }

        /// <summary>
        /// Ported from usp_autoshiplogs_report - JHE
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public PaginatedList<AutoshipBatchReportData> GetAutoshipRunReport(AutoshipOrderSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<AutoshipBatchReportData> results = new PaginatedList<AutoshipBatchReportData>(searchParameters);

                    var matchingItems = from a in context.AutoshipBatches
                                        join u in context.Users on a.UserID equals u.UserID
                                        select new
                                        {
                                            a.AutoshipBatchID,
                                            a.StartDateUTC,
                                            a.EndDateUTC,
                                            u.Username,
                                            a.Notes,
                                            SucceededCount = (from al in context.AutoshipLogs
                                                              where al.AutoshipBatchID == a.AutoshipBatchID && al.Succeeded == true
                                                              select al).Count(),
                                            FailureCount = (from al in context.AutoshipLogs
                                                            where al.AutoshipBatchID == a.AutoshipBatchID && al.Succeeded == false
                                                            select al).Count()
                                        };

                    //if (searchParameters.WhereClause != null)
                    //    matchingItems = matchingItems.Where(searchParameters.WhereClause);

                    if (!searchParameters.OrderBy.IsNullOrEmpty())
                        matchingItems = matchingItems.ApplyOrderByFilter(searchParameters, context);
                    else
                        matchingItems = matchingItems.OrderBy(o => o.StartDateUTC);

                    results.TotalCount = matchingItems.Count();

                    if (searchParameters.PageSize.HasValue)
                        matchingItems = matchingItems.Skip(searchParameters.PageIndex * searchParameters.PageSize.Value).Take(searchParameters.PageSize.Value);

                    var ordersInfos = from o in matchingItems
                                      select new
                                      {
                                          o.AutoshipBatchID,
                                          o.StartDateUTC,
                                          o.EndDateUTC,
                                          o.Username,
                                          o.Notes,
                                          o.SucceededCount,
                                          o.FailureCount
                                      };

                    foreach (var o in ordersInfos.ToList())
                        results.Add(new AutoshipBatchReportData()
                        {
                            AutoshipBatchID = o.AutoshipBatchID,
                            StartDate = o.StartDateUTC.HasValue ? o.StartDateUTC.UTCToLocal() : (DateTime?)null,
                            EndDate = o.EndDateUTC.HasValue ? o.EndDateUTC.UTCToLocal() : (DateTime?)null,
                            UserName = o.Username,
                            Notes = o.Notes,
                            SucceededCount = o.SucceededCount,
                            FailureCount = o.FailureCount
                        });

                    return results;
                }
            });
        }

        /// <summary>
        /// Ported from usp_autoshiplogs_select_by_autoshipbatchid - JHE
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public PaginatedList<AutoshipLogReportData> LoadAutoshipLogsByAutoshipBatchID(AutoshipLogSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<AutoshipLogReportData> results = new PaginatedList<AutoshipLogReportData>(searchParameters);

                    var matchingItems = from a in context.AutoshipLogs
                                        join ao in context.AutoshipOrders on a.TemplateOrderID equals ao.TemplateOrderID
                                        join ac in context.Accounts on ao.AccountID equals ac.AccountID
                                        where a.AutoshipBatchID == searchParameters.AutoshipBatchID
                                        select new
                                        {
                                            ao.AccountID,
                                            ac.AccountNumber,
                                            ao.AutoshipScheduleID,
                                            a.AutoshipLogID,
                                            a.TemplateOrderID,
                                            a.NewOrderID,
                                            NewOrderNumber = context.Orders.FirstOrDefault(o => o.OrderID == a.NewOrderID).OrderNumber,
                                            a.Succeeded,
                                            a.Results,
                                            a.RunDate
                                        };

                    //if (searchParameters.WhereClause != null)
                    //    matchingItems = matchingItems.Where(searchParameters.WhereClause);

                    if (!searchParameters.OrderBy.IsNullOrEmpty())
                        matchingItems = matchingItems.ApplyOrderByFilter(searchParameters, context);
                    else
                        matchingItems = matchingItems.OrderBy(o => o.AutoshipLogID);

                    results.TotalCount = matchingItems.Count();

                    if (searchParameters.PageSize.HasValue)
                        matchingItems = matchingItems.Skip(searchParameters.PageIndex * searchParameters.PageSize.Value).Take(searchParameters.PageSize.Value);

                    foreach (var o in matchingItems.ToList())
                        results.Add(new AutoshipLogReportData()
                        {
                            AccountID = o.AccountID,
                            AccountNumber = o.AccountNumber,
                            AutoshipScheduleID = o.AutoshipScheduleID,
                            AutoshipLogID = o.AutoshipLogID,
                            TemplateOrderID = o.TemplateOrderID,
                            NewOrderID = o.NewOrderID,
                            NewOrderNumber = o.NewOrderNumber,
                            Succeeded = o.Succeeded,
                            Results = o.Results,
                            DateAutoshipRan = o.RunDate
                        });

                    return results;
                }
            });

        }

        public override PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, AuditLogSearchParameters searchParameters)
        {
            AutoshipOrder autoshipOrder = null;
            using (NetStepsEntities context = CreateContext())
            {
                autoshipOrder = LoadForAudit(context, primaryKey);
            }

            List<AuditTableValueItem> list = new List<AuditTableValueItem>();

            AddRecordToAuditTable(list, EntitySetName, Convert.ToInt32(primaryKey));

            if (HasValidOrder(autoshipOrder))
            {
                AddRecordToAuditTable(list, "Orders", autoshipOrder.Order.OrderID);

                if (HasOrderCustomer(autoshipOrder))
                {
                    foreach (var orderCustomer in autoshipOrder.Order.OrderCustomers)
                    {
                        AddRecordToAuditTable(list, "OrderCustomers", orderCustomer.OrderCustomerID);

                        AddOrderItemsAndReturnsRecordToAuditTable(list, orderCustomer);

                        AddOrderPaymentsRecordToAuditTable(list, orderCustomer);

                        AddOrderShipmentsRecordToAuditTable(list, orderCustomer);

                    }
                }
            }

            if (HasSites(autoshipOrder))
            {
                foreach (var site in autoshipOrder.Sites)
                {
                    AddRecordToAuditTable(list, "Sites", site.SiteID);

                    AddSitesRecordToAuditTable(list, site);
                }
            }

            return GetAuditLog(list, searchParameters);
        }

        /// <summary>
        /// Retrieves autoship overview info from the database.
        /// </summary>
        /// <param name="accountID">The account ID whose autoships are being loaded</param>
        /// <param name="autoshipScheduleIDs">The autoship schedules to include</param>
        /// <param name="includeAllActiveOrders">Indicates if active autoships from other autoship schedules should also be returned</param>
        /// <param name="includeOrderItemData">Indicates if order item data should be included in the result</param>
        /// <returns>A <see cref="List<AutoshipOverviewData>"/> containing the overview data for the given account</returns>
        public List<AutoshipOverviewData> LoadOverviews(
            int accountID,
            IEnumerable<int> autoshipScheduleIDs,
            bool includeAllActiveOrders = false,
            bool includeOrderItemData = false)
        {
            if (autoshipScheduleIDs == null)
            {
                throw new ArgumentNullException("autoshipScheduleIDs");
            }

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    // "Select" before "Where" seems to result in slightly better SQL here. - Lundy
                    var query = context.AutoshipOrders
                        .Select(x => new AutoshipOverviewData
                        {
                            AccountID = x.AccountID,
                            AccountStatusID = x.Account.AccountStatusID,
                            AutoshipOrderID = x.AutoshipOrderID,
                            AutoshipScheduleID = x.AutoshipScheduleID,
                            TemplateOrderID = x.TemplateOrderID,
                            TemplateOrderNumber = x.Order.OrderNumber,
                            OrderTypeID = x.Order.OrderTypeID,
                            OrderStatusID = x.Order.OrderStatusID,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            NextRunDate = x.NextRunDate
                        });

                    // Start with all autoships for the account
                    query = query
                        .Where(x => x.AccountID == accountID);

                    // Filter by schedule / status
                    query = includeAllActiveOrders
                        ? query.Where(x =>
                            autoshipScheduleIDs.Contains(x.AutoshipScheduleID)
                            || x.OrderStatusID == (short)Constants.OrderStatus.Paid)
                        : query.Where(x =>
                            autoshipScheduleIDs.Contains(x.AutoshipScheduleID));

                    var autoshipOverviews = query
                        .ToList();

                    // Get order items (it's faster to query them separately)
                    if (includeOrderItemData)
                    {
                        var orderIDs = autoshipOverviews
                            .Select(x => x.TemplateOrderID)
                            .ToList();

                        // "Select" before "Where" is MUCH faster here. - Lundy
                        var orderItems = context.OrderItems
                            .Select(oi => new
                            {
                                oi.OrderCustomer.OrderID,
                                oi.Quantity,
                                oi.SKU
                            })
                            .Where(x => orderIDs.Contains(x.OrderID))
                            .ToList();

                        autoshipOverviews.ForEach(x =>
                        {
                            x.OrderItems = orderItems
                                .Where(oi => oi.OrderID == x.TemplateOrderID)
                                .Select(oi => new AutoshipOverviewData.OrderItemData
                                {
                                    Quantity = oi.Quantity,
                                    SKU = oi.SKU
                                })
                                .ToList();
                        });
                    }
                    else
                    {
                        autoshipOverviews.ForEach(x => x.OrderItems = new List<AutoshipOverviewData.OrderItemData>());
                    }

                    return autoshipOverviews;
                }
            });
        }

        public List<AutoshipOrder> LoadAllByScheduleID(int scheduleID)
        {
            return WhereFull(ao => ao.AutoshipScheduleID == scheduleID);
        }

        public List<AutoshipOrder> LoadBatchFullByAccountID(List<int> accountIDs)
        {
			return WhereFull(ao => accountIDs.Contains(ao.AccountID));
        }

        #region Helpers
        private AutoshipOrder LoadForAudit(NetStepsEntities context, int autoshipOrderID)
        {
            return FirstOrDefault(
				ao => ao.AutoshipOrderID == autoshipOrderID,
                AutoshipOrder.Relations.LoadForAudit
            );
        }

        private bool HasOrderCustomer(AutoshipOrder autoshipOrder)
        {
            return autoshipOrder.Order.OrderCustomers != null;
        }

        private void AddRecordToAuditTable(List<AuditTableValueItem> list, string tableName, int primaryKey)
        {
            AuditTableValueItem auditTable = new AuditTableValueItem { TableName = tableName, PrimaryKey = primaryKey };
            list.Add(auditTable);
        }

        private void AddOrderItemsAndReturnsRecordToAuditTable(List<AuditTableValueItem> list, OrderCustomer orderCustomer)
        {
            if (orderCustomer.OrderItems != null)
            {
                foreach (var orderItem in orderCustomer.OrderItems)
                {
                    AddRecordToAuditTable(list, "OrderItems", orderItem.OrderItemID);

                    AddReturnItemRecordToAuditTable(list, orderItem);
                }
            }
        }

        private void AddReturnItemRecordToAuditTable(List<AuditTableValueItem> list, OrderItem orderItem)
        {
            if (orderItem.OrderItemReturns != null)
            {
                foreach (var orderItemReturn in orderItem.OrderItemReturns)
                {
                    AddRecordToAuditTable(list, "OrderItemReturns", orderItemReturn.OrderItemReturnID);
                }
            }
        }

        private void AddOrderPaymentsRecordToAuditTable(List<AuditTableValueItem> list, OrderCustomer orderCustomer)
        {
            if (orderCustomer.OrderPayments != null)
            {
                foreach (var orderPayment in orderCustomer.OrderPayments)
                {
                    AddRecordToAuditTable(list, "OrderPayments", orderPayment.OrderPaymentID);
                }
            }
        }

        private void AddOrderShipmentsRecordToAuditTable(List<AuditTableValueItem> list, OrderCustomer orderCustomer)
        {
            if (orderCustomer.OrderShipments != null)
            {
                foreach (var orderShipment in orderCustomer.OrderShipments)
                {
                    AddRecordToAuditTable(list, "OrderShipments", orderShipment.OrderShipmentID);
                }
            }
        }

        private bool HasValidOrder(AutoshipOrder autoshipOrder)
        {
            return autoshipOrder.Order != null && autoshipOrder.Order.OrderID > 0;
        }

        private bool HasSites(AutoshipOrder autoshipOrder)
        {
            return autoshipOrder.Sites != null && autoshipOrder.Sites.Any();
        }

        private void AddSitesRecordToAuditTable(List<AuditTableValueItem> list, Site site)
        {
            if (site.SiteUrls != null)
            {
                foreach (var siteUrl in site.SiteUrls)
                {
                    AddRecordToAuditTable(list, "SiteUrls", siteUrl.SiteUrlID);
                }
            }
        }
		#endregion

		#region Load Helpers
		public override AutoshipOrder LoadFull(int autoshipOrderID)
		{
			var autoshipOrder = FirstOrDefaultFull(x => x.AutoshipOrderID == autoshipOrderID);

			if (autoshipOrder == null)
			{
				throw new NetStepsDataException(string.Format("No AutoshipOrder found with AutoshipOrderID = {0}.", autoshipOrderID));
			}

			return autoshipOrder;
		}

		public override List<AutoshipOrder> LoadBatchFull(IEnumerable<int> autoshipOrderIDs)
		{
			return WhereFull(x => autoshipOrderIDs.Contains(x.AutoshipOrderID));
		}

		public override List<AutoshipOrder> LoadAllFull()
		{
			return WhereFull(x => true);
		}

		public virtual AutoshipOrder FirstOrDefaultFull(Expression<Func<AutoshipOrder, bool>> predicate)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return FirstOrDefaultFull(predicate, context);
				}
			});
		}

		public virtual AutoshipOrder FirstOrDefaultFull(Expression<Func<AutoshipOrder, bool>> predicate, NetStepsEntities context)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);
			Contract.Requires<ArgumentNullException>(context != null);

			return FirstOrDefault(predicate, AutoshipOrder.Relations.LoadFull, context);
		}

		public virtual AutoshipOrder FirstOrDefault(Expression<Func<AutoshipOrder, bool>> predicate, AutoshipOrder.Relations relations)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return FirstOrDefault(predicate, relations, context);
				}
			});
		}

		public virtual AutoshipOrder FirstOrDefault(Expression<Func<AutoshipOrder, bool>> predicate, AutoshipOrder.Relations relations, NetStepsEntities context)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);
			Contract.Requires<ArgumentNullException>(context != null);

			var autoshipOrder = context.AutoshipOrders
				.FirstOrDefault(predicate);

			if (autoshipOrder == null)
			{
				return null;
			}

			autoshipOrder.LoadRelations(context, relations);

			return autoshipOrder;
		}

		public virtual List<AutoshipOrder> WhereFull(Expression<Func<AutoshipOrder, bool>> predicate)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return WhereFull(predicate, context);
				}
			});
		}

		public virtual List<AutoshipOrder> WhereFull(Expression<Func<AutoshipOrder, bool>> predicate, NetStepsEntities context)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);
			Contract.Requires<ArgumentNullException>(context != null);

			return Where(predicate, AutoshipOrder.Relations.LoadFull, context);
		}

		public virtual List<AutoshipOrder> Where(Expression<Func<AutoshipOrder, bool>> predicate, AutoshipOrder.Relations relations)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return Where(predicate, relations, context);
				}
			});
		}

		public virtual List<AutoshipOrder> Where(Expression<Func<AutoshipOrder, bool>> predicate, AutoshipOrder.Relations relations, NetStepsEntities context)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);
			Contract.Requires<ArgumentNullException>(context != null);

			var autoshipOrders = context.AutoshipOrders
				.Where(predicate)
				.ToList();

			autoshipOrders.LoadRelations(context, relations);

			return autoshipOrders;
		}
		#endregion
	}
}
