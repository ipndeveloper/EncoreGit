using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AccountPublicContactInfoRepository
    {
        public AccountPublicContactInfo LoadByAccountID(int accountID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var accountContactInfo = context.AccountPublicContactInfoes.FirstOrDefault(apci => apci.AccountID == accountID);

                    if (accountContactInfo != null)
                    {
                        accountContactInfo.StartEntityTracking();
                    }

                    return accountContactInfo;
                }
            });
        }
    }
}
