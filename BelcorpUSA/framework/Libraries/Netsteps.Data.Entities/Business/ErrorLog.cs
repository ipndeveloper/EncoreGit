using System;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class ErrorLog
    {
        #region Methods
        public static PaginatedList<ErrorLog> Search(ErrorLogSearchParameters searchParameters)
        {
            try
            {
                var errorLogs = Repository.Search(searchParameters);
                return errorLogs;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static PaginatedList<ErrorLog> Search(ErrorLogSearchParameters searchParameters, string connectionString)
        {
            try
            {
                var errorLogs = Repository.Search(searchParameters, connectionString);
                return errorLogs;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion

        public override void Save()
        {
            try
            {
                var repository = GetRepository();
                BusinessLogic.CleanDataBeforeSave(repository, this);
                var result = ValidateRecursive();
                if (result.IsValid)
                    BusinessLogic.Save(repository, this);
                else
                {
                    throw new NetStepsBusinessException(string.Format("Invalid Entity: {0}. {1}{2}", typeof(ErrorLog), Environment.NewLine, result.BrokenRulesList.ToString(brokenRule => string.Format("{0}.{1} - {2}{3}", brokenRule.EntityName, brokenRule.Property, brokenRule.Description, Environment.NewLine))));
                }
            }
            catch (Exception ex)
            {
                // Swallow errors here to avoid any possible recursive logging of error that could occur. - JHE
                if (ApplicationContext.Instance.IsDebug)
                    throw ex;
            }
        }
    }
}
