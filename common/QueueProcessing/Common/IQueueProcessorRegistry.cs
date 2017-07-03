using System;
using System.Collections.Generic;

namespace NetSteps.QueueProcessing.Common
{
	public interface IQueueProcessorRegistry
	{
		Type GetRegisteredProcessorType(string name);
		void Register<P>(string name) where P : IQueueProcessor;
		bool Unregister(string name);
		IEnumerable<Type> AllProcessorsTypes { get; } 
	}
}
