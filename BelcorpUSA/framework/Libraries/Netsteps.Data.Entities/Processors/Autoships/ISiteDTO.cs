namespace NetSteps.Data.Entities.Processors.Autoships
{
	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface ISiteDTO
	{
		int SiteStatusID { get; set; }

		int SiteID { get; set; }
	}
}
