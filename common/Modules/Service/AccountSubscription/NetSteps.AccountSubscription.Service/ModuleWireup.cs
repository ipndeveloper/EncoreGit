using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.IoC;
using NetSteps.AccountSubscription.Common;
using NetSteps.AccountSubscription.Service;

[assembly: Wireup(typeof(NetSteps.AccountSubscription.Service.ModuleWireup))]

namespace NetSteps.AccountSubscription.Service
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    public class ModuleWireup : WireupCommand
    {
        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        /// <seealso cref="IWireupCoordinator"/>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Current.ForType<IAccountSubscriptionService>()
                .Register<AccountSubscriptionService>()
                .ResolveAnInstancePerRequest()
                .End();            
        }
    }
}
