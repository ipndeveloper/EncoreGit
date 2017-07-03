using System;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Type registration for type T
	/// </summary>
	/// <typeparam name="T">type T</typeparam>
	public interface ITypeRegistration<T> : ITypeRegistration
	{
		/// <summary>
		/// Gets the resolver for type T.
		/// </summary>
		IResolver<T> Resolver { get; }
	}
}
