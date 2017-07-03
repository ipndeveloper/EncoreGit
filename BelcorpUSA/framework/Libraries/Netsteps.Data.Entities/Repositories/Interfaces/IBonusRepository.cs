using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
   public  interface IBonusRepository
    {
       bool CalcularBonusTeamBuilding(int PeriodID);
       bool CalcularBonuTurboInfinityByPeriod(int PeriodID);
       bool CalcularDiscountType(int PeriodID);


    }
}
