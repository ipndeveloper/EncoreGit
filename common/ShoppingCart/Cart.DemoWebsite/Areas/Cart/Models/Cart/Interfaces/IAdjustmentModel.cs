using NetSteps.Encore.Core.Dto;

namespace Cart.DemoWebsite.Areas.Cart.Cart.Models.Interfaces
{
	[DTO]
	public interface IAdjustmentModel
	{
		decimal Amount { get; set; }
		string Description { get; set; }
	}
}
