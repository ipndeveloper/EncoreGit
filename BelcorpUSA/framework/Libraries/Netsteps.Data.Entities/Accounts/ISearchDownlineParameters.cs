using NetSteps.Encore.Core.Dto;

namespace NetSteps.Accounts.Common.Models
{
	[DTO]
	public interface ISearchDownlineParameters
	{
		int RootAccountId { get; set; }
		string Query { get; set; }
	}
}
