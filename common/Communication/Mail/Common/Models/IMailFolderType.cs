using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MailFolderType.
	/// </summary>
	[ContractClass(typeof(Contracts.MailFolderTypeContracts))]
	public interface IMailFolderType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailFolderTypeID for this MailFolderType.
		/// </summary>
		short MailFolderTypeID { get; set; }
	
		/// <summary>
		/// The Description for this MailFolderType.
		/// </summary>
		string Description { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessages for this MailFolderType.
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
		[ContractClassFor(typeof(IMailFolderType))]
		internal abstract class MailFolderTypeContracts : IMailFolderType
		{
		    #region Primitive properties
		
			short IMailFolderType.MailFolderTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailFolderType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessage> IMailFolderType.MailMessages
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMailFolderType.AddMailMessage(IMailMessage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMailFolderType.RemoveMailMessage(IMailMessage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
