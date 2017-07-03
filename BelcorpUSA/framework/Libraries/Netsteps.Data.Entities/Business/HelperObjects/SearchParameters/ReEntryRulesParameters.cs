using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class ReEntryRulesParameters
    {

        public decimal ReEntryRuleID { get; set; }

        public int ReEntryRuleValueID { get; set; }

        public int ReEntryRuleTypeID { get; set; }

        public bool Active { get; set; }
       
    }
}
