using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.Properties;


namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Indicates how a container should track the lifespan an 
	/// object it creates.
	/// </summary>
	public enum LifespanTracking
	{
		/// <summary>
		/// Default tracking == Automatic.
		/// </summary>
		Default = 0,
		/// <summary>
		/// Indicates the container must automatically track the lifespans
		/// and ensure IDisposable instances are disposed.
		/// </summary>
		Automatic = 0,
		/// <summary>
		/// Indicates the instances are externally tracked. Callers are
		/// responsible for cleaning up IDisposable instances.
		/// </summary>
		External = 1,
	}

	/// <summary>
	/// Container interface.
	/// </summary>
	public interface IContainer : IDisposable
	{
		/// <summary>
		/// Gets the container's unique ID.
		/// </summary>
		Guid Key { get; }

		/// <summary>
		/// Gets the container's registry.
		/// </summary>
		IContainerRegistry Registry { get; }
		
		/// <summary>
		/// Indicates whether the container is the root.
		/// </summary>
		bool IsRoot { get; }
		
		/// <summary>
		/// Indicates whether the container is within a tenant scope.
		/// </summary>
		bool IsTenant { get; }
		
		/// <summary>
		/// Gets the current tenant identifier if the container is
		/// within a tentant scope; otherwise null.
		/// </summary>
		object TenantID { get; }

		/// <summary>
		/// Creates a new instance of the target type.
		/// </summary>
		/// <param name="tracking"></param>
		/// <param name="targetType"></param>
		/// <returns></returns>
		object NewUntyped(LifespanTracking tracking, Type targetType);
				
		/// <summary>
		/// Resolves type T to an instance according to it's registration.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="tracking">Lifespan tracking used for the instance
		/// if it is newly created.</param>
		/// <returns>an instance of type T</returns>
		T New<T>(LifespanTracking tracking);

		/// <summary>
		/// Resolves type T to an instance according to it's registration, utilizing the
		/// parameters given if the instance must be newly created.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="tracking">Lifespan tracking used for the instance
		/// if it is newly created.</param>
		/// <param name="parameters">Initialization parameters whose values are used
		/// if an instance must be newly created.</param>
		/// <returns>an instance of type T</returns>
		T NewWithParams<T>(LifespanTracking tracking, params Param[] parameters);

		/// <summary>
		/// Resolves type T to an instance according to a named registration.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="tracking">Lifespan tracking used for the instance
		/// if it is newly created.</param>
		/// <param name="name">the name</param>
		/// <returns>an instance of type T</returns>
		T NewNamed<T>(LifespanTracking tracking, string name);

		/// <summary>
		/// Resolves type T to an instance according to a named registration, utilizing the
		/// parameters given if the instance must be newly created.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="tracking">Lifespan tracking used for the instance
		/// if it is newly created.</param>
		/// <param name="name">the name</param>
		/// <param name="parameters">Initialization parameters whose values are used
		/// if an instance must be newly created.</param>
		/// <returns>an instance of type T</returns>
		T NewNamedWithParams<T>(LifespanTracking tracking, string name, params Param[] parameters);

		/// <summary>
		/// Resolves a specific implementation of type T according to the implementation's registration.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="tracking">Lifespan tracking used for the instance
		/// if it is newly created.</param>
		/// <param name="subtype"></param>
		/// <returns>an instance of the implementation type</returns>
		T NewImplementationOf<T>(LifespanTracking tracking, Type subtype);

		/// <summary>
		/// Makes a child container from the current container.
		/// </summary>
		/// <param name="options">options </param>
		/// <returns></returns>
		IContainer MakeChildContainer(CreationContextOptions options);

		/// <summary>
		/// Prepares the container for being shared in multiple threads.
		/// </summary>
		/// <returns></returns>
		IContainer ShareContainer();

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
		/// Ensures a cache is registered with the context and returns
		/// that cache.
		/// </summary>
		/// <typeparam name="K">Registration key type K</typeparam>
		/// <typeparam name="C">Cache type C</typeparam>
		/// <param name="key">registration key</param>
		/// <param name="factory">factory method that will be used to create a new cache if one is not
		/// already present.</param>
		/// <returns>a cache</returns>
		C EnsureCache<K, C>(K key, Func<C> factory);
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
