using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: NetSteps.Encore.Core.IoC.ModuleWireupTask]

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Wires this module.
	/// </summary>
	public class ModuleWireupTask : WireupTaskAttribute
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public ModuleWireupTask()
			: base(Wireup.WireupPhase.Immediate)
		{
		}

		/// <summary>
		/// Performs wireup.
		/// </summary>
		/// <param name="coordinator"></param>
		protected override void PerformTask(Wireup.IWireupCoordinator coordinator)
		{
			// Attach the root container as a wireup observer...
			coordinator.RegisterObserver(Container.Root as IRootContainer);
		}
	}
}
