using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;
using System.Data;

namespace NetSteps.Data.Entities.Business
{
    public class SupportMotiveSearchParameters : FilterDateRangePaginatedListParameters<SupportMotiveSearchData>
    {
        static readonly int CHashCodeSeed = typeof(OrderSearchParameters).GetKeyForType().GetHashCode();

        public int? SupportMotiveID { get; set; }

        public string Name { get; set; }

        public bool? Active { get; set; }

        public int SupportLevelID { get; set; }

        public DataTable dtSupportLevelIDs { get; set; }   
        

    }
}
