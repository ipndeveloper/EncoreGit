using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Logistics.Common.Models
{
	/// <summary>
	/// Common interface for Warehouse.
	/// </summary>
	[ContractClass(typeof(Contracts.WarehouseContracts))]
	public interface IWarehouse
	{
	    #region Primitive properties
	
		/// <summary>
		/// The WarehouseID for this Warehouse.
		/// </summary>
		int WarehouseID { get; set; }
	
		/// <summary>
		/// The AddressID for this Warehouse.
		/// </summary>
		Nullable<int> AddressID { get; set; }
	
		/// <summary>
		/// The Name for this Warehouse.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this Warehouse.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this Warehouse.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this Warehouse.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Address for this Warehouse.
		/// </summary>
	    IAddress Address { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ShippingRegions for this Warehouse.
		/// </summary>
		IEnumerable<IShippingRegion> ShippingRegions { get; }
	
		/// <summary>
		/// Adds an <see cref="IShippingRegion"/> to the ShippingRegions collection.
		/// </summary>
		/// <param name="item">The <see cref="IShippingRegion"/> to add.</param>
		void AddShippingRegion(IShippingRegion item);
	
		/// <summary>
		/// Removes an <see cref="IShippingRegion"/> from the ShippingRegions collection.
		/// </summary>
		/// <param name="item">The <see cref="IShippingRegion"/> to remove.</param>
		void RemoveShippingRegion(IShippingRegion item);
	
		/// <summary>
		/// The WarehouseProducts for this Warehouse.
		/// </summary>
		IEnumerable<IWarehouseProduct> WarehouseProducts { get; }
	
		/// <summary>
		/// Adds an <see cref="IWarehouseProduct"/> to the WarehouseProducts collection.
		/// </summary>
		/// <param name="item">The <see cref="IWarehouseProduct"/> to add.</param>
		void AddWarehouseProduct(IWarehouseProduct item);
	
		/// <summary>
		/// Removes an <see cref="IWarehouseProduct"/> from the WarehouseProducts collection.
		/// </summary>
		/// <param name="item">The <see cref="IWarehouseProduct"/> to remove.</param>
		void RemoveWarehouseProduct(IWarehouseProduct item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IWarehouse))]
		internal abstract class WarehouseContracts : IWarehouse
		{
		    #region Primitive properties
		
			int IWarehouse.WarehouseID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IWarehouse.AddressID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IWarehouse.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IWarehouse.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IWarehouse.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IWarehouse.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAddress IWarehouse.Address
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IShippingRegion> IWarehouse.ShippingRegions
			{
				get { throw new NotImplementedException(); }
			}
		
			void IWarehouse.AddShippingRegion(IShippingRegion item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IWarehouse.RemoveShippingRegion(IShippingRegion item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IWarehouseProduct> IWarehouse.WarehouseProducts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IWarehouse.AddWarehouseProduct(IWarehouseProduct item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IWarehouse.RemoveWarehouseProduct(IWarehouseProduct item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
