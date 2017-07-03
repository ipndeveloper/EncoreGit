using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderPayment.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderPaymentContracts))]
	public interface IOrderPayment
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderPaymentID for this OrderPayment.
		/// </summary>
		int OrderPaymentID { get; set; }
	
		/// <summary>
		/// The OrderID for this OrderPayment.
		/// </summary>
		int OrderID { get; set; }
	
		/// <summary>
		/// The OrderCustomerID for this OrderPayment.
		/// </summary>
		Nullable<int> OrderCustomerID { get; set; }
	
		/// <summary>
		/// The PaymentTypeID for this OrderPayment.
		/// </summary>
		int PaymentTypeID { get; set; }
	
		/// <summary>
		/// The AccountNumber for this OrderPayment.
		/// </summary>
		string AccountNumber { get; }
	
		/// <summary>
		/// The BillingFirstName for this OrderPayment.
		/// </summary>
		string BillingFirstName { get; set; }
	
		/// <summary>
		/// The BillingLastName for this OrderPayment.
		/// </summary>
		string BillingLastName { get; set; }
	
		/// <summary>
		/// The BillingName for this OrderPayment.
		/// </summary>
		string BillingName { get; set; }
	
		/// <summary>
		/// The BillingAddress1 for this OrderPayment.
		/// </summary>
		string BillingAddress1 { get; set; }
	
		/// <summary>
		/// The BillingAddress2 for this OrderPayment.
		/// </summary>
		string BillingAddress2 { get; set; }
	
		/// <summary>
		/// The BillingAddress3 for this OrderPayment.
		/// </summary>
		string BillingAddress3 { get; set; }
	
		/// <summary>
		/// The BillingCity for this OrderPayment.
		/// </summary>
		string BillingCity { get; set; }
	
		/// <summary>
		/// The BillingCounty for this OrderPayment.
		/// </summary>
		string BillingCounty { get; set; }
	
		/// <summary>
		/// The BillingState for this OrderPayment.
		/// </summary>
		string BillingState { get; set; }
	
		/// <summary>
		/// The BillingStateProvinceID for this OrderPayment.
		/// </summary>
		Nullable<int> BillingStateProvinceID { get; set; }
	
		/// <summary>
		/// The BillingPostalCode for this OrderPayment.
		/// </summary>
		string BillingPostalCode { get; set; }
	
		/// <summary>
		/// The BillingCountryID for this OrderPayment.
		/// </summary>
		Nullable<int> BillingCountryID { get; set; }
	
		/// <summary>
		/// The BillingPhoneNumber for this OrderPayment.
		/// </summary>
		string BillingPhoneNumber { get; set; }
	
		/// <summary>
		/// The IdentityNumber for this OrderPayment.
		/// </summary>
		string IdentityNumber { get; set; }
	
		/// <summary>
		/// The IdentityState for this OrderPayment.
		/// </summary>
		string IdentityState { get; set; }
	
		/// <summary>
		/// The Amount for this OrderPayment.
		/// </summary>
		decimal Amount { get; set; }
	
		/// <summary>
		/// The RoutingNumber for this OrderPayment.
		/// </summary>
		string RoutingNumber { get; set; }
	
		/// <summary>
		/// The OrderPaymentStatusID for this OrderPayment.
		/// </summary>
		short OrderPaymentStatusID { get; set; }
	
		/// <summary>
		/// The IsDeferred for this OrderPayment.
		/// </summary>
		bool IsDeferred { get; set; }
	
		/// <summary>
		/// The ProcessOnDateUTC for this OrderPayment.
		/// </summary>
		Nullable<System.DateTime> ProcessOnDateUTC { get; set; }
	
		/// <summary>
		/// The ProcessedDateUTC for this OrderPayment.
		/// </summary>
		Nullable<System.DateTime> ProcessedDateUTC { get; set; }
	
		/// <summary>
		/// The TransactionID for this OrderPayment.
		/// </summary>
		string TransactionID { get; set; }
	
		/// <summary>
		/// The DeferredAmount for this OrderPayment.
		/// </summary>
		Nullable<decimal> DeferredAmount { get; set; }
	
		/// <summary>
		/// The DeferredTransactionID for this OrderPayment.
		/// </summary>
		string DeferredTransactionID { get; set; }
	
		/// <summary>
		/// The ExpirationDateUTC for this OrderPayment.
		/// </summary>
		Nullable<System.DateTime> ExpirationDateUTC { get; set; }
	
		/// <summary>
		/// The DataVersion for this OrderPayment.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The CurrencyID for this OrderPayment.
		/// </summary>
		int CurrencyID { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this OrderPayment.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The NameOnCard for this OrderPayment.
		/// </summary>
		string NameOnCard { get; set; }
	
		/// <summary>
		/// The CreditCardTypeID for this OrderPayment.
		/// </summary>
		Nullable<short> CreditCardTypeID { get; set; }
	
		/// <summary>
		/// The Request for this OrderPayment.
		/// </summary>
		string Request { get; set; }
	
		/// <summary>
		/// The AccountNumberLastFour for this OrderPayment.
		/// </summary>
		string AccountNumberLastFour { get; set; }
	
		/// <summary>
		/// The PaymentGatewayID for this OrderPayment.
		/// </summary>
		Nullable<short> PaymentGatewayID { get; set; }
	
		/// <summary>
		/// The SourceAccountPaymentMethodID for this OrderPayment.
		/// </summary>
		Nullable<int> SourceAccountPaymentMethodID { get; set; }
	
		/// <summary>
		/// The BankAccountTypeID for this OrderPayment.
		/// </summary>
		Nullable<short> BankAccountTypeID { get; set; }
	
		/// <summary>
		/// The BankName for this OrderPayment.
		/// </summary>
		string BankName { get; set; }
	
		/// <summary>
		/// The NachaClassType for this OrderPayment.
		/// </summary>
		string NachaClassType { get; set; }
	
		/// <summary>
		/// The NachaSentDate for this OrderPayment.
		/// </summary>
		Nullable<System.DateTime> NachaSentDate { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Order for this OrderPayment.
		/// </summary>
	    IOrder Order { get; set; }
	
		/// <summary>
		/// The OrderCustomer for this OrderPayment.
		/// </summary>
	    IOrderCustomer OrderCustomer { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderPaymentResults for this OrderPayment.
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

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderPayment))]
		internal abstract class OrderPaymentContracts : IOrderPayment
		{
		    #region Primitive properties
		
			int IOrderPayment.OrderPaymentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderPayment.OrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderPayment.OrderCustomerID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderPayment.PaymentTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.AccountNumber
			{
				get { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BillingFirstName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BillingLastName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BillingName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BillingAddress1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BillingAddress2
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BillingAddress3
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BillingCity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BillingCounty
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BillingState
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderPayment.BillingStateProvinceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BillingPostalCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderPayment.BillingCountryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BillingPhoneNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.IdentityNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.IdentityState
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal IOrderPayment.Amount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.RoutingNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IOrderPayment.OrderPaymentStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderPayment.IsDeferred
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IOrderPayment.ProcessOnDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IOrderPayment.ProcessedDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.TransactionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderPayment.DeferredAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.DeferredTransactionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IOrderPayment.ExpirationDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IOrderPayment.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderPayment.CurrencyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderPayment.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.NameOnCard
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IOrderPayment.CreditCardTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.Request
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.AccountNumberLastFour
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IOrderPayment.PaymentGatewayID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderPayment.SourceAccountPaymentMethodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IOrderPayment.BankAccountTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.BankName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPayment.NachaClassType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IOrderPayment.NachaSentDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrder IOrderPayment.Order
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderCustomer IOrderPayment.OrderCustomer
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderPaymentResult> IOrderPayment.OrderPaymentResults
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderPayment.AddOrderPaymentResult(IOrderPaymentResult item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderPayment.RemoveOrderPaymentResult(IOrderPaymentResult item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
