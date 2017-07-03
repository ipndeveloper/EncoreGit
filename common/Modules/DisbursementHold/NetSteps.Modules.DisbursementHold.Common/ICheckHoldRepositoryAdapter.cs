using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.DisbursementHold.Common.Models;
using NetSteps.Modules.DisbursementHold.Common.Results;

namespace NetSteps.Modules.DisbursementHold.Common
{
    /// <summary>
    /// Check Hold Repository Adapter
    /// </summary>
    public interface ICheckHoldRepositoryAdapter
    {
        /// <summary>
        /// Load the Disbursement Hold
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        ICheckHoldResult LoadDisbursementHold(int accountID);

        /// <summary>
        /// Loads the applicationID, override reasonID, notes, and userID.
        /// </summary>
        /// <returns></returns>
        ICheckHoldValues LoadCheckHoldValues();

        /// <summary>
        /// Save the Disbursement Hold
        /// </summary>
        /// <param name="checkHold"></param>
        /// <returns></returns>
        ICheckHoldResult SaveDisbursementHold(ICheckHold checkHold);

        /// <summary>
        /// Update the Disbursement Hold
        /// </summary>
        /// <param name="checkHold"></param>
        /// <returns></returns>
        ICheckHoldResult UpdateDisbursementHold(ICheckHold checkHold);
    }
}
