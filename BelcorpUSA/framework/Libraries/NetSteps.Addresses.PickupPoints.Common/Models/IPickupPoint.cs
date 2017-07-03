using NetSteps.Encore.Core.Dto;

namespace NetSteps.Addresses.PickupPoints.Common.Models
{
	[DTO]
	public interface IPickupPoint
	{
		string Code { get; set; }
		int AddressID { get; set; }
		int PickupPointID { get; set; }
	}
}
