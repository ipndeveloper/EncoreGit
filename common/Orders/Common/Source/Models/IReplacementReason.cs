using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for ReplacementReason.
	/// </summary>
	[ContractClass(typeof(Contracts.ReplacementReasonContracts))]
	public interface IReplacementReason
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ReplacementReasonID for this ReplacementReason.
		/// </summary>
		int ReplacementReasonID { get; set; }
	
		/// <summary>
		/// The Name for this ReplacementReason.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this ReplacementReason.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this ReplacementReason.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this ReplacementReason.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The ModifiedUserID for this ReplacementReason.
		/// </summary>
		Nullable<int> ModifiedUserID { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderItemReplacements for this ReplacementReason.
		/// </summary>
		IEnumerable<IOrderItemReplacement> OrderItemReplacements { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderItemReplacement"/> to the OrderItemReplacements collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemReplacement"/> to add.</param>
		void AddOrderItemReplacement(IOrderItemReplacement item);
	
		/// <summary>
		/// Removes an <see cref="IOrderItemReplacement"/> from the OrderItemReplacements collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderItemReplacement"/> to remove.</param>
		void RemoveOrderItemReplacement(IOrderItemReplacement item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IReplacementReason))]
		internal abstract class ReplacementReasonContracts : IReplacementReason
		{
		    #region Primitive properties
		
			int IReplacementReason.ReplacementReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IReplacementReason.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IReplacementReason.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IReplacementReason.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IReplacementReason.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IReplacementReason.ModifiedUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderItemReplacement> IReplacementReason.OrderItemReplacements
			{
				get { throw new NotImplementedException(); }
			}
		
			void IReplacementReason.AddOrderItemReplacement(IOrderItemReplacement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IReplacementReason.RemoveOrderItemReplacement(IOrderItemReplacement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
