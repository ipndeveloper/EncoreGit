using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class AccountWithoutSponsorSearchParameters
    {
        public int AccountID { get; set; }
        public int PeriodID { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
