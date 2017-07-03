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
    public class PaymentMethods
    {

        public static PaginatedList<PaymentMethodsSearchData> SearchDetails(PaymentMethodsSearchParameters searchParameter)
        {
            try
            {
                return PaymentMethodsRepository.SearchDetails(searchParameter);             
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        } 

        public static int GetNumberCuotasByPaymentConfigurationID(int PaymentConfigurationID)
        {
            try
            {
                return PaymentMethodsRepository.GetNumberCuotasByPaymentConfigurationID(PaymentConfigurationID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static bool IsTargetCreditByPaymentConfiguration(int PaymentConfigurationID)
        {
            try
            {
                return PaymentMethodsRepository.IsTargetCreditByPaymentConfiguration(PaymentConfigurationID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static bool IsTargetCredit(int CollectionEntityID)
        {
            try
            {
                return PaymentMethodsRepository.IsTargetCredit(CollectionEntityID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
           
        }
        public static string GetTDescPaymentConfiguationByOrderPayment(int OrderPaymentID)
        {
            try
            {
                return PaymentMethodsRepository.GetTDescPaymentConfiguationByOrderPayment(OrderPaymentID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }
        public static  PaymentConfigurations EditPaymentConfigurations(int ID)
        {
            try
            {
                return PaymentMethodsRepository.EditPaymentConfigurations(ID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static List<PaymentConfigurationPerAccountSearchData> EditPaymentConfigurationPerAccount(int ID)
        {
            try
            {
                return PaymentMethodsRepository.EditPaymentConfigurationPerAccount(ID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }
        public static List<PaymentConfigurationPerOrderTypesSearchData> EditPaymentConfigurationPerOrderTypes(int ID)
        {
            try
            {
                return PaymentMethodsRepository.EditPaymentConfigurationPerOrderTypes(ID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static List<ZonesData> EditPaymentConfigurationGeoScope(int ID)
        {
            try
            {
                return PaymentMethodsRepository.EditPaymentConfigurationGeoScope(ID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }
    }
}
