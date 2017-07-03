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
    public partial class DisbursementReport
    {
        /// <summary>
        /// author          : mescobar
        /// company         : CSTI - Peru
        /// create          : 12/18/2015
        /// reason          : class business logic where are the methods of DisbursementReport
        /// modified        : 
        /// reason          :
        /// </summary>
        /// 
        public static PaginatedList<DisbursementReportSearchData> Search(DisbursementReportSearchParameters parameters, bool GetAll = false)
        {
            try
            {
                return DisbursementReportExtensions.Search(parameters, GetAll);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }


        public static Dictionary<Int32, string> ListDisbursementStatuses()
        {
            try
            {
                return DisbursementReportExtensions.ListDisbursementStatuses();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }

        }

    }
}
