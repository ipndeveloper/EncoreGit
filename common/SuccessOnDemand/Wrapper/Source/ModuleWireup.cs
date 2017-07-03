using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.SOD.Common;
using NetSteps.Encore.Core.IoC;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.SOD.Wrapper.ModuleWireup))]

namespace NetSteps.SOD.Wrapper
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.SOD.Common.ModuleWireup))]
    public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		/// <seealso cref="NetSteps.Encore.Core.IWireupCoordinator"/>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
            coordinator.WireupDependencies(typeof(IResponse).Assembly);
            var response = Create.New<IResponse>(); // ensures the response is generated/wired up
		}
	}
}