using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for AddressType.
	/// </summary>
	[ContractClass(typeof(Contracts.AddressTypeContracts))]
	public interface IAddressType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AddressTypeID for this AddressType.
		/// </summary>
		short AddressTypeID { get; set; }
	
		/// <summary>
		/// The Description for this AddressType.
		/// </summary>
		string Description { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessageGroupAddresses for this AddressType.
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
		[ContractClassFor(typeof(IAddressType))]
		internal abstract class AddressTypeContracts : IAddressType
		{
		    #region Primitive properties
		
			short IAddressType.AddressTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddressType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessageGroupAddress> IAddressType.MailMessageGroupAddresses
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAddressType.AddMailMessageGroupAddress(IMailMessageGroupAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAddressType.RemoveMailMessageGroupAddress(IMailMessageGroupAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
