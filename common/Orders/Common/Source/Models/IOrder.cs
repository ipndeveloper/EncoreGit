using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Common.Models;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for Order.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderContracts))]
	public interface IOrder
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderID for this Order.
		/// </summary>
		int OrderID { get; set; }
	
		/// <summary>
		/// The OrderNumber for this Order.
		/// </summary>
		string OrderNumber { get; set; }
	
		/// <summary>
		/// The OrderStatusID for this Order.
		/// </summary>
		short OrderStatusID { get; set; }
	
		/// <summary>
		/// The OrderTypeID for this Order.
		/// </summary>
		short OrderTypeID { get; set; }
	
		/// <summary>
		/// The ConsultantID for this Order.
		/// </summary>
		int ConsultantID { get; set; }
	
		/// <summary>
		/// The SiteID for this Order.
		/// </summary>
		Nullable<int> SiteID { get; set; }
	
		/// <summary>
		/// The ParentOrderID for this Order.
		/// </summary>
		Nullable<int> ParentOrderID { get; set; }
	
		/// <summary>
		/// The CurrencyID for this Order.
		/// </summary>
		int CurrencyID { get; set; }
	
		/// <summary>
		/// The CompleteDateUTC for this Order.
		/// </summary>
		Nullable<System.DateTime> CompleteDateUTC { get; set; }
	
		/// <summary>
		/// The CommissionDateUTC for this Order.
		/// </summary>
		Nullable<System.DateTime> CommissionDateUTC { get; set; }
	
		/// <summary>
		/// The HostessRewardsEarned for this Order.
		/// </summary>
		Nullable<decimal> HostessRewardsEarned { get; set; }
	
		/// <summary>
		/// The HostessRewardsUsed for this Order.
		/// </summary>
		Nullable<decimal> HostessRewardsUsed { get; set; }
	
		/// <summary>
		/// The IsTaxExempt for this Order.
		/// </summary>
		Nullable<bool> IsTaxExempt { get; set; }
	
		/// <summary>
		/// The TaxAmountTotal for this Order.
		/// </summary>
		Nullable<decimal> TaxAmountTotal { get; set; }
	
		/// <summary>
		/// The TaxAmountTotalOverride for this Order.
		/// </summary>
		Nullable<decimal> TaxAmountTotalOverride { get; set; }
	
		/// <summary>
		/// The TaxableTotal for this Order.
		/// </summary>
		Nullable<decimal> TaxableTotal { get; set; }
	
		/// <summary>
		/// The TaxAmountOrderItems for this Order.
		/// </summary>
		Nullable<decimal> TaxAmountOrderItems { get; set; }
	
		/// <summary>
		/// The TaxAmountShipping for this Order.
		/// </summary>
		Nullable<decimal> TaxAmountShipping { get; set; }
	
		/// <summary>
		/// The TaxAmount for this Order.
		/// </summary>
		Nullable<decimal> TaxAmount { get; set; }
	
		/// <summary>
		/// The Subtotal for this Order.
		/// </summary>
		Nullable<decimal> Subtotal { get; set; }
	
		/// <summary>
		/// The DiscountTotal for this Order.
		/// </summary>
		Nullable<decimal> DiscountTotal { get; set; }
	
		/// <summary>
		/// The ShippingTotal for this Order.
		/// </summary>
		Nullable<decimal> ShippingTotal { get; set; }
	
		/// <summary>
		/// The ShippingTotalOverride for this Order.
		/// </summary>
		Nullable<decimal> ShippingTotalOverride { get; set; }
	
		/// <summary>
		/// The GrandTotal for this Order.
		/// </summary>
		Nullable<decimal> GrandTotal { get; set; }
	
		/// <summary>
		/// The PaymentTotal for this Order.
		/// </summary>
		Nullable<decimal> PaymentTotal { get; set; }
	
		/// <summary>
		/// The Balance for this Order.
		/// </summary>
		Nullable<decimal> Balance { get; set; }
	
		/// <summary>
		/// The CommissionableTotal for this Order.
		/// </summary>
		Nullable<decimal> CommissionableTotal { get; set; }
	
		/// <summary>
		/// The ReturnTypeID for this Order.
		/// </summary>
		Nullable<int> ReturnTypeID { get; set; }
	
		/// <summary>
		/// The StepUrl for this Order.
		/// </summary>
		string StepUrl { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this Order.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The DateCreatedUTC for this Order.
		/// </summary>
		System.DateTime DateCreatedUTC { get; set; }
	
		/// <summary>
		/// The CreatedByUserID for this Order.
		/// </summary>
		Nullable<int> CreatedByUserID { get; set; }
	
		/// <summary>
		/// The DataVersion for this Order.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The HandlingTotal for this Order.
		/// </summary>
		Nullable<decimal> HandlingTotal { get; set; }
	
		/// <summary>
		/// The DiscountPercent for this Order.
		/// </summary>
		Nullable<decimal> DiscountPercent { get; set; }
	
		/// <summary>
		/// The PartyShipmentTotal for this Order.
		/// </summary>
		Nullable<decimal> PartyShipmentTotal { get; set; }
	
		/// <summary>
		/// The PartyHandlingTotal for this Order.
		/// </summary>
		Nullable<decimal> PartyHandlingTotal { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Notes for this Order.
		/// </summary>
		IEnumerable<INote> Notes { get; }
	
		/// <summary>
		/// Adds an <see cref="INote"/> to the Notes collection.
		/// </summary>
		/// <param name="item">The <see cref="INote"/> to add.</param>
		void AddNote(INote item);
	
		/// <summary>
		/// Removes an <see cref="INote"/> from the Notes collection.
		/// </summary>
		/// <param name="item">The <see cref="INote"/> to remove.</param>
		void RemoveNote(INote item);
	
		/// <summary>
		/// The OrderAdjustments for this Order.
		/// </summary>
		IEnumerable<IOrderAdjustment> OrderAdjustments { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderAdjustment"/> to the OrderAdjustments collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderAdjustment"/> to add.</param>
		void AddOrderAdjustment(IOrderAdjustment item);
	
		/// <summary>
		/// Removes an <see cref="IOrderAdjustment"/> from the OrderAdjustments collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderAdjustment"/> to remove.</param>
		void RemoveOrderAdjustment(IOrderAdjustment item);
	
		/// <summary>
		/// The OrderCustomers for this Order.
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
	
		/// <summary>
		/// The OrderPaymentResults for this Order.
		/// </summary>
		IEnumerable<IOrderPaymentResult> OrderPaymentResults { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderPaymentResult"/> to the OrderPaymentResults collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderPaymentResult"/> to add.</param>
		void AddOrderPaymentResult(IOrderPaymentResult item);
	
		/// <summary>
		/// Removes an <see cref="IOrderPaymentResult"/> from the OrderPaymentResults collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderPaymentResult"/> to remove.</param>
		void RemoveOrderPaymentResult(IOrderPaymentResult item);
	
		/// <summary>
		/// The OrderPayments for this Order.
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
	
		/// <summary>
		/// The OrderShipments for this Order.
		/// </summary>
		IEnumerable<IOrderShipment> OrderShipments { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderShipment"/> to the OrderShipments collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderShipment"/> to add.</param>
		void AddOrderShipment(IOrderShipment item);
	
		/// <summary>
		/// Removes an <see cref="IOrderShipment"/> from the OrderShipments collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderShipment"/> to remove.</param>
		void RemoveOrderShipment(IOrderShipment item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrder))]
		internal abstract class OrderContracts : IOrder
		{
		    #region Primitive properties
		
			int IOrder.OrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrder.OrderNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IOrder.OrderStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IOrder.OrderTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrder.ConsultantID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrder.SiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrder.ParentOrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrder.CurrencyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IOrder.CompleteDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IOrder.CommissionDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.HostessRewardsEarned
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.HostessRewardsUsed
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IOrder.IsTaxExempt
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.TaxAmountTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.TaxAmountTotalOverride
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.TaxableTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.TaxAmountOrderItems
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.TaxAmountShipping
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.TaxAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.Subtotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.DiscountTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.ShippingTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.ShippingTotalOverride
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.GrandTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.PaymentTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.Balance
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.CommissionableTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrder.ReturnTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrder.StepUrl
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrder.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IOrder.DateCreatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrder.CreatedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IOrder.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.HandlingTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.DiscountPercent
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.PartyShipmentTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrder.PartyHandlingTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<INote> IOrder.Notes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrder.AddNote(INote item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrder.RemoveNote(INote item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderAdjustment> IOrder.OrderAdjustments
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrder.AddOrderAdjustment(IOrderAdjustment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrder.RemoveOrderAdjustment(IOrderAdjustment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderCustomer> IOrder.OrderCustomers
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrder.AddOrderCustomer(IOrderCustomer item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrder.RemoveOrderCustomer(IOrderCustomer item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderPaymentResult> IOrder.OrderPaymentResults
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrder.AddOrderPaymentResult(IOrderPaymentResult item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrder.RemoveOrderPaymentResult(IOrderPaymentResult item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderPayment> IOrder.OrderPayments
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrder.AddOrderPayment(IOrderPayment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrder.RemoveOrderPayment(IOrderPayment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderShipment> IOrder.OrderShipments
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrder.AddOrderShipment(IOrderShipment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrder.RemoveOrderShipment(IOrderShipment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
