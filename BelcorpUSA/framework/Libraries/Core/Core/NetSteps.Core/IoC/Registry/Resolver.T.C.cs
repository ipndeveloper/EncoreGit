using System;
using System.Collections.Concurrent;
using NetSteps.Encore.Core.IoC.Constructors;

namespace NetSteps.Encore.Core.IoC.Registry
{
	internal class Resolver<T,C> : IResolver<T> where C: class, T
	{
		protected static readonly bool IsDisposable = typeof(IDisposable).IsAssignableFrom(typeof(C));		
		
		public Resolver(ConstructorSet<T, C> constructors)
		{
			this.Constructors = constructors;
		}

		protected ConstructorSet<T, C> Constructors { get; private set; }

		public Type TargetType { get { return typeof(C); } }

		public virtual bool TryResolve(IContainer container, LifespanTracking tracking, string name, out T instance)
		{
			return TryResolve(container, tracking, name, out instance, Param.EmptyParams);
		}

		public virtual bool TryResolve(IContainer container, LifespanTracking tracking, string name, out T instance, params Param[] parameters)
		{
			CommandBinding<T> command;
			if (Constructors.TryMatchAndBind(parameters, out command))
			{
				instance = command.Execute(container, name);
				if (IsDisposable && tracking == LifespanTracking.Automatic && !Object.ReferenceEquals(container, instance))
				{
					container.Scope.Add(instance as IDisposable);
				}
				container.NotifyObserversOfCreationEvent(typeof(T), instance, name, CreationEventKind.Created);
				return true;
			}
			instance = default(T);
			return false;
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
