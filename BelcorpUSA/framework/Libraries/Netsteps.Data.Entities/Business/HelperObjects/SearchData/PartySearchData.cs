using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public class PartySearchData
	{
		[Display(AutoGenerateField = false)]
		public int PartyID { get; set; }

		[TermName("Name")]
		public string Name { get; set; }

		[TermName("OrderID", "Order ID")]
		public string OrderNumber { get; set; }

		[Display(AutoGenerateField = false)]
		public int OrderID { get; set; }

		[TermName("Status")]
		[Display(AutoGenerateField = false)]
		public int OrderStatusID { get; set; }

		[Display(AutoGenerateField = false)]
		public int CurrencyID { get; set; }

		[TermName("Date")]
		public DateTime StartDate { get; set; }

		[TermName("Total")]
		public decimal? Total { get; set; }

		[Display(AutoGenerateField = false)]
		public bool UseEvites { get; set; }

		[TermName("Host")]
		public string HostFirstName { get; set; }

		[Display(AutoGenerateField = false)]
		public string HostLastName { get; set; }
	}
}
