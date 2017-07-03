using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MailMessageAttachment.
	/// </summary>
	[ContractClass(typeof(Contracts.MailMessageAttachmentContracts))]
	public interface IMailMessageAttachment
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailMessageAttachmentID for this MailMessageAttachment.
		/// </summary>
		int MailMessageAttachmentID { get; set; }
	
		/// <summary>
		/// The MailMessageID for this MailMessageAttachment.
		/// </summary>
		int MailMessageID { get; set; }
	
		/// <summary>
		/// The MailAttachmentID for this MailMessageAttachment.
		/// </summary>
		int MailAttachmentID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The MailAttachment for this MailMessageAttachment.
		/// </summary>
	    IMailAttachment MailAttachment { get; set; }
	
		/// <summary>
		/// The MailMessage for this MailMessageAttachment.
		/// </summary>
	    IMailMessage MailMessage { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMailMessageAttachment))]
		internal abstract class MailMessageAttachmentContracts : IMailMessageAttachment
		{
		    #region Primitive properties
		
			int IMailMessageAttachment.MailMessageAttachmentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMailMessageAttachment.MailMessageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMailMessageAttachment.MailAttachmentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IMailAttachment IMailMessageAttachment.MailAttachment
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IMailMessage IMailMessageAttachment.MailMessage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
