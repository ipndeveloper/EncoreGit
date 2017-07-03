using System;

namespace NetSteps.Web.Mvc.Controls.Models.Enrollment
{
	[Serializable]
	public class EnrollmentProductInfo
	{
		public string SKU { get; set; }
		public int Quantity { get; set; }
		public bool IsEditable { get; set; }
		public int? MarketID { get; set; }
	}
}
