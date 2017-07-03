using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class PaymentTicktesBussinessLogic
    {
        public static Dictionary<int, string> AccountSearchAuto(string AccountID)
        {
            try
            {
                return PaymentTicktesRepository.AccountSearchAuto(AccountID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static Dictionary<int, string> OrderPaymentStatuDrop()
        {
            try
            {
                return PaymentTicktesRepository.OrderPaymentStatuDrop( );
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static Dictionary<int, string> CountriesActiveDrop()
        {
            try
            {
                return PaymentTicktesRepository.CountriesActiveDrop();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static Dictionary<int, string> BanksActiveDrop()
        {
            try
            {
                return PaymentTicktesRepository.BanksActiveDrop();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static Dictionary<int, string> NegotiationLevelsActiveDrop()
        {
            try
            {
                return PaymentTicktesRepository.NegotiationLevelsActiveDrop();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static Dictionary<int, string> BankSearchAuto(string Query)
        {
            try
            {
                return PaymentTicktesRepository.BankSearchAuto(Query);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static Dictionary<int, string> ExpirationStatusesDrop()
        {
            try
            {
                return PaymentTicktesRepository.ExpirationStatusesDrop();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
