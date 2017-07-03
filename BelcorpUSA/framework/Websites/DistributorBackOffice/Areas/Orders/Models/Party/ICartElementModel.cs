using NetSteps.Encore.Core.Dto;

namespace DistributorBackOffice.Areas.Orders.Models.Party
{
	[DTO]
	public interface ICartElement
	{
		string PartialViewName { get; set; }
		object Model { get; set; }
	}
}
