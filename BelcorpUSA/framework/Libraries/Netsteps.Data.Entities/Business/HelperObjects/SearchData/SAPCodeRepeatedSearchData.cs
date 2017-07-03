using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class SAPCodeRepeatedSearchData
    {
        [TermName("SAPCode")]
        public string SAPCode { get; set; }

        [TermName("Quantity")]
        public string Quantity { get; set; }    

    }
}
