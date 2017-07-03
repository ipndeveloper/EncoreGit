﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.IoC;

[module: WireupDependency(typeof(NetSteps.Modules.Downline.Common.ModuleWireup))]

namespace NetSteps.Modules.Downline.Common
{
	/// <summary>
	/// Wireup dependencies
	/// </summary>
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wireup dependencies
		/// </summary>
		/// <param name="coordinator"></param>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			Container.Current.ForType<IDownlineSearch>()
				.Register<DefaultDownlineSearch>()
				.ResolveAsSingleton()
				.End();
		}
	}
}
