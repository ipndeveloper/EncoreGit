using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderStatusContracts))]
	public interface IOrderStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderStatusID for this OrderStatus.
		/// </summary>
		short OrderStatusID { get; set; }
	
		/// <summary>
		/// The Name for this OrderStatus.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this OrderStatus.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this OrderStatus.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this OrderStatus.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsCommissionable for this OrderStatus.
		/// </summary>
		bool IsCommissionable { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Orders for this OrderStatus.
		/// </summary>
		IEnumerable<IOrder> Orders { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrder"/> to the Orders collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrder"/> to add.</param>
		void AddOrder(IOrder item);
	
		/// <summary>
		/// Removes an <see cref="IOrder"/> from the Orders collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrder"/> to remove.</param>
		void RemoveOrder(IOrder item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderStatus))]
		internal abstract class OrderStatusContracts : IOrderStatus
		{
		    #region Primitive properties
		
			short IOrderStatus.OrderStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderStatus.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderStatus.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderStatus.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderStatus.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderStatus.IsCommissionable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrder> IOrderStatus.Orders
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderStatus.AddOrder(IOrder item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderStatus.RemoveOrder(IOrder item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
