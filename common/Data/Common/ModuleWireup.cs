using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Common.Registries;
using NetSteps.Data.Common.Registries.Concrete;


// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Data.Common.ModuleWireup))]

namespace NetSteps.Data.Common
{
	public class ModuleWireup : WireupCommand
	{

		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			Container.Current.ForType<IOrderStepHandlerRegistry>()
				.Register<OrderStepHandlerRegistry>()
				.ResolveAsSingleton()
				.End();
		}
	}
}
