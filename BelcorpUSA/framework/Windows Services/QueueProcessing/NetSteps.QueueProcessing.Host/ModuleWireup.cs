using NetSteps.Common.Interfaces;

// Indicates this assembly is dependent upon its own wireup command:

using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.QueueProcessing.Common;
using NetSteps.Commissions.Service;

[assembly: Wireup(typeof(NetSteps.QueueProcessing.Windows.Host.ModuleWireup))]
namespace NetSteps.QueueProcessing.Windows.Host
{
	using NetSteps.QueueProcessing.Host;

	/// <summary>
	/// Wireup command called at bootstrap time by the wire up coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Extensibility.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Data.Entities.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Plugins.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.QueueProcessing.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Commissions.Service.CommissionsServiceModuleWireup))]
	[WireupDependency(typeof(Service.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			var root = Container.Root;
			root.ForType<IServiceHost>()
			   .Register<QueueProcessingHost>(Param.Resolve<ILogger>())
			   .End();
		}
	}
}