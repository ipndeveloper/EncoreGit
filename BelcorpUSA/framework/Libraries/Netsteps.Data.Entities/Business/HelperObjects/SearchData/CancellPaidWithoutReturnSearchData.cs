using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class CancellPaidWithoutReturnSearchData
    {
        [TermName("OrderNumber")]
        public string OrderNumber { get; set; }

        [TermName("OrderType")]
        public string OrderType { get; set; }

        [TermName("OrderStatus")]
        public String OrderStatus { get; set; }

        [TermName("AccountNumber")]
        public string AccountNumber { get; set; }

        [TermName("Name")]
        public string Name { get; set; }

        [TermName("OrderDate")]
        public String OrderDate { get; set; }       
    }
}
