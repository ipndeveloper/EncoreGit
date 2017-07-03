using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
	public class ApplicationUsageLogSearchParameters : FilterDateRangePaginatedListParameters<ApplicationUsageLog>
	{
		public int? ApplicationID { get; set; }
		public int? UserID { get; set; }

		public string AssemblyName { get; set; }
		public string MachineName { get; set; }
		public string ClassName { get; set; }
		public string MethodName { get; set; }
	}
}
