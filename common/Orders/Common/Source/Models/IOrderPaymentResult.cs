using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderPaymentResult.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderPaymentResultContracts))]
	public interface IOrderPaymentResult
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderPaymentResultID for this OrderPaymentResult.
		/// </summary>
		int OrderPaymentResultID { get; set; }
	
		/// <summary>
		/// The OrderPaymentID for this OrderPaymentResult.
		/// </summary>
		int OrderPaymentID { get; set; }
	
		/// <summary>
		/// The OrderID for this OrderPaymentResult.
		/// </summary>
		int OrderID { get; set; }
	
		/// <summary>
		/// The OrderCustomerID for this OrderPaymentResult.
		/// </summary>
		Nullable<int> OrderCustomerID { get; set; }
	
		/// <summary>
		/// The CurrencyID for this OrderPaymentResult.
		/// </summary>
		int CurrencyID { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this OrderPaymentResult.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The DateAuthorizedUTC for this OrderPaymentResult.
		/// </summary>
		Nullable<System.DateTime> DateAuthorizedUTC { get; set; }
	
		/// <summary>
		/// The AuthorizeType for this OrderPaymentResult.
		/// </summary>
		string AuthorizeType { get; set; }
	
		/// <summary>
		/// The RoutingNumber for this OrderPaymentResult.
		/// </summary>
		string RoutingNumber { get; set; }
	
		/// <summary>
		/// The AccountNumber for this OrderPaymentResult.
		/// </summary>
		string AccountNumber { get; set; }
	
		/// <summary>
		/// The BankName for this OrderPaymentResult.
		/// </summary>
		string BankName { get; set; }
	
		/// <summary>
		/// The ExpirationDateUTC for this OrderPaymentResult.
		/// </summary>
		Nullable<System.DateTime> ExpirationDateUTC { get; set; }
	
		/// <summary>
		/// The Amount for this OrderPaymentResult.
		/// </summary>
		Nullable<decimal> Amount { get; set; }
	
		/// <summary>
		/// The ErrorLevel for this OrderPaymentResult.
		/// </summary>
		Nullable<int> ErrorLevel { get; set; }
	
		/// <summary>
		/// The ErrorMessage for this OrderPaymentResult.
		/// </summary>
		string ErrorMessage { get; set; }
	
		/// <summary>
		/// The ResponseCode for this OrderPaymentResult.
		/// </summary>
		string ResponseCode { get; set; }
	
		/// <summary>
		/// The ResponseSubCode for this OrderPaymentResult.
		/// </summary>
		string ResponseSubCode { get; set; }
	
		/// <summary>
		/// The ResponseReasonCode for this OrderPaymentResult.
		/// </summary>
		string ResponseReasonCode { get; set; }
	
		/// <summary>
		/// The ResponseReasonText for this OrderPaymentResult.
		/// </summary>
		string ResponseReasonText { get; set; }
	
		/// <summary>
		/// The AVSResult for this OrderPaymentResult.
		/// </summary>
		string AVSResult { get; set; }
	
		/// <summary>
		/// The CardCodeResponse for this OrderPaymentResult.
		/// </summary>
		string CardCodeResponse { get; set; }
	
		/// <summary>
		/// The ApprovalCode for this OrderPaymentResult.
		/// </summary>
		string ApprovalCode { get; set; }
	
		/// <summary>
		/// The Response for this OrderPaymentResult.
		/// </summary>
		string Response { get; set; }
	
		/// <summary>
		/// The TransactionID for this OrderPaymentResult.
		/// </summary>
		string TransactionID { get; set; }
	
		/// <summary>
		/// The IsTesting for this OrderPaymentResult.
		/// </summary>
		Nullable<bool> IsTesting { get; set; }
	
		/// <summary>
		/// The PaymentGatewayID for this OrderPaymentResult.
		/// </summary>
		Nullable<short> PaymentGatewayID { get; set; }
	
		/// <summary>
		/// The BalanceOnCard for this OrderPaymentResult.
		/// </summary>
		Nullable<decimal> BalanceOnCard { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Order for this OrderPaymentResult.
		/// </summary>
	    IOrder Order { get; set; }
	
		/// <summary>
		/// The OrderCustomer for this OrderPaymentResult.
		/// </summary>
	    IOrderCustomer OrderCustomer { get; set; }
	
		/// <summary>
		/// The OrderPayment for this OrderPaymentResult.
		/// </summary>
	    IOrderPayment OrderPayment { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderPaymentResult))]
		internal abstract class OrderPaymentResultContracts : IOrderPaymentResult
		{
		    #region Primitive properties
		
			int IOrderPaymentResult.OrderPaymentResultID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderPaymentResult.OrderPaymentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderPaymentResult.OrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderPaymentResult.OrderCustomerID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderPaymentResult.CurrencyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderPaymentResult.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IOrderPaymentResult.DateAuthorizedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.AuthorizeType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.RoutingNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.AccountNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.BankName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IOrderPaymentResult.ExpirationDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderPaymentResult.Amount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderPaymentResult.ErrorLevel
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.ErrorMessage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.ResponseCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.ResponseSubCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.ResponseReasonCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.ResponseReasonText
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.AVSResult
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.CardCodeResponse
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.ApprovalCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.Response
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderPaymentResult.TransactionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IOrderPaymentResult.IsTesting
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IOrderPaymentResult.PaymentGatewayID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IOrderPaymentResult.BalanceOnCard
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrder IOrderPaymentResult.Order
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderCustomer IOrderPaymentResult.OrderCustomer
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderPayment IOrderPaymentResult.OrderPayment
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
