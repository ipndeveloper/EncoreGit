using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Cache;

namespace nsCore.Areas.Accounts.Models.AccountTitleOverrides
{
	public class AccountTitleOverrideIndexModel
	{
		ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		public IEnumerable<IOverrideReason> OverrideReasons
		{
			get
			{
				var overrideReasonSource = this._commissionsService.GetOverrideReasonSources().FirstOrDefault(x => x.Code == "ATO");
				return this._commissionsService.GetOverrideReasonsForSource(overrideReasonSource);
			}
		}

		public Dictionary<string, string> DisplayReasons
		{
			get
			{
				return 
					this.OverrideReasons
					.ToDictionary(ors => ors.OverrideReasonId.ToString(), ors => this.GetTerm(ors))
					;
			}
		}

		public string GetTerm(IOverrideReason overrideReason)
		{
			return CachedData.Translation.GetTerm(overrideReason.TermName, overrideReason.Name);
		}
	}
}