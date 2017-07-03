using System;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Registration for a type.
	/// </summary>
	public interface ITypeRegistration : IContainerRegistrationParticipant
	{
		/// <summary>
		/// The registered type.
		/// </summary>
		Type RegisteredType { get; }

		/// <summary>
		/// The target type.
		/// </summary>
		Type TargetType { get; }

		/// <summary>
		/// Indicates whether the registration is named.
		/// </summary>
		bool IsNamed { get; }

		/// <summary>
		/// Gets the registration's scope behavior.
		/// </summary>
		ScopeBehavior ScopeBehavior { get; }

		/// <summary>
		/// Indicates that a type hsould be resolved per request.
		/// </summary>
		/// <returns>the registration (for chaining)</returns>
		ITypeRegistration ResolveAnInstancePerRequest();
		/// <summary>
		/// Indicates that a type hsould be resolved per request.
		/// </summary>
		/// <returns>the registration (for chaining)</returns>
		ITypeRegistration ResolveAnInstancePerScope();
		/// <summary>
		/// Indicates that a type hsould be resolved per request.
		/// </summary>
		/// <returns>the registration (for chaining)</returns>
		ITypeRegistration ResolveAsSingleton();
		/// <summary>
		/// Indicates that a type hsould be resolved per request.
		/// </summary>
		/// <returns>the registration (for chaining)</returns>
		ITypeRegistration DisallowSpecialization();

		/// <summary>
		/// Gets the registered type's untyped resolver.
		/// </summary>
		IResolver UntypedResolver { get; }		
	}

}
