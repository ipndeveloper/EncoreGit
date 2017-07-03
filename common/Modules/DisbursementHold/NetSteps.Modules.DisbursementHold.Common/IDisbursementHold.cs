using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Modules.DisbursementHold.Common.Models;
using NetSteps.Modules.DisbursementHold.Common.Results;

namespace NetSteps.Modules.DisbursementHold.Common
{    
    /// <summary>
    /// Disbursement Hold Functions.
    /// </summary>
    [ContractClass(typeof(DisbursementHoldContract))]
    public interface IDisbursementHold
    {
        /// <summary>
        /// loads a disbursement hold.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        ICheckHoldResult LoadDisbursementHold(int accountID);

        /// <summary>
        /// Saves or updates a disbursement hold.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        ICheckHoldResult SaveDisbursementHold(int accountID, DateTime? holdUntil);
    }

    [ContractClassFor(typeof(IDisbursementHold))]
    internal abstract class DisbursementHoldContract : IDisbursementHold
    {
        public ICheckHoldResult LoadDisbursementHold(int accountID)
        {
            Contract.Requires<ArgumentNullException>(accountID != 0);
            Contract.Ensures(Contract.Result<ICheckHoldResult>() != null);

            throw new NotImplementedException();
        }

        public ICheckHoldResult SaveDisbursementHold(int accountID, DateTime? holdUntil)
        {
            Contract.Requires<ArgumentNullException>(accountID != 0);
            Contract.Ensures(Contract.Result<ICheckHoldResult>() != null);

            throw new NotImplementedException();
        }
    }
}
