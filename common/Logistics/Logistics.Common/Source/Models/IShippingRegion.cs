using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Logistics.Common.Models
{
	/// <summary>
	/// Common interface for ShippingRegion.
	/// </summary>
	[ContractClass(typeof(Contracts.ShippingRegionContracts))]
	public interface IShippingRegion
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ShippingRegionID for this ShippingRegion.
		/// </summary>
		int ShippingRegionID { get; set; }
	
		/// <summary>
		/// The Name for this ShippingRegion.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this ShippingRegion.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The TermName for this ShippingRegion.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Active for this ShippingRegion.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The Editable for this ShippingRegion.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The WarehouseID for this ShippingRegion.
		/// </summary>
		Nullable<int> WarehouseID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Warehouse for this ShippingRegion.
		/// </summary>
	    IWarehouse Warehouse { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ShippingOrderTypes for this ShippingRegion.
		/// </summary>
		IEnumerable<IShippingOrderType> ShippingOrderTypes { get; }
	
		/// <summary>
		/// Adds an <see cref="IShippingOrderType"/> to the ShippingOrderTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IShippingOrderType"/> to add.</param>
		void AddShippingOrderType(IShippingOrderType item);
	
		/// <summary>
		/// Removes an <see cref="IShippingOrderType"/> from the ShippingOrderTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IShippingOrderType"/> to remove.</param>
		void RemoveShippingOrderType(IShippingOrderType item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IShippingRegion))]
		internal abstract class ShippingRegionContracts : IShippingRegion
		{
		    #region Primitive properties
		
			int IShippingRegion.ShippingRegionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingRegion.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingRegion.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingRegion.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IShippingRegion.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IShippingRegion.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IShippingRegion.WarehouseID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IWarehouse IShippingRegion.Warehouse
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IShippingOrderType> IShippingRegion.ShippingOrderTypes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IShippingRegion.AddShippingOrderType(IShippingOrderType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IShippingRegion.RemoveShippingOrderType(IShippingOrderType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
