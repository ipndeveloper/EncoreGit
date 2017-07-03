using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderItemParentType.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderItemParentTypeContracts))]
	public interface IOrderItemParentType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderItemParentTypeID for this OrderItemParentType.
		/// </summary>
		short OrderItemParentTypeID { get; set; }
	
		/// <summary>
		/// The Name for this OrderItemParentType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this OrderItemParentType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this OrderItemParentType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this OrderItemParentType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderItems for this OrderItemParentType.
		/// </summary>
		IEnumerable<IOrderItem> OrderItems { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderItem"/> to the OrderItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItem"/> to add.</param>
		void AddOrderItem(IOrderItem item);
	
		/// <summary>
		/// Removes an <see cref="IOrderItem"/> from the OrderItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItem"/> to remove.</param>
		void RemoveOrderItem(IOrderItem item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderItemParentType))]
		internal abstract class OrderItemParentTypeContracts : IOrderItemParentType
		{
		    #region Primitive properties
		
			short IOrderItemParentType.OrderItemParentTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemParentType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemParentType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemParentType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItemParentType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderItem> IOrderItemParentType.OrderItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItemParentType.AddOrderItem(IOrderItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItemParentType.RemoveOrderItem(IOrderItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
