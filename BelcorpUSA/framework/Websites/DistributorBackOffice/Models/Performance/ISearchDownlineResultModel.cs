using NetSteps.Encore.Core.Dto;

namespace DistributorBackOffice.Models.Performance
{
	[DTO]
	public interface ISearchDownlineResultModel
	{
		int id { get; set; }
		string text { get; set; }
	}
}
