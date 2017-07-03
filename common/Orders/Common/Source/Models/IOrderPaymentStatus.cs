using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderPaymentStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderPaymentStatusContracts))]
	public interface IOrderPaymentStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderPaymentStatusID for this OrderPaymentStatus.
		/// </summary>
		short OrderPaymentStatusID { get; set; }
	
		/// <summary>
		/// The Name for this OrderPaymentStatus.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this OrderPaymentStatus.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this OrderPaymentStatus.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this OrderPaymentStatus.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderPayments for this OrderPaymentStatus.
		/// </summary>
		IEnumerable<IOrderPayment> OrderPayments { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderPayment"/> to the OrderPayments collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderPayment"/> to add.</param>
		void AddOrderPayment(IOrderPayment item);
	
		/// <summary>
		/// Removes an <see cref="IOrderPayment"/> from the OrderPayments collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderPayment"/> to remove.</param>
		void RemoveOrderPayment(IOrderPayment item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderPaymentStatus))]
		internal abstract class OrderPaymentStatusContracts : IOrderPaymentStatus
		{
		    #region Primitive properties
		
			short IOrderPaymentStatus.OrderPaymentStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentStatus.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentStatus.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentStatus.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderPaymentStatus.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderPayment> IOrderPaymentStatus.OrderPayments
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderPaymentStatus.AddOrderPayment(IOrderPayment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderPaymentStatus.RemoveOrderPayment(IOrderPayment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
