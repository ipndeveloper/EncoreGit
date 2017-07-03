using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface ICalendarEventRepository : ISearchRepository<CalendarEventSearchParameters, PaginatedList<CalendarEventSearchData>>
	{
		List<CalendarEvent> LoadByDateRange(DateTime fromDate, DateTime toDate, int siteId);
	}
}
