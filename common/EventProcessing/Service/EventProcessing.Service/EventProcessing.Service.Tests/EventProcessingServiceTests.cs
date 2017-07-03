using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Common;
using NetSteps.EventProcessing.Common.Data;
using NetSteps.EventProcessing.Common.Models;

namespace NetSteps.EventProcessing.Service.Tests
{
	[TestClass]
	public class EventProcessingServiceTests
	{
		private void init()
		{
			var root = Container.Root;
			root.Registry.ForType<IEventTypeRegistry>().Register<MockEventTypeRegistry>().ResolveAsSingleton().End();
			root.Registry.ForType<IEventRepository>().Register<MockEventRepository>().ResolveAsSingleton().End();
			root.Registry.ForType<IEventTypeRepository>().Register<MockEventTypeRepository>().ResolveAsSingleton().End();
			root.Registry.ForType<IExceptionLogger>().Register<MockExceptionLogger>().ResolveAsSingleton().End();
			root.Registry.ForType<IRecurringEventRepository>().Register<MockRecurringEventRepository>().ResolveAsSingleton().End();
		}

		[TestMethod]
		public void GetNoType()
		{
			init();

			var service = new EventProcessingService(1, 100, "/", false, "");
			MockEventTypeRegistry.toReturn = null;

			var result = service.ExecuteEventAssembly("TestClass", 1);


			Assert.IsNull(result);
		}

		[TestMethod]
		public void ExecuteAssemblyEvent_Success_ReturnsTrue()
		{
			init();
			var service = new EventProcessingService(1, 100, "/", false, "");
			MockEventTypeRegistry.toReturn = typeof(MockEventHandler);

			var result = service.ExecuteEventAssembly("doesn'tMatter", 1);

			Assert.IsTrue(result.Value);
		}

		[TestMethod]
		public void ExecuteAssemblyEvent_Failure_ReturnsFalse()
		{
			init();
			var service = new EventProcessingService(1, 100, "/", false, "");
			MockEventTypeRegistry.toReturn = typeof(MockEventFailureHandler);

			var result = service.ExecuteEventAssembly("doesn'tMatter", 1);

			Assert.IsFalse(result.Value);
		}

		[TestMethod]
		public void ExecuteAssemblyEvent_Exception_ReturnsFalse()
		{
			init();
			var service = new EventProcessingService(1, 100, "/", false, "");
			MockEventTypeRegistry.toReturn = typeof(MockEventExceptionHandler);

			var result = service.ExecuteEventAssembly("doesn'tMatter", 1);

			Assert.IsTrue(result.IsCompleted && result.IsFaulted);
		}

		[TestMethod]
		public void GetEventQueue_RepositoryThrowsException_ExceptionIngested()
		{
			using (var root = Container.Root)
			{
				root.Registry.ForType<IEventRepository>().Register<MockFailureEventRepository>().ResolveAsSingleton().End();
				root.Registry.ForType<IEventTypeRegistry>().Register<MockEventTypeRegistry>().ResolveAsSingleton().End();
				root.Registry.ForType<IEventTypeRepository>().Register<MockEventTypeRepository>().ResolveAsSingleton().End();
				root.Registry.ForType<IExceptionLogger>().Register<MockExceptionLogger>().ResolveAsSingleton().End();

				var service = new EventProcessingService(1, 100, "/", false, "");
				var result = service.getEventQueue();
				Assert.IsNull(result);
			}
		}

		[TestMethod]
		public void GetLastRunTimeThatSyncsWithInterval_JustExecuted_SmallInterval()
		{
			init();
			var service = new EventProcessingService(1, 100, "/", false, "");
			DateTime lastRunTime = DateTime.UtcNow;
			DateTime fiveMinutesInTheFuture = lastRunTime.AddMinutes(5);

			var actual = service.getLastRunTimeThatSyncsWithInterval(lastRunTime, 5, fiveMinutesInTheFuture);

			Assert.AreEqual(fiveMinutesInTheFuture, actual);
		}

		[TestMethod]
		public void GetLastRunTimeThatSyncsWithInterval_Delay_SmallInterval()
		{
			init();
			var service = new EventProcessingService(1, 100, "/", false, "");
			var lastRunTime = DateTime.UtcNow;
			var tenMinutesInTheFuture = lastRunTime.AddMinutes(10);

			var actual = service.getLastRunTimeThatSyncsWithInterval(lastRunTime, 5, tenMinutesInTheFuture);

			Assert.AreEqual(tenMinutesInTheFuture, actual);
		}

