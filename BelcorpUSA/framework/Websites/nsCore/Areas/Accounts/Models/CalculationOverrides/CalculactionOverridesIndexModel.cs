using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Cache;

namespace nsCore.Areas.Accounts.Models.CalculationOverrides
{
	public class CalculactionOverridesIndexModel
	{
		ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		public CalculactionOverridesIndexModel()
			: this(new List<ICalculationOverride>())
		{
		}

		public CalculactionOverridesIndexModel(IEnumerable<ICalculationOverride> calculationOverrides)
		{
			this.CalculationOverrides = calculationOverrides;
		}

		public IEnumerable<ICalculationOverride> CalculationOverrides { get; private set; }

		public Dictionary<string, string> OverrideReasons
		{
			get
			{
				var overrideReasonSource = this._commissionsService.GetOverrideReasonSources().FirstOrDefault(x => x.Code == "CO");
				return this._commissionsService
					.GetOverrideReasonsForSource(overrideReasonSource)
					.ToDictionary(or => or.OverrideReasonId.ToString(), or => this.GetTerm(or));
			}
		}

		string GetTerm(IOverrideReason obj)
		{
			return CachedData.Translation.GetTerm(obj.TermName, obj.Name);
		}
	}
}