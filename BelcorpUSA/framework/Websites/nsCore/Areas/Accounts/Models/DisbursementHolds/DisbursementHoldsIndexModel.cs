using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities.Cache;

namespace nsCore.Areas.Accounts.Models.DisbursementHolds
{
	public class DisbursementHoldsIndexModel
	{
		ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		public Dictionary<string, string> OverrideReasons
		{
			get
			{
				var overrideReasonSource = this._commissionsService.GetOverrideReasonSources().FirstOrDefault(x => x.Code == "DH");
				return this._commissionsService
					.GetOverrideReasonsForSource(overrideReasonSource)
					.ToDictionary(ors => ors.OverrideReasonId.ToString(), ors => this.GetTerm(ors));
			}
		}

		string GetTerm(IOverrideReason obj)
		{
			return CachedData.Translation.GetTerm(obj.TermName, obj.Name);
		}
	}
}