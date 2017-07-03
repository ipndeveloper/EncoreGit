
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace NetSteps.Encore.Core.IoC.Containers
{
	internal partial class Container : Disposable, IContainer
	{
		Lazy<ConcurrentDictionary<object, object>> _localCaches = new Lazy<ConcurrentDictionary<object, object>>(LazyThreadSafetyMode.PublicationOnly);

		public C EnsureCache<K, C>(K key, Func<C> factory)
		{
			if (_parent != null && _options.HasFlag(CreationContextOptions.InheritCache))
			{
				return _parent.EnsureCache(key, factory);
			}
			else
			{
				var caches = _localCaches.Value;
				return (C)caches.GetOrAdd(key, ignored => factory());
			}
		}

		public C EnsureCache<K, C>(K key)
			where C : new()
		{
			if (_parent != null && _options.HasFlag(CreationContextOptions.InheritCache))
			{
				return _parent.EnsureCache<K, C>(key);
			}
			else
			{
				var caches = _localCaches.Value;
				return (C)caches.GetOrAdd(key, ignored => new C());
			}
		}

		public bool TryGetCache<K, C>(K key, out C cache)
			where C : new()
		{
			if (_options.HasFlag(CreationContextOptions.EnableCaching))
			{
				var caches = _localCaches.Value;
				object instance;
				var result = caches.TryGetValue(key, out instance);
				cache = (C)instance;
				return result;
			}
			cache = default(C);
			return false;
		}
	}

}
