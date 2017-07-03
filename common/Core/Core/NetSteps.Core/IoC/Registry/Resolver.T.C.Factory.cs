using System;
using System.Collections.Concurrent;
using NetSteps.Encore.Core.IoC.Constructors;

namespace NetSteps.Encore.Core.IoC.Registry
{
	internal class FactoryResolver<T, C>: IResolver<T> where C: T
	{
		protected static readonly bool IsDisposable = typeof(IDisposable).IsAssignableFrom(typeof(C));

		public FactoryResolver(Func<IContainer, Param[], C> factory)
		{
			this.Factory = factory;
		}

		protected Func<IContainer, Param[], C> Factory { get; private set; }

		public Type TargetType { get { return typeof(C); } }

		public virtual bool TryResolve(IContainer container, LifespanTracking tracking, string name, out T instance)
		{
			return TryResolve(container, tracking, name, out instance, Param.EmptyParams);
		}

		public virtual bool TryResolve(IContainer container, LifespanTracking tracking, string name, out T instance, params Param[] parameters)
		{
			instance = Factory(container, parameters);
			if (IsDisposable && tracking == LifespanTracking.Automatic && !Object.ReferenceEquals(container, instance))
			{
				container.Scope.Add(instance as IDisposable);
			}
			container.NotifyObserversOfCreationEvent(typeof(T), instance, name, CreationEventKind.Created);
			return true;
		}

		public bool TryUntypedResolve(IContainer container, LifespanTracking tracking, string name, out object instance)
		{
			T temp;
			var result = TryResolve(container, tracking, name, out temp, Param.EmptyParams);
			instance = temp;
			return result;
		}

		public bool TryUntypedResolve(IContainer container, LifespanTracking tracking, string name, out object instance, params Param[] parameters)
		{
			T temp;
			var result = TryResolve(container, tracking, name, out temp, parameters);
			instance = temp;
			return result;
		}
	}
}
