using System;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Options for creation contexts.
	/// </summary>
	[Flags]
	public enum CreationContextOptions
	{
		/// <summary>
		/// No options.
		/// </summary>
		None = 0,
		/// <summary>
		/// Indicates the creation context should track instances.
		/// </summary>
		InstanceTracking = 1,
		/// <summary>
		/// Indicates that caching is enabled.
		/// </summary>
		EnableCaching = 2,
		/// <summary>
		/// Indicates the creation context should inherit cached items from the outer context.
		/// </summary>
		InheritCache = EnableCaching | 4,
		/// <summary>
		/// Indicates that the context should inherit its scope from an outer scope if one exists.
		/// </summary>
		InheritScope = 8,
	}

	/// <summary>
	/// Kinds of creation events.
	/// </summary>
	public enum CreationEventKind
	{
		/// <summary>
		/// Indicates the factory created the instance (or caused to be created).
		/// </summary>
		Created = 0,
		/// <summary>
		/// Indicates the factory invoked an initializer for the instance.
		/// </summary>
		Initialized = 1,
		/// <summary>
		/// Indicates the factory copy-constructed an instance based on another instance.
		/// </summary>
		Copied = 2,
		/// <summary>
		/// Indicates the factory cached the instance.
		/// </summary>
		Cached = 3,
		/// <summary>
		/// Indicates the factory reissued the instance.
		/// </summary>
		Reissued = 4,
		/// <summary>
		/// Indicates the factory ducktyped an instance of another type.
		/// </summary>
		DuckType = 99,
	}

	/// <summary>
	/// Interface for creation contexts.
	/// </summary>
	public interface ICreationContext : IDisposable
	{
		/// <summary>
		/// Creates a subscription to creation events against type T.
		/// </summary>
		/// <typeparam name="T">subscription target type T</typeparam>
		/// <param name="observer">An action that will be called upon creation events against type T</param>
		void Subscribe<T>(Action<Type, T, string, CreationEventKind> observer);

		/// <summary>
		/// Notifies observers of type T that a creation event occurred.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="requestedType"></param>
		/// <param name="instance"></param>
		/// <param name="name"></param>
		/// <param name="evt"></param>
		void NotifyObserversOfCreationEvent<T>(Type requestedType, T instance, string name, CreationEventKind evt);

		/// <summary>
		/// Ensures a cache is registered with the context, creating the cache if necessary
		/// using the factory given.
		/// </summary>
		/// <typeparam name="K">Registration key type K</typeparam>
		/// <typeparam name="C">Cache type C</typeparam>
		/// <param name="key">registration key</param>
		/// <param name="factory">factory method that will be used to create a new cache if one is not
		/// already present.</param>
		/// <returns>a cache</returns>
		C EnsureCache<K,C>(K key, Func<C> factory);
		/// <summary>
		/// Ensures a cache is registered with the context and returns
		/// that cache.
		/// </summary>
		/// <typeparam name="K">Registration key type K</typeparam>
		/// <typeparam name="C">Cache type C</typeparam>
		/// <param name="key">registration key</param>
		/// <returns>a cache</returns>
		C EnsureCache<K, C>(K key)
			where C : new();

		/// <summary>
		/// Tries to get a cache from the creation context.
		/// </summary>
		/// <typeparam name="K">Registration key type K</typeparam>
		/// <typeparam name="C">Cache type C</typeparam>
		/// <param name="key">registration key</param>
		/// <param name="cache">output variable where the cache will be returned upon success</param>
		/// <returns>true if the cache was returned; otherwise false.</returns>
		bool TryGetCache<K, C>(K key, out C cache)
			where C : new();

		/// <summary>
		/// Gets the cleanup scope for the context.
		/// </summary>
		ICleanupScope Scope { get; }
	}
}
