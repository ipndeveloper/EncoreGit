using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common;
using NetSteps.Data.Entities;
using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Helpers;

namespace DistributorBackOffice.Models.Home
{
	public class WidgetModel
	{
		private readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		public WidgetModel(Widget widget)
		{
			this.Widget = widget;
		}

		public Widget Widget { get; private set; }

		public IEnumerable<IPeriod> CurrentAndPastPeriods
		{
			get
			{
				return this._commissionsService.GetCurrentAndPastPeriods();
			}
		}

	}
}