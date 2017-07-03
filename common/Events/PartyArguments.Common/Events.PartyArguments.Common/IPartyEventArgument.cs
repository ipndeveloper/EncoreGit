using NetSteps.Encore.Core.Dto;

namespace NetSteps.Events.PartyArguments.Common
{
	[DTO]
	public interface IPartyEventArgument
	{
		int EventID { get; set; }
		int PartyID { get; set; }
		int ArgumentID { get; set; }
	}
}
