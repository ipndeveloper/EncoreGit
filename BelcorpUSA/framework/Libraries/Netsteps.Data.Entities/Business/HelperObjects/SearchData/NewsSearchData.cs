using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public struct NewsSearchData
	{
		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int NewsID { get; set; }

		[Display(AutoGenerateField = false)]
		public int NewsTypeID { get; set; }

		[TermName("Type")]
		[Display(Name = "Type")]
		public string NewsType { get; set; }

		[TermName("Title")]
		public string Title { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		[TermName("Active")]
		public bool Active { get; set; }
	}
}
