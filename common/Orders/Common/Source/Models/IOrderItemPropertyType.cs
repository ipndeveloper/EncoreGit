using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderItemPropertyType.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderItemPropertyTypeContracts))]
	public interface IOrderItemPropertyType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderItemPropertyTypeID for this OrderItemPropertyType.
		/// </summary>
		int OrderItemPropertyTypeID { get; set; }
	
		/// <summary>
		/// The Name for this OrderItemPropertyType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The DataType for this OrderItemPropertyType.
		/// </summary>
		string DataType { get; set; }
	
		/// <summary>
		/// The Required for this OrderItemPropertyType.
		/// </summary>
		bool Required { get; set; }
	
		/// <summary>
		/// The SortIndex for this OrderItemPropertyType.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The TermName for this OrderItemPropertyType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this OrderItemPropertyType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this OrderItemPropertyType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderItemProperties for this OrderItemPropertyType.
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
	
		/// <summary>
		/// The OrderItemPropertyValues for this OrderItemPropertyType.
		/// </summary>
		IEnumerable<IOrderItemPropertyValue> OrderItemPropertyValues { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderItemPropertyValue"/> to the OrderItemPropertyValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemPropertyValue"/> to add.</param>
		void AddOrderItemPropertyValue(IOrderItemPropertyValue item);
	
		/// <summary>
		/// Removes an <see cref="IOrderItemPropertyValue"/> from the OrderItemPropertyValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemPropertyValue"/> to remove.</param>
		void RemoveOrderItemPropertyValue(IOrderItemPropertyValue item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderItemPropertyType))]
		internal abstract class OrderItemPropertyTypeContracts : IOrderItemPropertyType
		{
		    #region Primitive properties
		
			int IOrderItemPropertyType.OrderItemPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemPropertyType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemPropertyType.DataType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItemPropertyType.Required
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderItemPropertyType.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemPropertyType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemPropertyType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItemPropertyType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderItemProperty> IOrderItemPropertyType.OrderItemProperties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItemPropertyType.AddOrderItemProperty(IOrderItemProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItemPropertyType.RemoveOrderItemProperty(IOrderItemProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderItemPropertyValue> IOrderItemPropertyType.OrderItemPropertyValues
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItemPropertyType.AddOrderItemPropertyValue(IOrderItemPropertyValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItemPropertyType.RemoveOrderItemPropertyValue(IOrderItemPropertyValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
