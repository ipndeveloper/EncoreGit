using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using MailBee.Mime;
using MailBee.SmtpMail;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Mail;

namespace NetSteps.QueueProcessing.Modules.SendMail
{
	using NetSteps.QueueProcessing.Modules.ModuleBase;

	#region Helper Classes
	public class QueueMailMessage
	{
		public NetSteps.Data.Entities.Mail.MailMessage message;
		public NetSteps.Data.Entities.Mail.Constants.EmailMessageType messageType;
		public int SiteID;
		public List<QueueMailAttachment> Attachments = new List<QueueMailAttachment>();
	}

	public class QueueMailAttachment
	{
		public int MailMessageAttachmentID;
		public int MailMessageID;
		public int MailAttachmentID;
		public String FileName;
	}

	public class QueueMessageAddress
	{
		public int MailMessageGroupID;
		public int MailMessageGroupAddressID;
		public string NickName;
		public NetSteps.Data.Entities.Mail.Constants.EmailAddressType AddressType;
		public string EmailAddress;
		public int MailAccountID;
		public NetSteps.Data.Entities.Mail.Constants.EmailRecipientStatus RecipientStatusID;
	}

	public class QueueMessageGroup
	{
		public QueueMailMessage Message;
		public int MailMessageGroupID;
		public int MailMessageID;

		public List<QueueMessageAddress> Addresses = new List<QueueMessageAddress>();
	}
	#endregion

	public class SendMailQueueProcessor : QueueProcessor<QueueMessageGroup>
	{
		private int _attempts = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.MailMessageSendAttempts, 5);
		private List<string> _siteDomains;

		public static readonly string CProcessorName = "SendMailQueueProcessor";

		public SendMailQueueProcessor()
		{
			Name = CProcessorName;

			// Pull this from the config file - JHE
			Smtp.LicenseKey = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpLicenseKey, "MN200-9F57689E57B057AB577699C15A6B-9A7C");
			_siteDomains = new List<string>();
			IDbCommand dbCommand = null;

