
namespace NetSteps.Data.Entities.Mail
{
	public class Constants
	{
		// TODO: Generate these enums from the DB - JHE
		#region Enums
		public enum EmailPriority
		{
			Lowest = 0,
			Low = 1,
			Normal = 2,
			High = 3,
			Highest = 4
		}

		public enum MailMessageRecipientType
		{
			Individual = 0,
			Group = 1
		}

		public enum MailFolderType
		{
			Inbox = 0,
			Trash = 1,
			SentItems = 2,
			Drafts = 3,
			Outbox = 4,
			Undeliverable = 5
		}
		public enum EmailAddressType
		{
			TO = 0,
			CC = 1,
			BCC = 2
		}

		public enum EmailMessageType
		{
			Campaign = 0,
			AdHoc = 1,
			Downline = 2
		}

		public enum EmailRecipientStatus
		{
			Unknown = 0,
			OptedOut = 1,
			Delivered = 2,
			DeliveryError = 3,
			InvalidAddress = 4
		}

		public enum MessageGroupStatusType
		{
			ToBeSent = 0,
			IntermediateQueue = 1,
			SMTPRelayQueue = 2,
			SMTPRelayRefused = 3,
			SMTPSent = 4,
			InboundReceived = 5
		}

        public enum MailMessageRecipientEventType
        {
            MessageOpened = 0,
            LinkClicked = 1,
            MessageBounced = 2
        }
	}
		#endregion
}
