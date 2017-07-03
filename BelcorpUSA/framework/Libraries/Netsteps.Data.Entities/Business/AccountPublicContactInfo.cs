using System;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class AccountPublicContactInfo
    {
        public static AccountPublicContactInfo LoadByAccountID(int accountID)
        {
            try
            {
                return Repository.LoadByAccountID(accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
