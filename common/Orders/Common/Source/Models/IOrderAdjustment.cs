using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderAdjustment.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderAdjustmentContracts))]
	public interface IOrderAdjustment
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderAdjustmentID for this OrderAdjustment.
		/// </summary>
		int OrderAdjustmentID { get; set; }
	
		/// <summary>
		/// The OrderID for this OrderAdjustment.
		/// </summary>
		int OrderID { get; set; }
	
		/// <summary>
		/// The Description for this OrderAdjustment.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The ExtensionProviderKey for this OrderAdjustment.
		/// </summary>
		string ExtensionProviderKey { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Order for this OrderAdjustment.
		/// </summary>
	    IOrder Order { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderAdjustmentOrderLineModifications for this OrderAdjustment.
		/// </summary>
		IEnumerable<IOrderAdjustmentOrderLineModification> OrderAdjustmentOrderLineModifications { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderAdjustmentOrderLineModification"/> to the OrderAdjustmentOrderLineModifications collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderAdjustmentOrderLineModification"/> to add.</param>
		void AddOrderAdjustmentOrderLineModification(IOrderAdjustmentOrderLineModification item);
	
		/// <summary>
		/// Removes an <see cref="IOrderAdjustmentOrderLineModification"/> from the OrderAdjustmentOrderLineModifications collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderAdjustmentOrderLineModification"/> to remove.</param>
		void RemoveOrderAdjustmentOrderLineModification(IOrderAdjustmentOrderLineModification item);
	
		/// <summary>
		/// The OrderAdjustmentOrderModifications for this OrderAdjustment.
		/// </summary>
		IEnumerable<IOrderAdjustmentOrderModification> OrderAdjustmentOrderModifications { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderAdjustmentOrderModification"/> to the OrderAdjustmentOrderModifications collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderAdjustmentOrderModification"/> to add.</param>
		void AddOrderAdjustmentOrderModification(IOrderAdjustmentOrderModification item);
	
		/// <summary>
		/// Removes an <see cref="IOrderAdjustmentOrderModification"/> from the OrderAdjustmentOrderModifications collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderAdjustmentOrderModification"/> to remove.</param>
		void RemoveOrderAdjustmentOrderModification(IOrderAdjustmentOrderModification item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderAdjustment))]
		internal abstract class OrderAdjustmentContracts : IOrderAdjustment
		{
		    #region Primitive properties
		
			int IOrderAdjustment.OrderAdjustmentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderAdjustment.OrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderAdjustment.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderAdjustment.ExtensionProviderKey
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrder IOrderAdjustment.Order
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderAdjustmentOrderLineModification> IOrderAdjustment.OrderAdjustmentOrderLineModifications
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderAdjustment.AddOrderAdjustmentOrderLineModification(IOrderAdjustmentOrderLineModification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderAdjustment.RemoveOrderAdjustmentOrderLineModification(IOrderAdjustmentOrderLineModification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderAdjustmentOrderModification> IOrderAdjustment.OrderAdjustmentOrderModifications
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderAdjustment.AddOrderAdjustmentOrderModification(IOrderAdjustmentOrderModification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderAdjustment.RemoveOrderAdjustmentOrderModification(IOrderAdjustmentOrderModification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
