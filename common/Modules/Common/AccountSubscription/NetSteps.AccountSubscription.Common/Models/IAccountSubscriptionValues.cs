using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.AccountSubscription.Common
{
    /// <summary>
    /// Account Subscription Values
    /// </summary>
    [DTO]
    public interface IAccountSubscriptionValues
    {
        /// <summary>
        /// SubscriptionKindID
        /// </summary>
        int SubscriptionKindID { get; set; }        
    }
}
