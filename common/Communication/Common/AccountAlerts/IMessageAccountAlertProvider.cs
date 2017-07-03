using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System;
namespace NetSteps.Communication.Common
{
    [ContractClass(typeof(Contracts.MessageAccountAlertProviderContracts))]
    public interface IMessageAccountAlertProvider : IAccountAlertProvider
    {
        new IList<IMessageAccountAlert> GetBatch(IEnumerable<int> accountAlertIds);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IMessageAccountAlertProvider))]
        internal abstract class MessageAccountAlertProviderContracts : IMessageAccountAlertProvider
        {
            IList<IMessageAccountAlert> IMessageAccountAlertProvider.GetBatch(IEnumerable<int> accountAlertIds)
            {
                Contract.Requires<ArgumentNullException>(accountAlertIds != null);
                throw new NotImplementedException();
            }

            IList<IAccountAlert> IAccountAlertProvider.GetBatch(IEnumerable<int> accountAlertIds)
            {
                throw new NotImplementedException();
            }
        }
    }
}
