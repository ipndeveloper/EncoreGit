﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.IoC;

[assembly: Wireup(typeof(NetSteps.Modules.AccountRenewal.Common.ModuleWireup))]

namespace NetSteps.Modules.AccountRenewal.Common
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
            Container.Current.ForType<IAccountRenewal>()
                .Register<DefaultAccountRenewal>()                
                .ResolveAnInstancePerRequest()
                .End();
        }
    }
}

