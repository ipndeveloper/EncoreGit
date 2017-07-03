using System;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
    public partial class FileResource
    {
        /// <summary>
        /// Check if account has file resources
        /// </summary>
        /// <param name="primaryKey">account ID</param>
        /// <returns>bool</returns>
        public static FileResource LoadFileResourceProperties(int primaryKey)
        {
            try
            {
                FileResourceRepository fileResourceRepository = new FileResourceRepository();
                return fileResourceRepository.GetFileResourceProperties(primaryKey);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
