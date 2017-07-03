using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MailMessage.
	/// </summary>
	[ContractClass(typeof(Contracts.MailMessageContracts))]
	public interface IMailMessage
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailMessageID for this MailMessage.
		/// </summary>
		int MailMessageID { get; set; }
	
		/// <summary>
		/// The Subject for this MailMessage.
		/// </summary>
		string Subject { get; set; }
	
		/// <summary>
		/// The Body for this MailMessage.
		/// </summary>
		string Body { get; set; }
	
		/// <summary>
		/// The HTMLBody for this MailMessage.
		/// </summary>
		string HTMLBody { get; set; }
	
		/// <summary>
		/// The FromAddress for this MailMessage.
		/// </summary>
		string FromAddress { get; set; }
	
		/// <summary>
		/// The FromNickName for this MailMessage.
		/// </summary>
		string FromNickName { get; set; }
	
		/// <summary>
		/// The MailAccountID for this MailMessage.
		/// </summary>
		Nullable<int> MailAccountID { get; set; }
	
		/// <summary>
		/// The IsOutbound for this MailMessage.
		/// </summary>
		bool IsOutbound { get; set; }
	
		/// <summary>
		/// The MailMessageTypeID for this MailMessage.
		/// </summary>
		short MailMessageTypeID { get; set; }
	
		/// <summary>
		/// The BeenRead for this MailMessage.
		/// </summary>
		bool BeenRead { get; set; }
	
		/// <summary>
		/// The MailMessagePriorityID for this MailMessage.
		/// </summary>
		short MailMessagePriorityID { get; set; }
	
		/// <summary>
		/// The VisualTemplateID for this MailMessage.
		/// </summary>
		Nullable<int> VisualTemplateID { get; set; }
	
		/// <summary>
		/// The MailFolderTypeID for this MailMessage.
		/// </summary>
		short MailFolderTypeID { get; set; }
	
		/// <summary>
		/// The AttachmentUniqueID for this MailMessage.
		/// </summary>
		string AttachmentUniqueID { get; set; }
	
		/// <summary>
		/// The Locked for this MailMessage.
		/// </summary>
		Nullable<bool> Locked { get; set; }
	
		/// <summary>
		/// The SiteID for this MailMessage.
		/// </summary>
		Nullable<int> SiteID { get; set; }
	
		/// <summary>
		/// The DateAddedUTC for this MailMessage.
		/// </summary>
		System.DateTime DateAddedUTC { get; set; }
	
		/// <summary>
		/// The CampaignActionID for this MailMessage.
		/// </summary>
		Nullable<int> CampaignActionID { get; set; }
	
		/// <summary>
		/// The EnableEventTracking for this MailMessage.
		/// </summary>
		bool EnableEventTracking { get; set; }
	
		/// <summary>
		/// The ReplyToAddress for this MailMessage.
		/// </summary>
		string ReplyToAddress { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The MailFolderType for this MailMessage.
		/// </summary>
	    IMailFolderType MailFolderType { get; set; }
	
		/// <summary>
		/// The MailMessagePriority for this MailMessage.
		/// </summary>
	    IMailMessagePriority MailMessagePriority { get; set; }
	
		/// <summary>
		/// The MailMessageType for this MailMessage.
		/// </summary>
	    IMailMessageType MailMessageType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessageAttachments for this MailMessage.
		/// </summary>
		IEnumerable<IMailMessageAttachment> MailMessageAttachments { get; }
	
		/// <summary>
		/// Adds an <see cref="IMailMessageAttachment"/> to the MailMessageAttachments collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageAttachment"/> to add.</param>
		void AddMailMessageAttachment(IMailMessageAttachment item);
	
		/// <summary>
		/// Removes an <see cref="IMailMessageAttachment"/> from the MailMessageAttachments collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageAttachment"/> to remove.</param>
		void RemoveMailMessageAttachment(IMailMessageAttachment item);
	
		/// <summary>
		/// The MailMessageGroups for this MailMessage.
		/// </summary>
		IEnumerable<IMailMessageGroup> MailMessageGroups { get; }
	
		/// <summary>
		/// Adds an <see cref="IMailMessageGroup"/> to the MailMessageGroups collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageGroup"/> to add.</param>
		void AddMailMessageGroup(IMailMessageGroup item);
	
		/// <summary>
		/// Removes an <see cref="IMailMessageGroup"/> from the MailMessageGroups collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageGroup"/> to remove.</param>
		void RemoveMailMessageGroup(IMailMessageGroup item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMailMessage))]
		internal abstract class MailMessageContracts : IMailMessage
		{
		    #region Primitive properties
		
			int IMailMessage.MailMessageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessage.Subject
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessage.Body
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessage.HTMLBody
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessage.FromAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessage.FromNickName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IMailMessage.MailAccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IMailMessage.IsOutbound
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IMailMessage.MailMessageTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IMailMessage.BeenRead
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IMailMessage.MailMessagePriorityID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IMailMessage.VisualTemplateID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IMailMessage.MailFolderTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessage.AttachmentUniqueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IMailMessage.Locked
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IMailMessage.SiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IMailMessage.DateAddedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IMailMessage.CampaignActionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IMailMessage.EnableEventTracking
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessage.ReplyToAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IMailFolderType IMailMessage.MailFolderType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IMailMessagePriority IMailMessage.MailMessagePriority
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IMailMessageType IMailMessage.MailMessageType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessageAttachment> IMailMessage.MailMessageAttachments
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMailMessage.AddMailMessageAttachment(IMailMessageAttachment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMailMessage.RemoveMailMessageAttachment(IMailMessageAttachment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IMailMessageGroup> IMailMessage.MailMessageGroups
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMailMessage.AddMailMessageGroup(IMailMessageGroup item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMailMessage.RemoveMailMessageGroup(IMailMessageGroup item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
