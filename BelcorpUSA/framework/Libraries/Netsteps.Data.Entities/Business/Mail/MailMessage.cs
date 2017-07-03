using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Mail
{
	public partial class MailMessage : EntityBusinessBase<MailMessage, Int32, IMailMessageRepository, IMailMessageBusinessLogic>
	{
		#region Members
		private ObservableCollection<MailMessageRecipient> _to;
		private ObservableCollection<MailMessageRecipient> _cc;
		private ObservableCollection<MailMessageRecipient> _bcc;

		private ObservableCollection<MailAttachment> _attachments;
		#endregion

		#region Constructors
		public MailMessage()
		{
		}

		public MailMessage(MailAccount mailAccount)
		{
			DefaultFromAddress(mailAccount);
		}
		#endregion

		#region Properties
		protected TimeZone _currentTimeZone = TimeZone.CurrentTimeZone;
		public virtual TimeZone CurrentTimeZone
		{
			get { return _currentTimeZone; }
			set { _currentTimeZone = value; }
		}

		public Nullable<System.DateTime> DateAdded
		{
			get { return DateAddedUTC.UTCToLocal(CurrentTimeZone); }
			set { DateAddedUTC = value.LocalToUTC(CurrentTimeZone).ToDateTime(); }
		}


		public ObservableCollection<MailMessageRecipient> To
		{
			get
			{
				if (_to == null)
					_to = new ObservableCollection<MailMessageRecipient>(); // TODO: Load results from MailMessageGroups ect.. - JHE
				return _to;
			}
			set
			{
				_to = value;
			}
		}

		public ObservableCollection<MailMessageRecipient> Cc
		{
			get
			{
				if (_cc == null)
					_cc = new ObservableCollection<MailMessageRecipient>();
				return _cc;
			}
			set
			{
				_cc = value;
			}
		}

		public ObservableCollection<MailMessageRecipient> Bcc
		{
			get
			{
				if (_bcc == null)
					_bcc = new ObservableCollection<MailMessageRecipient>();
				return _bcc;
			}
			set
			{
				_bcc = value;
			}
		}


		public string FullFolderName { get; set; }

		public bool HasAttachments { get; set; }

		public bool ShouldAssociate { get; set; }

		public List<string> FilterResults { get; set; }

		public ObservableCollection<MailAttachment> Attachments
		{
			get
			{
				return _attachments;
			}
			set
			{
				_attachments = value;
			}
		}

		/// <summary>
		/// Returns in the format of example: 'Bill Smith <billSmith@hotmail.com>' or 'billSmith@hotmail.com' (if Name is not set)
		/// </summary>
		public string FromAddressFormated
		{
			get
			{
				return MailMessageRecipient.GetAddressFormated(FromNickName, FromAddress);
			}
		}
		#endregion

		#region IAssociable Members

		//public string AssociationColumnName
		//{
		//    //This must match the appropriate Column Name in the Associations Table
		//    get { return "MailMessageID"; }
		//}

		//public int AssociationID
		//{
		//    get { return this.MailMessageID; }
		//}

		#endregion

		#region Methods
		public void EnsureDateIsSet()
		{
			if (this.DateAdded == null || this.DateAdded == DateTime.MinValue)
				this.DateAdded = DateTime.Now;
		}

		public int SaveAsDraft(MailAccount mailAccount)
		{
			try
			{
				DefaultFromAddress(mailAccount);
				return BusinessLogic.SaveAsDraft(GetRepository(), this, mailAccount);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public int Send(MailAccount mailAccount, int siteID)
		{
			try
			{
				this.DateAdded = DateTime.Now;
				DefaultFromAddress(mailAccount);
				Repository.Send(this, mailAccount, siteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
			return this.MailMessageID;
		}

		public override void Delete()
		{
			try
			{
				BusinessLogic.Delete(Repository, this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public bool Move(short destinationMailFolderTypeID)
		{
			try
			{
				EnsureDateIsSet();
				return Repository.Move(this, destinationMailFolderTypeID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static bool Move(int mailMessageID, short destinationMailFolderTypeID)
		{
			try
			{
				return Repository.Move(mailMessageID, destinationMailFolderTypeID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void MarkAsRead(int mailMessageID)
		{
			try
			{
				Repository.MarkAsRead(mailMessageID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static MailAttachment LoadAttachment(MailMessage mailMessage, string name, MailAccount mailAccount)
		{
			try
			{
				return Repository.RetrieveMailAttachment(mailMessage, name, mailAccount);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<MailMessage> LoadCollection(NetSteps.Data.Entities.Mail.Constants.MailFolderType mailFolderType, MailAccount mailAccount)
		{
			return Repository.LoadCollection(mailFolderType, mailAccount);
		}

		protected void DefaultFromAddress(MailAccount mailAccount)
		{
			if (this.FromAddress.IsNullOrEmpty() && !mailAccount.EmailAddress.IsNullOrEmpty())
				this.FromAddress = mailAccount.EmailAddress;

			if (this.FromNickName.IsNullOrEmpty() && mailAccount.Account != null && !mailAccount.Account.FullName.IsNullOrEmpty())
				this.FromNickName = mailAccount.Account.FullName;
		}

		/// <summary>
		/// Use to reply to the current MailMessage.
		/// This will return a new MailMessage with the appropriate defaulted values from the current
		/// MailMessage and mailAccount passed in as a paramater. - JHE
		/// </summary>
		/// <param name="mailAccount"></param>
		/// <returns></returns>
		public MailMessage Reply(MailAccount mailAccount)
		{
			MailMessage newMailMessage = new MailMessage();
			newMailMessage.FromAddress = mailAccount.EmailAddress;
			newMailMessage.FromNickName = mailAccount.Account.FullName;
			newMailMessage.To.Add(new MailMessageRecipient() { Name = this.FromNickName, Email = this.FromAddress });
			newMailMessage.Subject = string.Format("RE: {0}", this.Subject);
			if (!this.HTMLBody.IsNullOrEmpty())
				newMailMessage.HTMLBody = string.Format("{0}{1}{0}{2}",
											"<br/>",
											"---------------------------------------------",
											this.HTMLBody);
			if (!this.Body.IsNullOrEmpty())
				newMailMessage.Body = string.Format("{0}{0}{1}{0}{2}",
											Environment.NewLine,
											"---------------------------------------------",
											this.Body);

			return newMailMessage;
		}

		/// <summary>
		/// Use to Reply To All to the current MailMessage.
		/// This will return a new MailMessage with the appropriate defaulted values from the current
		/// MailMessage and mailAccount passed in as a paramater. - JHE
		/// </summary>
		/// <param name="mailAccount"></param>
		/// <returns></returns>
		public MailMessage ReplyToAll(MailAccount mailAccount)
		{
			MailMessage newMailMessage = new MailMessage();
			newMailMessage.FromAddress = mailAccount.EmailAddress;
			newMailMessage.FromNickName = mailAccount.Account.FullName;
			newMailMessage.To.Add(new MailMessageRecipient() { Name = this.FromNickName, Email = this.FromAddress });
			foreach (MailMessageRecipient mailMessageRecipient in this.To)
			{
				if (mailMessageRecipient.Email.ToLower() != mailAccount.EmailAddress.ToLower())
					newMailMessage.To.Add(new MailMessageRecipient() { Email = mailMessageRecipient.Email, Name = mailMessageRecipient.Name });
			}

			if (this.Cc != null)
			{
				foreach (MailMessageRecipient mailMessageRecipient in this.Cc)
					if (mailMessageRecipient.Email.ToLower() != mailAccount.EmailAddress.ToLower())
						newMailMessage.Cc.Add(new MailMessageRecipient() { Email = mailMessageRecipient.Email, Name = mailMessageRecipient.Name });
			}

			newMailMessage.Subject = string.Format("RE: {0}", this.Subject);

			if (!this.HTMLBody.IsNullOrEmpty())
				newMailMessage.HTMLBody = string.Format("{0}{1}{0}{2}",
											"<br/>",
											"---------------------------------------------",
											this.HTMLBody);

			if (!this.Body.IsNullOrEmpty())
				newMailMessage.Body = string.Format("{0}{0}{1}{0}{2}",
										Environment.NewLine,
										"---------------------------------------------",
										this.Body);

			return newMailMessage;
		}

		/// <summary>
		/// Use to Forward to the current MailMessage.
		/// This will return a new MailMessage with the appropriate defaulted values from the current
		/// MailMessage and mailAccount passed in as a paramater. - JHE
		/// </summary>
		/// <param name="mailAccount"></param>
		/// <returns></returns>
		public MailMessage Forward(MailAccount mailAccount)
		{
			MailMessage newMailMessage = new MailMessage();
			newMailMessage.FromAddress = mailAccount.EmailAddress;
			newMailMessage.FromNickName = mailAccount.Account.FullName;
			newMailMessage.Subject = string.Format("FW: {0}", this.Subject);
			newMailMessage.HTMLBody = this.HTMLBody;
			newMailMessage.Body = this.Body;

			// Any attachments to mailMessageModel should be attached also to newMailMessage
			newMailMessage.AttachmentUniqueID = this.AttachmentUniqueID;
			newMailMessage.Attachments = this.Attachments;

			return newMailMessage;
		}

		/// <summary>
		/// Removes all the items from a folder. Deletes from child tables first
		/// </summary>
		/// <param name="mailFolderType"></param>
		/// <param name="mailAccount"></param>
		/// <returns></returns>
		public static bool PurgeFolder(NetSteps.Data.Entities.Mail.Constants.MailFolderType mailFolderType, MailAccount mailAccount)
		{
			try
			{
				return Repository.PurgeFolder(mailFolderType, mailAccount);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void DeleteBatch(IEnumerable<int> mailMessageIDs)
		{
			try
			{
				Repository.DeleteBatch(mailMessageIDs);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		public static void MarkAsUnReadBatch(List<int> mailMessageIDs)
		{
			try
			{
				Repository.MarkAsUnReadBatch(mailMessageIDs);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		public static void MarkAsReadBatch(List<int> mailMessageIDs)
		{
			try
			{
				Repository.MarkAsReadBatch(mailMessageIDs);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static PaginatedList<MailMessageSearchData> Search(MailMessageSearchParameters searchParameters)
		{
			try
			{
				return Repository.Search(searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<MailMessage> LoadAll()
		{
			return BusinessLogic.LoadAll(Repository);
		}

		public static string GetMailConnectionString()
		{
			return MailMessageRepository.GetMailConnectionString();
		}

		public static string ApplyEventTrackingToInternalMessage(string htmlBody, int? campaignActionID, int mailMessageGroupID, string recipientEmailAddress)
		{
			return BusinessLogic.ApplyEventTrackingToInternalMessage(htmlBody, campaignActionID, mailMessageGroupID, recipientEmailAddress);
		}

		public static string ApplyEventTrackingToExternalMessage(string htmlBody)
		{
			return BusinessLogic.ApplyEventTrackingToExternalMessage(htmlBody);
		}

		public static bool SetReplyToEmailAddress()
		{
			return BusinessLogic.SetReplyToEmailAddress();
		}
		#endregion
	}
}
