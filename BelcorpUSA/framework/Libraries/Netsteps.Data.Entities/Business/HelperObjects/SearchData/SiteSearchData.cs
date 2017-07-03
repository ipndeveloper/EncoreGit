using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public struct SiteSearchData
	{
		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int SiteID { get; set; }

		[TermName("Name")]
		[PropertyName("Name")]
		public string SiteName { get; set; }

		public string Url { get; set; }

		[TermName("Status")]
		public string SiteStatus { get; set; }

		[Display(AutoGenerateField = false)]
		public short SiteStatusID { get; set; }

		[TermName("AccountID")]
		[Display(AutoGenerateField = false)]
		public int AccountID { get; set; }

		[TermName("AccountNumber", "Account Number")]
		[Display(AutoGenerateField = false)]
		public string AccountNumber { get; set; }

		[Display(AutoGenerateField = false)]
		public int? AutoshipOrderID { get; set; }

		public DateTime Enrolled { get; set; }
	}
}
