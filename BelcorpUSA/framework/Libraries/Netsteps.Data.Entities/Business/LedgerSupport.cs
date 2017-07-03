using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
namespace NetSteps.Data.Entities.Business
{
    public class LedgerSupport
    {
        public static Dictionary<string, string> GetEntryReason()
        {
            try
            {
                return LedgerExtension.GetEntryReason();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        public static Dictionary<string, string> GetLedgerEntryType()
        {
            try
            {
                return LedgerExtension.GetLedgerEntryType();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        public static Dictionary<string, string> GetBonusTypes()
        {
            try
            {
                return LedgerExtension.GetBonusTypes();            
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        public static List<LedgerSupportSearchData> GetLedgerSupportTicketByID(int accountID, int orderID)
        {
            try
            {
                return LedgerExtension.GetLedgerSupportTicketByID(accountID, orderID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

    }
}
