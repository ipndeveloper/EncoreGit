using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{   
    /// <summary>
    /// Interface for Monthly Closing
    /// </summary>
    public interface IMonthlyClosingRepository
    {
        Dictionary<string, string> ListAvailablePlans();
        string GetActivePeriod();
        string GetOpenPeriod();
    }
}
