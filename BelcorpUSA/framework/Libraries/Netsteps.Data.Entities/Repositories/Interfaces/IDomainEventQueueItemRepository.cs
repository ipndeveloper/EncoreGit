using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IDomainEventQueueItemRepository
    {
		PaginatedList<int> QueueDomainEventItemIDs(int numberToFetch);
		IEnumerable<DomainEventQueueItem> LoadAllWhere(Expression<Func<DomainEventQueueItem, bool>> filter);
    }
}
