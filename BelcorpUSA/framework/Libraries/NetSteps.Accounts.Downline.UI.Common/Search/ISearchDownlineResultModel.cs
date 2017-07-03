using NetSteps.Encore.Core.Dto;

namespace NetSteps.Accounts.Downline.UI.Common.Search
{
	[DTO]
	public interface ISearchDownlineResultModel
	{
		int id { get; set; }
		string text { get; set; }
	}
}
