using System;
using System.Collections.Generic;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.Common.Repositories;
using NetSteps.Events.EmailEventTemplate.Common;

namespace NetSteps.Data.Entities.Repositories
{
	using System.Globalization;
	using System.Linq;
	using System.Threading;

	using NetSteps.Data.Entities.Cache;

	[ContainerRegister(typeof(IEmailRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class EmailRepository : IEmailRepository
	{
		private IEmailEventTemplateRepository emailEventEmailTemplateRepo;
		protected IEmailEventTemplateRepository EmailEventEmailTemplateRepository
		{
			get
			{
				if (emailEventEmailTemplateRepo == null)
				{
					emailEventEmailTemplateRepo = Create.New<IEmailEventTemplateRepository>();
				}
				return emailEventEmailTemplateRepo;
			}
		}



		protected virtual EmailTemplate GetEmailTemplate(int eventTypeID)
		{
			int emailTemplateId = this.EmailEventEmailTemplateRepository.GetEmailTemplateIdByEventTypeID(eventTypeID);
			if (emailTemplateId < 1)
			{
				throw new Exception(string.Format("There is no emailTemplate for eventTypeID {0}", eventTypeID));
			}
			return EmailTemplate.LoadFull(emailTemplateId);
		}


		public virtual void SendEmail(int accountId, int eventTypeId, ITokenValueProvider tokenValueProvider, IEnumerable<string> extraRecipients)
		{
			var account = Account.Load(accountId);

			if (account == null) throw new ArgumentException(string.Format("Account {0} does not exist.", accountId), "accountId");
			if (String.IsNullOrWhiteSpace(account.FullName)) throw new NetStepsException(string.Format("Account {0} Full Name is empty.", accountId));
			if (String.IsNullOrWhiteSpace(account.EmailAddress)) throw new NetStepsException(string.Format("Account {0} email address is empty.", accountId));

			if (tokenValueProvider == null) throw new NetStepsException("TokenValueProvider is null.");

			EmailTemplate emailTemplate = GetEmailTemplate(eventTypeId);
			if (emailTemplate == null) throw new NetStepsException("EmailTemplate is null.");

			var emailTemplateTranslation = emailTemplate.EmailTemplateTranslations.GetByLanguageID(account.DefaultLanguageID)
				 ?? emailTemplate.EmailTemplateTranslations.GetByLanguageID((int)Constants.Language.English);

			if (emailTemplateTranslation == null) throw new NetStepsException(string.Format("EmailTemplateTranslation for EmailTemplate {0} not found.", emailTemplate.Name));

			//Sets the Subject, Body, & FromAddress
			var mailMessage = emailTemplateTranslation.GetTokenReplacedMailMessage(tokenValueProvider);

			mailMessage.To.Add(
					new NetSteps.Data.Entities.Mail.MailMessageRecipient(account.FullName, account.EmailAddress));

			foreach (var address in extraRecipients)
			{
				mailMessage.To.Add(new NetSteps.Data.Entities.Mail.MailMessageRecipient(address));
			}

			mailMessage.VisualTemplateID = emailTemplateTranslation.EmailTemplateTranslationID;

			var mailAccountId = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.CorporateMailAccountID, 1);

			mailMessage.Send(MailAccount.LoadFull(mailAccountId), 420);
		}

		public void LogException(Exception ex)
		{
			ex.Log(Constants.NetStepsExceptionType.NetStepsApplicationException);
		}

		public string FormatDecimalAccordingToCulture(decimal toFormat, int currencyID)
		{
			var currency = SmallCollectionCache.Instance.Currencies.FirstOrDefault(c => c.CurrencyID == currencyID);
			var culture = currencyID == 0 ? Thread.CurrentThread.CurrentCulture : SmallCollectionCache.Instance.Currencies.GetById(currencyID).Culture;

			if(currency == null)
			{
				return toFormat.TwoDecimalPlaces();
			}

			return FormatCurrency(toFormat, culture);

		}

		public string FormatCurrency(decimal toFormat, CultureInfo culture)
		{
			string toFormatString = toFormat.TwoDecimalPlaces();

			decimal value = Decimal.Parse(toFormatString);
			return value.ToString("C", culture);
		}
	}

}
