using System;
using NetSteps.Data.Entities.Exceptions;
using System.Collections.Generic;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities
{
    public partial class UserSiteWidget
    {
        public static void SetCustomUserWidgetVisibility(Account account)
        {
            try
            {
                BusinessLogic.SetCustomUserWidgetVisibility(account);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        /// <summary>
        /// Create by FHP
        /// </summary>
        /// <returns>Select Id, Name</returns>
        public static Dictionary<string, string> GetUserTypes()
        {
            try
            {
                return UserExtensions.GetUserTypes();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
         
    }
}
