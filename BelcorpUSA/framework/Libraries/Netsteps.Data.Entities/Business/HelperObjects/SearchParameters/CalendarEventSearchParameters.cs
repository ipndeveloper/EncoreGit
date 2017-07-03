using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class CalendarEventSearchParameters : FilterDateRangePaginatedListParameters<CalendarEvent>
    {
        public int? CalendarEventTypeID { get; set; }
        public int? CalendarCategoryID { get; set; }
        public int? CalendarPriorityID { get; set; }
        public int? CalendarStatusID { get; set; }
        public int? CalendarColorCodingID { get; set; }
        public int? MarketID { get; set; }
        public int? AccountID { get; set; }
        public string State { get; set; }
        public int? StateProvinceID { get; set; }
        public bool? IsCorporate { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsAllDayEvent { get; set; }
    }
}
