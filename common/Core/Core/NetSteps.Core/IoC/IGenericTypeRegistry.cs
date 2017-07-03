using System;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Type registry for generics.
	/// </summary>
	public interface IGenericTypeRegistry : ITypeRegistry
	{
		/// <summary>
		/// Gets a resolver for generic type T
		/// </summary>
		/// <typeparam name="T">generic type T</typeparam>
		/// <returns>gets the type's resolver</returns>
		IResolver<T> ResolverFor<T>();
	}	
}
