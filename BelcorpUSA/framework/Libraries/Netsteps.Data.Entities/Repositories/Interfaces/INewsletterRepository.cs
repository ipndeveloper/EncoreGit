using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface INewsletterRepository
	{
        List<Newsletter> LoadHistoricalNewslettersForConsultant(int accountId);
    }
}
