using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderAdjustmentOrderModification.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderAdjustmentOrderModificationContracts))]
	public interface IOrderAdjustmentOrderModification
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderAdjustmentOrderModificationID for this OrderAdjustmentOrderModification.
		/// </summary>
		int OrderAdjustmentOrderModificationID { get; set; }
	
		/// <summary>
		/// The OrderAdjustmentID for this OrderAdjustmentOrderModification.
		/// </summary>
		int OrderAdjustmentID { get; set; }
	
		/// <summary>
		/// The PropertyName for this OrderAdjustmentOrderModification.
		/// </summary>
		string PropertyName { get; set; }
	
		/// <summary>
		/// The ModificationOperationID for this OrderAdjustmentOrderModification.
		/// </summary>
		int ModificationOperationID { get; set; }
	
		/// <summary>
		/// The ModificationDecimalValue for this OrderAdjustmentOrderModification.
		/// </summary>
		Nullable<decimal> ModificationDecimalValue { get; set; }
	
		/// <summary>
		/// The ModificationDescription for this OrderAdjustmentOrderModification.
		/// </summary>
		string ModificationDescription { get; set; }
	
		/// <summary>
		/// The OrderCustomerID for this OrderAdjustmentOrderModification.
		/// </summary>
		Nullable<int> OrderCustomerID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OrderAdjustment for this OrderAdjustmentOrderModification.
		/// </summary>
	    IOrderAdjustment OrderAdjustment { get; set; }
	
		/// <summary>
		/// The OrderCustomer for this OrderAdjustmentOrderModification.
		/// </summary>
	    IOrderCustomer OrderCustomer { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderAdjustmentOrderModification))]
		internal abstract class OrderAdjustmentOrderModificationContracts : IOrderAdjustmentOrderModification
		{
		    #region Primitive properties
		
			int IOrderAdjustmentOrderModification.OrderAdjustmentOrderModificationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderAdjustmentOrderModification.OrderAdjustmentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderAdjustmentOrderModification.PropertyName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderAdjustmentOrderModification.ModificationOperationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderAdjustmentOrderModification.ModificationDecimalValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderAdjustmentOrderModification.ModificationDescription
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderAdjustmentOrderModification.OrderCustomerID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrderAdjustment IOrderAdjustmentOrderModification.OrderAdjustment
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderCustomer IOrderAdjustmentOrderModification.OrderCustomer
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
