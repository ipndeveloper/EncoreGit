using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class DebtsPerAgeSearchParameters
    {

        public int? AccountId { get; set; }

        public DateTime? StartBirthDate { get; set; }
        public DateTime? EndBirthDate { get; set; }

        public DateTime? StartDueDate { get; set; }
        public DateTime? EndDueDate { get; set; }

        public int? DaysOverdueStart { get; set; }
        public int? DaysOverdueEnd { get; set; }

        public string OrderNumber { get; set; }

        public bool? Forfeit { get; set; }

        public Constants.SortDirection SortOrder { get; set; }
        public string OrderBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string AccountText { get; set; }
    }
}
