using NetSteps.Common.Extensions;
using NetSteps.Security;

namespace NetSteps.Data.Entities
{
	public partial class OrderPaymentResult
	{
		#region Member
		private string _decryptedAccountNumber = string.Empty;
		#endregion

		#region Properties
		public string DecryptedAccountNumber
		{
			get
			{
				if (_decryptedAccountNumber.IsNullOrEmpty())
					_decryptedAccountNumber = Encryption.DecryptTripleDES(_accountNumber);

				return _decryptedAccountNumber;
			}
			set
			{
				_decryptedAccountNumber = value ?? string.Empty;

				if (!string.IsNullOrEmpty(value))
					AccountNumber = Encryption.EncryptTripleDES(value);
			}
		}

		public string MaskedAccountNumber
		{
			get
			{
				return DecryptedAccountNumber.MaskString(4);
			}
		}
		#endregion

		#region Methods
		public OrderPaymentResult(OrderPayment orderPayment)
		{
			_orderPaymentID = orderPayment.PaymentTypeID;
			_orderID = orderPayment.OrderID;
			_orderCustomerID = orderPayment.OrderCustomerID;
			_accountNumber = orderPayment.AccountNumber;
			_amount = orderPayment.Amount;
			_routingNumber = orderPayment.RoutingNumber;
            _bankName = orderPayment.BankName;
			_transactionID = orderPayment.TransactionID;
			ExpirationDate = orderPayment.ExpirationDate;
		}
		#endregion
	}
}
