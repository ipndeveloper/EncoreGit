using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business
{
    public class UpdateBalance
    {
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="NumDocument"></param>
        /// <returns>BR-CC-014 - INTERESES Y  MULTAS : Proceso Update Balance</returns>
        public static List<CTEUpdateBalance> UpdateBalances(int NumDocument)
        {
            try
            {
                return UpdateBalanceExtensions.UpdateBalance(NumDocument);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<CTEUpdateBalance> UpdateBalancesPayments(CTEUpdateBalance dat)
        {
            try
            {
                return UpdateBalanceExtensions.UpdateBalancesPayments(dat);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
