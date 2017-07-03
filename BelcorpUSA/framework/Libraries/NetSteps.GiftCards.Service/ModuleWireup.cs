using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.GiftCards.Service.ModuleWireup))]

namespace NetSteps.GiftCards.Service
{
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
        }
    }
}
