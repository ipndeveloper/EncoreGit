

using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Encore.Core.IoC.Containers
{
	internal partial class Container : Disposable, IContainer
	{
		interface IObservationKey
		{
			Type TargetType { get; }
			void AddObserver(Delegate observer);
		}
		class ObservationKey<T> : IObservationKey
		{
			readonly object _innerLock = new Object();
			readonly List<Action<Type, T, string, CreationEventKind>> _observers = new List<Action<Type, T, string, CreationEventKind>>();
			List<WeakReference<T>> _instances;

			public ObservationKey(bool tracking)
			{
				if (tracking)
					_instances = new List<WeakReference<T>>();
			}
			public Type TargetType { get { return typeof(T); } }

			public void AddObserver(Delegate observer)
			{
				lock (_innerLock)
				{
					_observers.Add((Action<Type, T, string, CreationEventKind>)observer);
				}
			}

			internal void NotifyObservers(Type requestedType, T instance, string name, CreationEventKind evt)
			{
				if (_instances != null)
				{
					_instances.Add(new WeakReference<T>(instance));
				}
				foreach (var observer in _observers)
				{
					observer(requestedType, instance, name, evt);
				}
			}
			internal IEnumerable<T> Instances
			{
				get
				{
					if (_instances == null) return new T[0];
					else return from i in _instances
											where i.IsAlive
											select i.StrongTarget;
				}
			}
		}
		Lazy<Dictionary<Type, IObservationKey>> _observers = new Lazy<Dictionary<Type, IObservationKey>>(System.Threading.LazyThreadSafetyMode.PublicationOnly);

		public void Subscribe<T>(Action<Type, T, string, CreationEventKind> observer)
		{
			var observers = _observers.Value;
			IObservationKey key;
			if (!observers.TryGetValue(typeof(T), out key))
			{
				key = new ObservationKey<T>(_options.HasFlag(CreationContextOptions.InstanceTracking));
				observers.Add(typeof(T), key);
			}
			key.AddObserver(observer);
		}

		public void NotifyObserversOfCreationEvent<T>(Type requestedType, T instance, string name, CreationEventKind evt)
		{
			var observers = _observers.Value;
			IObservationKey key;
			if (observers.TryGetValue(typeof(T), out key))
			{
				ObservationKey<T> okey = key as ObservationKey<T>;
				if (okey != null)
				{
					okey.NotifyObservers(requestedType, instance, name, evt);
				}
			}
			if (_parent != null)
			{
				_parent.NotifyObserversOfCreationEvent(requestedType, instance, name, evt);
			}
		}
	}
}
