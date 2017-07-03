using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Extensions;
using System;

namespace NetSteps.Data.Entities.Business
{
    public class BlockingType
    {
        public static PaginatedList<BlockingTypeSearchData> Get(BlockingTypeSearchParameters searchParameter)
        {
            return BlockingTypeExtensions.Get(searchParameter);
        }

        public static Int16 Save(BlockingTypeSearchParameters Parameter)
        {
            return BlockingTypeExtensions.Save(Parameter);
        }

        public static AccountBlockingSearchData GetAccountIsLocked(BlockingTypeSearchParameters Parameter)
        {
            return BlockingTypeExtensions.GetAccountIsLocked(Parameter);
        }

        public static PaginatedList<BlockingTypeSearchData> GetAccountBlockingHistory(BlockingTypeSearchParameters searchParameter)
        {
            return BlockingTypeExtensions.GetAccountBlockingHistory(searchParameter);
        }

        public static int SaveAccountBlockingHistory(BlockingTypeSearchParameters parameter)
        {
            return BlockingTypeExtensions.SaveAccountBlockingHistory(parameter);
        }

    }
}
