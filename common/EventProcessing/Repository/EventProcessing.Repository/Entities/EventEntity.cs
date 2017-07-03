using System;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.EventProcessing.Repository.Entity
{
	[Table("Events", Schema = "Event")]
	public class EventEntity
	{
		[Key]
		public int EventID { get; set; }
		public int EventTypeID { get; set; }
		public DateTime ScheduledDateUTC { get; set; }
		public DateTime? CompletedDateUTC { get; set; }
		public byte RetryCount { get; set; }
	}
}
