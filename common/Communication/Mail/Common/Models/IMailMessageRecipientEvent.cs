using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MailMessageRecipientEvent.
	/// </summary>
	[ContractClass(typeof(Contracts.MailMessageRecipientEventContracts))]
	public interface IMailMessageRecipientEvent
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailMessageRecipientEventID for this MailMessageRecipientEvent.
		/// </summary>
		int MailMessageRecipientEventID { get; set; }
	
		/// <summary>
		/// The MailMessageGroupAddressID for this MailMessageRecipientEvent.
		/// </summary>
		int MailMessageGroupAddressID { get; set; }
	
		/// <summary>
		/// The MailMessageRecipientEventTypeID for this MailMessageRecipientEvent.
		/// </summary>
		short MailMessageRecipientEventTypeID { get; set; }
	
		/// <summary>
		/// The DateCreatedUTC for this MailMessageRecipientEvent.
		/// </summary>
		System.DateTime DateCreatedUTC { get; set; }
	
		/// <summary>
		/// The Text for this MailMessageRecipientEvent.
		/// </summary>
		string Text { get; set; }
	
		/// <summary>
		/// The Url for this MailMessageRecipientEvent.
		/// </summary>
		string Url { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The MailMessageGroupAddress for this MailMessageRecipientEvent.
		/// </summary>
	    IMailMessageGroupAddress MailMessageGroupAddress { get; set; }
	
		/// <summary>
		/// The MailMessageRecipientEventType for this MailMessageRecipientEvent.
		/// </summary>
	    IMailMessageRecipientEventType MailMessageRecipientEventType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMailMessageRecipientEvent))]
		internal abstract class MailMessageRecipientEventContracts : IMailMessageRecipientEvent
		{
		    #region Primitive properties
		
			int IMailMessageRecipientEvent.MailMessageRecipientEventID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMailMessageRecipientEvent.MailMessageGroupAddressID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IMailMessageRecipientEvent.MailMessageRecipientEventTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IMailMessageRecipientEvent.DateCreatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessageRecipientEvent.Text
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessageRecipientEvent.Url
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IMailMessageGroupAddress IMailMessageRecipientEvent.MailMessageGroupAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IMailMessageRecipientEventType IMailMessageRecipientEvent.MailMessageRecipientEventType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
