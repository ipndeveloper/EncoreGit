using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.DisbursementHold.Common
{    
    /// <summary>
    /// Disbursement Hold Functions.
    /// </summary>
    [ContractClass(typeof(DisbursementHoldServiceContract))]
    public interface IDisbursementHoldService
    {
        /// <summary>
        /// Loads a disbursement hold.
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

    [ContractClassFor(typeof(IDisbursementHoldService))]
    internal abstract class DisbursementHoldServiceContract : IDisbursementHoldService
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
