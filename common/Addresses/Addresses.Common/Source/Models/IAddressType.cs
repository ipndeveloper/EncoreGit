using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Addresses.Common.Models
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
		/// The Name for this AddressType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this AddressType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this AddressType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this AddressType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Addresses for this AddressType.
		/// </summary>
		IEnumerable<IAddress> Addresses { get; }
	
		/// <summary>
		/// Adds an <see cref="IAddress"/> to the Addresses collection.
		/// </summary>
		/// <param name="item">The <see cref="IAddress"/> to add.</param>
		void AddAddress(IAddress item);
	
		/// <summary>
		/// Removes an <see cref="IAddress"/> from the Addresses collection.
		/// </summary>
		/// <param name="item">The <see cref="IAddress"/> to remove.</param>
		void RemoveAddress(IAddress item);

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
		
			string IAddressType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddressType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddressType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAddressType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAddress> IAddressType.Addresses
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAddressType.AddAddress(IAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAddressType.RemoveAddress(IAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
