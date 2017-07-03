using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class CalendarEvent
	{
		// TODO: Remove these enums later and generate from the correct tables - JHE
		#region Enums

		public enum Priority
		{
			Low,
			Average,
			High,
		}

		public enum Status
		{
			Active,
			Passed,
			Cancelled
		}

		public enum ColorCoding
		{
			Red,
			Green,
			Blue,
			Yellow,
			Cyan,
			Magenta,
			Orange,
			White,
			Black
		}

		#endregion

		#region Basic Crud
		public static List<CalendarEvent> LoadByDateRange(DateTime fromDate, DateTime toDate, int siteId)
		{
			try
			{
				var list = Repository.LoadByDateRange(fromDate, toDate, siteId);
				foreach (var item in list)
				{
					item.StartTracking();
					item.IsLazyLoadingEnabled = true;
				}
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static PaginatedList<CalendarEventSearchData> SearchCalendarEvents(CalendarEventSearchParameters searchParameters)
		{
			try
			{
				return Repository.Search(searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		#endregion
	}
}
