using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetSteps.Accounts.Common.Models;
using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.Common.Repositories;

namespace NetSteps.Events.Service.AccountEvents
{
	public abstract class BaseAccountEmailEvent : BaseAccountEvent
	{
		private IAccountRepository accountRepository;
		protected IAccountRepository AccountRepository
		{
			get
			{
				if (accountRepository == null)
				{
					accountRepository = Create.New<IAccountRepository>();
				}
				return accountRepository;
			}
		}

		private IEmailRepository emailRepository;
		protected IEmailRepository EmailRepository
		{
			get
			{
				if (emailRepository == null)
				{
					emailRepository = Create.New<IEmailRepository>();
				}
				return emailRepository;
			}
		}

		public override bool Execute(int eventID)
		{
			int accountId = ArgumentRepository.GetAccountIdByEventId(eventID);
			if (accountId == 0) throw new InvalidDataException("No account is associated with event " + eventID);

			int eventTypeID = EventRepository.GetEventTypeID(eventID);

			try
			{
				SendEmail(accountId, eventTypeID);
				return true;
			}
			catch (Exception ex)
			{
				EmailRepository.LogException(ex);
			}

			return false;
		}

		protected virtual IEnumerable<string> GetAdditionalRecipientEmailAddresses()
		{
			return Enumerable.Empty<string>();
		}

		protected virtual IEnumerable<string> GetCcEmailAddresses()
		{
			return Enumerable.Empty<string>();
		}

		protected abstract ITokenValueProvider GetTokenValueProvider(IAccount account);

		protected virtual void SendEmail(int accountId, int eventTypeId)
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

			EmailRepository.SendEmail(accountId, eventTypeId, tokenValueProvider, GetAdditionalRecipientEmailAddresses().Concat(GetCcEmailAddresses()));
		}
	}
}
