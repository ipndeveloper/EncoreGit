using NetSteps.Encore.Core.Dto;

namespace NetSteps.Accounts.Common.Models
{
	[DTO]
	public interface IDownlineAccountData
	{
		int AccountId { get; set; }
		int? ParentAccountId { get; set; }
		int TreeLevel { get; set; }
		short AccountTypeId { get; set; }
		short AccountStatusId { get; set; }
		string AccountNumber { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
	}
}
