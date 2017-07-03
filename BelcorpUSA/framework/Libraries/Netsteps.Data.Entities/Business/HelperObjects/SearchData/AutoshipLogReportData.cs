using System;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public struct AutoshipLogReportData
	{
		public int AccountID { get; set; }

		public string AccountNumber { get; set; }

		public int AutoshipScheduleID { get; set; }

		public int AutoshipLogID { get; set; }

		public int TemplateOrderID { get; set; }

		public int? NewOrderID { get; set; }

		public string NewOrderNumber { get; set; }

		public bool Succeeded { get; set; }

		public string Results { get; set; }

		public DateTime? DateAutoshipRan { get; set; }
	}
}
