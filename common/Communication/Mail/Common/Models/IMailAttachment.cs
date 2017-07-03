using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MailAttachment.
	/// </summary>
	[ContractClass(typeof(Contracts.MailAttachmentContracts))]
	public interface IMailAttachment
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailAttachmentID for this MailAttachment.
		/// </summary>
		int MailAttachmentID { get; set; }
	
		/// <summary>
		/// The FileName for this MailAttachment.
		/// </summary>
		string FileName { get; set; }
	
		/// <summary>
		/// The Size for this MailAttachment.
		/// </summary>
		Nullable<int> Size { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessageAttachments for this MailAttachment.
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

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMailAttachment))]
		internal abstract class MailAttachmentContracts : IMailAttachment
		{
		    #region Primitive properties
		
			int IMailAttachment.MailAttachmentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailAttachment.FileName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IMailAttachment.Size
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessageAttachment> IMailAttachment.MailMessageAttachments
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMailAttachment.AddMailMessageAttachment(IMailMessageAttachment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMailAttachment.RemoveMailMessageAttachment(IMailMessageAttachment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
