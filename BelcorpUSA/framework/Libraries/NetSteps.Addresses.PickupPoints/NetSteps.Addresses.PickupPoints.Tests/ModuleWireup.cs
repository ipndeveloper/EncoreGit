using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Addresses.PickupPoints.Tests.ModuleWireup))]

namespace NetSteps.Addresses.PickupPoints.Tests
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Addresses.PickupPoints.ModuleWireup))]
	
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		/// <seealso cref="IWireupCoordinator"/>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			
		}
	}
}
