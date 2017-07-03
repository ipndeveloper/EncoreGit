using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class PickingPendingsSearchData
    {
        [TermName("OrderNumber")]
        public string OrderNumber { get; set; }

        [TermName("OrderType")]
        public string OrderType { get; set; }

        [TermName("OrderStatus")]
        public string OrderStatus { get; set; }

        [TermName("AccountNumber")]
        public string AccountNumber { get; set; }

        [TermName("REPName")]
        public string REPName { get; set; }

        [TermName("OrderDate")]
        public string OrderDate { get; set; }

    }
}
