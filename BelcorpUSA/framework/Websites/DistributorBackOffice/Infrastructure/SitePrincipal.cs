using NetSteps.Data.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Helpers;

namespace DistributorBackOffice.Infrastructure
{
    [ContainerRegister(typeof(ISitePrincipal), RegistrationBehaviors.OverrideDefault, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class SitePrincipal : ISitePrincipal
    {
        /// <summary>
        /// Represents the ID of the current sites principal
        /// In this case it represents the current logged in user of the distributor work station.
        /// </summary>
        public int ID
        {
            get { return CoreContext.CurrentAccount.AccountID; }
        }
    }
}