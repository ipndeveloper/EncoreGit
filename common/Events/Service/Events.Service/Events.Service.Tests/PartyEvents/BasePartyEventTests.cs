using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Events.Service.PartyEvents;
using NetSteps.Events.Service.Tests.Mocks;

namespace NetSteps.Events.Service.Tests.PartyEvents
{
	[TestClass]
	public class BasePartyEventTests : BasePartyEvent
	{
		private string termName;
		private DateTime scheduleTime;

		[TestInitialize]
		public void Initialize()
		{
			termName = "TermName";
			scheduleTime = new DateTime(2010, 1, 1);

			FakePartyEventArgumentRepository.Initialize();
			FakeEventRepository.Initialize();
			FakeEventTypeRepository.Initialize();
		}

		[TestMethod]
		public void ScheduleEventAndSaveArgs_EventSaveFails_ThrowsException()
		{
			FakeEventRepository.eventToReturn.EventID = 0;

			try
			{
				ScheduleEventAndSaveArgs(1);
			}
			catch (Exception)
			{
				return;
			}
			Assert.Fail();
		}

		[TestMethod]
		public void ScheduleEventAndSaveArgs_EventSaveSucceeds_Success()
		{
			int newEventID = ScheduleEventAndSaveArgs(1);

			Assert.AreEqual(1, newEventID);
		}

		protected override string GetEventTypeTermName()
		{
			return termName;
		}

		protected override DateTime GetDateTimeForSchedule(int partyID)
		{
			return scheduleTime;
		}

		public override bool Execute(int eventID)
		{
			return true;
		}
	}
}
