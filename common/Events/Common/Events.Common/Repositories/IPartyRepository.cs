using NetSteps.Orders.Common.Models;

namespace NetSteps.Events.Common.Repositories
{
	public interface IPartyRepository
	{
		IParty GetPartyByPartyID(int partyID);
		int GetDistributorIDFromParty(IParty party);
	}
}
