using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities.Cache;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Accounts.Models.CalculationOverrides
{
	public class CalculationOverridesEditModel
	{
		ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		public CalculationOverridesEditModel(ICalculationOverride calculationOverride)
		{
			this.CalculationOverride = calculationOverride;
		}

		public ICalculationOverride CalculationOverride { get; private set; }

		public Dictionary<string, string> CalculationTypes
		{
			get
			{
				return this._commissionsService
					.GetCalculationKinds()
					.Where(ct => ct.IsUserOverridable)
					.ToDictionary(ors => ors.CalculationKindId.ToString(), ors => this.GetTerm(ors));
			}
		}

		public Dictionary<string, string> OverrideTypes
		{
			get
			{
				return this._commissionsService
					.GetOverrideKinds()
					.ToDictionary(ors => ors.OverrideKindId.ToString(), ors => ors.OverrideCode);
			}
		}

		public Dictionary<string, string> OverrideReasons
		{
			get
			{
				var overrideReasonSource = this._commissionsService.GetOverrideReasonSources().FirstOrDefault(x => x.Code == "CO");
				return this._commissionsService
					.GetOverrideReasonsForSource(overrideReasonSource)
					.ToDictionary(ors => ors.OverrideReasonId.ToString(), ors => this.GetTerm(ors));
			}
		}

		public IEnumerable<IPeriod> CurrentPeriods
		{
			get
			{
				return this._commissionsService
					.GetCurrentPeriods();
			}
		}

		string GetTerm(IOverrideReason obj)
		{
			return CachedData.Translation.GetTerm(obj.TermName, obj.Name);

		}

		string GetTerm(ICalculationKind obj)
		{
			return CachedData.Translation.GetTerm(obj.TermName, obj.Name);
		}

	}
}