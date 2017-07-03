using System;
using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Common.Data;
using NetSteps.EventProcessing.Common.Models;

namespace NetSteps.Events.Service.Tests.Mocks
{
	public class FakeEventTypeRepository : IEventTypeRepository
	{
		public static void Initialize()
		{
			FakeEventTypeRepository.returnEvent = Create.New<IEventType>();
			FakeEventTypeRepository.returnEvent.Description = "";
			FakeEventTypeRepository.returnEvent.Enabled = true;
			FakeEventTypeRepository.returnEvent.EventHandler = "Class.Type";
			FakeEventTypeRepository.returnEvent.EventTypeID = 1;
			FakeEventTypeRepository.returnEvent.TermName = "typeName";
			var root = Container.Root;
			root.ForType<IEventTypeRepository>()
				.Register<FakeEventTypeRepository>()
				.ResolveAsSingleton()
				.End();

		}

		public static IEventType returnEvent;

		public IEventType GetByEventTypeID(int eventTypeID)
		{
			return returnEvent;
		}

		public IEventType Save(IEventType eventType)
		{
			return returnEvent;
		}

		public IEventType GetByTermName(string termName)
		{
			return returnEvent;
		}

		public List<IEventType> GetAllEventTypes()
		{
			return new List<IEventType>{};
		}

		public List<Tuple<int, int>> GetAllRetryCounts()
		{
			return new List<Tuple<int, int>>{};
		}
	}
}
