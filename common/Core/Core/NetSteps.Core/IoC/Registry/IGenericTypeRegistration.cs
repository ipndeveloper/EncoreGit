using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Encore.Core.IoC.Registry
{
	/// <summary>
	/// Registration for a generic type.
	/// </summary>
	public interface IGenericTypeRegistration : ITypeRegistration
	{
		/// <summary>
		/// Gets a resolver for type T
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <returns>the resolver for type T</returns>
		IResolver<T> ResolverFor<T>();
	}
}
