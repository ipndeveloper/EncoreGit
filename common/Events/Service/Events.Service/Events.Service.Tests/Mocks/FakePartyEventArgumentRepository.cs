using NetSteps.Encore.Core.IoC;
using NetSteps.Events.PartyArguments.Common;

namespace NetSteps.Events.Service.Tests.Mocks
{
	public class FakePartyEventArgumentRepository : IPartyEventArgumentRepository
	{
		public static int PartyID;
		public static IPartyEventArgument Argument;


		public static void Initialize()
		{
			var root = Container.Root;
			root.ForType<IPartyEventArgumentRepository>()
				.Register<FakePartyEventArgumentRepository>()
				.ResolveAsSingleton()
				.End();

			PartyID = 1;
			Argument = Create.New<IPartyEventArgument>();
			Argument.ArgumentID = 1;
			Argument.EventID = 1;
			Argument.PartyID = 1;
		}

		public int GetPartyIDByEventID(int eventID)
		{
			return PartyID;
		}

		public IPartyEventArgument Save(IPartyEventArgument partyArg)
		{
			return Argument;
		}
	}
}
