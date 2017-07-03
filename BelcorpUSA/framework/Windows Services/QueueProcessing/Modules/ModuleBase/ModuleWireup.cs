using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

// Indicates this assembly is dependent upon its own wireup command:
[module: WireupDependency(typeof(NetSteps.QueueProcessing.Modules.ModuleBase.ModuleWireup))]

namespace NetSteps.QueueProcessing.Modules.ModuleBase
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
		/// <seealso cref="NetSteps.Encore.Core.IWireupCoordinator"/>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}
