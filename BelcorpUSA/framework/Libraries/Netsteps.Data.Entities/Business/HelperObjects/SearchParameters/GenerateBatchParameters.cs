using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;
using System.Data;

namespace NetSteps.Data.Entities.Business
{
    public class GenerateBatchParameters : FilterDateRangePaginatedListParameters<OrderToBatch>
    {
        public int WarehouseID { get; set; }
        public int AccountID { get; set; }
        public int MaterialID { get; set; }
        public int ProductID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PeriodID { get; set; }
        public int PeriodID2 { get; set; }
        public int ShippingMethodID { get; set; }
        public int AccountTypeID { get; set; }
        public int OrderTypeID { get; set; }
        public int WarehousePrinterID { get; set; }
        public string OrderNumber { get; set; }
        public int LogisticProviderID { get; set; }
        public int RouteID { get; set; }
        public DataTable dtOrderCustomerIDs { get; set; }
        public bool Reprocess { get; set; }
        public byte ShowGenerated { get; set; }
    }
}
