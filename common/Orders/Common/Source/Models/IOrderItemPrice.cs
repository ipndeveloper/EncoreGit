using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderItemPrice.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderItemPriceContracts))]
	public interface IOrderItemPrice
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderItemPriceID for this OrderItemPrice.
		/// </summary>
		int OrderItemPriceID { get; set; }
	
		/// <summary>
		/// The OrderItemID for this OrderItemPrice.
		/// </summary>
		int OrderItemID { get; set; }
	
		/// <summary>
		/// The OriginalUnitPrice for this OrderItemPrice.
		/// </summary>
		Nullable<decimal> OriginalUnitPrice { get; set; }
	
		/// <summary>
		/// The ProductPriceTypeID for this OrderItemPrice.
		/// </summary>
		int ProductPriceTypeID { get; set; }
	
		/// <summary>
		/// The UnitPrice for this OrderItemPrice.
		/// </summary>
		decimal UnitPrice { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OrderItem for this OrderItemPrice.
		/// </summary>
	    IOrderItem OrderItem { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderItemPrice))]
		internal abstract class OrderItemPriceContracts : IOrderItemPrice
		{
		    #region Primitive properties
		
			int IOrderItemPrice.OrderItemPriceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemPrice.OrderItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderItemPrice.OriginalUnitPrice
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemPrice.ProductPriceTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal IOrderItemPrice.UnitPrice
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrderItem IOrderItemPrice.OrderItem
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
