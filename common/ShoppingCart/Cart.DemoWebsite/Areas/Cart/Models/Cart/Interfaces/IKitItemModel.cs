using NetSteps.Encore.Core.Dto;

namespace Cart.DemoWebsite.Areas.Cart.Cart.Models.Interfaces
{
	[DTO]
	public interface IKitItemModel
	{
		string Sku { get; set; }
		string Description { get; set; }
		uint Quantity { get; set; }
	}
}
