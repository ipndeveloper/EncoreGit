namespace NetSteps.Data.Entities.Processors.Autoships
{
	using System;

	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface IAutoshipOrderDTO
	{
		int AutoshipOrderID { get; set; }

		int AutoshipScheduleID { get; set; }

		int AccountID { get; set; }

		int TemplateOrderID { get; set; }

		int MaximumIntervals { get; set; }

		DateTime? StartDate { get; set; }
	}
}
