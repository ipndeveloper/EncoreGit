// -----------------------------------------------------------------------
// <copyright file="AuthenticatedPrincipalProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Web.Providers
{
    using NetSteps.Data.Common;
    using NetSteps.Data.Common.Providers;
    using NetSteps.Encore.Core.IoC;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ContainerRegister(typeof(IAuthenticatedPrincipalProvider), RegistrationBehaviors.OverrideDefault, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class AuthenticatedPrincipalProvider : IAuthenticatedPrincipalProvider
    {
        public Data.Common.ISitePrincipal GetCurrent()
        {
            return Create.New<ISitePrincipal>();
        }
    }
}
