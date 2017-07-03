using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class FailedSubProcessPersonalIndicatorSearchData
    {
        public int PersonalIndicatorLogID { get; set; }
        public string ProcessName { get; set; }
        public int PersonalIndicatorDetailLogID { get; set; }
        public string SubProcessName { get; set; }
        public string MessageToShow { get; set; }
        public string RealError { get; set; }
    }
}
