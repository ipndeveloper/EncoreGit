using System;
using System.Linq;
using NetSteps.Accounts.Common.Models;
using NetSteps.Common.Interfaces;

namespace NetSteps.Events.Service.AccountEvents
{
	public abstract class BaseAccountSponsorEmailEvent : BaseAccountEmailEvent
	{
		protected override void SendEmail(int accountId, int eventTypeId)
		{
			IAccount account = AccountRepository.GetAccountByAccountID(accountId);
			if (account == null)
			{
				throw new Exception(string.Format("No account for AccountID: {0}", accountId));
			}

			ITokenValueProvider tokenValueProvider = GetTokenValueProvider(account);
			if (tokenValueProvider == null)
			{
				throw new Exception(string.Format("No token value provider"));
			}


			if (!account.SponsorID.HasValue)
			{
				return;
			}

			EmailRepository.SendEmail(account.SponsorID.Value, eventTypeId, tokenValueProvider,
						  GetAdditionalRecipientEmailAddresses().Concat(GetCcEmailAddresses()));
		}
	}
}
