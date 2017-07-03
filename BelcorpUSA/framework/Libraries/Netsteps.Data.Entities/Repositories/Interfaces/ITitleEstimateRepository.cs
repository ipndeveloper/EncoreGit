using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface ITitleEstimateRepository
    {

        bool CalculateCareerTitle(int? PlanID);

        bool CalculatePaidAsTitle(int PlanID);

    }
}
