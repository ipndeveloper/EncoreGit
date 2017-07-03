using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
	public class AutoshipLogSearchParameters : FilterDateRangePaginatedListParameters<AutoshipLog>
	{
		public int? AutoshipBatchID { get; set; }
	}
}
