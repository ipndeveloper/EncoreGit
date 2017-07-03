using NetSteps.Encore.Core.Dto;

namespace Cart.DemoWebsite.Areas.Cart.Cart.Models.Interfaces
{
	[DTO]
	public interface ICartServerOptions
	{
		string UpdateQuantitiesUrl { get; set; }
		string UpdateQuantitiesBaseErrorMessage { get; set; }
		
		string RemoveUrl { get; set; }
		string RemoveBaseErrorMessage { get; set; }
	}
}
