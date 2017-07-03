using System;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Named type registration for type T.
	/// </summary>
	/// <typeparam name="T">type T</typeparam>
	public interface INamedTypeRegistration<T> : ITypeRegistration<T>, INamedRegistration
	{
	}	
}
