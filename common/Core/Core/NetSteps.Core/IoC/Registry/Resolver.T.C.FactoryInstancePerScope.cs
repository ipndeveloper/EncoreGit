using System;
using System.Collections.Concurrent;
using NetSteps.Encore.Core.IoC.Constructors;

namespace NetSteps.Encore.Core.IoC.Registry
{
	internal class FactoryInstancePerScopeResolver<T, C>: FactoryResolver<T, C> where C: T
	{
		readonly ConcurrentDictionary<Guid, T> _containerInstances = new ConcurrentDictionary<Guid, T>();

		public FactoryInstancePerScopeResolver(Func<IContainer, Param[], C> factory)
			: base(factory)
		{
		}
				
		public override bool TryResolve(IContainer container, LifespanTracking tracking, string name, out T instance, params Param[] parameters)
		{
			var kind = CreationEventKind.Reissued;
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
				if (!tempIssued)
				{
					temp = Factory(container, parameters);
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
					kind = CreationEventKind.Created;
					instance = temp;
					break;
				}
			}
			container.NotifyObserversOfCreationEvent(typeof(T), instance, name, kind);
			return true;
		}
	}

}
