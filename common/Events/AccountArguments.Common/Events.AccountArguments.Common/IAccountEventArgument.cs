using NetSteps.Encore.Core.Dto;

namespace NetSteps.Events.AccountArguments.Common
{
	[DTO]
	public interface IAccountEventArgument
	{
		int ArgumentID { get; set; }
		int EventID { get; set; }
		int AccountID { get; set; }
	}
}
