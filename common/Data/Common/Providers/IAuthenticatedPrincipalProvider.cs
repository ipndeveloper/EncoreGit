// -----------------------------------------------------------------------
// <copyright file="IAccountProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Data.Common.Providers
{

    /// <summary>
    /// Provides access to current account information
    /// </summary>
    public interface IAuthenticatedPrincipalProvider
    {
        /// <summary>
        /// Get ISitePrincipal based on current authenticated account
        /// </summary>
        /// <returns></returns>
        ISitePrincipal GetCurrent();
    }
}
