using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto; 

namespace NetSteps.Modules.AccountRenewal.Common.Results
{
    /// <summary>
    /// Renewal Account Result
    /// </summary>
    [DTO]
    public interface IRenewalAccountResult : IResult
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
        /// LastRenewal
        /// </summary>
        DateTime? LastRenewalDateUTC { get; set; }

        /// <summary>
        /// NextRenewal
        /// </summary>
        DateTime? NextRenewalDateUTC { get; set; }
    }
}
