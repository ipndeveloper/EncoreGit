using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderCustomerType.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderCustomerTypeContracts))]
	public interface IOrderCustomerType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderCustomerTypeID for this OrderCustomerType.
		/// </summary>
		short OrderCustomerTypeID { get; set; }
	
		/// <summary>
		/// The Name for this OrderCustomerType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this OrderCustomerType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this OrderCustomerType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this OrderCustomerType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderCustomers for this OrderCustomerType.
		/// </summary>
		IEnumerable<IOrderCustomer> OrderCustomers { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderCustomer"/> to the OrderCustomers collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderCustomer"/> to add.</param>
		void AddOrderCustomer(IOrderCustomer item);
	
		/// <summary>
		/// Removes an <see cref="IOrderCustomer"/> from the OrderCustomers collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderCustomer"/> to remove.</param>
		void RemoveOrderCustomer(IOrderCustomer item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderCustomerType))]
		internal abstract class OrderCustomerTypeContracts : IOrderCustomerType
		{
		    #region Primitive properties
		
			short IOrderCustomerType.OrderCustomerTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderCustomerType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderCustomerType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderCustomerType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderCustomerType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderCustomer> IOrderCustomerType.OrderCustomers
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderCustomerType.AddOrderCustomer(IOrderCustomer item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderCustomerType.RemoveOrderCustomer(IOrderCustomer item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
