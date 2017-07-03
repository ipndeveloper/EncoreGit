// -----------------------------------------------------------------------
// <copyright file="SitePrincipal.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Web.Providers
{
    using System;
    using NetSteps.Data.Common;
    using NetSteps.Encore.Core.IoC;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ContainerRegister(typeof(ISitePrincipal), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class SitePrincipal : ISitePrincipal
    {
        public int ID
        {
            get { throw new NotImplementedException(); }
        }
    }
}