		[TestMethod]
		public void GetLastRunTimeThatSyncsWithInterval_SixMinutesPast_FiveMinuteInterval()
		{
			init();
			var service = new EventProcessingService(1, 100, "/", false, "");
			var lastRunTime = DateTime.UtcNow;
			var sixMinutesInTheFuture = lastRunTime.AddMinutes(6);

			var actual = service.getLastRunTimeThatSyncsWithInterval(lastRunTime, 5, sixMinutesInTheFuture);

			Assert.AreEqual(sixMinutesInTheFuture.AddMinutes(-1), actual);
		}
	}

	public class MockEventTypeRegistry : IEventTypeRegistry
	{
		public static Type toReturn;
		public bool AddEventHandlerTypesFromAssembly(Assembly assembly)
		{
			return true;
		}

		public void AddType(Type handler, string name)
		{
		}

		public Type GetType(string fullName)
		{
			return toReturn;
		}
	}

	public class MockRecurringEventRepository : IRecurringEventRepository
	{
		public IEnumerable<IRecurringEvent> GetActiveRecurringEvents()
		{
			return null;
		}

		public IEnumerable<IRecurringEvent> GetActiveRecurringEvents(int top)
		{
			return null;
		}

		public void MarkRecurringEventAsCompleted(IRecurringEvent recurringEvent)
		{
		}

		public void MarkRecurringEventAsCompleted(IRecurringEvent recurringEvent, DateTime dateTime)
		{
		}
	}

	public class MockEventRepository : IEventRepository
	{
		public IEvent GetByEventID(int eventID)
		{
			var returnValue = Create.New<IEvent>();

			returnValue.CompletedDateUTC = null;
			returnValue.EventID = eventID;
			returnValue.EventTypeID = 1;
			returnValue.RetryCount = 0;
			returnValue.ScheduledDateUTC = new DateTime();
			return returnValue;
		}

		public IEnumerable<IEvent> GetTopEvents(int top)
		{
			return new [] {GetByEventID(1)};
		}

		public IEnumerable<IEvent> GetByEventIDs(IEnumerable<int> eventIDs)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IEvent> GetByEventTypeID(int eventTypeID)
		{
			throw new NotImplementedException();
		}

		public int GetEventTypeID(int eventID)
		{
			throw new NotImplementedException();
		}

		public IEvent Save(IEvent Event)
		{
			return Event;
		}
	}

	public class MockFailureEventRepository : IEventRepository
	{
		public IEvent GetByEventID(int eventID)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IEvent> GetTopEvents(int top)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IEvent> GetByEventIDs(IEnumerable<int> eventIDs)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IEvent> GetByEventTypeID(int eventTypeID)
		{
			throw new NotImplementedException();
		}

		public int GetEventTypeID(int eventID)
		{
			throw new NotImplementedException();
		}

		public IEvent Save(IEvent Event)
		{
			throw new NotImplementedException();
		}
	}

	public class MockEventTypeRepository : IEventTypeRepository
	{
		public IEventType GetByEventTypeID(int eventTypeID)
		{
			var returnValue = Create.New<IEventType>();

			returnValue.Enabled = true;
			returnValue.EventHandler = "Test.Test";
			returnValue.EventTypeID = 1;
			returnValue.MaxRetryCount = 0;
			returnValue.Name = "Test";
			returnValue.RetryInterval = new TimeSpan();
			returnValue.TermName = "Test";

			return returnValue;
		}

		public IEventType Save(IEventType eventType)
		{
			return eventType;
		}

		public IEventType GetByTermName(string termName)
		{
			return GetByEventTypeID(1);
		}

		public List<IEventType> GetAllEventTypes()
		{
			return new List<IEventType> {  };
		}

		public List<Tuple<int, int>> GetAllRetryCounts()
		{
			return new List<Tuple<int, int>>{};
		}
	}

	public class MockEventHandler : IEventHandler
	{
		public bool Execute(int eventID)
		{
			return true;
		}
	}

	public class MockEventFailureHandler : IEventHandler
	{
		public bool Execute(int eventID)
		{
			return false;
		}
	}

	public class MockEventExceptionHandler : IEventHandler
	{
		public bool Execute(int eventID)
		{
			throw new Exception();
		}
	}

	public class MockExceptionLogger : IExceptionLogger
	{
		public void LogException(Exception ex)
		{
		}
	}
}
