using NetSteps.Accounts.Downline.Common.Repositories;
using NetSteps.Accounts.Downline.UI.Common;
using NetSteps.Accounts.Downline.UI.Common.Configuration;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.Accounts.Downline.UI.Service.ModuleWireup))]

namespace NetSteps.Accounts.Downline.UI.Service
{
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			var root = Container.Root;

			root.ForType<IDownlineUIService>()
				.Register<DownlineUIService>(
					Param.Resolve<IDownlineRepository>(),
					Param.Resolve<IDownlineUIConfiguration>()
				)
				.End();
		}
	}
}
