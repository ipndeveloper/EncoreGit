using System.Collections.Generic;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
	public class EmailTemplateSearchParameters : FilterPaginatedListParameters<EmailTemplate>
	{
		public bool? Active { get; set; }

		public List<short> EmailTemplateTypeIDs { get; set; }
	}
}
