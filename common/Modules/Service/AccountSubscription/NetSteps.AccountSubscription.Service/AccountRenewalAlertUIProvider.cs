using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Communication.Common;
using NetSteps.Communication.Services;
using NetSteps.Communication.UI.Common;
using NetSteps.Communication.UI.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Locale.Common;
using NetSteps.AccountSubscription.Common;
using NetSteps.Content.Common;

namespace NetSteps.AccountSubscription.Service
{
    public class AccountRenewalAlertUIProvider : MessageAccountAlertUIProvider
    {
        protected readonly IAccountSubscriptionService _accountSubscriptionService;
        protected readonly ITermResolver _termProvider;

        public AccountRenewalAlertUIProvider(
            IMessageAccountAlertService messageAccountAlertService, IAccountSubscriptionService accountSubscription, ITermResolver termProvider)
            : base(messageAccountAlertService)
        {
            _accountSubscriptionService = accountSubscription ?? Create.New<IAccountSubscriptionService>();
            _termProvider = termProvider ?? Create.New<ITermResolver>();
        }

        public IEnumerable<IAccountAlertMessageModel> GetMessages(IAccountAlertUISearchParameters alertParams, ILocalizationInfo localizationInfo)
        {
            var alert = _accountSubscriptionService.LoadAccountSubscription(alertParams.AccountId.Value);

            if (alert.RenewalEndDate <= DateTime.Now)
            {
                yield return Create.Mutation(Create.New<IAccountAlertMessageModel>(), x =>
                {
                    x.IsDismissable = true;
                    x.Message = _termProvider.Term("Account_Renewal_Past_Due_Alert",
                        "Your renewal date is past due.  Please purchase the renewal SKU or contact Customer Service.");
                });
            }
        }

        public IEnumerable<IAccountAlertModalModel> GetModals(IAccountAlertUISearchParameters alertParams, ILocalizationInfo localizationInfo)
        {
            var alert = _accountSubscriptionService.LoadAccountSubscription(alertParams.AccountId.Value);

            if (alert.RenewalEndDate <= DateTime.Now)
            {
                yield return Create.Mutation(Create.New<IMessageAccountAlertModalModel>(), x =>
                {
                    x.PartialName = "_MessageAccountAlertModal";
                    x.PartialTitle = "";
                    x.Message = _termProvider.Term("Account_Renewal_Past_Due_Alert",
                            "Your renewal date is past due.  Please purchase the renewal SKU or contact Customer Service.");
                });
            }
        }
    }

}
