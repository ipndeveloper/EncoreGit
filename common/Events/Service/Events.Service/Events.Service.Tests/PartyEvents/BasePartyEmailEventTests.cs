using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Interfaces;
using NetSteps.Events.Service.PartyEvents;
using NetSteps.Events.Service.Tests.Mocks;
using NetSteps.Orders.Common.Models;

namespace NetSteps.Events.Service.Tests.PartyEvents
{
	[TestClass]
	public class BasePartyEmailEventTests : BasePartyEmailEvent
	{
		private string typeName;
		private DateTime scheduleTime;
		private ITokenValueProvider provider;

		protected override string GetEventTypeTermName()
		{
			return typeName;
		}

		protected override DateTime GetDateTimeForSchedule(int partyID)
		{
			return scheduleTime;
		}

		protected override ITokenValueProvider GetTokenValueProvider(IParty party)
		{
			return provider;
		}

		[TestInitialize]
		public void Initialize()
		{
			typeName = "typeName";
			scheduleTime = new DateTime(2010, 1, 1);

			FakePartyEventArgumentRepository.Initialize();
			FakeEventRepository.Initialize();
			FakeEventTypeRepository.Initialize();
			FakeEmailRepository.Initialize();
			FakePartyRepository.Initialize();
			provider = new FakeTokenValueProvider();
		}

		[TestMethod]
		public void SendEmail_InvalidPartyID_ThrowsException()
		{
			FakePartyRepository.returnParty = null;
			try
			{
				SendEmail(0, 1);
			}
			catch (Exception)
			{
				return;
			}
			Assert.Fail();
		}

		[TestMethod]
		public void SendEmail_NoTokenProvider_ThrowsException()
		{
			provider = null;

			try
			{
				SendEmail(1, 1);
			}
			catch (Exception)
			{
				return;
			}
			Assert.Fail();
		}

		[TestMethod]
		public void SendEmail_Success()
		{
			SendEmail(1, 1);
		}

		[TestMethod]
		public void Execute_NoMatchingArgument_ThrowsException()
		{
			FakePartyEventArgumentRepository.PartyID = 0;

			try
			{
				Execute(1);
			}
			catch (Exception)
			{
				return;
			}
			Assert.Fail();
		}

		[TestMethod]
		public void Execute_NoEventType_ThrowsException()
		{
			FakeEventRepository.eventTypeID = 0;

			try
			{
				Execute(1);
			}
			catch (Exception)
			{
				return;
			}
			Assert.Fail();
		}

		[TestMethod]
		public void Execute_ReturnsTrue()
		{
			bool result = Execute(1);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Execute_EmailFails_ReturnsFalse()
		{
			FakePartyRepository.returnParty = null;

			bool result = Execute(1);

			Assert.IsFalse(result);
		}
	}
}
