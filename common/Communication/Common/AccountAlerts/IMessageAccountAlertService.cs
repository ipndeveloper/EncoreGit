using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
namespace NetSteps.Communication.Common
{
    [ContractClass(typeof(Contracts.MessageAccountAlertServiceContracts))]
    public interface IMessageAccountAlertService
    {
        void Add(IMessageAccountAlert model);
        IList<IMessageAccountAlert> GetAll();
        IList<IMessageAccountAlert> GetBatch(IEnumerable<int> accountAlertIds);
        void Dismiss(int accountAlertId, int accountId);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IMessageAccountAlertService))]
        internal abstract class MessageAccountAlertServiceContracts : IMessageAccountAlertService
        {
            void IMessageAccountAlertService.Add(IMessageAccountAlert model)
            {
                Contract.Requires<ArgumentNullException>(model != null);
                throw new System.NotImplementedException();
            }

            void IMessageAccountAlertService.Dismiss(int accountAlertId, int accountId)
            {
                Contract.Requires<ArgumentOutOfRangeException>(accountAlertId > 0);
                Contract.Requires<ArgumentOutOfRangeException>(accountId > 0);
                throw new System.NotImplementedException();
            }

            IList<IMessageAccountAlert> IMessageAccountAlertService.GetBatch(IEnumerable<int> accountAlertIds)
            {
                throw new NotImplementedException();
            }

            IList<IMessageAccountAlert> IMessageAccountAlertService.GetAll()
            {
                throw new NotImplementedException();
            }
        }
    }
}
