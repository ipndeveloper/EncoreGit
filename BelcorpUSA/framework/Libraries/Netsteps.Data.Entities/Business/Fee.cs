using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Business
{
    public class Fee
    {
        public static PaginatedList<FeeSearchData> Get(FeeSearchParameters searchParameter)
        {
            return FeeExtensions.Get(searchParameter);
        }

        public static int Save(FeeSearchParameters searchParameter){
            return FeeExtensions.Save(searchParameter);
        }

        public static void Delete(FeeSearchParameters searchParameter)
        {
            FeeExtensions.Delete(searchParameter);
        }
    }
}
