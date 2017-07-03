using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class ErrorLogSearchParameters : FilterDateRangePaginatedListParameters<ErrorLog>
    {
        public string SessionID { get; set; }
        public int? ApplicationID { get; set; }
        public int? AccountID { get; set; }
        public int? UserID { get; set; }
        public int? OrderID { get; set; }

        public string MachineName { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
