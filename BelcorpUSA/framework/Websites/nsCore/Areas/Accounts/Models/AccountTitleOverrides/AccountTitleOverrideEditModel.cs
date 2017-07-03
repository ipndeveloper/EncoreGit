using System.Collections.Generic;
using System.Linq;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities.Cache;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Accounts.Models.AccountTitleOverrides
{
	public class AccountTitleOverrideEditModel
	{
		readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		public AccountTitleOverrideEditModel(IAccountTitleOverride accountTitleOverride)
		{
			AccountTitleOverride = accountTitleOverride;
		}

		public IAccountTitleOverride AccountTitleOverride { get; private set; }

		public IEnumerable<IPeriod> CurrentPeriods
		{
			get
			{
				return _commissionsService.GetCurrentPeriods();
			}
		}

		public IEnumerable<ITitle> Titles
		{
			get
			{
				return _commissionsService.GetTitles();
			}
		}

		public IEnumerable<ITitleKind> TitleTypes
		{
			get
			{
				return _commissionsService.GetTitleKinds();
			}
		}

		public IEnumerable<IOverrideReason> OverrideReasons
		{
			get
			{
				var overrideReasonSource = _commissionsService.GetOverrideReasonSources().FirstOrDefault(x => x.Code == "ATO");
				return _commissionsService.GetOverrideReasonsForSource(overrideReasonSource);
			}
		}

		public string GetTerm(ITitleKind titleKind)
		{
			return CachedData.Translation.GetTerm(titleKind.TermName, titleKind.Name);
		}

		public string GetTerm(ITitle title)
		{
			return CachedData.Translation.GetTerm(title.TermName, title.TitleName);
		}

		public string GetTerm(IOverrideReason overrideReason)
		{
			return CachedData.Translation.GetTerm(overrideReason.TermName, overrideReason.Name);
		}
	}
}