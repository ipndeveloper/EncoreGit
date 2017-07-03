using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Business
{
    public class Minimum
    {
        public static PaginatedList<MinimumSearchData> Get(MinimumSearchParameters searchParameter)
        {
            return MinimumExtensions.Get(searchParameter);
        }

        public static int Save(MinimumSearchParameters searchParameter)
        {
            return MinimumExtensions.Save(searchParameter);
        }

        public static void Delete(MinimumSearchParameters searchParameter)
        {
            MinimumExtensions.Delete(searchParameter);
        }

        public static List<MinimumSearchData> ListDropDownTypes(string tableName)
        {
            return MinimumExtensions.ListDropDownTypes(tableName);
        }

    }
}
