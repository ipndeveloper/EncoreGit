using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Common;
using NetSteps.EventProcessing.Common.Data;

namespace NetSteps.EventProcessing.Service
{
	[ContainerRegister(typeof(IEventTypeRegistry), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class EventTypeRegistry : IEventTypeRegistry
	{
		private readonly Dictionary<string, Type> TypeRegistry;

		public EventTypeRegistry()
		{
			TypeRegistry = new Dictionary<string, Type>();
		}

		public bool AddEventHandlerTypesFromAssembly(Assembly assembly)
		{
			bool addedClass = false;
			foreach (Type assemblyType in assembly.GetTypes())
			{
				if (string.IsNullOrEmpty(assemblyType.FullName))
				{
					continue;
				}

				Type interfaceType = assemblyType.GetInterface(typeof(IEventHandler).FullName);
				if (interfaceType == null)
				{
					continue;
				}

				TypeRegistry.Add(assemblyType.FullName, assemblyType);
				addedClass = true;
			}

			return addedClass;
		}


		public void AddType(Type handler, string name)
		{
			TypeRegistry.Add(name, handler);
		}

		public Type GetType(string fullName)
		{
			if (!TypeRegistry.ContainsKey(fullName))
			{
				return null;
			}

			return TypeRegistry[fullName];
		}
	}
}
