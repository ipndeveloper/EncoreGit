using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Events.Service.Tests.Mocks;

namespace NetSteps.Events.Service.Tests
{
	[TestClass]
	public class SchedulerBaseTests : SchedulerBase
	{
		private string typeName;
		protected override string GetEventTypeTermName()
		{
			return typeName;
		}

		[TestInitialize]
		public void Initialize()
		{
			FakeEventTypeRepository.Initialize();
			typeName = "TypeName";
		}

		[TestMethod]
		public void GetEventTypeIDFromTermName_TypeExists_ReturnsID()
		{
			FakeEventTypeRepository.returnEvent.EventTypeID = 1;

			int id = GetEventTypeIDFromTermName();

			Assert.AreEqual(1, id);
		}

		[TestMethod]
		public void GetEventTypeIDFromTermName_NoTermName_ThrowsException()
		{
			typeName = "";

			try
			{
				int id = GetEventTypeIDFromTermName();
			}
			catch (Exception)
			{
				return;
			}

			Assert.Fail();
		}

		[TestMethod]
		public void GetEventTypeIDFromTermName_NoEventForTermName_ThrowsException()
		{
			FakeEventTypeRepository.returnEvent = null;

			try
			{
				GetEventTypeIDFromTermName();
			}
			catch (Exception)
			{
				return;
			}

			Assert.Fail();
		}
	}
}
