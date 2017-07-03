using System;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class UserSiteWidgetBusinessLogic
    {
        /// <summary>
        /// Overridden in client implementation
        /// </summary>
        public virtual void SetCustomUserWidgetVisibility(Account account)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
