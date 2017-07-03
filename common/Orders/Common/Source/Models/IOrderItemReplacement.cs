using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderItemReplacement.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderItemReplacementContracts))]
	public interface IOrderItemReplacement
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderItemID for this OrderItemReplacement.
		/// </summary>
		int OrderItemID { get; set; }
	
		/// <summary>
		/// The ReplacementReasonID for this OrderItemReplacement.
		/// </summary>
		int ReplacementReasonID { get; set; }
	
		/// <summary>
		/// The Notes for this OrderItemReplacement.
		/// </summary>
		string Notes { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OrderItem for this OrderItemReplacement.
		/// </summary>
	    IOrderItem OrderItem { get; set; }
	
		/// <summary>
		/// The ReplacementReason for this OrderItemReplacement.
		/// </summary>
	    IReplacementReason ReplacementReason { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderItemReplacement))]
		internal abstract class OrderItemReplacementContracts : IOrderItemReplacement
		{
		    #region Primitive properties
		
			int IOrderItemReplacement.OrderItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemReplacement.ReplacementReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemReplacement.Notes
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrderItem IOrderItemReplacement.OrderItem
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IReplacementReason IOrderItemReplacement.ReplacementReason
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
