using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business
{
    public static class GetPaymentMethods
    {
        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-CC-008 - Gestionar Cartera Pedidos - Cta Cte
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="OrderTypeID"></param>
        /// <returns>Retorna los PaymentConfigurationID</returns>

        public static Dictionary<string, string> GetPaymentMethod(int AccountID, int OrderTypeID)
        {
            try
            {
                return PaymentsMethodsExtensions.GetPaymentsMethods(AccountID, OrderTypeID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }
         
        public static int UPDPaymentConfigurations(int OrderPaymentID, int OrderID, int PaymentConfigurationID, int cuota)
        {
            try
            {
                return 1;//PaymentsMethodsExtensions.UPDPaymentConfigurations(OrderPaymentID, OrderID, PaymentConfigurationID, cuota);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
             
        } 
        //public static int UPDPaymentConfigurations(int OrderPaymentID, int OrderID, int PaymentConfigurationID, int cuota)
        //{
        //    try
        //    {
        //        return PaymentsMethodsExtensions.UPDPaymentConfigurations(OrderPaymentID, OrderID, PaymentConfigurationID, cuota);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
        //    }

        //} 
    }
}
