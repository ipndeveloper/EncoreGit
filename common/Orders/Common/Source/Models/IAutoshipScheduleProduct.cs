using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for AutoshipScheduleProduct.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoshipScheduleProductContracts))]
	public interface IAutoshipScheduleProduct
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoshipScheduleProductID for this AutoshipScheduleProduct.
		/// </summary>
		int AutoshipScheduleProductID { get; set; }
	
		/// <summary>
		/// The AutoshipScheduleID for this AutoshipScheduleProduct.
		/// </summary>
		int AutoshipScheduleID { get; set; }
	
		/// <summary>
		/// The ProductID for this AutoshipScheduleProduct.
		/// </summary>
		int ProductID { get; set; }
	
		/// <summary>
		/// The Quantity for this AutoshipScheduleProduct.
		/// </summary>
		int Quantity { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoshipScheduleProduct))]
		internal abstract class AutoshipScheduleProductContracts : IAutoshipScheduleProduct
		{
		    #region Primitive properties
		
			int IAutoshipScheduleProduct.AutoshipScheduleProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipScheduleProduct.AutoshipScheduleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipScheduleProduct.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipScheduleProduct.Quantity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
