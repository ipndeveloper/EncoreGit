using System;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public struct AutoshipRunReportDetailData
	{
		public int AutoshipBatchID { get; set; }

		public int AutoshipLogID { get; set; }

		public int TemplateOrderID { get; set; }

		public int NewOrderID { get; set; }

		public int SucceededCount { get; set; }

		//		//public int FailureCount { get; set; }

		public int Results { get; set; }

		public DateTime? DateAutoshipRan { get; set; }
	}
}
