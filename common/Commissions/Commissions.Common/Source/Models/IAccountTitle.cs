using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
    /// <summary>
    /// account title
    /// </summary>
    public interface IAccountTitle
    {
        /// <summary>
        /// Gets the account identifier
        /// </summary>
        int AccountId { get; }

        /// <summary>
        /// Gets the title identifier
        /// </summary>
        int TitleId { get; }

        /// <summary>
        /// Gets the title kind identifier
        /// </summary>
        int TitleKindId { get; }

        /// <summary>
        /// Gets the period identifier
        /// </summary>
        int PeriodId { get; }

        /// <summary>
        /// Gets the last date this was modified
        /// </summary>
        DateTime DateModified { get; }
    }
}
