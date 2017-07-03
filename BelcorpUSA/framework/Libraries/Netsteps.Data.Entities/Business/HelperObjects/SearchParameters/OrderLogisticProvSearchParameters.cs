using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class OrderLogisticProvParameters : FilterDateRangePaginatedListParameters<OrderLogisticProviderSearchData>
    {
        public int? OrderNumber { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? LogisticProviderID { get; set; }
        public int? PeriodID { get; set; }
        public int OrderShipmentID { get; set; }
    }
}
