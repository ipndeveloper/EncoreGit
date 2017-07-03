using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderItemProperty.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderItemPropertyContracts))]
	public interface IOrderItemProperty
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderItemPropertyID for this OrderItemProperty.
		/// </summary>
		int OrderItemPropertyID { get; set; }
	
		/// <summary>
		/// The OrderItemID for this OrderItemProperty.
		/// </summary>
		int OrderItemID { get; set; }
	
		/// <summary>
		/// The OrderItemPropertyTypeID for this OrderItemProperty.
		/// </summary>
		int OrderItemPropertyTypeID { get; set; }
	
		/// <summary>
		/// The OrderItemPropertyValueID for this OrderItemProperty.
		/// </summary>
		Nullable<int> OrderItemPropertyValueID { get; set; }
	
		/// <summary>
		/// The PropertyValue for this OrderItemProperty.
		/// </summary>
		string PropertyValue { get; set; }
	
		/// <summary>
		/// The Active for this OrderItemProperty.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OrderItem for this OrderItemProperty.
		/// </summary>
	    IOrderItem OrderItem { get; set; }
	
		/// <summary>
		/// The OrderItemPropertyType for this OrderItemProperty.
		/// </summary>
	    IOrderItemPropertyType OrderItemPropertyType { get; set; }
	
		/// <summary>
		/// The OrderItemPropertyValue for this OrderItemProperty.
		/// </summary>
	    IOrderItemPropertyValue OrderItemPropertyValue { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderItemProperty))]
		internal abstract class OrderItemPropertyContracts : IOrderItemProperty
		{
		    #region Primitive properties
		
			int IOrderItemProperty.OrderItemPropertyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemProperty.OrderItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemProperty.OrderItemPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderItemProperty.OrderItemPropertyValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemProperty.PropertyValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItemProperty.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrderItem IOrderItemProperty.OrderItem
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderItemPropertyType IOrderItemProperty.OrderItemPropertyType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderItemPropertyValue IOrderItemProperty.OrderItemPropertyValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
