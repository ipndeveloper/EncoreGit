using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Addresses.Common.Models
{
	/// <summary>
	/// Common interface for AddressProperty.
	/// </summary>
	[ContractClass(typeof(Contracts.AddressPropertyContracts))]
	public interface IAddressProperty
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AddressPropertyID for this AddressProperty.
		/// </summary>
		int AddressPropertyID { get; set; }
	
		/// <summary>
		/// The AddressID for this AddressProperty.
		/// </summary>
		int AddressID { get; set; }
	
		/// <summary>
		/// The AddressPropertyTypeID for this AddressProperty.
		/// </summary>
		int AddressPropertyTypeID { get; set; }
	
		/// <summary>
		/// The PropertyValue for this AddressProperty.
		/// </summary>
		string PropertyValue { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Address for this AddressProperty.
		/// </summary>
	    IAddress Address { get; set; }
	
		/// <summary>
		/// The AddressPropertyType for this AddressProperty.
		/// </summary>
	    IAddressPropertyType AddressPropertyType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAddressProperty))]
		internal abstract class AddressPropertyContracts : IAddressProperty
		{
		    #region Primitive properties
		
			int IAddressProperty.AddressPropertyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAddressProperty.AddressID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAddressProperty.AddressPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddressProperty.PropertyValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAddress IAddressProperty.Address
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAddressPropertyType IAddressProperty.AddressPropertyType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
