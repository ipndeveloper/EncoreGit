using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.QueueProcessing.Common;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.ModuleWireup))]

namespace NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Data.Entities.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.QueueProcessing.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Caching.ModuleWireup))]
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
			root.ForType<ExpiringPromotionReminderProcessor>()
			   .Register<ExpiringPromotionReminderProcessor>()
			   .End();

			IQueueProcessorRegistry registry = Create.New<IQueueProcessorRegistry>();
			registry.Register<ExpiringPromotionReminderProcessor>(ExpiringPromotionReminderProcessor.CProcessorName);
		}
	}
}