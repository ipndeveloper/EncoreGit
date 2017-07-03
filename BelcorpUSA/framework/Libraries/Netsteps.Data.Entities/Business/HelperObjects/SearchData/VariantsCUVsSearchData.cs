using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class VariantsCUVsSearchData
    {
        public int ProductRelationID { get; set; }

        public int MaterialID { get; set; }

        public string SapCode { get; set; }

        public int? OfferType { get; set; }

        public int? ExternalCode { get; set; }
    }
}
