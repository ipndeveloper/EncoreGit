using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.AccountSubscription.Common
{
    /// <summary>
    /// Subscription Account
    /// </summary>
    [DTO]
    public interface ISubscriptionAccount
    {
        /// <summary>
        /// AccountID
        /// </summary>
        int AccountID { get; set; }

        /// <summary>
        /// OrderID
        /// </summary>
        int OrderID { get; set; }

        /// <summary>
        /// RenewalDate (Optional)
        /// </summary>
        DateTime? RenewalDate { get; set; }
    }
}
