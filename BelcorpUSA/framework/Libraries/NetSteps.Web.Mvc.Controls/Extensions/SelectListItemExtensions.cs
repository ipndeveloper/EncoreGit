using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Web.Mvc.Controls.Configuration;

namespace NetSteps.Web.Mvc.Controls.Extensions
{
    public static class SelectListItemExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItemsForProvince(
            this IEnumerable<StateProvince> stateProvinces, int countryId, int? selectedId = null)
        {
            var iso = AddressConfiguration.GetISO(SmallCollectionCache.Instance.Countries.GetById(countryId).CountryCode);

            bool isoUseName = iso.ProvinceValueToUse.UseProvinceName;

            return stateProvinces.Where(state => state.CountryID == countryId)
                .OrderBy(state => state.StateAbbreviation)
                .Select(state =>
                        new SelectListItem
                            {
                                Text = isoUseName ? state.Name : state.StateAbbreviation,
                                Value = state.StateProvinceID.ToString(),
                                Selected = selectedId != null && (state.StateProvinceID == selectedId)
                            });
        }
    }
}
