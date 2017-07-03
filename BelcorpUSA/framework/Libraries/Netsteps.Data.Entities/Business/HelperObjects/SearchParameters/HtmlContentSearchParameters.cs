using System;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
	public class HtmlContentSearchParameters : FilterDateRangePaginatedListParameters<HtmlContent>
	{
		public int HtmlSectionID { get; set; }

		new public int? LanguageID { get; set; }

		public int? HtmlContentStatusID { get; set; }

		public string Name { get; set; }

		public DateTime? PublishDate { get; set; }
	}
}
