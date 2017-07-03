using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class DistributionList
    {
        #region Methods
        public static List<DistributionList> LoadByAccountID(int accountID)
        {
            try
            {
                var results = Repository.LoadByAccountID(accountID);

                foreach (var item in results)
                {
                    item.StartEntityTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return results;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<DistributionList> LoadByAccountIDFull(int accountID)
        {
            try
            {
                var results = Repository.LoadByAccountIDFull(accountID);

                foreach (var item in results)
                {
                    item.StartEntityTracking();
                    item.IsLazyLoadingEnabled = true;
                }

                return results;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #endregion
    }
}
