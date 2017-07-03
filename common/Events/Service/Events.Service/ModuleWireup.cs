﻿using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.Events.Service.ModuleWireup))]

namespace NetSteps.Events.Service
{
	[WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}