using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.NonCommissionablePaymentTypeProvider.Service;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(ModuleWireup))]

namespace NetSteps.NonCommissionablePaymentTypeProvider.Service
{
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Encore.Core.Wireup;
    using NetSteps.Encore.Core.Wireup.Meta;
    using NetSteps.Extensibility.Core;
    using NetSteps.NonCommissionablePaymentTypeProvider.Common;
    using NetSteps.OrderAdjustments.Common;

	[WireupDependency(typeof(NetSteps.Data.Entities.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.OrderAdjustments.Common.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			// If I registered this guy using the class attribute it wasn't registered by the time
			// the RegisterAdjustmentProvider method was called below. So it's getting registered here.
			Container.Current.ForType<INonCommissionablePaymentOrderAdjustmentProvider>()
					 .Register<NonCommissionablePaymentOrderAdjustmentProvider>()
					 .ResolveAsSingleton()
					 .End();

			Container.Current.ForType<INonCommissionablePaymentOrderAdjustmentProfile>()
					 .Register<NonCommissionablePaymentOrderAdjustmentProfile>()
					 .ResolveAnInstancePerRequest()
					 .End();

			Create.New<IDataObjectExtensionProviderRegistry>().RegisterExtensionProvider<INonCommissionablePaymentOrderAdjustmentProvider>(NonCommissionablePaymentOrderAdjustmentProviderInfo.OrderAdjustmentProviderKey);
			Create.New<IOrderAdjustmentProviderManager>().RegisterAdjustmentProvider(Create.New<INonCommissionablePaymentOrderAdjustmentProvider>());
		}
	}
}
