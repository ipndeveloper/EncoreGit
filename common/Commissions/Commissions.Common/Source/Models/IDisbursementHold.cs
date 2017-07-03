using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
    /// <summary>
    /// disbursement hold
    /// </summary>
    public interface IDisbursementHold
    {
        /// <summary>
        /// Gets or sets the disbursement hold identifier.
        /// </summary>
        /// <value>
        /// The check hold identifier.
        /// </value>
        int DisbursementHoldId { get; set; }

        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        int UserId { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        IOverrideReason Reason { get; set; }

        /// <summary>
        /// Gets or sets the hold until date.
        /// </summary>
        /// <value>
        /// The hold until.
        /// </value>
        DateTime? HoldUntil { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        string Notes { get; set; }

        /// <summary>
        /// Gets or sets the application source identifier.
        /// </summary>
        /// <value>
        /// The application source identifier.
        /// </value>
        int? ApplicationSourceId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        /// <value>
        /// The updated date.
        /// </value>
        DateTime UpdatedDate { get; set; }
    }
}
