using System;
using System.Collections.Concurrent;
using NetSteps.Encore.Core.IoC.Constructors;
using System.Threading;

namespace NetSteps.Encore.Core.IoC.Registry
{
	internal class FactorySingletonResolver<T, C>: FactoryResolver<T, C> where C: T
	{
		IContainer _owner;
		Object _synch = new Object();
		C _singleton;

		public FactorySingletonResolver(IContainer owner, Func<IContainer, Param[], C> factory)
			: base(factory)
		{
			if (owner == null) throw new ArgumentNullException("owner");
			_owner = owner;
		}
			
		public override bool TryResolve(IContainer container, LifespanTracking tracking, string name, out T instance, params Param[] parameters)
		{
			var kind = CreationEventKind.Reissued;
			
			lock (_synch)
			{
				if (_singleton != null)
				{
					instance = _singleton;
				}
				else
				{
					var value = _singleton = Factory(container, Param.EmptyParams);					
					_owner.Scope.AddAction(() =>
					{
						lock (_synch)
						{
							if (Object.ReferenceEquals(value, _singleton))
							{
								if (IsDisposable) ((IDisposable)_singleton).Dispose();
								_singleton = default(C);
							}
						}
					});
					instance = value;
					kind = CreationEventKind.Created;
				}
			}
			// don't notify within the lock!
			container.NotifyObserversOfCreationEvent(typeof(T), instance, name, kind);
			return true;
		}
	}

}
