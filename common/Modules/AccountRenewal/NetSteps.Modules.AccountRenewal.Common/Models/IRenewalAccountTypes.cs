using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.AccountRenewal.Common.Models
{
    /// <summary>
    /// Renewal Account Type
    /// </summary> 
    [DTO]
    public interface IRenewalAccountTypes
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
