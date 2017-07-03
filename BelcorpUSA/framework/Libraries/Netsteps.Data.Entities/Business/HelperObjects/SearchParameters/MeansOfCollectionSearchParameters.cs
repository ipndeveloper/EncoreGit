using NetSteps.Common.Base;
using NetSteps.Encore.Core;

namespace NetSteps.Data.Entities.Business
{
    public class MeansOfCollectionSearchParameters : FilterDateRangePaginatedListParameters<MeansOfCollectionSearchData>
    {
        static readonly int CHashCodeSeed = typeof(OrderSearchParameters).GetKeyForType().GetHashCode();

        public int LocationID { get; set; }
        public int PaymentTypeID { get; set; }
        public int CollectionEntityID { get; set; }
        public int StatusID { get; set; }
    }
}
