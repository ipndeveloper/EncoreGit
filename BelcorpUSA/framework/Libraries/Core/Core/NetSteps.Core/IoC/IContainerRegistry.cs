using System.Collections.Generic;
using System;
using NetSteps.Encore.Core.IoC.Registry;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// A container registry is used to register types and resolve those registrations.
	/// </summary>
	public interface IContainerRegistry : IContainerOwned
	{
		/// <summary>
		/// Gets the registry specific to type T
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <returns>the type registry</returns>
		ITypeRegistry<T> ForType<T>();
		/// <summary>
		/// Gets the registry for a generic type.
		/// </summary>
		/// <param name="generic">the generic type</param>
		/// <returns>the registry for the generic type</returns>
		IGenericTypeRegistry ForGenericType(Type generic);

		/// <summary>
		/// Determines if type T is registered.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <returns>true if type T is registered; otherwise false</returns>
		bool IsTypeRegistered<T>();
		/// <summary>
		/// Determins if a type is registered.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>true if the type is registered; otherwise false</returns>
		bool IsTypeRegistered(Type type);

        /// <summary>
        /// Determines if a type is registered with the name given.
        /// </summary>
        /// <typeparam name="T">typeof T</typeparam>
        /// <param name="name">the name</param>
        /// <returns>true if the type is registered; otherwise false</returns>
        bool IsTypeRegisteredWithName<T>(string name);

        /// <summary>
        /// Determines if a type is registered with the name given.
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="name">the name</param>
        /// <returns>true if the type is registered; otherwise false</returns>
        bool IsTypeRegisteredWithName(Type type, string name);

		/// <summary>
		/// Tries to get the resolver for a type.
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="value">variable to hold the resolver upon success</param>
		/// <returns>true if successful; otherwise false</returns>
		bool TryGetResolverForType(Type type, out IResolver value);
		/// <summary>
		/// Tries to get the resolver for type T
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="value">variable to hold the resolver upon success</param>
		/// <returns>true if successful; otherwise false</returns>
		bool TryGetResolverForType<T>(out IResolver<T> value);
		/// <summary>
		/// Tries to get a named resolver for type T
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="name">the name</param>
		/// <param name="value">variable to hold the resolver upon success</param>
		/// <returns>true if successful; otherwise false</returns>
		bool TryGetNamedResolverForType<T>(string name, out IResolver<T> value);

		/// <summary>
		/// Tries to get the type registry management object for a type.
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="value">variable to hold the result upon success</param>
		/// <returns>true if successful; otherwise false</returns>
		bool TryGetTypeRegistryManagement(Type type, out ITypeRegistryManagement value);
	}
}
