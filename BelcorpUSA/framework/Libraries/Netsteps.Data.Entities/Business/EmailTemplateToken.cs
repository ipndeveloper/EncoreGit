using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class EmailTemplateToken
	{
        public static List<string> GetTokenNames()
        {
            return BusinessLogic.GetTokenNames();
        }

        public static List<EmailTemplateToken> GetStandardTokens(Account account)
        {
            return BusinessLogic.GetStandardTokens(account);
        }

        public static List<EmailTemplateToken> GetSponsorStandardTokens(Account account)
        {
            return BusinessLogic.GetSponsorStandardTokens(account);
        }

        public static List<EmailTemplateToken> GetFakeTokensForPreview()
        {
            return BusinessLogic.GetFakeTokensForPreview();
        }

        public static List<EmailTemplateToken> GetPreviewTokens(EmailTemplate emailTemplate, Account account, Account sponsorAccount)
        {
            return BusinessLogic.GetPreviewTokens(emailTemplate, account, sponsorAccount);
        }

        public static List<EmailTemplateToken> CombineTokens(List<EmailTemplateToken> defaultTokens, List<EmailTemplateToken> overrideTokens)
        {
            return BusinessLogic.CombineTokens(defaultTokens, overrideTokens);
        }

        public static List<EmailTemplateToken> GetAllCustomTokensForAccount(int emailTemplateId, int accountId)
        {
            try
            {
                List<EmailTemplateToken> result = Repository.GetAllCustomTokensForAccount(emailTemplateId, accountId);
                result.ForEach(x => x.StartEntityTracking());
                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<EmailTemplateToken> GetDefaultTokens(int emailTemplateId)
        {
            try
            {
                List<EmailTemplateToken> result = Repository.GetDefaultTokens(emailTemplateId);
                result.ForEach(x => x.StartEntityTracking());
                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
	}
}
