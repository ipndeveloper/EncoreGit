using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Communication.Common
{
    [ContractClass(typeof(Contracts.PromotionAccountAlertProviderContracts))]
    public interface IPromotionAccountAlertProvider : IAccountAlertProvider
    {
        new IList<IPromotionAccountAlert> GetBatch(IEnumerable<int> accountAlertIds);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IPromotionAccountAlertProvider))]
        internal abstract class PromotionAccountAlertProviderContracts : IPromotionAccountAlertProvider
        {
            IList<IPromotionAccountAlert> IPromotionAccountAlertProvider.GetBatch(IEnumerable<int> accountAlertIds)
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
