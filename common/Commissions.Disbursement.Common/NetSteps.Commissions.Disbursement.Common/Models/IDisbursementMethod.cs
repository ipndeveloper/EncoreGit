using System;
using NetSteps.Core.Dto;

namespace NetSteps.Commissions.Disbursement.Common.Models
{
    /// <summary>
    /// the disbursement method
    /// </summary>
    [DTO]
    public interface IDisbursementMethod
    {
        /// <summary>
        /// the disbursement method identifier
        /// </summary>
        int DisbursementMethodId { get; set; }

        /// <summary>
        /// the disbursement method name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// the maximum number of this type of method allowed
        /// </summary>
        int NumberAllowed { get; set; }

        /// <summary>
        /// is the method enabled
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// is the method editable
        /// </summary>
        bool IsEditable { get; set; }

        /// <summary>
        /// the term name
        /// </summary>
        string TermName { get; set; }

        /// <summary>
        /// the method code
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// date the method was modified
        /// </summary>
        DateTime DateModified { get; set; }
    }
}
