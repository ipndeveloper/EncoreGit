using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Business
{
    public class BlockingSubType
    {

        public static PaginatedList<BlockingSubTypeSearchData> Get(BlockingSubTypeSearchParameters SearchParameter)
        {
            return BlockingSubTypeExtensions.Get(SearchParameter);
        }

        public static Int16 Save(BlockingSubTypeSearchParameters parameter)
        {
           Int16 AccountBlockingSubTypeID =  BlockingSubTypeExtensions.Save(parameter);          

           return AccountBlockingSubTypeID;
        }
        public static PaginatedList<BlockingSubTypeSearchData> ListTypeProcess(BlockingSubTypeSearchParameters parameter)
        {
            return BlockingSubTypeExtensions.ListTypeProcess(parameter);
        }

        public static PaginatedList<BlockingSubTypeSearchData> GetAccountSubTypeProcess(BlockingSubTypeSearchParameters searchParameter)
        {
            return BlockingSubTypeExtensions.GetAccountSubTypeProcess(searchParameter);
        }

    }
}
