using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderItemPropertyValue.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderItemPropertyValueContracts))]
	public interface IOrderItemPropertyValue
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderItemPropertyValueID for this OrderItemPropertyValue.
		/// </summary>
		int OrderItemPropertyValueID { get; set; }
	
		/// <summary>
		/// The OrderItemPropertyTypeID for this OrderItemPropertyValue.
		/// </summary>
		int OrderItemPropertyTypeID { get; set; }
	
		/// <summary>
		/// The Name for this OrderItemPropertyValue.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Value for this OrderItemPropertyValue.
		/// </summary>
		string Value { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OrderItemPropertyType for this OrderItemPropertyValue.
		/// </summary>
	    IOrderItemPropertyType OrderItemPropertyType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderItemProperties for this OrderItemPropertyValue.
		/// </summary>
		IEnumerable<IOrderItemProperty> OrderItemProperties { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderItemProperty"/> to the OrderItemProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemProperty"/> to add.</param>
		void AddOrderItemProperty(IOrderItemProperty item);
	
		/// <summary>
		/// Removes an <see cref="IOrderItemProperty"/> from the OrderItemProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemProperty"/> to remove.</param>
		void RemoveOrderItemProperty(IOrderItemProperty item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderItemPropertyValue))]
		internal abstract class OrderItemPropertyValueContracts : IOrderItemPropertyValue
		{
		    #region Primitive properties
		
			int IOrderItemPropertyValue.OrderItemPropertyValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemPropertyValue.OrderItemPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemPropertyValue.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemPropertyValue.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrderItemPropertyType IOrderItemPropertyValue.OrderItemPropertyType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderItemProperty> IOrderItemPropertyValue.OrderItemProperties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItemPropertyValue.AddOrderItemProperty(IOrderItemProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItemPropertyValue.RemoveOrderItemProperty(IOrderItemProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
