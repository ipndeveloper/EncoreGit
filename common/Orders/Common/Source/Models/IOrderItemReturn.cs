using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderItemReturn.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderItemReturnContracts))]
	public interface IOrderItemReturn
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderItemReturnID for this OrderItemReturn.
		/// </summary>
		int OrderItemReturnID { get; set; }
	
		/// <summary>
		/// The OrderItemID for this OrderItemReturn.
		/// </summary>
		int OrderItemID { get; set; }
	
		/// <summary>
		/// The IsDestroyed for this OrderItemReturn.
		/// </summary>
		bool IsDestroyed { get; set; }
	
		/// <summary>
		/// The IsRestocked for this OrderItemReturn.
		/// </summary>
		bool IsRestocked { get; set; }
	
		/// <summary>
		/// The HasBeenReceived for this OrderItemReturn.
		/// </summary>
		bool HasBeenReceived { get; set; }
	
		/// <summary>
		/// The ReturnReasonID for this OrderItemReturn.
		/// </summary>
		int ReturnReasonID { get; set; }
	
		/// <summary>
		/// The Quantity for this OrderItemReturn.
		/// </summary>
		int Quantity { get; set; }
	
		/// <summary>
		/// The OriginalOrderItemID for this OrderItemReturn.
		/// </summary>
		Nullable<int> OriginalOrderItemID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OrderItem for this OrderItemReturn.
		/// </summary>
	    IOrderItem OrderItem { get; set; }
	
		/// <summary>
		/// The OriginalOrderItem for this OrderItemReturn.
		/// </summary>
	    IOrderItem OriginalOrderItem { get; set; }
	
		/// <summary>
		/// The ReturnReason for this OrderItemReturn.
		/// </summary>
	    IReturnReason ReturnReason { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderItemReturn))]
		internal abstract class OrderItemReturnContracts : IOrderItemReturn
		{
		    #region Primitive properties
		
			int IOrderItemReturn.OrderItemReturnID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemReturn.OrderItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItemReturn.IsDestroyed
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItemReturn.IsRestocked
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItemReturn.HasBeenReceived
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemReturn.ReturnReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemReturn.Quantity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderItemReturn.OriginalOrderItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrderItem IOrderItemReturn.OrderItem
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderItem IOrderItemReturn.OriginalOrderItem
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IReturnReason IOrderItemReturn.ReturnReason
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
