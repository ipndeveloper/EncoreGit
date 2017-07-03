using System;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.EventProcessing.Common.Models
{
	[DTO]
	public interface IEvent
	{
		int EventID { get; set; }
		int EventTypeID { get; set; }
		DateTime ScheduledDateUTC { get; set; }
		DateTime? CompletedDateUTC { get; set; }
		byte RetryCount { get; set; }
	}
}
