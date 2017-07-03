using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.AccountSubscription.Common
{
    /// <summary>
    /// Account Subscription Result
    /// </summary>
    [DTO]
    public interface ISubscriptionAccountResult : IResult
    {
        /// <summary>
        /// AccountID
        /// </summary>
        int AccountID { get; set; }

        /// <summary>
        /// AccountTypeID
        /// </summary>
        short AccountTypeID { get; set; }

        /// <summary>
        /// AccountStatusID
        /// </summary>
        short AccountStatusID { get; set; }

        /// <summary>
        /// OrderID
        /// </summary>
        int OrderID { get; set; }

        /// <summary>
        /// EnrollmentDate
        /// </summary>
        DateTime? EnrollmentDateUTC { get; set; }

        /// <summary>
        /// RenewalStartDate
        /// </summary>
        DateTime? RenewalStartDate { get; set; }

        /// <summary>
        /// RenewalEndDate
        /// </summary>
        DateTime? RenewalEndDate { get; set; }
    }
}
