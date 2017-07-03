using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for RecipientType.
	/// </summary>
	[ContractClass(typeof(Contracts.RecipientTypeContracts))]
	public interface IRecipientType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The RecipientTypeID for this RecipientType.
		/// </summary>
		short RecipientTypeID { get; set; }
	
		/// <summary>
		/// The Description for this RecipientType.
		/// </summary>
		string Description { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessageGroupAddresses for this RecipientType.
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
		[ContractClassFor(typeof(IRecipientType))]
		internal abstract class RecipientTypeContracts : IRecipientType
		{
		    #region Primitive properties
		
			short IRecipientType.RecipientTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRecipientType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessageGroupAddress> IRecipientType.MailMessageGroupAddresses
			{
				get { throw new NotImplementedException(); }
			}
		
			void IRecipientType.AddMailMessageGroupAddress(IMailMessageGroupAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IRecipientType.RemoveMailMessageGroupAddress(IMailMessageGroupAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
