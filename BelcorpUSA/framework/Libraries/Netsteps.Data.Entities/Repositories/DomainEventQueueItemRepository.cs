using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class DomainEventQueueItemRepository
	{
		private static object _queueLock = new object();

		private PaginatedList<int> queueDomainEventItems(int maxNumberToPoll)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				var logger = Create.New<ILogger>();
				logger.Debug("queueDomainEventItems trying to get {0} items", maxNumberToPoll);

				lock (_queueLock)
				{
					using (NetStepsEntities context = CreateContext())
					{
						var results = new PaginatedList<int>();

						int attemptCountLimit = 5;
						short queuedStatusID = Constants.QueueItemStatus.Queued.ToShort();
						short runningStatusID = Constants.QueueItemStatus.Running.ToShort();
						short failedStatusID = Constants.QueueItemStatus.Failed.ToShort();
						TimeSpan retryInterval = TimeSpan.FromMinutes(10);
						DateTime cutoffTime = DateTime.UtcNow.Subtract(retryInterval);

						IQueryable<DomainEventQueueItem> domainEventQueueItems =
							from a in context.DomainEventQueueItems.Include("EventContext").Include("DomainEventType.DomainEventTypeCategory")
							where
								(a.QueueItemStatusID == queuedStatusID) ||
								(a.QueueItemStatusID == failedStatusID && a.AttemptCount < attemptCountLimit && a.LastRunDateUTC <= cutoffTime) ||
								(a.QueueItemStatusID == runningStatusID && a.LastRunDateUTC <= cutoffTime)
							orderby a.QueueItemPriorityID descending
							select a;

						results.TotalCount = domainEventQueueItems.Count();
						logger.Debug("queueDomainEventItems query got {0} total items", results.TotalCount);

						domainEventQueueItems = domainEventQueueItems.Take(maxNumberToPoll);

						// Update DB to mark the polled items as running - JHE
						var items = domainEventQueueItems.ToList();
						if (items.Count > 0)
						{
							logger.Debug("queueDomainEventItems took {0} items", items.Count);
							foreach (var domainEventQueueItem in items)
							{
								// Update the items to failed status after 5 unsuccessful attempts - JHE
								if (domainEventQueueItem.AttemptCount >= attemptCountLimit)
								{
									logger.Debug("domainEventQueueItem {0} being set to failed due to attempt count of {1}"
										, domainEventQueueItem.DomainEventQueueItemID, domainEventQueueItem.AttemptCount);
									domainEventQueueItem.QueueItemStatusID = Constants.QueueItemStatus.Failed.ToShort();
								}
								else
								{
									logger.Debug("domainEventQueueItem {0} initial status {1} : attempt count {2} : last run date {3}"
										, domainEventQueueItem.DomainEventQueueItemID
										, domainEventQueueItem.QueueItemStatusID
										, domainEventQueueItem.AttemptCount
										, domainEventQueueItem.LastRunDate.HasValue ? domainEventQueueItem.LastRunDate.ToString() : "NONE");
									domainEventQueueItem.AttemptCount++;
									domainEventQueueItem.QueueItemStatusID = Constants.QueueItemStatus.Running.ToShort();
									domainEventQueueItem.LastRunDate = DateTime.Now;
								}
							}
							context.SaveChanges();
							var idList = items.Select(x => x.DomainEventQueueItemID).ToList();
							results.AddRange(idList);
						}
						else
						{
							logger.Debug("queueDomainEventItems was empty or null");
						}

						logger.Debug("queueDomainEventItems returning {0} items", results.Count);
						return results;
					}
				}
			});
		}
		public PaginatedList<int> QueueDomainEventItemIDs(int maxNumberToPoll)
		{
			return queueDomainEventItems(maxNumberToPoll);
		}

		public IEnumerable<DomainEventQueueItem> LoadAllWhere(Expression<Func<DomainEventQueueItem, bool>> filter)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (var context = CreateContext())
				{
					return context.DomainEventQueueItems.Include("EventContext").Where(filter).ToList();
				}
			});
		}

		protected override Func<NetStepsEntities, IQueryable<DomainEventQueueItem>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<DomainEventQueueItem>>(
				   context => from l in context.DomainEventQueueItems
											   .Include("EventContext")
											   .Include("DomainEventType.DomainEventTypeCategory")
								select l);
			}
		}
	}
}