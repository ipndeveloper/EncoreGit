using System;
using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Events.Common.Repositories;
using NetSteps.Orders.Common.Models;

namespace NetSteps.Events.Service.Tests.Mocks
{
	public class FakePartyRepository : IPartyRepository
	{
		public static IParty returnParty;
		public static List<DateTime> returnPartyDates;

		public static void Initialize()
		{
			WireupCoordinator.SelfConfigure();

			var root = Container.Root;
			root.ForType<IPartyRepository>()
				.Register<FakePartyRepository>()
				.ResolveAsSingleton()
				.End();

			root.ForType<IParty>()
				.Register<FakeParty>()
				.ResolveAnInstancePerRequest()
				.End();

			returnParty = Create.New<IParty>();
			returnParty.Name = "PartyParty";
			returnParty.Order.GrandTotal = 1.11M;

			returnPartyDates = new List<DateTime> { new DateTime(2010, 1, 1) };
		}

		public IParty GetPartyByPartyID(int partyID)
		{
			return returnParty;
		}

		public int GetDistributorIDFromParty(IParty party)
		{
			return 1;
		}

		public IEnumerable<DateTime> GetNewestCompletedPartyDates(int accountID, int numberToTake)
		{
			return returnPartyDates;
		}
	}
}
