using NetSteps.Communication.Common;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System;
namespace NetSteps.Communication.Services
{
    public class MessageAccountAlertProvider: IMessageAccountAlertProvider
    {
        protected readonly IMessageAccountAlertService _messageAccountAlertService;

        public MessageAccountAlertProvider(IMessageAccountAlertService messageAccountAlertService)
        {
            Contract.Requires<ArgumentNullException>(messageAccountAlertService != null);

            _messageAccountAlertService = messageAccountAlertService;
        }

        public IList<IMessageAccountAlert> GetBatch(IEnumerable<int> accountAlertIds)
        {
            return _messageAccountAlertService.GetBatch(accountAlertIds);
        }

        IList<IAccountAlert> IAccountAlertProvider.GetBatch(IEnumerable<int> accountAlertIds)
        {
            return new List<IAccountAlert>(_messageAccountAlertService.GetBatch(accountAlertIds));
        }
    }
}
