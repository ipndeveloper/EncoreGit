using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class GLControlLogParameters : FilterDateRangePaginatedListParameters<GLControlLogSearchData>
    {
        static readonly int CHashCodeSeed = typeof(OrderSearchParameters).GetKeyForType().GetHashCode();
        public int TicketNumber { get; set; }
        public int? ModifiedByUserID { get; set; }
        public DateTime? DateModifiedUTC { get; set; }
        public int OrderPaymentID { get; set; }
    }
}
