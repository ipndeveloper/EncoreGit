using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class AccountSuppliedIDsParameters
    {

        public decimal AccountSuppliedID { get; set; }

        public int IDTypeID { get; set; }

        public int AccountID { get; set; }

        public string AccountSuppliedIDValue { get; set; }

        public bool IsPrimaryID { get; set; }

        public DateTime? IDExpeditionIDate { get; set; }

        public string ExpeditionEntity { get; set; }
         
    }
}
