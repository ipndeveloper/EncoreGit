using NetSteps.Encore.Core.Dto;

namespace NetSteps.Addresses.PickupPoints.Common.Models
{
	[DTO]
	public interface IPickupPointModel
	{
		int PickupPointID { get; set; }
		string PickupPointCode { get; set; }
		string Name { get; set; }
		int Distance { get; set; }
		IPickupPointAddress PickupPointAddress{ get; set; }
	}
}
