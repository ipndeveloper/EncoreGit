using System;
using NetSteps.Data.Entities.Exceptions;


namespace NetSteps.Data.Entities
{
    public partial class OptOut
    {
        public static OptOut Search(string emailAddress)
        {
            try
            {
                return BusinessLogic.Search(Repository, emailAddress);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
