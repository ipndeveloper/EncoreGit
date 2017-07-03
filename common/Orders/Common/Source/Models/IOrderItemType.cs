using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderItemType.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderItemTypeContracts))]
	public interface IOrderItemType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderItemTypeID for this OrderItemType.
		/// </summary>
		short OrderItemTypeID { get; set; }
	
		/// <summary>
		/// The Name for this OrderItemType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this OrderItemType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this OrderItemType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this OrderItemType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsHostessReward for this OrderItemType.
		/// </summary>
		bool IsHostessReward { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HostessRewardRules for this OrderItemType.
		/// </summary>
		IEnumerable<IHostessRewardRule> HostessRewardRules { get; }
	
		/// <summary>
		/// Adds an <see cref="IHostessRewardRule"/> to the HostessRewardRules collection.
		/// </summary>
		/// <param name="item">The <see cref="IHostessRewardRule"/> to add.</param>
		void AddHostessRewardRule(IHostessRewardRule item);
	
		/// <summary>
		/// Removes an <see cref="IHostessRewardRule"/> from the HostessRewardRules collection.
		/// </summary>
		/// <param name="item">The <see cref="IHostessRewardRule"/> to remove.</param>
		void RemoveHostessRewardRule(IHostessRewardRule item);
	
		/// <summary>
		/// The OrderItems for this OrderItemType.
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
		[ContractClassFor(typeof(IOrderItemType))]
		internal abstract class OrderItemTypeContracts : IOrderItemType
		{
		    #region Primitive properties
		
			short IOrderItemType.OrderItemTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItemType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItemType.IsHostessReward
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHostessRewardRule> IOrderItemType.HostessRewardRules
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItemType.AddHostessRewardRule(IHostessRewardRule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItemType.RemoveHostessRewardRule(IHostessRewardRule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderItem> IOrderItemType.OrderItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderItemType.AddOrderItem(IOrderItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderItemType.RemoveOrderItem(IOrderItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
