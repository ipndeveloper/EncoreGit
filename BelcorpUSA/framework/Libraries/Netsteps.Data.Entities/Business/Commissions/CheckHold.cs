using System;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Commissions
{
    public partial class CheckHold
    {
        #region Members

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static PaginatedList<CheckHoldSearchData> Search(CheckHoldSearchParameters searchParameters)
        {
            try
            {
                return Repository.Search(searchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void InsertOrUpdate(CheckHold checkHold)
        {
            try
            {
                BusinessLogic.InsertOrUpdate(Repository, checkHold);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #endregion

    }
}
