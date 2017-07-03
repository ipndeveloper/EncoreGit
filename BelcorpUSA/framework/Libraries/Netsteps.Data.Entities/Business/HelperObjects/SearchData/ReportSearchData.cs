using System;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class ReportSearchData
    {
        public object ReportName { get; set; }

        public string Status { get; set; }

        public string ReportID { get; set; }
    }
}
