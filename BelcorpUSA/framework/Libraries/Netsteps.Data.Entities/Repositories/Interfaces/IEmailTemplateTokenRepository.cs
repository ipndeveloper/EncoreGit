using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IEmailTemplateTokenRepository
	{
	    List<EmailTemplateToken> GetAllCustomTokensForAccount(int emailTemplateId, int accountId);
	    List<EmailTemplateToken> GetDefaultTokens(int emailTemplateId);
	}
}
