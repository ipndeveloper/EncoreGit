using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.TaxCalculator.Vertex.Tests.ModuleWireup))]

namespace NetSteps.TaxCalculator.Vertex.Tests
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.TaxCalculator.Vertex.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		/// <seealso cref="IWireupCoordinator"/>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			var root = Container.Root;

		}
	}
}
