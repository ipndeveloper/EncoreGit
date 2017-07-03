using System;
using System.Linq;

namespace NetSteps.Data.Common.Entities
{
    public interface IAccountTitle
    {
        ITitle Title { get; set; }
        int AccountID { get; set; }
        int PeriodID { get; set; }
        int TitleTypeID { get; set; }
    }
}
