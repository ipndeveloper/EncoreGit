using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class AccountAdditionalPhonesParameters
    {
        public int AccountAdditionalPhoneID { get; set; }

        public int AccountAdditionalTitularID { get; set; }

        public int PhoneTypeID { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsDefault { get; set; }

        public byte[] DataVersion { get; set; }

        public int ModifiedByUserID { get; set; }
         
    }
}