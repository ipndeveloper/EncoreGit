using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderType.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderTypeContracts))]
	public interface IOrderType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderTypeID for this OrderType.
		/// </summary>
		short OrderTypeID { get; set; }
	
		/// <summary>
		/// The Name for this OrderType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this OrderType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this OrderType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The IsTemplate for this OrderType.
		/// </summary>
		bool IsTemplate { get; set; }
	
		/// <summary>
		/// The Active for this OrderType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderType))]
		internal abstract class OrderTypeContracts : IOrderType
		{
		    #region Primitive properties
		
			short IOrderType.OrderTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderType.IsTemplate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
