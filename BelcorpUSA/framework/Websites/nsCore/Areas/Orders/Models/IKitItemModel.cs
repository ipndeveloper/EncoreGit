using NetSteps.Encore.Core.Dto;

namespace nsCore.Areas.Orders.Models
{
	[DTO]
	public interface IKitItemModel
	{
		string SKU { get; set; }
		string ProductName { get; set; }
		int Quantity { get; set; }
	}
}
