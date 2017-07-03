using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class AlertTemplateSearchParameters : FilterDateRangePaginatedListParameters<AlertTemplate>
    {
        public int? AlertTemplateID { get; set; }

        public string Name { get; set; }

        public string StoredProcedureName { get; set; }

        public short? AlertPriorityID { get; set; }

        public bool? Active { get; set; }
    }
}
