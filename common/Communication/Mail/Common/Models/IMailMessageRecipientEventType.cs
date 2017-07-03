using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MailMessageRecipientEventType.
	/// </summary>
	[ContractClass(typeof(Contracts.MailMessageRecipientEventTypeContracts))]
	public interface IMailMessageRecipientEventType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailMessageRecipientEventTypeID for this MailMessageRecipientEventType.
		/// </summary>
		short MailMessageRecipientEventTypeID { get; set; }
	
		/// <summary>
		/// The Name for this MailMessageRecipientEventType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this MailMessageRecipientEventType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this MailMessageRecipientEventType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this MailMessageRecipientEventType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessageRecipientEvents for this MailMessageRecipientEventType.
		/// </summary>
		IEnumerable<IMailMessageRecipientEvent> MailMessageRecipientEvents { get; }
	
		/// <summary>
		/// Adds an <see cref="IMailMessageRecipientEvent"/> to the MailMessageRecipientEvents collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageRecipientEvent"/> to add.</param>
		void AddMailMessageRecipientEvent(IMailMessageRecipientEvent item);
	
		/// <summary>
		/// Removes an <see cref="IMailMessageRecipientEvent"/> from the MailMessageRecipientEvents collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageRecipientEvent"/> to remove.</param>
		void RemoveMailMessageRecipientEvent(IMailMessageRecipientEvent item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMailMessageRecipientEventType))]
		internal abstract class MailMessageRecipientEventTypeContracts : IMailMessageRecipientEventType
		{
		    #region Primitive properties
		
			short IMailMessageRecipientEventType.MailMessageRecipientEventTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessageRecipientEventType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessageRecipientEventType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessageRecipientEventType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IMailMessageRecipientEventType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessageRecipientEvent> IMailMessageRecipientEventType.MailMessageRecipientEvents
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMailMessageRecipientEventType.AddMailMessageRecipientEvent(IMailMessageRecipientEvent item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMailMessageRecipientEventType.RemoveMailMessageRecipientEvent(IMailMessageRecipientEvent item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
