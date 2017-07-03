using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.IoC;

[module: WireupDependency(typeof(AddressValidator.Common.ModuleWireup))]

namespace AddressValidator.Common
{
	public class ModuleWireup : WireupCommand
	{
		protected override void  PerformWireup(IWireupCoordinator coordinator)
		{
			Container.Current.ForType<IAddressValidator>()
				.Register<NullAddressValidator>()
				.ResolveAsSingleton()
				.End();
		}
	}
}
