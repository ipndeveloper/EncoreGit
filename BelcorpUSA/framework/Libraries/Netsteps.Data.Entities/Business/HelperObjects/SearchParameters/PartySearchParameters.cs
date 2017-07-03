using System.Collections.Generic;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class PartySearchParameters : FilterPaginatedListParameters<Party>
    {
        public int? OrderStatusID { get; set; }

        public int? AcountID { get; set; }

        public int? NumberOfOpenDays { get; set; }

        public List<int> OrderStatuses { get; set; }

        public List<int> ExcludedOrderTypes { get; set; }
    }
}
