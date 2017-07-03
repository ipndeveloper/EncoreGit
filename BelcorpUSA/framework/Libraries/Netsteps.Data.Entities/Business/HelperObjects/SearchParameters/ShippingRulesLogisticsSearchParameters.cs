using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Encore.Core;

namespace NetSteps.Data.Entities.Business
{
    public class ShippingRulesLogisticsSearchParameters : FilterDateRangePaginatedListParameters<ShippingRulesLogisticsSearchData>
    {
        static readonly int CHashCodeSeed = typeof(OrderSearchParameters).GetKeyForType().GetHashCode();

        public int ShippingOrderTypeID { get; set; }
        public int ShippingMethodID { get; set; }
        public int StatusID { get; set; }
        public int WarehouseID { get; set; }
        public int LogisticsProviderID { get; set; }
    }
}
