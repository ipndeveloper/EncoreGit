using NetSteps.Encore.Core.Dto;

namespace nsDistributor.Models.Shared
{
	[DTO]
	public interface IPromotionInfoModel
	{
		string Description { get; set; }
		string StepID { get; set; }
		bool Available { get; set; }
	}
}