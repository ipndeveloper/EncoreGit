using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class CollectionEntitiesSearchParameter : FilterDateRangePaginatedListParameters<CollectionEntitySearchData>
    {

        static readonly int CHashCodeSeed = typeof(CollectionEntitiesSearchParameter).GetKeyForType().GetHashCode();

        public int? CompanyID { get; set; }
        public int? PaymentTypeID { get; set; }
        public string CollectionEntityID { get; set; }
        public string status { get; set; }
    }
}
