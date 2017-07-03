using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using System;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;

namespace NetSteps.Extensibility
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		/// <seealso cref="IWireupCoordinator"/>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
            Container.Root.ForType<IDataObjectExtensionProviderRegistry>()
                .Register<DataObjectExtensionProviderRegistry>()
                .ResolveAsSingleton()
                .End();
		}
	}
}