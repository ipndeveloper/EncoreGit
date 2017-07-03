using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class AuditLogSearchParameters : FilterDateRangePaginatedListParameters<AuditLog>
    {
        public int? PK { get; set; }

        public string TableName { get; set; }

        public string ColumnName { get; set; }
    }
}
