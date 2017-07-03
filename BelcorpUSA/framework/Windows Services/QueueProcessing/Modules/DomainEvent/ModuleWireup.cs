using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.QueueProcessing.Common;

// Indicates this assembly is dependent upon its own wireup command:
[module: WireupDependency(typeof(NetSteps.QueueProcessing.Modules.DomainEvent.ModuleWireup))]

namespace NetSteps.QueueProcessing.Modules.DomainEvent
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Data.Entities.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.QueueProcessing.Modules.ModuleBase.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{

		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		/// <seealso cref="NetSteps.Encore.Core.IWireupCoordinator"/>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			var root = Container.Root;
			root.ForType<DomainEventQueueProcessor>()
			   .Register<DomainEventQueueProcessor>()
			   .End();

			IQueueProcessorRegistry registry = Create.New<IQueueProcessorRegistry>();
			registry.Register<DomainEventQueueProcessor>(DomainEventQueueProcessor.CProcessorName);
		}
	}
}

