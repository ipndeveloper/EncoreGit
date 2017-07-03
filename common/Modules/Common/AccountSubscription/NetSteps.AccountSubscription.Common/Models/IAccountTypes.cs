using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.AccountSubscription.Common
{
    /// <summary>
    /// Account Types
    /// </summary>
    [DTO]
    public interface IAccountTypes
    {
        /// <summary>
        /// DistributorAccountTypeID
        /// </summary>
        short DistributorAccountTypeID { get; set; }

        /// <summary>
        /// LoyalCustomerAccountTypeID
        /// </summary>
        short LoyalCustomerAccountTypeID { get; set; }

        /// <summary>
        /// RetailCustomerAccountTypeID
        /// </summary>
        short RetailCustomerAccountTypeID { get; set; }
    }
}
