using System;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public struct AutoshipBatchReportData
	{
		public int AutoshipBatchID { get; set; }

		public int SucceededCount { get; set; }

		public int FailureCount { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public string UserName { get; set; }

		public string Notes { get; set; }
	}
}
