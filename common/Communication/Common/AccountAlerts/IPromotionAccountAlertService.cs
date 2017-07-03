using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Communication.Common
{
    [ContractClass(typeof(Contracts.PromotionAccountAlertServiceContracts))]
    public interface IPromotionAccountAlertService
    {
        void Add(IPromotionAccountAlert model);
        IList<IPromotionAccountAlert> GetAll();
        IList<IPromotionAccountAlert> GetBatch(IEnumerable<int> accountAlertIds);
        void Dismiss(int accountAlertId, int accountId);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IPromotionAccountAlertService))]
        internal abstract class PromotionAccountAlertServiceContracts : IPromotionAccountAlertService
        {
            void IPromotionAccountAlertService.Add(IPromotionAccountAlert model)
            {
                Contract.Requires<ArgumentNullException>(model != null);
            }

            IList<IPromotionAccountAlert> IPromotionAccountAlertService.GetAll()
            {
                throw new NotImplementedException();
            }

            IList<IPromotionAccountAlert> IPromotionAccountAlertService.GetBatch(IEnumerable<int> accountAlertIds)
            {
                Contract.Requires<ArgumentNullException>(accountAlertIds != null);
                throw new NotImplementedException();
            }

            void IPromotionAccountAlertService.Dismiss(int accountAlertId, int accountId)
            {
                Contract.Requires<ArgumentOutOfRangeException>(accountAlertId > 0);
                Contract.Requires<ArgumentOutOfRangeException>(accountId > 0);
                throw new NotImplementedException();
            }
        }
    }
}
