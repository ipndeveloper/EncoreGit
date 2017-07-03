namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Interface for tenant resolvers
	/// </summary>
	public interface ITenantResolver
	{
		/// <summary>
		/// Tries to resolve the current tenant ID.
		/// </summary>
		/// <param name="tenantID">variable to hold the tenant id</param>
		/// <returns>true if a tenant ID is resolved; otherwise false</returns>
		bool TryResolveTenant(out object tenantID);
	}
}
