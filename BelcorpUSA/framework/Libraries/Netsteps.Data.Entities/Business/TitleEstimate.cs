using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business
{
    public class TitleEstimate
    {

        #region Instance

        private static TitleEstimate _instance;

        public static TitleEstimate Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TitleEstimate();
                return _instance;
            }
        }


        #endregion


        public bool CalculateCareerTitle(int? PlanID)
        {
            return TitleEstimateRepository.Instance.CalculateCareerTitle(PlanID);
        }

        public bool CalculatePaidAsTitle(int PlanID)
        {
            return TitleEstimateRepository.Instance.CalculatePaidAsTitle(PlanID);
        }

    }

}
