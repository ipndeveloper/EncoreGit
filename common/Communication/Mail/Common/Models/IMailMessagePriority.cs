using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MailMessagePriority.
	/// </summary>
	[ContractClass(typeof(Contracts.MailMessagePriorityContracts))]
	public interface IMailMessagePriority
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailMessagePriorityID for this MailMessagePriority.
		/// </summary>
		short MailMessagePriorityID { get; set; }
	
		/// <summary>
		/// The Description for this MailMessagePriority.
		/// </summary>
		string Description { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessages for this MailMessagePriority.
		/// </summary>
		IEnumerable<IMailMessage> MailMessages { get; }
	
		/// <summary>
		/// Adds an <see cref="IMailMessage"/> to the MailMessages collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessage"/> to add.</param>
		void AddMailMessage(IMailMessage item);
	
		/// <summary>
		/// Removes an <see cref="IMailMessage"/> from the MailMessages collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessage"/> to remove.</param>
		void RemoveMailMessage(IMailMessage item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMailMessagePriority))]
		internal abstract class MailMessagePriorityContracts : IMailMessagePriority
		{
		    #region Primitive properties
		
			short IMailMessagePriority.MailMessagePriorityID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessagePriority.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessage> IMailMessagePriority.MailMessages
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMailMessagePriority.AddMailMessage(IMailMessage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMailMessagePriority.RemoveMailMessage(IMailMessage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
