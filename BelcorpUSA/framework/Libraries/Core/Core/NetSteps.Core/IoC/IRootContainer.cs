using System;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Interface for root containers.
	/// </summary>
	public interface IRootContainer : IContainer, IWireupObserver
	{
		/// <summary>
		/// Registers a multi-tenant resolver with the container.
		/// </summary>
		/// <typeparam name="TTenantResolver">tenant resolver type TTenantResolver</typeparam>
		/// <returns>A type registration for the tenant resolver</returns>
		ITypeRegistration RegisterMultiTenant<TTenantResolver>() where TTenantResolver : class, ITenantResolver, new();
		/// <summary>
		/// Registers a multi-tenant resolver with the container.
		/// </summary>
		/// <typeparam name="TTenantResolver">tenant resolver type TTenantResolver</typeparam>
		/// <param name="factory">factory that will provide the tenant resolver instances</param>
		/// <returns>A type registration for the tenant resolver</returns>
		ITypeRegistration RegisterMultiTenant<TTenantResolver>(Func<IContainer, Param[], TTenantResolver> factory) where TTenantResolver : class, ITenantResolver;

		/// <summary>
		/// Indicates whether the container has multi-tenant support.
		/// </summary>
		bool SupportsMultipleTenants { get; }
		/// <summary>
		/// Tries to resolve a tenant ID.
		/// </summary>
		/// <param name="id">variable to hold the tenant ID upon success</param>
		/// <returns>true if the tenant is resolved; otherwise false</returns>
		bool TryResolveTenant(out object id);
		/// <summary>
		/// Registers a tenant ID returning the tenant specific root container.
		/// </summary>
		/// <param name="id">tenant ID</param>
		/// <returns>the tenant specific root container</returns>
		IContainer RegisterTenant(object id);
		/// <summary>
		/// Resolves the current tenant and returns the tenant specific root container.
		/// </summary>
		/// <returns>the tenant specific root container</returns>
		IContainer ResolveCurrentTenant();
		/// <summary>
		/// Resolves the tenant by ID and returns the tenant specific root container.
		/// </summary>
		/// <param name="id">the tenant ID</param>
		/// <returns>the tenant specific root container</returns>
		IContainer ResolveTenantByID(object id);
	}	
}
