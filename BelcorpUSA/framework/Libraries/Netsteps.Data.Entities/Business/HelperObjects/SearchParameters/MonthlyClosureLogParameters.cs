using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class MonthlyClosureLogParameters
    {
        public int MonthlyClosureLogID { get; set; }
        public int PlanID { get; set; }
        public int PeriodID { get; set; }
        public string TermName { get; set; }
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Result { get; set; }
    }
}
