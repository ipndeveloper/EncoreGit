
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;



namespace NetSteps.QueueProcessing.Modules.ModuleBase
{
	public abstract class CampaignEmailSender : ICampaignEmailSender
	{
		protected IQueueProcessorLogger Logger { get; private set; }

		protected DomainEventQueueItem DomainEventQueueItem { get; private set; }

		protected EmailCampaignAction EmailCampaignAction { get; private set; }

		protected virtual IEnumerable<PartyGuest> GetRecipientEmailNameAndAddress()
		{
			return new List<PartyGuest>();
		}

		protected virtual ITokenValueProvider GetTokenValueProvider(PartyGuest guest)
		{
			return new PartyGuestTokenValueProvider(guest);
		}

		public CampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
		{
			Logger = Create.New<IQueueProcessorLogger>();

			DomainEventQueueItem = domainEventQueueItem;
			EmailCampaignAction = emailCampaignAction;
		}

		protected abstract string GetRecipientEmailAddress();

		protected virtual IEnumerable<string> GetAdditionalRecipientEmailAddresses()
		{
			return Enumerable.Empty<string>();
		}

		protected abstract string GetRecipientFullName();

		protected abstract int GetRecipientLanguageID();

		protected abstract ITokenValueProvider GetTokenValueProvider();

		private void LogCallBegin(DomainEventQueueItem domainEventQueueItem, string name)
		{
			this.Logger.Debug("{0} for DomainEventQueueItem {1} : change tracking {2}"
				, name
				, domainEventQueueItem.DomainEventQueueItemID
				, domainEventQueueItem.ChangeTracker.ChangeTrackingEnabled ? "ENABLED" : "DISABLED"
				);
		}

		private void LogCallEnd(DomainEventQueueItem domainEventQueueItem, string name)
		{
			this.Logger.Debug("finished {0} for DomainEventQueueItem {1} : change tracking {2}"
				, name
				, domainEventQueueItem.DomainEventQueueItemID
				, domainEventQueueItem.ChangeTracker.ChangeTrackingEnabled ? "ENABLED" : "DISABLED"
				);
		}

		public virtual bool SendEmail()
		{
			this.Logger.Debug("starting SendEmail for {0} : change tracking {1}"
				, this.DomainEventQueueItem.DomainEventQueueItemID
				, this.DomainEventQueueItem.ChangeTracker.ChangeTrackingEnabled ? "ENABLED" : "DISABLED"
				);

			bool result = false;
			string errorMessage = string.Empty;

			if (EmailCampaignAction != null && EmailCampaignAction.EmailTemplateID > 0)
			{
				// TODO: Cache the EmailTemplates in memory for a little while to boost performance - JHE
				this.LogCallBegin(this.DomainEventQueueItem, "EmailTemplate.LoadFull");
				var emailTemplate = EmailTemplate.LoadFull(EmailCampaignAction.EmailTemplateID);
				this.LogCallEnd(this.DomainEventQueueItem, "EmailTemplate.LoadFull");

				this.LogCallBegin(this.DomainEventQueueItem, "GetByLanguageID");
				var emailTemplateTranslation = emailTemplate.EmailTemplateTranslations.GetByLanguageID(GetRecipientLanguageID())
					?? emailTemplate.EmailTemplateTranslations.GetByLanguageID((int)Constants.Language.English);
				this.LogCallBegin(this.DomainEventQueueItem, "GetByLanguageID");

				//TODO: COMM-ENG: Implement business logic for when an email template is not found. - SOK

				if (emailTemplateTranslation != null)
				{
					this.Logger.Info("got emailTemplateTranslation {0}", emailTemplateTranslation.EmailTemplateTranslationID);

					this.LogCallBegin(this.DomainEventQueueItem, "GetTokenValueProvider");
					var tokenValueProvider = GetTokenValueProvider();
					this.LogCallEnd(this.DomainEventQueueItem, "GetTokenValueProvider");

					//Sets the Subject, Body, & FromAddress
					this.LogCallBegin(this.DomainEventQueueItem, "GetTokenReplacedMailMessage");
					var mailMessage = emailTemplateTranslation.GetTokenReplacedMailMessage(tokenValueProvider);
					this.LogCallEnd(this.DomainEventQueueItem, "GetTokenReplacedMailMessage");

					var recipientFullName = GetRecipientFullName();
					var recipientEmailAddress = GetRecipientEmailAddress();
					this.Logger.Info("adding recipient {0} {1}", recipientFullName, recipientEmailAddress);
					mailMessage.To.Add(new NetSteps.Data.Entities.Mail.MailMessageRecipient(recipientFullName, recipientEmailAddress));

					foreach (var address in GetAdditionalRecipientEmailAddresses())
					{
						this.Logger.Info("adding additional recipient {0}", address);
						mailMessage.To.Add(new NetSteps.Data.Entities.Mail.MailMessageRecipient(address));
					}

					mailMessage.VisualTemplateID = emailTemplateTranslation.EmailTemplateTranslationID;


                    if (emailTemplateTranslation.AttachmentPath != null)
                    {
                    Data.Entities.Mail.MailMessageAttachment MessAttach = new Data.Entities.Mail.MailMessageAttachment();
                    Data.Entities.Mail.MailAttachment attach = new Data.Entities.Mail.MailAttachment ();
                    attach.MailAttachmentID = MessAttach.MailMessageAttachmentID;
                    attach.FileName = emailTemplateTranslation.AttachmentPath;
                    MessAttach.MailMessageID = mailMessage.MailMessageID;
                    MessAttach.MailAttachmentID = attach.MailAttachmentID;

                    mailMessage.Attachments.Add(attach);
                    }

					//TODO: COMM-ENG: Attachments! - SOK
					var mailAccountId = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.CorporateMailAccountID, 1);

					this.LogCallBegin(this.DomainEventQueueItem, "Send");
					mailMessage.Send(MailAccount.LoadFull(mailAccountId), 420);
					this.LogCallEnd(this.DomainEventQueueItem, "Send");

					result = true;

					this.Logger.Info("SendEmail - DomainEventQueueItemID: {0} - Email Successfully Saved!"
						, DomainEventQueueItem.DomainEventQueueItemID
						);
				}
				else
				{
					errorMessage = string.Format("SendEmail - DomainEventQueueItemID: {0} - Missing EmailTemplateTranslation for LanguageID: {1}", DomainEventQueueItem.DomainEventQueueItemID, GetRecipientLanguageID());
				}
			}
			else
			{
				if (EmailCampaignAction != null && EmailCampaignAction.EmailTemplateID > 0)
				{
					errorMessage = string.Format("SendEmail - DomainEventQueueItemID: {0} - Missing EmailTemplateID: {1}", DomainEventQueueItem.DomainEventQueueItemID, EmailCampaignAction.EmailTemplateID);
				}
				else
				{
					errorMessage = string.Format("SendEmail - DomainEventQueueItemID: {0} - Null EmailCampaignAction");
				}
			}

