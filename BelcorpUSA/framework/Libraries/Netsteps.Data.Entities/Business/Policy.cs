using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class Policy
    {
        #region Methods
        public static List<Policy> LoadAll()
        {
            try
            {
                return BusinessLogic.LoadAll(Repository);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<Policy> GetPolicies(int accountTypeID, int marketID)
        {
            try
            {
                return Repository.GetPolicies(accountTypeID, marketID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion
    }
}
