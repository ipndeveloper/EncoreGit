using System;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IMailDomainRepository : IBaseRepository<MailDomain, Int32>
	{
		IEnumerable<string> LoadInternalDomains();
		MailDomain LoadDefaultForInternal();
	}
}
