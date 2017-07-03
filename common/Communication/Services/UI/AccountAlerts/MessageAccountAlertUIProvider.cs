using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Models;
using NetSteps.Communication.Common;
using NetSteps.Communication.UI.Common;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Communication.UI.Services
{
    public class MessageAccountAlertUIProvider : IMessageAccountAlertUIProvider
    {
        protected readonly IMessageAccountAlertService _messageAccountAlertService;

        public MessageAccountAlertUIProvider(
            IMessageAccountAlertService messageAccountAlertService)
        {
            Contract.Requires<ArgumentNullException>(messageAccountAlertService != null);

            _messageAccountAlertService = messageAccountAlertService;
        }

        public IEnumerable<IAccountAlertMessageModel> GetMessages(IEnumerable<int> accountAlertIds, ILocalizationInfo localizationInfo)
        {
            return
                _messageAccountAlertService.GetBatch(accountAlertIds)
                .Select(x =>
                {
                    var model = Create.New<IAccountAlertMessageModel>();
                    model.AccountAlertId = x.AccountAlertId;
                    model.Message = x.Message;
                    model.IsDismissable = true;
                    return model;
                });
        }

        public IEnumerable<IAccountAlertModalModel> GetModals(IEnumerable<int> accountAlertIds, ILocalizationInfo localizationInfo)
        {
            return
                _messageAccountAlertService.GetBatch(accountAlertIds)
                .Select(x =>
                {
                    var model = Create.New<IMessageAccountAlertModalModel>();
                    model.AccountAlertId = x.AccountAlertId;
                    model.PartialName = "_MessageAccountAlertModal";
                    model.PartialTitle = "";
                    model.Message = x.Message;
                    return model;
                });
        }

        public void Dismiss(int accountAlertId, int accountId)
        {
            _messageAccountAlertService.Dismiss(accountAlertId, accountId);
        }
    }
}
