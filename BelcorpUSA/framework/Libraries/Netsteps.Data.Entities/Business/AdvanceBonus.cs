using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business
{
    public class AdvanceBonus
    {
       /// <summary>
       /// Developed By KLC - CSTI
        /// REQ: BR- BR-BO-002
        /// Registra el bono de avance a pagar a las consultoras que alcanzaron el titulo de empresaria por primera vez
       /// </summary>
       /// <param name="PeriodID"></param>
        public static void InsAdvanceBonus(int PeriodID)
        {
            try
            {
                AdvanceBonusExtensions.InsAdvanceBonus(PeriodID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
