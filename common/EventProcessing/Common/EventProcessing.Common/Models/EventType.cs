using System;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.EventProcessing.Common.Models
{
	[DTO]
	public interface IEventType
	{
		int EventTypeID { get; set; }
		string Name { get; set; }
		string TermName { get; set; }
		string Description { get; set; }
		string EventHandler { get; set; }
		byte? MaxRetryCount { get; set; }
		TimeSpan RetryInterval { get; set; }
		bool Enabled { get; set; }
	}
}
