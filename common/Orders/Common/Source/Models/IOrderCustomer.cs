using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderCustomer.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderCustomerContracts))]
	public interface IOrderCustomer
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderCustomerID for this OrderCustomer.
		/// </summary>
		int OrderCustomerID { get; set; }
	
		/// <summary>
		/// The OrderID for this OrderCustomer.
		/// </summary>
		int OrderID { get; set; }
	
		/// <summary>
		/// The AccountID for this OrderCustomer.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The TaxAmountTotal for this OrderCustomer.
		/// </summary>
		Nullable<decimal> TaxAmountTotal { get; set; }
	
		/// <summary>
		/// The TaxAmountCity for this OrderCustomer.
		/// </summary>
		Nullable<decimal> TaxAmountCity { get; set; }
	
		/// <summary>
		/// The TaxAmountState for this OrderCustomer.
		/// </summary>
		Nullable<decimal> TaxAmountState { get; set; }
	
		/// <summary>
		/// The TaxAmountCounty for this OrderCustomer.
		/// </summary>
		Nullable<decimal> TaxAmountCounty { get; set; }
	
		/// <summary>
		/// The TaxAmountDistrict for this OrderCustomer.
		/// </summary>
		Nullable<decimal> TaxAmountDistrict { get; set; }
	
		/// <summary>
		/// The TaxableTotal for this OrderCustomer.
		/// </summary>
		Nullable<decimal> TaxableTotal { get; set; }
	
		/// <summary>
		/// The IsTaxExempt for this OrderCustomer.
		/// </summary>
		Nullable<bool> IsTaxExempt { get; set; }
	
		/// <summary>
		/// The DiscountAmount for this OrderCustomer.
		/// </summary>
		Nullable<decimal> DiscountAmount { get; set; }
	
		/// <summary>
		/// The Subtotal for this OrderCustomer.
		/// </summary>
		Nullable<decimal> Subtotal { get; set; }
	
		/// <summary>
		/// The PaymentTotal for this OrderCustomer.
		/// </summary>
		Nullable<decimal> PaymentTotal { get; set; }
	
		/// <summary>
		/// The Balance for this OrderCustomer.
		/// </summary>
		Nullable<decimal> Balance { get; set; }
	
		/// <summary>
		/// The Total for this OrderCustomer.
		/// </summary>
		Nullable<decimal> Total { get; set; }
	
		/// <summary>
		/// The FutureBookingDateUTC for this OrderCustomer.
		/// </summary>
		Nullable<System.DateTime> FutureBookingDateUTC { get; set; }
	
		/// <summary>
		/// The DataVersion for this OrderCustomer.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The TaxAmountOrderItems for this OrderCustomer.
		/// </summary>
		Nullable<decimal> TaxAmountOrderItems { get; set; }
	
		/// <summary>
		/// The TaxAmountShipping for this OrderCustomer.
		/// </summary>
		Nullable<decimal> TaxAmountShipping { get; set; }
	
		/// <summary>
		/// The TaxAmount for this OrderCustomer.
		/// </summary>
		Nullable<decimal> TaxAmount { get; set; }
	
		/// <summary>
		/// The CommissionableTotal for this OrderCustomer.
		/// </summary>
		Nullable<decimal> CommissionableTotal { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this OrderCustomer.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The ShippingTotal for this OrderCustomer.
		/// </summary>
		Nullable<decimal> ShippingTotal { get; set; }
	
		/// <summary>
		/// The OrderCustomerTypeID for this OrderCustomer.
		/// </summary>
		short OrderCustomerTypeID { get; set; }
	
		/// <summary>
		/// The HandlingTotal for this OrderCustomer.
		/// </summary>
		Nullable<decimal> HandlingTotal { get; set; }
	
		/// <summary>
		/// The TaxAmountCountry for this OrderCustomer.
		/// </summary>
		Nullable<decimal> TaxAmountCountry { get; set; }
	
		/// <summary>
		/// The IsBookingCredit for this OrderCustomer.
		/// </summary>
		bool IsBookingCredit { get; set; }
	
		/// <summary>
		/// The TaxGeocode for this OrderCustomer.
		/// </summary>
		string TaxGeocode { get; set; }
	
		/// <summary>
		/// The SalesTaxTransactionNumber for this OrderCustomer.
		/// </summary>
		string SalesTaxTransactionNumber { get; set; }
	
		/// <summary>
		/// The UseTaxTransactionNumber for this OrderCustomer.
		/// </summary>
		string UseTaxTransactionNumber { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Order for this OrderCustomer.
		/// </summary>
	    IOrder Order { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderAdjustmentOrderModifications for this OrderCustomer.
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
	
		/// <summary>
		/// The OrderItems for this OrderCustomer.
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
	
		/// <summary>
		/// The OrderPaymentResults for this OrderCustomer.
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
		/// The OrderPayments for this OrderCustomer.
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
		/// The OrderShipments for this OrderCustomer.
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
		[ContractClassFor(typeof(IOrderCustomer))]
		internal abstract class OrderCustomerContracts : IOrderCustomer
		{
		    #region Primitive properties
		
			int IOrderCustomer.OrderCustomerID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderCustomer.OrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderCustomer.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.TaxAmountTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.TaxAmountCity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.TaxAmountState
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.TaxAmountCounty
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.TaxAmountDistrict
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.TaxableTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IOrderCustomer.IsTaxExempt
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.DiscountAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.Subtotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.PaymentTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.Balance
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.Total
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IOrderCustomer.FutureBookingDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IOrderCustomer.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.TaxAmountOrderItems
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.TaxAmountShipping
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.TaxAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.CommissionableTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderCustomer.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.ShippingTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IOrderCustomer.OrderCustomerTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.HandlingTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderCustomer.TaxAmountCountry
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderCustomer.IsBookingCredit
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderCustomer.TaxGeocode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderCustomer.SalesTaxTransactionNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderCustomer.UseTaxTransactionNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrder IOrderCustomer.Order
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderAdjustmentOrderModification> IOrderCustomer.OrderAdjustmentOrderModifications
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderCustomer.AddOrderAdjustmentOrderModification(IOrderAdjustmentOrderModification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderCustomer.RemoveOrderAdjustmentOrderModification(IOrderAdjustmentOrderModification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderItem> IOrderCustomer.OrderItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderCustomer.AddOrderItem(IOrderItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderCustomer.RemoveOrderItem(IOrderItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderPaymentResult> IOrderCustomer.OrderPaymentResults
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderCustomer.AddOrderPaymentResult(IOrderPaymentResult item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderCustomer.RemoveOrderPaymentResult(IOrderPaymentResult item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderPayment> IOrderCustomer.OrderPayments
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderCustomer.AddOrderPayment(IOrderPayment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderCustomer.RemoveOrderPayment(IOrderPayment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOrderShipment> IOrderCustomer.OrderShipments
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderCustomer.AddOrderShipment(IOrderShipment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderCustomer.RemoveOrderShipment(IOrderShipment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
