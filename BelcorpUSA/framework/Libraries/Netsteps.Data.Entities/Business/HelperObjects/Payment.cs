using System;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Security;

namespace NetSteps.Data.Entities
{
	[Serializable]
	public class Payment : CloneableBase<Payment>, IPayment
	{

		#region Members
		private string _nameOnCard;
		private IAddress _billingAddress;
		private string _billingFirstName;
		private string _billingLastName;
		private string _accountNumber;
		private string _routingNumber;
		private string _bankName;
		private short? _bankAccountTypeID;
		private string _encryptedNumber;
		private string _decryptedAccountNumber = string.Empty;
		private DateTime? _expirationDate;
		private string _cvv = String.Empty;
		private Constants.PaymentType _paymentType = Constants.PaymentType.NotSet; 
		#endregion

		#region Properties 

		public string NameOnCard
		{
			get { return _nameOnCard; }
			set { _nameOnCard = value; }
		}

		public IAddress BillingAddress
		{
			get { return _billingAddress; }
			set { _billingAddress = value; }
		}

		public string BillingFirstName
		{
			get { return _billingFirstName; }
			set { _billingFirstName = value; }
		}

		public string BillingLastName
		{
			get { return _billingLastName; }
			set { _billingLastName = value; }
		}

		public string RoutingNumber
		{
			get
			{
				return _routingNumber;
			}
			set
			{
				_routingNumber = value;
			}
		}

		public string BankName
		{
			get
			{
				return _bankName;
			}
			set
			{
				_bankName = value;
			}
		}

		public short? BankAccountTypeID
		{
			get
			{
				return _bankAccountTypeID;
			}
			set
			{
				_bankAccountTypeID = value;
			}
		}

		public string AccountNumber
		{
			get
			{
				return _accountNumber;
			}
			set
			{
				_accountNumber = value.MaskString(4);
				_encryptedNumber = Encryption.EncryptTripleDES(value);
			}
		}

		public string EncryptedAccountNumber
		{
			get { return _encryptedNumber; }
			set
			{
				_encryptedNumber = value;
				if (!value.IsNullOrEmpty())
					_accountNumber = Encryption.DecryptTripleDES(value).MaskString(4);
				else
					_accountNumber = string.Empty;
			}
		}

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

		public DateTime? ExpirationDate
		{
			get { return _expirationDate; }
			set { _expirationDate = value; }
		}

		public string CVV
		{
			get { return _cvv; }
			set { _cvv = value; }
		}

		/// <summary>
		/// Just used by UI for now - JHE
		/// </summary>
		public bool IsDefault { get; set; }

		public Constants.PaymentType PaymentType
		{
			get { return _paymentType; }
			set { _paymentType = value; }
		}
		#endregion

		#region IPayment
		int IPayment.PaymentTypeID
		{
			get
			{
				return (int)_paymentType;
			}
			set
			{
				_paymentType = (Constants.PaymentType)value;
			}
		}
		string IPayment.AccountName
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}
		string IPayment.FirstName
		{
			get
			{
				return this.BillingFirstName;
			}
			set
			{
				this.BillingFirstName = value;
			}
		}
		string IPayment.LastName
		{
			get
			{
				return this.BillingLastName;
			}
			set
			{
				this.BillingLastName = value;
			}
		}
		short? IPayment.PaymentGatewayID { get; set; }

		private bool? _canPayForTax;
		public bool CanPayForTax
		{
			get
			{
				if (!_canPayForTax.HasValue)
					_canPayForTax = SmallCollectionCache.Instance.PaymentTypes.GetById((int)PaymentType).CanPayForTax;

				return (bool)_canPayForTax;
			}
		}

		private bool? _canPayForShippingAndHandling;
		public bool CanPayForShippingAndHandling
		{
			get
			{
				if (!_canPayForShippingAndHandling.HasValue)
					_canPayForShippingAndHandling = SmallCollectionCache.Instance.PaymentTypes.GetById((int)PaymentType).CanPayForShippingAndHandling;

				return (bool)_canPayForShippingAndHandling;
			}
		}

		private bool? _isCommissionable;
		public bool IsCommissionable
		{
			get
			{
				if (!_isCommissionable.HasValue)
					_isCommissionable = SmallCollectionCache.Instance.PaymentTypes.GetById((int)PaymentType).IsCommissionable;

				return (bool)_isCommissionable;
			}
		}
		#endregion

		#region Methods
		public override string ToString()
		{
			switch (_paymentType)
			{
				case Constants.PaymentType.Cash: return "Cash";
				case Constants.PaymentType.Check: return "Check Number: " + DecryptedAccountNumber;
				case Constants.PaymentType.GiftCard: return "Gift Certificate Number: " + DecryptedAccountNumber;
				case Constants.PaymentType.ProductCredit: return "Product Credit";
				case Constants.PaymentType.EFT: return "Bank Account Number: " + MaskedAccountNumber;
				default:
					return String.Format("Billing Address:<br />{0}<br /><br />Name on card: {1}<br />CC Number: {2}<br />Expiration Date: {3}",
						this.BillingAddress.ToDisplay(),
						this.NameOnCard,
						this.MaskedAccountNumber,
						this.ExpirationDate.Value.Year.ToString() + "/" + this.ExpirationDate.Value.Month.ToString());
			}
		}

		#endregion
	}
}