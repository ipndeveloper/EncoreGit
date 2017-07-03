using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities;


namespace DistributorBackOffice.Models.Home
{
	public class AutoshipOverviewModel
	{
		public bool AutoshipEnabled { get; set; }
		public Widget Widget { get; set; }

		public AutoshipOverviewModel(NetSteps.Data.Entities.Widget widget)
		{
			AutoshipEnabled = ConfigurationManager.GetAppSetting("AutoshipEnabled", true);
			Widget = widget;
		}
	}
}