using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class RouterParametros : FilterDateRangePaginatedListParameters<RoutesData>
    {
        static readonly int CHashCodeSeed = typeof(RouterParametros).GetKeyForType().GetHashCode();
        public int? RouteID { get; set; }
    }
}
