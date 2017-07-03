using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IEmailTemplateRepository : ISearchRepository<EmailTemplateSearchParameters, PaginatedList<EmailTemplate>>
	{
		Dictionary<short, List<EmailTemplate>> GetEvitesEmailTemplates();
        List<EmailTemplate> GetEmailTemplatesByEmailTemplateTypeID(short emailTemplateTypeID);
	}
}
