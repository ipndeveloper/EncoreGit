using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Common.Context
{
    public interface IActivityInfo
    {
        short AccountConsistencyStatusID { get; set; }
        int AccountID { get; set; }
        short ActivityStatusID { get; set; }
        bool HasContinuity { get; set; }
        int PeriodID { get; set; }
    }
}
