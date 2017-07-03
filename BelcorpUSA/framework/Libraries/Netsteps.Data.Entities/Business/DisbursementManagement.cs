using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#region Referencia Agregadas
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Exceptions;
#endregion

namespace NetSteps.Data.Entities.Business
{
    public partial class DisbursementManagement
    {
        public static string ExistsDisbursementsByPeriod(int periodID)
        {
            return DisbursementManagementExtensions.ExistsDisbursementsByPeriod(periodID);
        }

        public static void MoveBonusValuesToLedgers(int periodID)
        {
            try
            {
                DisbursementManagementExtensions.MoveBonusValuesToLedgers(periodID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static string ExistsDibsCreateRecordsByPeriod(int periodID)
        {
            return DisbursementManagementExtensions.ExistsDibsCreateRecordsByPeriod(periodID);
        }

        public static void DisbCreateRecords(int periodID)
        {
            try
            {
                DisbursementManagementExtensions.DisbCreateRecords(periodID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static string ExistsSendToBankByPeriod(int periodID)
        {
            return DisbursementManagementExtensions.ExistsSendToBankByPeriod(periodID);
        }

        public static void SendToBank(int periodID)
        {
            try
            {
                DisbursementManagementExtensions.SendToBank(periodID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int  GetLatestClosedPeriodByPlan()
        {
            try
            {
                return DisbursementManagementExtensions.GetLatestClosedPeriodByPlan();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

    }
}
