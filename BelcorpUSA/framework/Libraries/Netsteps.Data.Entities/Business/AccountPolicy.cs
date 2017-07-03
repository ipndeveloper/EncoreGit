using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class AccountPolicy
    {
        #region Basic Crud
        public static List<AccountPolicy> LoadByAccountID(int accountID)
        {
            try
            {
                var list = Repository.LoadByAccountID(accountID);
                foreach (var item in list)
                {
                    item.StartTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion
    }
}
