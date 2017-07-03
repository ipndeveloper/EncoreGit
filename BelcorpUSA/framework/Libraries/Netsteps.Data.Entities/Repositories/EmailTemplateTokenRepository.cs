using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class EmailTemplateTokenRepository
	{
        public List<EmailTemplateToken> GetAllCustomTokensForAccount(int emailTemplateId, int accountId)
        {
            return new List<EmailTemplateToken>();

            //return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            //{
            //    using (NetStepsEntities context = CreateContext())
            //    {
            //        return (from ett in context.EmailTemplateTokens
            //                where ett.EmailTemplateID == emailTemplateId && ett.AccountID == accountId
            //                select ett).ToList();
            //    }
            //});
        }

        public List<EmailTemplateToken> GetDefaultTokens(int emailTemplateId)
        {
            return new List<EmailTemplateToken>();


            //return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            //{
            //    using (NetStepsEntities context = CreateContext())
            //    {
            //        return (from ett in context.EmailTemplateTokens
            //                where ett.EmailTemplateID == emailTemplateId && ett.AccountID == null
            //                select ett).ToList();
            //    }
            //});
        }
    }
}
