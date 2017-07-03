using System.Diagnostics.Contracts;

namespace NetSteps.Events.AccountArguments.Common
{
	[ContractClass(typeof(IAccountEventArgumentRepositoryContracts))]
	public interface IAccountEventArgumentRepository
	{
		int GetAccountIdByEventId(int eventId);
		IAccountEventArgument Save(IAccountEventArgument accountArg);
	}

	[ContractClassFor(typeof(IAccountEventArgumentRepository))]
	internal abstract class IAccountEventArgumentRepositoryContracts : IAccountEventArgumentRepository
	{
		public int GetAccountIdByEventId(int eventId)
		{
			Contract.Requires(eventId > 0);
			int result = Contract.Result<int>();
			Contract.Ensures(result >= 0);
			return default(int);
		}

		public IAccountEventArgument Save(IAccountEventArgument accountArg)
		{
			Contract.Requires(accountArg.AccountID > 0);
			Contract.Requires(accountArg.EventID > 0);
			Contract.Ensures(Contract.Result<IAccountEventArgument>() != null);
			return default(IAccountEventArgument);
		}
	}
}
