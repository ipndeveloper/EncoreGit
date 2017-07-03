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
    public class ManagmentLedger
    {
        /// <summary>
        /// author          : mescobar
        /// company         : CSTI - Peru
        /// create          : 12/18/2015
        /// reason          : class business logic where are the methods of ManagmentLedger
        /// modified        : 
        /// reason          :
        /// </summary>
        /// 
        public static PaginatedList<ManagmentLedgerSearchData> Search(ManagmentLedgerSearchParameters parameters, bool GetAll = false)
        {
            try
            {
                return ManagmentLedgerExtension.Search(parameters, GetAll);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }
        public static Dictionary<Int32, string> ListBonusTypesML()
        {
            try
            {
                return ManagmentLedgerExtension.ListBonusTypesML();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }

        }

        public static Dictionary<Int32, string> ListLedgerEntryOriginsML()
        {
            try
            {
                return ManagmentLedgerExtension.ListLedgerEntryOriginsML();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }

        }

        public static Dictionary<Int32, string> ListLedgerEntryReasonsML()
        {
            try
            {
                return ManagmentLedgerExtension.ListLedgerEntryReasonsML();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }

        }

        public static Dictionary<Int32, string> ListLedgerEntryTypesML()
        {
            try
            {
                return ManagmentLedgerExtension.ListLedgerEntryTypesML();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }

        }

    }
}
