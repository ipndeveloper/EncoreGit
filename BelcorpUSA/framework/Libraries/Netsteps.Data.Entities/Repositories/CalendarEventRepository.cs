using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class CalendarEventRepository : IDefaultImplementation, ICalendarEventRepository
	{
		#region Members
		protected override Func<NetStepsEntities, IQueryable<CalendarEvent>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<CalendarEvent>>(
				   (context) => from c in context.CalendarEvents
												   .Include("Address")
												   .Include("HtmlSection")
												   .Include("HtmlSection.HtmlSectionContents.HtmlContent")
												   .Include("HtmlSection.HtmlSectionContents.HtmlContent.HtmlElements")
									//.Include("Language")
												   .Include("CalendarEventAttributes")
								select c);
			}
		}
		#endregion

		// TODO: Make another method for consumers of calendar events - DES
		public List<CalendarEvent> LoadByDateRange(DateTime fromDate, DateTime toDate, int siteId)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					// Get Site Events - DES
					fromDate = fromDate.LocalToUTC();
					toDate = toDate.LocalToUTC();

					var result = loadAllFullQuery(context).Where(c => c.StartDateUTC >= fromDate && c.StartDateUTC < toDate && c.Sites.Select(s => s.SiteID).Contains(siteId));
					var calendarEvents = result.OrderBy(c => c.StartDateUTC).ToList();
					return calendarEvents;
				}
			});
		}

		public PaginatedList<CalendarEventSearchData> Search(CalendarEventSearchParameters searchParameters)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					PaginatedList<CalendarEventSearchData> results = new PaginatedList<CalendarEventSearchData>(searchParameters);

					var matchingItems = from a in context.CalendarEvents
										select a;

					if (searchParameters.CalendarCategoryID.HasValue)
						matchingItems = from a in matchingItems
										where a.CalendarCategoryID == searchParameters.CalendarCategoryID.Value
										select a;
					if (searchParameters.CalendarColorCodingID.HasValue)
						matchingItems = from a in matchingItems
										where a.CalendarColorCodingID == searchParameters.CalendarColorCodingID.Value
										select a;
					if (searchParameters.CalendarEventTypeID.HasValue)
						matchingItems = from a in matchingItems
										where a.CalendarEventTypeID == searchParameters.CalendarEventTypeID.Value
										select a;
					if (searchParameters.CalendarPriorityID.HasValue)
						matchingItems = from a in matchingItems
										where a.CalendarPriorityID == searchParameters.CalendarPriorityID.Value
										select a;
					if (searchParameters.CalendarStatusID.HasValue)
						matchingItems = from a in matchingItems
										where a.CalendarStatusID == searchParameters.CalendarStatusID.Value
										select a;
					if (searchParameters.MarketID.HasValue)
						matchingItems = from a in matchingItems
										where a.MarketID == searchParameters.MarketID.Value
										select a;

					if (searchParameters.IsAllDayEvent.HasValue)
						matchingItems = from a in matchingItems
										where a.IsAllDayEvent == searchParameters.IsAllDayEvent.Value
										select a;
					if (searchParameters.IsCorporate.HasValue)
						matchingItems = from a in matchingItems
										where a.IsCorporate == searchParameters.IsCorporate.Value
										select a;
					if (searchParameters.IsPublic.HasValue)
						matchingItems = from a in matchingItems
										where a.IsPublic == searchParameters.IsPublic.Value
										select a;

					if (!string.IsNullOrEmpty(searchParameters.State))
						matchingItems = from a in matchingItems
										where a.Address.State == searchParameters.State
										select a;

					if (searchParameters.StateProvinceID.HasValue)
						matchingItems = matchingItems.Where(a => a.Address.StateProvinceID == searchParameters.StateProvinceID.Value);

					matchingItems = matchingItems.ApplyDateRangeFilters("StartDateUTC", "EndDateUTC", searchParameters);

					if (searchParameters.WhereClause != null)
						matchingItems = matchingItems.Where(searchParameters.WhereClause);

					if (!searchParameters.OrderBy.IsNullOrEmpty())
					{
						switch (searchParameters.OrderBy)
						{
							case "City":
								matchingItems = matchingItems.ApplyOrderByFilter(searchParameters.OrderByDirection, a => (a.Address == null) ? string.Empty : a.Address.City);
								break;
							case "State":
								matchingItems = matchingItems.ApplyOrderByFilter(searchParameters.OrderByDirection, a => (a.Address == null) ? string.Empty : a.Address.State);
								break;
							default:
								matchingItems = matchingItems.ApplyOrderByFilter(searchParameters, context);
								break;
						}
					}
					else
						matchingItems = matchingItems.OrderBy(a => a.StartDateUTC);

					// TotalCount must be set before applying Pagination - JHE
					results.TotalCount = matchingItems.Count();

					matchingItems = matchingItems.ApplyPagination(searchParameters);

					var accountInfos = from a in matchingItems
									   select new
									   {
										   a.CalendarEventID,
										   a.CalendarEventTypeID,
										   a.CalendarCategoryID,
										   a.CalendarPriorityID,
										   a.CalendarStatusID,
										   a.CalendarColorCodingID,
										   a.MarketID,
										   a.AccountID,
										   a.StartDateUTC,
										   a.EndDateUTC,
										   a.Address,
										   a.IsCorporate,
										   a.IsPublic,
										   a.IsAllDayEvent
									   };

					var listValues = AccountListValue.LoadAllCorporateListValues();

					foreach (var a in accountInfos.ToList())
						results.Add(new CalendarEventSearchData()
						{
							CalendarEventID = a.CalendarEventID,
							CalendarEventTypeID = a.CalendarEventTypeID,
							CalendarEventType = listValues.GetTranslatedValue(a.CalendarEventTypeID.ToInt()),
							CalendarCategoryID = a.CalendarCategoryID,
							CalendarCategory = listValues.GetTranslatedValue(a.CalendarCategoryID.ToInt()),
							CalendarPriorityID = a.CalendarPriorityID,
							CalendarPriority = listValues.GetTranslatedValue(a.CalendarPriorityID.ToInt()),
							CalendarStatusID = a.CalendarStatusID,
							CalendarStatus = listValues.GetTranslatedValue(a.CalendarStatusID.ToInt()),
							CalendarColorCodingID = a.CalendarColorCodingID,
							CalendarColorCoding = listValues.GetTranslatedValue(a.CalendarColorCodingID.ToInt()),

							MarketID = a.MarketID,
							Market = SmallCollectionCache.Instance.Markets.GetById(a.MarketID.ToInt()).GetTerm(),
							AccountID = a.AccountID,
							StartDate = a.StartDateUTC.UTCToLocal(),
							EndDate = a.StartDateUTC.UTCToLocal(),
							State = a.Address.State,
							StateProvinceID = a.Address.StateProvinceID,
							IsCorporate = a.IsCorporate,
							IsPublic = a.IsPublic,
							IsAllDayEvent = a.IsAllDayEvent,
						});

					return results;
				}
			});
		}
	}
}
