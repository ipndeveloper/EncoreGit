using System.Collections.Generic;
using System.Linq;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Cache;

namespace nsCore.Areas.Accounts.Models.DisbursementHolds
{
	public class DisbursementHoldsEditModel
	{
		readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		public DisbursementHoldsEditModel(IDisbursementHold disbursementHold)
		{
			DisbursementHold = disbursementHold;
		}

		public IDisbursementHold DisbursementHold { get; private set; }

		public Dictionary<string, string> OverrideReasons
		{
			get
			{
				var overrideReasonSource = _commissionsService.GetOverrideReasonSources().FirstOrDefault(x => x.Code == "DH");
				return _commissionsService
					.GetOverrideReasonsForSource(overrideReasonSource)
					.ToDictionary(ors => ors.OverrideReasonId.ToString(), ors => GetTerm(ors));
			}
		}

		string GetTerm(IOverrideReason obj)
		{
			return CachedData.Translation.GetTerm(obj.TermName, obj.Name);
		}
	}
}