using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class RequirementRuleSearchParameters : FilterDateRangePaginatedListParameters<RequirementRuleSearchData>
    {
        public int? RuleRequirementID { get; set; }
        public int? RuleTypeID { get; set; }
        public int? PlanID { get; set; }

    }
}
