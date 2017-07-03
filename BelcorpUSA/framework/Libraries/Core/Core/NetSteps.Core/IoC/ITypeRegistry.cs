using System;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Root, untyped interface for type registry.
	/// </summary>
	public interface ITypeRegistry : IContainerOwned
	{
		/// <summary>
		/// Gets the registered type.
		/// </summary>
		Type RegisteredType { get; }

		/// <summary>
		/// Indicates whether the registration can be specialized or 
		/// overridden by subsequent registrations.
		/// </summary>
		bool CanSpecializeRegistration { get; }

		/// <summary>
		/// Registers the concrete type.
		/// </summary>
		/// <param name="concreteType">A concrete type to be issued by 
		/// the container when instances of the registered type are resolved.</param>
		/// <returns>The concrete registration (can be used to specialize the
		/// registration).</returns>
		ITypeRegistration Register(Type concreteType);

        /// <summary>
        /// Indicates whether named registration is present.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool HasNamedRegistration(string name);

		/// <summary>
		/// Gets the type's untyped resolver.
		/// </summary>
		IResolver UntypedResolver { get; }
	}

}
