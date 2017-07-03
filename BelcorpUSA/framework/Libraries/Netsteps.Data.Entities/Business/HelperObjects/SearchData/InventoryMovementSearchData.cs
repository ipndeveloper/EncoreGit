using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class InventoryMovementSearchData
    {
        [TermName("CUV")]
        public string CUV { get; set; }

        [TermName("SAPcode")]
        public string SAPcode { get; set; }

        [TermName("Description")]
        public string Description { get; set; }

        [TermName("Date")]
        public string Date { get; set; }

        [TermName("AllocatedBefore")]
        public string AllocatedBefore { get; set; }

        [TermName("AllocatedAfter")]
        public string AllocatedAfter { get; set; }

        [TermName("OnHandBefore")]
        public string OnHandBefore { get; set; }

        [TermName("OnHandAfter")]
        public string OnHandAfter { get; set; }
    }
}
