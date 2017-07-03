namespace NetSteps.Data.Entities.Processors.Autoships
{
	using System;

	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface IIntervalTypeDTO
	{
		int IntervalTypeID { get; set; }
		DateTime StartOfInterval { get; set; }
		DateTime StartOfNextInterval { get; set; }
	}
}
