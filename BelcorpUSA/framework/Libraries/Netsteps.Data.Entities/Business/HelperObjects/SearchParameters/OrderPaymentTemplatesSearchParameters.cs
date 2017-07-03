using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business
{
    public class OrderPaymentTemplatesSearchParameters : FilterDateRangePaginatedListParameters<OrderPaymentTemplatesSearchData>
    {
        public int OrderPaymentTemplateId { get; set; }

        public string Description { get; set; }
       
        public int Days { get; set; }
       
        public int MinimalAmount { get; set; }

        public string Type { get; set; }

        public int EmailTemplateNameId { get; set; }
    }
}
