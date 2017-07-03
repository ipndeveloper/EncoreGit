using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business
{
    public class GeneralLedger
    {
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="TicketNumber"></param>
        /// <returns>Ticket Details</returns>
        public static List<GLPaymeTycketsSearchData> BrowseTicketDetails(int TicketNumber)
        {
            try
            {
              return GeneralLedgerRepository.BrowseTicketDetails(TicketNumber);            
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="TicketNumber"></param>
        /// <returns>Calculate Update Balance</returns>
        public static List<GLCalculateUpdateBalanceSearchData> GetCalculateUpdateBalance(int TicketNumber)
        {
            try
            {
                return GeneralLedgerRepository.GetCalculateUpdateBalance(TicketNumber);
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        /// <summary>
        ///  Developed By KLC - CSTI
        /// </summary>
        /// <param name="TicketNumber"></param>
        /// <returns>Return datos de Control  Log</returns>
        public static PaginatedList<GLControlLogSearchData> GetControlLog(GLControlLogParameters TicketNumber)
        {
            try
            {
                return GeneralLedgerRepository.GetControlLog(TicketNumber);
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }
        
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <returns>Return datos de Combo Entitys </returns>
        public static List<GLDropdownlistUtilSearchData> GetEntity()
        {
            return GeneralLedgerRepository.GetEntity();
        }

        public static List<GLDropdownlistUtilSearchData> GetPaymentType(int BanckID)
        {
            return GeneralLedgerRepository.GetPaymentType(BanckID);
        }        

        public static void spUpdateAlterDueDate(int TicketNumber,string NewDate)
        { 
            try
            {
                AlterDueDuateExtensions.AlterDueDate(TicketNumber,NewDate);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }        
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="TicketNumber"></param>
        /// <returns></returns>
        public static List<GeneralLedgerNegotiationData> BrowseRulesNegotiation(int TicketNumber)
        {
            try
            {
                return GeneralLedgerRepository.BrowseRulesNegotiation(TicketNumber);
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        public static PaginatedList<PaymentTicketsSearchData> SearchPaymentTickets(PaymentTicketsSearchParameters PaymentTicketsSearchParameter)
        {
            try
            {
                return GeneralLedgerRepository.SearchPaymentTickets(PaymentTicketsSearchParameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

    }
}
