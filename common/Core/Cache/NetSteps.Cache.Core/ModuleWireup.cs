using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[module: WireupDependency(typeof(NetSteps.Core.Cache.ModuleWireup))]

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Wireup command. Performs default wireup for the module.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{		
		/// <summary>
		/// Wires up this module.
		/// </summary>
		/// <param name="coordinator"></param>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}
