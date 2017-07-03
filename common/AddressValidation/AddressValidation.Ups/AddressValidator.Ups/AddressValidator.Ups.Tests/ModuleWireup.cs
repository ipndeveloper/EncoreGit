using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using AddressValidator.Common;

[module: WireupDependency(typeof(AddressValidator.Ups.Tests.ModuleWireup))]

namespace AddressValidator.Ups.Tests
{
    [WireupDependency(typeof(Ups.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            var root = Container.Root;

			root.ForType<IAddressValidator>()
				.Register<UpsAddressValidator>()
				.ResolveAsSingleton()
				.End();
        }
    }
}
