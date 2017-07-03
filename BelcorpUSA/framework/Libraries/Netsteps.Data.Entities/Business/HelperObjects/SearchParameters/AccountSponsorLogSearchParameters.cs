using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class AccountSponsorLogSearchParameters
    {
        public int AccountID { get; set; }

        public int OldSponsorID { get; set; }
        public bool Active { get; set; }
        public int CreatedUserID { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
