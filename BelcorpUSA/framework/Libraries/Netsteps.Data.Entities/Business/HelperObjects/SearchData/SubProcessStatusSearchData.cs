using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class SubProcessStatusSearchData
    {
        public int MonthlyClosureLogID { get; set; }
        public string ProcessName { get; set; }
        public int MonthlyClosureDetailLogID { get; set; }
        public string SubProcessName { get; set; }
        public int StatusProcessMonthlyClosureID { get; set; }
        public string Status { get; set; }
        public string Process { get; set; }
        public string RowTotal { get; set; }
    }
}
