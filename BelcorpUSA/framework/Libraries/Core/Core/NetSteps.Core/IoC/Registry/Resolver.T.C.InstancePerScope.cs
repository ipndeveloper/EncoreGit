using System;
using System.Collections.Concurrent;
using NetSteps.Encore.Core.IoC.Constructors;

namespace NetSteps.Encore.Core.IoC.Registry
{
	internal class InstancePerScopeResolver<T, C>: Resolver<T, C> where C: class, T
	{
		readonly ConcurrentDictionary<Guid, T> _containerInstances = new ConcurrentDictionary<Guid, T>();

		public InstancePerScopeResolver(ConstructorSet<T, C> constructors)
			: base(constructors)
		{
		}

		public override bool TryResolve(IContainer container, LifespanTracking tracking, string name, out T instance, params Param[] parameters)
		{
			var kind = CreationEventKind.Reissued;

			CommandBinding<T> command = null;
			Guid key = container.Key;
			bool tempIssued = false;
			T temp = default(T);
			while (true)
			{
				if (_containerInstances.TryGetValue(key, out instance))
				{
					if (tempIssued && IsDisposable)
						((IDisposable)temp).Dispose();
					break;
				}
				if (command == null && !Constructors.TryMatchAndBind(parameters, out command))
				{
					instance = default(T);
					return false;
				}
				if (!tempIssued)
				{
					temp = command.Execute(container, name);
					tempIssued = true;
				}
				if (_containerInstances.TryAdd(key, temp))
				{
					container.Scope.AddAction(() =>
						{
							T value;
							if (_containerInstances.TryRemove(key, out value))
							{
								if (IsDisposable)
								{
									((IDisposable)value).Dispose();
								}
							}
						});
					instance = temp;
					kind = CreationEventKind.Created;
					break;
				}
			}
			container.NotifyObserversOfCreationEvent(typeof(T), instance, name, kind);
			return true;
		}
	}

}
