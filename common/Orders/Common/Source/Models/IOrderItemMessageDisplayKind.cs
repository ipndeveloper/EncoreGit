using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderItemMessageDisplayKind.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderItemMessageDisplayKindContracts))]
	public interface IOrderItemMessageDisplayKind
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderItemMessageDisplayKindID for this OrderItemMessageDisplayKind.
		/// </summary>
		int OrderItemMessageDisplayKindID { get; set; }
	
		/// <summary>
		/// The Name for this OrderItemMessageDisplayKind.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this OrderItemMessageDisplayKind.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Active for this OrderItemMessageDisplayKind.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderItemMessageDisplayKind))]
		internal abstract class OrderItemMessageDisplayKindContracts : IOrderItemMessageDisplayKind
		{
		    #region Primitive properties
		
			int IOrderItemMessageDisplayKind.OrderItemMessageDisplayKindID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemMessageDisplayKind.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderItemMessageDisplayKind.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderItemMessageDisplayKind.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
