using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.DisbursementHold.Common
{
    /// <summary>
    /// Check Hold
    /// </summary>
    [DTO]
    public interface ICheckHold
    {
        /// <summary>
        /// DisbursementHoldID
        /// </summary>
        int DisbursementHoldID { get; set; }

        /// <summary>
        /// AccountID
        /// </summary>
        int AccountID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        int UserID { get; set; }

        /// <summary>
        /// ApplicationID
        /// </summary>
        int ApplicationID { get; set; }

        /// <summary>
        /// OverrideReasonID
        /// </summary>
        int OverrideReasonID { get; set; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        DateTime StartDate { get; set; }

        /// <summary>
        /// UpdatedDate
        /// </summary>
        DateTime UpdatedDate { get; set; }

        /// <summary>
        /// HoldUntil
        /// </summary>
        DateTime? HoldUntil { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        string Notes { get; set; }
    }
}
