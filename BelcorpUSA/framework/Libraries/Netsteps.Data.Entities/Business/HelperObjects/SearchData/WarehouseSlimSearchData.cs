using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class WarehouseSlimSearchData
    {

        [TermName("CUV")]
        public string SKU { get; set; }

        [TermName("Name")]
        public string Name { get; set; }

        [TermName("SAPCODE")]
        public String SAPCODE { get; set; }

        [TermName("ProductType")]
        public String ProductType { get; set; }

        [TermName("QtyonHand")]
        public String QtyonHand { get; set; }

        [TermName("Buffer")]
        public String Buffer { get; set; }

        [TermName("ReorderLevel")]
        public String ReorderLevel { get; set; }

        [TermName("Allocated")]
        public String Allocated { get; set; }

        [TermName("Avalible")]
        public String Avalible { get; set; }

        [TermName("OfferType")]
        public string OfferType { get; set; }
        
        [TermName("Active")]
        public string Active { get; set; }

        [TermName("Variant")]
        public string Variant { get; set; }
    }        
}