			try
			{
				dbCommand = DataAccess.SetCommand("usp_GetSiteDomains", connectionString: NetSteps.Data.Entities.Mail.MailMessage.GetMailConnectionString());
				IDataReader reader = DataAccess.ExecuteReader(dbCommand);
				while (reader.Read())
					_siteDomains.Add(((string)reader["DomainName"]).ToLower());
			}
			finally
			{
				DataAccess.Close(dbCommand);
			}
		}

		Dictionary<int, string> optoutMessages = new Dictionary<int, string>();

		public string GetSiteOptoutMessage(int siteID)
		{
			// This builds a caches list of outputs by siteid so we don't have to lookup every time
			string optoutMessage = "";

			if (!optoutMessages.ContainsKey(siteID))
			{
				HtmlContent HtmlContent = HtmlContent.FindBySectionNameAndSiteId("CampaignOptOut", siteID);

				// TODO: Finish porting the content stuff to the new Entities solution this. - JHE
				throw new NotImplementedException();
				//ContentControl cc = new ContentControl();
				//cc.Content = HtmlContent;
				//optoutMessage = cc.ReplaceMarkupTokens();
				//optoutMessages.Add(siteID, optoutMessage);
			}

			return optoutMessage;
		}

		public override void CreateQueueItems(int maxNumberToEnqueue)
		{
			Dictionary<int, QueueMailMessage> messages = new Dictionary<int, QueueMailMessage>();
			Dictionary<int, QueueMessageGroup> messageGroups = new Dictionary<int, QueueMessageGroup>();

			Logger.Info("SendMailQueueProcessor - CreateQueueItems");

			IDbCommand dbCommand = null;

			try
			{
				// This returns 3 data sets
				dbCommand = DataAccess.SetCommand("usp_QueueMailMessages", connectionString: NetSteps.Data.Entities.Mail.MailMessage.GetMailConnectionString());
				DataAccess.AddInputParameter("MaxNumberToPoll", maxNumberToEnqueue, dbCommand);
				DataAccess.AddInputParameter("RetryCount", _attempts, dbCommand);

				DataSet dataset = DataAccess.GetDataSet(dbCommand);

				// First : build a list of Messages
				foreach (DataRow row in dataset.Tables[0].Rows)
				{
					QueueMailMessage queueMessage = new QueueMailMessage();

					NetSteps.Data.Entities.Mail.MailMessage message = new NetSteps.Data.Entities.Mail.MailMessage();

					message.MailMessageID = (int)row["MailMessageID"];
					message.Subject = (string)row["Subject"];
					message.Body = (string)row["Body"];
					message.HTMLBody = (string)row["HTMLBody"];
					message.DateAddedUTC = (DateTime)row["DateAddedUTC"];
					message.FromNickName = (string)row["FromNickName"];
					message.FromAddress = (string)row["FromAddress"];
					if (!row.IsNull("ReplyToAddress"))
						message.ReplyToAddress = (string)row["ReplyToAddress"];
					message.MailMessagePriorityID = ((short)row["MailMessagePriorityID"]);
					message.VisualTemplateID = (int)row["VisualTemplateID"];
					message.AttachmentUniqueID = (string)row["AttachmentUniqueID"];
					queueMessage.messageType = (NetSteps.Data.Entities.Mail.Constants.EmailMessageType)((short)row["MailMessageTypeID"]);
					queueMessage.SiteID = (int)row["SiteID"];
					message.CampaignActionID = row["CampaignActionID"] == DBNull.Value ? null : (int?)row["CampaignActionID"];
					message.EnableEventTracking = (bool)row["EnableEventTracking"];

					// Right now is the perfect time to modify the message
					//  Add opt-out footers etc.

					// Only add opt-out to downline emails
					//if (queueMessage.messageType == NetSteps.Data.Entities.Mail.Constants.EmailMessageType.Downline)
					//{
					//    message.Body += "<br\\>" + GetSiteOptoutMessage(queueMessage.SiteID);
					//    message.HTMLBody += "<br\\>" + GetSiteOptoutMessage(queueMessage.SiteID);
					//}

					queueMessage.message = message;
					messages.Add(message.MailMessageID, queueMessage);
				}

				// Second : Add attachment list to messages
				foreach (DataRow row in dataset.Tables[1].Rows)
				{
					QueueMailAttachment attachment = new QueueMailAttachment();

					attachment.MailMessageAttachmentID = (int)row["MailMessageAttachmentID"];
					attachment.MailMessageID = (int)row["MailMessageID"];
					attachment.MailAttachmentID = (int)row["MailAttachmentID"];
					attachment.FileName = (string)row["FileName"];

					messages[attachment.MailMessageID].Attachments.Add(attachment);
				}

				// Third : Build List of Groups
				foreach (DataRow row in dataset.Tables[2].Rows)
				{
					QueueMessageGroup group = new QueueMessageGroup();

					group.MailMessageGroupID = (int)row["MailMessageGroupID"];
					group.MailMessageID = (int)row["MailMessageID"];
					group.Message = messages[group.MailMessageID];

					messageGroups.Add(group.MailMessageGroupID, group);
				}

				// Forth : Add Addresses to the Groups
				foreach (DataRow row in dataset.Tables[3].Rows)
				{
					QueueMessageAddress address = new QueueMessageAddress();

					address.MailMessageGroupID = (int)row["MailMessageGroupID"];
					address.MailMessageGroupAddressID = (int)row["MailMessageGroupAddressID"];
					address.NickName = (string)row["NickName"];
					address.EmailAddress = (string)row["EmailAddress"];
					address.AddressType = (NetSteps.Data.Entities.Mail.Constants.EmailAddressType)((short)row["AddressTypeID"]);
					address.MailAccountID = (int)row["MailAccountID"];
					address.RecipientStatusID = (NetSteps.Data.Entities.Mail.Constants.EmailRecipientStatus)((short)row["RecipientStatusID"]);

					messageGroups[address.MailMessageGroupID].Addresses.Add(address);
				}

				// Fifth : Enqueue each group item
				int itemCount = 0;
				foreach (QueueMessageGroup group in messageGroups.Values)
				{
					EnqueueItem(group);
					itemCount++;
				}

				Logger.Info("SendMailQueueProcessor - Enqueued {0} Items", itemCount);
			}
			finally
			{
				DataAccess.Close(dbCommand);
			}
		}

		public override void ProcessQueueItem(QueueMessageGroup group)
		{
			this.Logger.Debug("Processing individual mail item: {0}", group.MailMessageID);

			// ** IMPORTANT **
			// This is a multi threaded operation, meaning multiple threads are running ProcessQueueItem in parallel
			// THE ELEMENTS ARE NOT THREADSAFE. The means you can read from group, but you shouldn't modify it because
			// another thread may have a reference to a member also (especially the message member). Please treat
			// group and all its members as strictly READONLY

			CreateExternalEmail(group);

			// Loop through all addresses for the message
			foreach (QueueMessageAddress address in group.Addresses)
			{
				bool isInternal;

				// if the address status is not "unknown" then it was already determined that we don't need to email them
				// like they opted out or something. Their addresses are in the list to preserve them on the TO line
				// so people know they were emailed...
				if (address.RecipientStatusID != NetSteps.Data.Entities.Mail.Constants.EmailRecipientStatus.Unknown ||
					!IsValidEmailAddress(address, out isInternal))
				{
					this.Logger.Info("did not process address {0} because status id is {1} or address not valid", address, address.RecipientStatusID);
					continue;
				}
				// We have a valid message, so now we need to send the email
				if (isInternal || address.MailAccountID > 0)
				{
					// if it's internal then just send directly to the db to group.MailAccountID
					CreateInternalEmail(address.MailAccountID, address.EmailAddress, group, address);
				}
			}

			IDbCommand dbCommand = null;

			this.Logger.Debug("handled message, updating status");
			try
			{
				dbCommand = DataAccess.SetCommand("usp_MailMessageGroupChangeStatus", connectionString: NetSteps.Data.Entities.Mail.MailMessage.GetMailConnectionString());
				DataAccess.AddInputParameter("MailMessageGroupID", group.MailMessageGroupID, dbCommand);
				DataAccess.AddInputParameter("MessageGroupStatusID", (short)NetSteps.Data.Entities.Mail.Constants.MessageGroupStatusType.SMTPSent, dbCommand);

				DataAccess.ExecuteNonQuery(dbCommand);
			}
			catch (Exception excp)
			{
				this.Logger.Error(excp.ToString());
				throw;
			}
			finally
			{
				DataAccess.Close(dbCommand);
			}

			this.Logger.Debug("Finished processing individual mail item: {0}", group.MailMessageID);
		}

		private void CreateInternalEmail(int mailAccountID, string emailAddress, QueueMessageGroup group, QueueMessageAddress address)
		{
			this.Logger.Debug("creating internal email for {0}", emailAddress);
			try
			{
				NetSteps.Data.Entities.Mail.MailMessage obj = group.Message.message;

				//create a table with column names and types per the DB type
				DataTable addressTable = new DataTable("AddressList");
				addressTable.Columns.Add("EmailAddress", typeof(string));
				addressTable.Columns.Add("NickName", typeof(string));
				addressTable.Columns.Add("AddressTypeID", typeof(short));
				addressTable.Columns.Add("RecipientTypeID", typeof(short));

				// if you are BCC'd then you can only see yourself, you can't see all the other addresses that were emailed
				// and you're name goes to the TO line.
				if (address.AddressType == NetSteps.Data.Entities.Mail.Constants.EmailAddressType.BCC)
				{
					addressTable.Rows.Add(address.EmailAddress, address.NickName, (short)NetSteps.Data.Entities.Mail.Constants.EmailAddressType.TO, (short)NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientType.Individual);
				}
				else
				{
					foreach (QueueMessageAddress recipient in group.Addresses)
					{
						// PURPOSEFULLY SKIP BCC - when sending messages, the recipient cannot see the BCC values.
						if (recipient.AddressType != NetSteps.Data.Entities.Mail.Constants.EmailAddressType.BCC)
							addressTable.Rows.Add(recipient.EmailAddress, recipient.NickName, recipient.AddressType, (short)NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientType.Individual);
					}
				}

				DataTable attachmentTable = new DataTable("AttachmentList");
				attachmentTable.Columns.Add("MailAttachmentID", typeof(Int32));

				foreach (QueueMailAttachment attachment in group.Message.Attachments)
				{
					attachmentTable.Rows.Add(attachment.MailAttachmentID);
				}

				IDbCommand dbCommand = null;

				try
				{
					// If the mail message already exists (MailMessageID != 0) then it will update the record, otherwise it will insert
					dbCommand = DataAccess.SetCommand("usp_MailMessageDeliverInternal", connectionString: NetSteps.Data.Entities.Mail.MailMessage.GetMailConnectionString());

					// IMPORTANT: this makes a copy of the Body and HTMLBody so we can modify them
					//            remember, the group is READONLY because this is multi threaded and more than one thread
					//            is referencing the message object underneath.
					string Body = obj.Body;
					string HTMLBody = obj.HTMLBody;

					// THIS IS THE PERFECT PLACE TO DO MAIL MERGE REPLACEMENTS
					if (obj.EnableEventTracking)
						HTMLBody = NetSteps.Data.Entities.Mail.MailMessage.ApplyEventTrackingToInternalMessage(HTMLBody, obj.CampaignActionID, group.MailMessageGroupID, emailAddress);

					//Put the email address in the opt out string
					Body = Body.Replace("%%email%%", emailAddress);
					HTMLBody = HTMLBody.Replace("%%email%%", emailAddress);

					DataAccess.AddInputParameter("MailMessageGroupAddressID", address.MailMessageGroupAddressID, dbCommand);
					DataAccess.AddInputParameter("Subject", obj.Subject, dbCommand);
					DataAccess.AddInputParameter("Body", Body, dbCommand);
					DataAccess.AddInputParameter("HTMLBody", HTMLBody, dbCommand);
					DataAccess.AddInputParameter("DateAddedUTC", obj.DateAddedUTC, dbCommand); // date of original message
					DataAccess.AddInputParameter("FromAddress", obj.FromAddress, dbCommand);
					DataAccess.AddInputParameter("ReplyToAddress", obj.ReplyToAddress, dbCommand);
					DataAccess.AddInputParameter("FromNickName", obj.FromNickName, dbCommand);
					DataAccess.AddInputParameter("MailAccountID", mailAccountID, dbCommand);
					DataAccess.AddInputParameter("IsOutbound", false, dbCommand);
					DataAccess.AddInputParameter("MailMessageTypeID", (short)NetSteps.Data.Entities.Mail.Constants.EmailMessageType.AdHoc, dbCommand); // casting enum to int should give me the int value
					DataAccess.AddInputParameter("BeenRead", false, dbCommand);
					DataAccess.AddInputParameter("MailMessagePriorityID", (short)obj.MailMessagePriorityID, dbCommand); // casting enum to int should give me the int value
					DataAccess.AddInputParameter("VisualTemplateID", obj.VisualTemplateID, dbCommand);
					DataAccess.AddInputParameter("MailFolderTypeID", (short)NetSteps.Data.Entities.Mail.Constants.MailFolderType.Inbox, dbCommand); // casting enum to int should give me the int value 
					DataAccess.AddInputParameter("AttachmentUniqueID", obj.AttachmentUniqueID, dbCommand);
					DataAccess.AddInputParameter("SiteID", group.Message.SiteID, dbCommand);

					DataAccess.AddInputParameterStructured("Addresses", "dbo.TTAddressList", addressTable, dbCommand);
					DataAccess.AddInputParameterStructured("Attachments", "dbo.TTInternalAttachmentList", attachmentTable, dbCommand);

					DataAccess.ExecuteNonQuery(dbCommand);

					this.Logger.Debug("successfully created internal email for {0}", emailAddress);
				}
				finally
				{
					DataAccess.Close(dbCommand);
				}
			}
			catch (Exception ex)
			{
				this.Logger.Error(ex.ToString());
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsDataException);
			}
		}

		//private void CreateExternalEmail(int mailAccountID, string emailAddress, QueueMessageGroup group, QueueMessageAddress address)
		private void CreateExternalEmail(QueueMessageGroup group)
		{
			this.Logger.Debug("creating external email for Group ID {0}", group.MailMessageGroupID);
			try
			{
				string smtpServerName = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpServer);
				int smtpPort = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.SmtpPort, 25);
				bool useSMTPAuthentication = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UseSmtpAuthentication);
				string smtpUserName = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpUserName);
				string smtpPassword = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpPassword);
				EmailAddressCollection externalRecipients = new EmailAddressCollection();

				this.Logger.Debug("creating smtp connection to {0}:{1} : {2}:{3}", smtpServerName, smtpPort, smtpUserName, smtpPassword);
				var smtp = new Smtp();
				var smtpServer = useSMTPAuthentication
					? new SmtpServer(smtpServerName, smtpUserName, smtpPassword)
					: new SmtpServer(smtpServerName);
				smtpServer.Port = smtpPort;
				smtp.SmtpServers.Add(smtpServer);

				MailBee.Mime.MailMessage mail = smtp.Message;
				NetSteps.Data.Entities.Mail.MailMessage obj = group.Message.message;

				foreach (QueueMessageAddress recipient in group.Addresses)
				{
					this.Logger.Debug("trying to add recipient {0} to message", recipient.EmailAddress);
					bool isInternal;

					// when sending to external addresses, we need to build the list of To, BCC, CC 
					// but the smtp recipients list needs to only be external addresses. This
					// build a separate recipients list from the To, BCC, CC list. This important
					// so that the hurricane server doesn't try and deliver the message to internal people
					// because we have already inserted them directly into the db.
					bool isValid = IsValidEmailAddress(recipient, out isInternal);
					if (isValid && !isInternal)
					{
						this.Logger.Debug("adding {0} as an external address", recipient.EmailAddress);
						externalRecipients.Add(recipient.EmailAddress);
					}

					switch (recipient.AddressType)
					{
						case NetSteps.Data.Entities.Mail.Constants.EmailAddressType.BCC:
							this.Logger.Debug("adding {0} as a bcc address", recipient.EmailAddress);
							mail.Bcc.Add(recipient.EmailAddress, recipient.NickName);
							break;

						case NetSteps.Data.Entities.Mail.Constants.EmailAddressType.CC:
							this.Logger.Debug("adding {0} as a cc address", recipient.EmailAddress);
							mail.Cc.Add(recipient.EmailAddress, recipient.NickName);
							break;

						case NetSteps.Data.Entities.Mail.Constants.EmailAddressType.TO:
							this.Logger.Debug("adding {0} as a to address", recipient.EmailAddress);
							mail.To.Add(recipient.EmailAddress, recipient.NickName);
							break;
					}
				}

				mail.From.Email = obj.FromAddress;
				mail.From.DisplayName = obj.FromNickName;
				if (!obj.ReplyToAddress.IsNullOrEmpty())
					mail.ReplyTo = new EmailAddressCollection(obj.ReplyToAddress);

				// IMPORTANT: this makes a copy of the Body so we can modify it
				//            remember, the group is READONLY because this is multi threaded and more than one thread
				//            is referencing the message object underneath.
				string HTMLBody = obj.HTMLBody;
				string Body = obj.Body;

				// THIS IS THE PERFECT PLACE TO DO MAIL MERGE REPLACEMENTS
				if (obj.EnableEventTracking)
					HTMLBody = NetSteps.Data.Entities.Mail.MailMessage.ApplyEventTrackingToExternalMessage(HTMLBody);

				//Put the email address in the opt out string
				//HTMLBody = HTMLBody.Replace("%%email%%", emailAddress);
				//Body = Body.Replace("%%email%%", emailAddress);

				mail.Subject = obj.Subject;
				mail.BodyHtmlText = HTMLBody;
				mail.BodyPlainText = Body;
				mail.Priority = GetMailBeePriorityFromNetStepsMailPriority((NetSteps.Data.Entities.Mail.Constants.EmailPriority)((int)obj.MailMessagePriorityID));

				// Hurricane MTA Server proprietary headers for reporting and event tracking
				mail.Headers.Add("X-xsMessageId", group.MailMessageGroupID.ToString(), true);
				if (obj.CampaignActionID.HasValue)
					mail.Headers.Add("X-xsMailingId", obj.CampaignActionID.Value.ToString(), true);

				foreach (QueueMailAttachment attachment in group.Message.Attachments)
				{
                    string uniqueName = attachment.FileName;//string.Format("{0}_{1}", obj.AttachmentUniqueID, attachment.FileName);
					string fullPath = Path.Combine(MailAttachment.UploadFinalFolder, uniqueName);


					if (!NetSteps.Common.IO.FileExists(fullPath))
					{
						EntityExceptionHelper.GetAndLogNetStepsException(new Exception(string.Format("Attachment file not found: {0}", fullPath)), NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsDataException);
						return;
					}

					byte[] attachmentData = File.ReadAllBytes(fullPath);

					mail.Attachments.Add(fullPath, attachment.FileName);
				}

				this.Logger.Debug("mail has {0} recipients", externalRecipients.Count);
				if (externalRecipients.Count > 0)
				{
					string testEmailAddress = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.TestEmailAccount);

					if (!string.IsNullOrEmpty(testEmailAddress))
					{
						this.Logger.Info("overiding external recipients with test email address {0}", testEmailAddress);
						externalRecipients.Clear();

						foreach (string overrideAddress in testEmailAddress.Split(new char[] { ';', ',', '|' }))
						{
							externalRecipients.Add(overrideAddress);
						}
					}

					this.Logger.Debug("attempting to send mail");
					smtp.Send(obj.FromAddress, externalRecipients);

					this.Logger.Debug("successfully sent external email for Group ID {0}", group.MailMessageGroupID);
				}
			}
			catch (Exception ex)
			{
				this.Logger.Error(ex.ToString());
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsDataException);
			}
		}

		private MailBee.Mime.MailPriority GetMailBeePriorityFromNetStepsMailPriority(NetSteps.Data.Entities.Mail.Constants.EmailPriority netStepsPriority)
		{
			switch (netStepsPriority)
			{
				case NetSteps.Data.Entities.Mail.Constants.EmailPriority.Lowest:
					return MailBee.Mime.MailPriority.Lowest;
				case NetSteps.Data.Entities.Mail.Constants.EmailPriority.Low:
					return MailBee.Mime.MailPriority.Low;
				case NetSteps.Data.Entities.Mail.Constants.EmailPriority.Normal:
					return MailBee.Mime.MailPriority.Normal;
				case NetSteps.Data.Entities.Mail.Constants.EmailPriority.High:
					return MailBee.Mime.MailPriority.High;
				case NetSteps.Data.Entities.Mail.Constants.EmailPriority.Highest:
					return MailBee.Mime.MailPriority.Highest;
				default:
					return MailBee.Mime.MailPriority.None;
			}
		}

		private bool IsValidEmailAddress(QueueMessageAddress address, out bool isInternal)
		{
			bool isValidAddress = true;
			isInternal = false;

			// Check sitedomains to see if it is an internal address
			string emailAddress = address.EmailAddress.Trim();
			int idx = emailAddress.LastIndexOf('@');

			// if there is no @ sign in the email address, or if there is nothing after the @ then flag as invalid
			if (idx == -1 || emailAddress.Length == idx)
				isValidAddress = false;
			else
			{
				string ext = emailAddress.Substring(idx + 1);

				// if the extension is in the siteDomain list then we know this is an internal extension
				if (_siteDomains.Contains(ext.ToLower()))
					isInternal = true;

				// if this is an internal extension but we didn't find a matching active email address in the MailAccounts table, then 
				// its invalid
				if (isInternal && address.MailAccountID == -1)
					isValidAddress = false;
			}

			// if we have an invalid address, mark it in the address list and continue to the next address
			if (!isValidAddress)
			{
				IDbCommand dbCommand = null;

				try
				{
					dbCommand = DataAccess.SetCommand("usp_UpdateRecipientStatus", connectionString: NetSteps.Data.Entities.Mail.MailMessage.GetMailConnectionString());
					DataAccess.AddInputParameter("MailMessageGroupAddressID", address.MailMessageGroupAddressID, dbCommand);
					DataAccess.AddInputParameter("RecipientStatusID", (short)NetSteps.Data.Entities.Mail.Constants.EmailRecipientStatus.InvalidAddress, dbCommand);

					DataAccess.ExecuteNonQuery(dbCommand);
				}
				finally
				{
					DataAccess.Close(dbCommand);
				}
			}

			return isValidAddress;
		}
	}
}
