using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class StateProvinceBusinessLogic
	{
		public override void CleanDataBeforeSave(Repositories.IStateProvinceRepository repository, StateProvince entity)
		{
			entity.Name = entity.Name.ToCleanStringNullable();
			entity.StateAbbreviation = entity.StateAbbreviation.ToCleanStringNullable();
		}

        public Dictionary<string, string> DropDownListFromCache()
        {
            var states = from sp in SmallCollectionCache.Instance.StateProvinces
                         join c in SmallCollectionCache.Instance.Countries
                            on sp.CountryID equals c.CountryID
                         select new
                         {
                             Value = c.CountryCode3 + " - " + sp.StateAbbreviation,
                             Key = Convert.ToString(sp.StateProvinceID)
                         };

            return states.ToDictionary(x => x.Key, y => y.Value);
        }
	}
}
