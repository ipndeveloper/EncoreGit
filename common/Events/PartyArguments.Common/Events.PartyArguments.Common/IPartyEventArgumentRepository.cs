using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Events.PartyArguments.Common
{
	[ContractClass(typeof(IPartyEventArgumentRepositoryContracts))]
	public interface IPartyEventArgumentRepository
	{
		int GetPartyIDByEventID(int eventID);
		IPartyEventArgument Save(IPartyEventArgument partyArg);
	}

	[ContractClassFor(typeof(IPartyEventArgumentRepository))]
	internal abstract class IPartyEventArgumentRepositoryContracts : IPartyEventArgumentRepository
	{
		public int GetPartyIDByEventID(int eventID)
		{
			Contract.Requires<ArgumentException>(eventID > 0);
			int result = Contract.Result<int>();
			Contract.Ensures(result >= 0);
			return default(int);
		}

		public IPartyEventArgument Save(IPartyEventArgument partyArg)
		{
			Contract.Requires<ArgumentException>(partyArg.EventID > 0);
			Contract.Requires<ArgumentException>(partyArg.PartyID > 0);
			var result = Contract.Result<IPartyEventArgument>();
			Contract.Ensures(result != null && result.ArgumentID > 0);
			return default(IPartyEventArgument);
		}
	}
}
