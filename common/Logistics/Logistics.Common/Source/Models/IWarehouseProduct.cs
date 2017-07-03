using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Logistics.Common.Models
{
	/// <summary>
	/// Common interface for WarehouseProduct.
	/// </summary>
	[ContractClass(typeof(Contracts.WarehouseProductContracts))]
	public interface IWarehouseProduct
	{
	    #region Primitive properties
	
		/// <summary>
		/// The WarehouseProductID for this WarehouseProduct.
		/// </summary>
		int WarehouseProductID { get; set; }
	
		/// <summary>
		/// The WarehouseID for this WarehouseProduct.
		/// </summary>
		int WarehouseID { get; set; }
	
		/// <summary>
		/// The ProductID for this WarehouseProduct.
		/// </summary>
		int ProductID { get; set; }
	
		/// <summary>
		/// The QuantityOnHand for this WarehouseProduct.
		/// </summary>
		int QuantityOnHand { get; set; }
	
		/// <summary>
		/// The QuantityBuffer for this WarehouseProduct.
		/// </summary>
		int QuantityBuffer { get; set; }
	
		/// <summary>
		/// The IsAvailable for this WarehouseProduct.
		/// </summary>
		bool IsAvailable { get; set; }
	
		/// <summary>
		/// The QuantityAllocated for this WarehouseProduct.
		/// </summary>
		int QuantityAllocated { get; set; }
	
		/// <summary>
		/// The ReorderLevel for this WarehouseProduct.
		/// </summary>
		int ReorderLevel { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IWarehouseProduct))]
		internal abstract class WarehouseProductContracts : IWarehouseProduct
		{
		    #region Primitive properties
		
			int IWarehouseProduct.WarehouseProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IWarehouseProduct.WarehouseID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IWarehouseProduct.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IWarehouseProduct.QuantityOnHand
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IWarehouseProduct.QuantityBuffer
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IWarehouseProduct.IsAvailable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IWarehouseProduct.QuantityAllocated
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IWarehouseProduct.ReorderLevel
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
