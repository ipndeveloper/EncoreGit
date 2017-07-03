using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MailMessageType.
	/// </summary>
	[ContractClass(typeof(Contracts.MailMessageTypeContracts))]
	public interface IMailMessageType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailMessageTypeID for this MailMessageType.
		/// </summary>
		short MailMessageTypeID { get; set; }
	
		/// <summary>
		/// The Description for this MailMessageType.
		/// </summary>
		string Description { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessages for this MailMessageType.
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
		[ContractClassFor(typeof(IMailMessageType))]
		internal abstract class MailMessageTypeContracts : IMailMessageType
		{
		    #region Primitive properties
		
			short IMailMessageType.MailMessageTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessageType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessage> IMailMessageType.MailMessages
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMailMessageType.AddMailMessage(IMailMessage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMailMessageType.RemoveMailMessage(IMailMessage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
