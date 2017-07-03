namespace NetSteps.Data.Entities.Processors.Autoships
{
	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface IAutoshipScheduleDTO
	{
		int AutoshipScheduleTypeID { get; set; }

		int IntervalTypeID { get; set; }
	}
}
