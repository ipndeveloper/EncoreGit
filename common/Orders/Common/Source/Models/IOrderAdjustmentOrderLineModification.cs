using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderAdjustmentOrderLineModification.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderAdjustmentOrderLineModificationContracts))]
	public interface IOrderAdjustmentOrderLineModification
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderAdjustmentOrderLineModificationID for this OrderAdjustmentOrderLineModification.
		/// </summary>
		int OrderAdjustmentOrderLineModificationID { get; set; }
	
		/// <summary>
		/// The OrderItemID for this OrderAdjustmentOrderLineModification.
		/// </summary>
		int OrderItemID { get; set; }
	
		/// <summary>
		/// The OrderAdjustmentID for this OrderAdjustmentOrderLineModification.
		/// </summary>
		int OrderAdjustmentID { get; set; }
	
		/// <summary>
		/// The PropertyName for this OrderAdjustmentOrderLineModification.
		/// </summary>
		string PropertyName { get; set; }
	
		/// <summary>
		/// The ModificationOperationID for this OrderAdjustmentOrderLineModification.
		/// </summary>
		int ModificationOperationID { get; set; }
	
		/// <summary>
		/// The ModificationDecimalValue for this OrderAdjustmentOrderLineModification.
		/// </summary>
		Nullable<decimal> ModificationDecimalValue { get; set; }
	
		/// <summary>
		/// The ModificationDescription for this OrderAdjustmentOrderLineModification.
		/// </summary>
		string ModificationDescription { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OrderAdjustment for this OrderAdjustmentOrderLineModification.
		/// </summary>
	    IOrderAdjustment OrderAdjustment { get; set; }
	
		/// <summary>
		/// The OrderItem for this OrderAdjustmentOrderLineModification.
		/// </summary>
	    IOrderItem OrderItem { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderAdjustmentOrderLineModification))]
		internal abstract class OrderAdjustmentOrderLineModificationContracts : IOrderAdjustmentOrderLineModification
		{
		    #region Primitive properties
		
			int IOrderAdjustmentOrderLineModification.OrderAdjustmentOrderLineModificationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderAdjustmentOrderLineModification.OrderItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderAdjustmentOrderLineModification.OrderAdjustmentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderAdjustmentOrderLineModification.PropertyName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderAdjustmentOrderLineModification.ModificationOperationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderAdjustmentOrderLineModification.ModificationDecimalValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderAdjustmentOrderLineModification.ModificationDescription
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrderAdjustment IOrderAdjustmentOrderLineModification.OrderAdjustment
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderItem IOrderAdjustmentOrderLineModification.OrderItem
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
