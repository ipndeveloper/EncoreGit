using AddressValidator.Ups.Config;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using AddressValidator.Common;
using NetSteps.Encore.Core.Wireup.Meta;

[module: WireupDependency(typeof(AddressValidator.Ups.ModuleWireup))]
namespace AddressValidator.Ups
{
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}
