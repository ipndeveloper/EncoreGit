using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Communication.Common
{
    [ContractClass(typeof(Contracts.AccountAlertProviderContracts))]
    public interface IAccountAlertProvider
    {
        IList<IAccountAlert> GetBatch(IEnumerable<int> accountAlertIds);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IAccountAlertProvider))]
        internal abstract class AccountAlertProviderContracts : IAccountAlertProvider
        {
            IList<IAccountAlert> IAccountAlertProvider.GetBatch(IEnumerable<int> accountAlertIds)
            {
                Contract.Requires<ArgumentNullException>(accountAlertIds != null);
                throw new NotImplementedException();
            }
        }
    }
}
