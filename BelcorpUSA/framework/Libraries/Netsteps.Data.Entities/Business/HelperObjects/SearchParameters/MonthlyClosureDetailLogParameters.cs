using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class MonthlyClosureDetailLogParameters
    {
        public int MonthlyClosureDetailLogID { get; set; }
        public int MonthlyClosureLogID { get; set; }
        public string CodeSubProcess { get; set; }
        public string TermName { get; set; }
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public int StatusProcessMonthlyClosureID { get; set; }
        public string MessageToShow { get; set; }
        public string RealError { get; set; }
        public string CodeStatusName { get; set; }
    }
}