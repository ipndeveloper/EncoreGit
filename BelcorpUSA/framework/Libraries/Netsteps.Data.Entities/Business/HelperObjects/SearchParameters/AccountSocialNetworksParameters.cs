using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class AccountSocialNetworksParameters
    {

        public decimal AccountSocialNetworkID { get; set; }

        public int SocialNetworkID { get; set; }

        public int AccountID { get; set; }

        public string Value { get; set; }
    }
}
