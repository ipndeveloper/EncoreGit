using System;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
	public class AutoshipOrderSearchParameters : FilterDateRangePaginatedListParameters<AutoshipOrder>
	{
		public int? OrderStatusID { get; set; }
		public int? OrderTypeID { get; set; }

		public string OrderNumber { get; set; }

		public int? AccountID { get; set; }
		public string AccountNumberOrName { get; set; }

		public string CustomerName { get; set; }
		public string CustomerAccountNumber { get; set; }
		public string ConsultantName { get; set; }
		public string ConsultantAccountNumber { get; set; }
		public DateTime? CommissionDate { get; set; }

		public int? AutoshipBatchID { get; set; }

		public int? AutoshipScheduleID { get; set; }
	}
}
