using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public struct HtmlContentSearchData
	{
		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int HtmlContentID { get; set; }

		[Display(AutoGenerateField = false)]
		public int LanguageID { get; set; }

		[TermName("Language")]
		public string Language { get; set; }

		[Display(AutoGenerateField = false)]
		public int HtmlContentStatusID { get; set; }

		[TermName("Status")]
		public string HtmlContentStatus { get; set; }

		[TermName("Name")]
		public string Name { get; set; }

		[TermName("PublishDate", "Publish Date")]
		public DateTime? PublishDate { get; set; }
	}
}
