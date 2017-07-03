using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Cache;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace DistributorBackOffice.Models.Performance
{
	public class PartialDownlineToolGridModel
	{
		readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		public IEnumerable<IPeriod> CurrentAndPastPeriods
		{
			get
			{
				return this._commissionsService.GetCurrentAndPastPeriods().OrderByDescending(p => p.PeriodId);
			}
		}

		public List<KeyValuePair<int?, string>> ExtraPageSizeOptions
		{
			get
			{
				return new List<KeyValuePair<int?, string>>
								   {
									   new KeyValuePair<int?, string>(500, "500"),
									   new KeyValuePair<int?, string>(1000, "1000")
								   };
			}
		}

		public Account CurrentAccount
		{
			get
			{
				return CoreContext.CurrentAccount;
			}
		}

		public IEnumerable<StateProvince> ActiveStateProvinces
		{
			get
			{
				return (from stateProvince in SmallCollectionCache.Instance.StateProvinces
						join country in SmallCollectionCache.Instance.Countries on stateProvince.CountryID equals country.CountryID
						where country.Active
						select stateProvince);
			}
		}

		public IEnumerable<ITitle> Titles
		{
			get
			{
				return this._commissionsService.GetTitles();
			}
		}
	}
}