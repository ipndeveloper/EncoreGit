using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Commissions.Common.Models;

namespace nsCore.Areas.Accounts.Models.Browse
{
	public class AccountBrowseModel
	{
		public AccountBrowseModel(AccountSearchParameters accountSearchParameters)
		{
			this.SearchParameters = accountSearchParameters;
			this.Sponsor = accountSearchParameters.SponsorID.HasValue ? NetSteps.Data.Entities.Account.LoadSlim(accountSearchParameters.SponsorID.Value) : new AccountSlimSearchData(); 
		}

		public AccountSearchParameters SearchParameters { get; private set; }

		public AccountSlimSearchData Sponsor { get; private set; }

		public Dictionary<string,string> StatusList
		{
			get
			{
				return SmallCollectionCache
					.Instance
					.AccountStatuses
					.ToDictionary(ast => ast.AccountStatusID.ToString(), ast => ast.GetTerm())
					.OrderBy(x => x.Value)
					.ToDictionary(x => x.Key, y => y.Value);
			}
		}

		public Dictionary<string, string> AccoutTypeList
		{
			get
			{
				return SmallCollectionCache
					.Instance
					.AccountTypes
					.ToDictionary(at => at.AccountTypeID.ToString(), at => at.GetTerm())
					.OrderBy(x => x.Value)
					.ToDictionary(x => x.Key, y => y.Value);
			}
		}

		public Dictionary<string, string> StateList
		{
			get
			{
				return StateProvince.DropDownListFromCache();
			}
		}

		public Dictionary<string, string> CountryList
		{
			get
			{
				return TermTranslation.GetTranslatedCountries().ToDictionary(x => x.Key.ToString(), x => x.Value);
			}
		}

		public Dictionary<string, string> TitleList
		{
			get
			{
				var commissionsService = Create.New<ICommissionsService>();
				return commissionsService.GetTitles()
					.ToDictionary(sp => sp.TitleId.ToString(), sp => this.GetTerm(sp))
					.OrderBy(x => x.Value)
					.ToDictionary(x => x.Key, y => y.Value);
			}
		}

		string GetTerm(ITitle title)
		{
			return CachedData.Translation.GetTerm(title.TermName, title.TitleName);
		}


	}
}