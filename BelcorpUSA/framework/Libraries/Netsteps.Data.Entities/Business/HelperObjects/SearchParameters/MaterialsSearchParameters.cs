using System;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;


namespace NetSteps.Data.Entities.Business
{
    public class MaterialsSearchParameters : FilterDateRangePaginatedListParameters<MaterialBN>
    {
        static readonly int CHashCodeSeed = typeof(MaterialsSearchParameters).GetKeyForType().GetHashCode();

        public int? MaterialID { get; set; }
        public decimal? SKU { get; set; }
        public bool? Active { get; set; }
        public decimal? EANCode { get; set; }
        public string BPCSCode { get; set; }
        public string UnityType { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }
        public string NCM { get; set; }
        public decimal? Origin { get; set; }
        public decimal? OriginCountry { get; set; }
        public string Brand { get; set; }
        public string Group { get; set; }
        public int? MarketID { get; set; }

        public int? accountNumberOrName { get; set; }
        public int? ConsultantOrCustomerAccountID { get; set; }

        
    }
}