			if (!result)
			{
				this.Logger.Error(errorMessage);
				EntityExceptionHelper.GetAndLogNetStepsException(errorMessage, Data.Entities.Constants.NetStepsExceptionType.NetStepsDataException);
			}

			this.Logger.Debug("finished SendEmail for {0} : result {1} : change tracking {2}"
				, this.DomainEventQueueItem.DomainEventQueueItemID
				, result ? "SUCCESS" : "FAILED"
				, this.DomainEventQueueItem.ChangeTracker.ChangeTrackingEnabled ? "ENABLED" : "DISABLED"
				);

			return result;
		}

		/// <summary>
		/// Method to send the email for more than one email id
		/// currently it is used to send out the emails for party guests
		/// </summary>
		/// <returns></returns>
		public virtual bool SendEmails()
		{
			bool result = false;
			string logMessage = string.Empty;

			if (EmailCampaignAction != null && EmailCampaignAction.EmailTemplateID > 0)
			{
				// TODO: Cache the EmailTemplates in memory for a little while to boost performance - JHE
				var emailTemplate = EmailTemplate.LoadFull(EmailCampaignAction.EmailTemplateID);

				var emailTemplateTranslation = emailTemplate.EmailTemplateTranslations.GetByLanguageID(GetRecipientLanguageID())
					?? emailTemplate.EmailTemplateTranslations.GetByLanguageID((int)Constants.Language.English);

				//TODO: COMM-ENG: Implement business logic for when an email template is not found. - SOK
				if (emailTemplateTranslation != null)
				{
					//loop the list that is returned from the campainemailsender(class overrided by this) class and send out the emails
					foreach (var guest in GetRecipientEmailNameAndAddress())
					{
						var tokenValueProvider = GetTokenValueProvider(guest);

						//Sets the Subject, Body, & FromAddress
						var mailMessage = emailTemplateTranslation.GetTokenReplacedMailMessage(tokenValueProvider);

						mailMessage.To.Add(new NetSteps.Data.Entities.Mail.MailMessageRecipient(guest.FirstName, guest.EmailAddress));

						mailMessage.VisualTemplateID = emailTemplateTranslation.EmailTemplateTranslationID;

						//TODO: COMM-ENG: Attachments! - SOK
						var mailAccountId = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.CorporateMailAccountID);
						if (mailAccountId == 0)
							mailAccountId = 1;
						mailMessage.Send(MailAccount.LoadFull(mailAccountId), 420);
					}
					result = true;

					logMessage = string.Format("SendEmail - DomainEventQueueItemID: {0} - Email Successfully Saved!", DomainEventQueueItem.DomainEventQueueItemID);
				}
				else
				{
					logMessage = string.Format("SendEmail - DomainEventQueueItemID: {0} - Missing EmailTemplateTranslation for LanguageID: {1}", DomainEventQueueItem.DomainEventQueueItemID, GetRecipientLanguageID());
				}
			}
			else
			{
				if (EmailCampaignAction != null && EmailCampaignAction.EmailTemplateID > 0)
				{
					logMessage = string.Format("SendEmail - DomainEventQueueItemID: {0} - Missing EmailTemplateID: {1}", DomainEventQueueItem.DomainEventQueueItemID, EmailCampaignAction.EmailTemplateID);
				}
				else
				{
					logMessage = string.Format("SendEmail - DomainEventQueueItemID: {0} - Null EmailCampaignAction");
				}
			}

			Logger.Info(logMessage);
			EntityExceptionHelper.GetAndLogNetStepsException(logMessage, Data.Entities.Constants.NetStepsExceptionType.NetStepsDataException);

			return result;
		}
	}
}
