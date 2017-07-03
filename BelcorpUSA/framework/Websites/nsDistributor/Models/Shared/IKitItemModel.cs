using NetSteps.Encore.Core.Dto;

namespace nsDistributor.Models.Shared
{
	[DTO]
	public interface IKitItemModel
	{
		string SKU { get; set; }
		string ProductName { get; set; }
		int Quantity { get; set; }
		string ImageUrl { get; set; }
	}
}