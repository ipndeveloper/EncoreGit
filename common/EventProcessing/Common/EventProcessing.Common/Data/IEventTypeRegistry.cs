using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.EventProcessing.Common.Data
{
	using System.Reflection;

	public interface IEventTypeRegistry
	{
		bool AddEventHandlerTypesFromAssembly(Assembly assembly);
		void AddType(Type handler, string name);
		Type GetType(string fullName);
	}
}
