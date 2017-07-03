using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for RecipientStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.RecipientStatusContracts))]
	public interface IRecipientStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The RecipientStatusID for this RecipientStatus.
		/// </summary>
		short RecipientStatusID { get; set; }
	
		/// <summary>
		/// The Description for this RecipientStatus.
		/// </summary>
		string Description { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessageGroupAddresses for this RecipientStatus.
		/// </summary>
		IEnumerable<IMailMessageGroupAddress> MailMessageGroupAddresses { get; }
	
		/// <summary>
		/// Adds an <see cref="IMailMessageGroupAddress"/> to the MailMessageGroupAddresses collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageGroupAddress"/> to add.</param>
		void AddMailMessageGroupAddress(IMailMessageGroupAddress item);
	
		/// <summary>
		/// Removes an <see cref="IMailMessageGroupAddress"/> from the MailMessageGroupAddresses collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageGroupAddress"/> to remove.</param>
		void RemoveMailMessageGroupAddress(IMailMessageGroupAddress item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IRecipientStatus))]
		internal abstract class RecipientStatusContracts : IRecipientStatus
		{
		    #region Primitive properties
		
			short IRecipientStatus.RecipientStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRecipientStatus.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessageGroupAddress> IRecipientStatus.MailMessageGroupAddresses
			{
				get { throw new NotImplementedException(); }
			}
		
			void IRecipientStatus.AddMailMessageGroupAddress(IMailMessageGroupAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IRecipientStatus.RemoveMailMessageGroupAddress(IMailMessageGroupAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
