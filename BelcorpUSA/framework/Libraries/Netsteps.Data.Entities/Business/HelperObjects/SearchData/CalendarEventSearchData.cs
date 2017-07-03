using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public struct CalendarEventSearchData
	{
		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int CalendarEventID { get; set; }

		[Display(AutoGenerateField = false)]
		public int? CalendarEventTypeID { get; set; }

		[TermName("Type")]
		[Display(Name = "Type")]
		public string CalendarEventType { get; set; }

		[Display(AutoGenerateField = false)]
		public int? CalendarCategoryID { get; set; }

		[TermName("Category")]
		[Display(Name = "Category")]
		public string CalendarCategory { get; set; }

		[Display(AutoGenerateField = false)]
		public int? CalendarPriorityID { get; set; }

		[TermName("Priority")]
		[Display(Name = "Priority")]
		public string CalendarPriority { get; set; }

		[Display(AutoGenerateField = false)]
		public int? CalendarStatusID { get; set; }

		[TermName("Status")]
		[Display(Name = "Status")]
		public string CalendarStatus { get; set; }

		[Display(AutoGenerateField = false)]
		public int? CalendarColorCodingID { get; set; }

		[TermName("ColorCoding", "ColorCoding")]
		[Display(Name = "Color Coding")]
		public string CalendarColorCoding { get; set; }

		[Display(AutoGenerateField = false)]
		public int? MarketID { get; set; }

		[TermName("Market")]
		public string Market { get; set; }

		[TermName("AccountID")]
		[Display(AutoGenerateField = false)]
		public int? AccountID { get; set; }

		[TermName("Subject")]
		public string Subject { get; set; }

		[TermName("StartDate", "Start Date")]
		public DateTime StartDate { get; set; }

		[TermName("EndDate", "End Date")]
		public DateTime EndDate { get; set; }

		[TermName("State")]
		public string State { get; set; }

		public int? StateProvinceID { get; set; }

		public bool IsCorporate { get; set; }

		public bool IsPublic { get; set; }

		public bool IsAllDayEvent { get; set; }
	}
}
