using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class AccountAdditionalTitularSuppliedIDsParameters
    {

        public decimal AccountAdditionalTitularSuppliedID { get; set; }

        public int IDTypeID { get; set; }

        public int AccountAdditionalTitularID { get; set; }

        public string AccountSuppliedValue { get; set; }

        public bool IsPrimaryID { get; set; }

        public DateTime? IDExpeditionDate { get; set; }

        public string ExpeditionEntity { get; set; }
         
    }
}