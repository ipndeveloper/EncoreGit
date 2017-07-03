using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class ReportSearchParameters :  FilterPaginatedListParameters<Report>
    {
        public int ReportID { get; set; }

    }
}
