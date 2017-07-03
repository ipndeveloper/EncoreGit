// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Wireup.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   Wires up the IoC
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using NetSteps.Encore.Core.Wireup.Meta;

// Indicates this assembly is dependent upon its own wireup command:
[module: WireupDependency(typeof(HttpRedirectModule.Wireup))]

namespace HttpRedirectModule
{

    using NetSteps.Encore.Core.IoC;
    using NetSteps.Encore.Core.Wireup;

    /// <summary>
    /// Wires up the IoC
    /// </summary>
    public class Wireup : WireupCommand
    {
        /// <summary>
        /// Performs the wireups
        /// </summary>
        /// <param name="coordinator">
        /// The coordinator.
        /// </param>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            var root = Container.Root;
            root.ForType<IMappingBusinessLogic>()
                .Register<MappingBusinessLogic>(Param.Resolve<IMappingRepository>())
                .End();
        }
    }
}
