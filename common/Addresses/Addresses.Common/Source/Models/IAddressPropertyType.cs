using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Addresses.Common.Models
{
	/// <summary>
	/// Common interface for AddressPropertyType.
	/// </summary>
	[ContractClass(typeof(Contracts.AddressPropertyTypeContracts))]
	public interface IAddressPropertyType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AddressPropertyTypeID for this AddressPropertyType.
		/// </summary>
		int AddressPropertyTypeID { get; set; }
	
		/// <summary>
		/// The Name for this AddressPropertyType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this AddressPropertyType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this AddressPropertyType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this AddressPropertyType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AddressProperties for this AddressPropertyType.
		/// </summary>
		IEnumerable<IAddressProperty> AddressProperties { get; }
	
		/// <summary>
		/// Adds an <see cref="IAddressProperty"/> to the AddressProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IAddressProperty"/> to add.</param>
		void AddAddressProperty(IAddressProperty item);
	
		/// <summary>
		/// Removes an <see cref="IAddressProperty"/> from the AddressProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IAddressProperty"/> to remove.</param>
		void RemoveAddressProperty(IAddressProperty item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAddressPropertyType))]
		internal abstract class AddressPropertyTypeContracts : IAddressPropertyType
		{
		    #region Primitive properties
		
			int IAddressPropertyType.AddressPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddressPropertyType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddressPropertyType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddressPropertyType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAddressPropertyType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAddressProperty> IAddressPropertyType.AddressProperties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAddressPropertyType.AddAddressProperty(IAddressProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAddressPropertyType.RemoveAddressProperty(IAddressProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
