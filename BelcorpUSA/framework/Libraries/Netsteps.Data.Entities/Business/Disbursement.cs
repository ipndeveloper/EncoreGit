using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;


namespace NetSteps.Data.Entities.Business
{
    public partial class Disbursement 
    {
        /// <summary>
        /// author          : mescobar
        /// company         : CSTI - Peru
        /// modified         : 12/18/2015
        /// reason          : class business logic where are the methods of Disbursement
        /// modified        : 
        /// reason          :
        /// </summary>
        public static Dictionary<Int32, string> ListPeriod()
        {
            try
            {
                return DisbursementExtensions.ListPeriod();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }

        }
        //public static List<Disbursement> LoadDisbursementsByTypeAndPeriod(int disbursementTypeID, int periodID)
        //{
        //    try
        //    {
        //        return Repository.LoadDisbursementsByTypeAndPeriod(disbursementTypeID, periodID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
        //    }
        //}

        //public static List<Disbursement> LoadDisbursementByPeriod(int periodID)
        //{
        //    try
        //    {
        //        return Repository.LoadDisbursementsByPeriod(periodID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
        //    }
        //}
    }
}
