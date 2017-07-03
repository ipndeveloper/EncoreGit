using System;
using System.Collections.Generic;
using NetSteps.Common;

namespace DistributorBackOffice.Models.Performance
{
    public class GetFlatViewModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public Constants.SortDirection OrderByDirection { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BirthMonth { get; set; }
        public decimal? PvFrom { get; set; }
        public decimal? PvTo { get; set; }
        public decimal? GvFrom { get; set; }
        public decimal? GvTo { get; set; }
        public int? SponsorId { get; set; }
        public bool ShowMyTeam { get; set; }
        public int? MonthsInActive { get; set; }
        public List<int> Titles { get; set; }
        public List<int> AccountTypes { get; set; }
        public List<int> AccountStatuses { get; set; }
        public List<int> States { get; set; }
        public int? PeriodId { get; set; }
        public string SearchValue { get; set; }
        public bool IncludeCheckbox { get; set; }
        public int? CurrentTopOfTreeAccountId { get; set; }
        public bool? GroupBySponsorTree { get; set; }
		public string ReportName { get; set; }
        public int? LevelFrom { get; set; }
        public int? LevelTo { get; set; }
        public int? GenFrom { get; set; }
        public int? GenTo { get; set; }
        public int? DVFrom { get; set; }
        public int? DVTo { get; set; }

        public GetFlatViewModel()
        {
            IncludeCheckbox = true;
        }

    }
}