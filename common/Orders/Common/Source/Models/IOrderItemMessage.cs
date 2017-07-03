using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderItemMessage.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderItemMessageContracts))]
	public interface IOrderItemMessage
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderItemMessageID for this OrderItemMessage.
		/// </summary>
		int OrderItemMessageID { get; set; }
	
		/// <summary>
		/// The OrderItemID for this OrderItemMessage.
		/// </summary>
		int OrderItemID { get; set; }
	
		/// <summary>
		/// The OrderItemMessageDisplayKindID for this OrderItemMessage.
		/// </summary>
		int OrderItemMessageDisplayKindID { get; set; }
	
		/// <summary>
		/// The MessageSourceKey for this OrderItemMessage.
		/// </summary>
		string MessageSourceKey { get; set; }
	
		/// <summary>
		/// The Message for this OrderItemMessage.
		/// </summary>
		string Message { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OrderItem for this OrderItemMessage.
		/// </summary>
	    IOrderItem OrderItem { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderItemMessage))]
		internal abstract class OrderItemMessageContracts : IOrderItemMessage
		{
		    #region Primitive properties
		
			int IOrderItemMessage.OrderItemMessageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemMessage.OrderItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemMessage.OrderItemMessageDisplayKindID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemMessage.MessageSourceKey
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemMessage.Message
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrderItem IOrderItemMessage.OrderItem
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
