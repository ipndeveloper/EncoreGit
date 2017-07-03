using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class NewsletterRepository
    {
        protected override Func<NetStepsEntities, IQueryable<Newsletter>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Newsletter>>(
                   (context) => from n in context.Newsletters
                                               .Include("EmailTemplate")
                                               .Include("EmailTemplate.EmailTemplateTokens")
                                select n);
            }
        }

        public List<Newsletter> LoadHistoricalNewslettersForConsultant(int accountId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return (from n in context.Newsletters
                            //where n.ConsultantMergeHistory.AccountId = accountID
                            select n).ToList();
                }
            });
        }

        /*
        public List<Newsletter> LoadAll()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return (from n in context.Newsletters
                            select n).ToList();
                }
            });
        }

        public Newsletter Load(int newsletterId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return (from n in context.Newsletters
                            where n.NewsletterID == newsletterId
                            select n).FirstOrDefault();
                }
            });
        }
        */
    }
}
