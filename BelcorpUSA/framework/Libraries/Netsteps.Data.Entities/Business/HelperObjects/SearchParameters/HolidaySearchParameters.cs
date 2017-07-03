
using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class HolidaySearchParameter : FilterDateRangePaginatedListParameters<HolidaySearchData>
    {

        static readonly int CHashCodeSeed = typeof(HolidaySearchParameter).GetKeyForType().GetHashCode();
        public int? HolidayID { get; set; }
        public int? CountryID { get; set; }
        public int? StateID { get; set; }
        public string DateHoliday { get; set; }
    }
}
