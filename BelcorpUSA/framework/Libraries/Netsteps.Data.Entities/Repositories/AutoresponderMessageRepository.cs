using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AutoresponderMessageRepository
    {
        public List<AutoresponderMessage> GetUnseenMessagesForAccount(int accountID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.AutoresponderMessages.Include("AutoresponderMessageTokens")
                        .Include("Autoresponder")
                        .Include("Autoresponder.Translations")
                        .Where(am => am.AccountID == accountID && !am.HasBeenRead).ToList();
                }
            });
        }
    }
}
