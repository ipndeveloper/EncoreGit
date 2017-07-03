using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.DisbursementHold.Common
{
    /// <summary>
    /// Check Hold Result Values.
    /// </summary>
    [DTO]
    public interface ICheckHoldValues
    {
        /// <summary>
        /// ApplicationID
        /// </summary>
        int ApplicationID { get; set; }

        /// <summary>
        /// OverrideReasonID
        /// </summary>
        int OverrideReasonID { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        string Notes { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        int UserID { get; set; }
    }
}
