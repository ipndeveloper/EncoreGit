using System;
using System.Linq;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial class AccountAlertRepository : BaseRepository<AccountAlert, int, NetStepsEntities>, IDefaultImplementation, IAccountAlertRepository
    {
        public void Dismiss(AccountAlert alert)
        {
            alert.StartEntityTracking();
            alert.Dismissed = true;
            alert.DismissedDate = DateTime.Now;
         }

        public AccountAlert LoadByTemplateIDAndAccountID(int templateID, int accountID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    return (from aa in context.AccountAlerts.Include("EventContext")
                            where aa.AlertTemplateID == templateID && aa.EventContext.AccountID == accountID && !aa.Dismissed
                            select aa).FirstOrDefault();
                }
            });
        }
    }
}