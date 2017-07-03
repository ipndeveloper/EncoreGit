using System;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Repositories;
using System.Collections.Generic; 

namespace NetSteps.Data.Entities.Business
{
    public class BankConsolidateApplication 
    {

        public static PaginatedList<BankConsolidateApplicationSearchData> SearchBankConsolidateApplication(BankConsolidateApplicationSearchParameter searchParameter)
        {
            try
            {
                return BankConsolidateApplicationRepository.SearchBankConsolidateApplication(searchParameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }




        public static void InsertBankPayments(BankPaymentsSearchParameter Parameter)
        {
            try
            {
                BankConsolidateApplicationRepository.InsertBankPayments(Parameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static void ImplementationCollection(BankPaymentsSearchParameter Parameter)
        {
            try
            {
                BankConsolidateApplicationRepository.ImplementationCollection(Parameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static void ImplementationCollectionAsym(BankPaymentsSearchParameter Parameter)
        {
            try
            {
                AsincronaRepository asincronaRepository = new AsincronaRepository();
                asincronaRepository.ImplementationCollectionAsyn(Parameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static PaginatedList<BankPayments> SearchBankPayments(BankPaymentsSearchParameter searchParameter)
        {
            try
            {
                return BankConsolidateApplicationRepository.SearchBankPayments(searchParameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        } 
    }
}
