using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class BonusLogic
    {
        private static  BonusLogic _BonusLogic;
        private static    IBonusRepository _IBonusRepository;
        public static BonusLogic Instance
        {
            get 
             {
                     if (_BonusLogic == null)
                         { 
                             _BonusLogic = new BonusLogic();
                             _IBonusRepository = new BonusRepository();
                         }
                     return _BonusLogic;
            }
             
        }
        public bool CalcularBonusTeamBuilding(int PeriodID)
        {
            try
            {
                return _IBonusRepository.CalcularBonusTeamBuilding(PeriodID);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool CalcularBonuTurboInfinityByPeriod(int PeriodID)
        {
            try
            {
                return _IBonusRepository.CalcularBonuTurboInfinityByPeriod(PeriodID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CalcularDiscountType(int PeriodID)
        {
            try
            {
                return _IBonusRepository.CalcularDiscountType(PeriodID);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
