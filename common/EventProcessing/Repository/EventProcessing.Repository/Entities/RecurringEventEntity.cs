using System;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.EventProcessing.Repository.Entities
{
	[Table("RecurringEvents", Schema = "Event")]
	public class RecurringEventEntity
	{
		[Key]
		public int RecurringEventID { get; set; }
		public int EventTypeID { get; set; }
		public int IntervalInMinutes { get; set; }
		public DateTime? LastRunDateUTC { get; set; }
		public bool IsActive { get; set; }
	}
}
