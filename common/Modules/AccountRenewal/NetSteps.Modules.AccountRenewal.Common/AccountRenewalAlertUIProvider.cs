using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Communication.UI.Common;
using NetSteps.Communication.Common;
using NetSteps.Locale.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Communication.Services;
using NetSteps.Communication.UI.Services;

namespace NetSteps.Modules.AccountRenewal.Common
{
	public class AccountRenewalAlertUIProvider : MessageAccountAlertUIProvider
	{
		protected readonly IAccountRenewal _accountRenewal;
		protected readonly ITermTranslationProvider _termProvider;

		public AccountRenewalAlertUIProvider(
			IMessageAccountAlertService messageAccountAlertService, IAccountRenewal accountRenewal, ITermTranslationProvider termProvider)
			: base(messageAccountAlertService)
		{
			_accountRenewal = accountRenewal ?? Create.New<IAccountRenewal>();
			_termProvider = termProvider ?? Create.New<ITermTranslationProvider>();
		}

		public IEnumerable<IAccountAlertMessageModel> GetMessages(IAccountAlertUISearchParameters alertParams, ILocalizationInfo localizationInfo)
		{
			var alert = _accountRenewal.LoadRenewalAccount(alertParams.AccountId.Value);

			if(alert.NextRenewalDateUTC >= DateTime.Now)
			{
				yield return Create.Mutation(Create.New<IAccountAlertMessageModel>(), x =>
					{
						x.IsDismissable = true;
						x.Message = _termProvider.GetTerm("Account_Renewal_Past_Due_Alert", 
							"Your renewal date is past due.  Please purchase the renewal SKU or contact Customer Service.");
					});
			}
		}

		public IEnumerable<IAccountAlertModalModel> GetModals(IAccountAlertUISearchParameters alertParams, ILocalizationInfo localizationInfo)
		{
			var alert = _accountRenewal.LoadRenewalAccount(alertParams.AccountId.Value);

			if (alert.NextRenewalDateUTC >= DateTime.Now)
			{
				yield return Create.Mutation(Create.New<IMessageAccountAlertModalModel>(), x =>
				{
					x.PartialName = "_MessageAccountAlertModal";
					x.PartialTitle = "";
					x.Message = _termProvider.GetTerm("Account_Renewal_Past_Due_Alert",
							"Your renewal date is past due.  Please purchase the renewal SKU or contact Customer Service.");
				});
			}
		}
	}
}
