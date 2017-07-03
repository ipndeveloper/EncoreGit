using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

//@01 20150717 BR-CC-003 G&S LIB: Se crea la clase con sus respectivos métodos

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class PaymentMethodsSearchParameters : FilterDateRangePaginatedListParameters<PaymentMethodsSearchData>
    {

        static readonly int CHashCodeSeed = typeof(OrderSearchParameters).GetKeyForType().GetHashCode();

        public int? RuleID { get; set;}
        public int? OrderStatusID {get; set;}
        public string CollectionEntityID { get; set; }
        public string DaysPayment { get; set; }
        public string tolerance { get; set; }
        public int? AccountTypeId { get; set; }
        public int? OrderType { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string county { get; set; } 

    }
}
