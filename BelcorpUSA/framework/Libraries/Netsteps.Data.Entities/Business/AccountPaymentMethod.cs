namespace NetSteps.Data.Entities
{
	using System;

	using NetSteps.Addresses.Common.Models;
	using NetSteps.Common.Extensions;
	using NetSteps.Data.Entities.Business.Interfaces;
	using NetSteps.Data.Entities.Cache;
	using NetSteps.Data.Entities.Extensions;
	using NetSteps.Security;

	public partial class AccountPaymentMethod : IAddress, IPayment
	{
		#region Members
		private string _cvv = string.Empty;
		#endregion

		#region Properties

		public double? Latitude { get; set; }

		public double? Longitude { get; set; }

		private string _decryptedAccountNumber = string.Empty;
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

		public string FormatedExpirationDate
		{
			get
			{
				return ExpirationDateUTC != null ? ExpirationDateUTC.Value.ToExpirationString() : string.Empty;
			}
			set
			{
				var dateValues = value.Split('/');
				int year = dateValues[1].ToInt();
				int month = dateValues[0].ToInt();
				ExpirationDateUTC = new DateTime(year, month, DateTime.DaysInMonth(year, month));
			}
		}

		public string CVV
		{
			get
			{
				return _cvv;
			}
			set
			{
				_cvv = value;
			}
		}
		#endregion

		#region IAddress
		int IAddress.AddressID
		{
			get
			{
				return this.BillingAddressID ?? 0;
			}
			set { }
		}
		string IAddress.FirstName
		{
			get
			{
				return this.FirstName;
			}
			set
			{
				this.FirstName = value;
			}
		}
		string IAddress.LastName
		{
			get
			{
				return this.LastName;
			}
			set
			{
				this.LastName = value;
			}
		}
		string IAddress.Name
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}
		string IAddress.Attention
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}
		string IAddressBasic.Address1
		{
			get
			{
				return (this.BillingAddress == null) ? string.Empty : this.BillingAddress.Address1;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.Address1 = value;
			}
		}
		string IAddressBasic.Address2
		{
			get
			{
				return (this.BillingAddress == null) ? string.Empty : this.BillingAddress.Address2;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.Address2 = value;
			}
		}
		string IAddressBasic.Address3
		{
			get
			{
				return (this.BillingAddress == null) ? string.Empty : this.BillingAddress.Address3;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.Address3 = value;
			}
		}
		string IAddressBasic.City
		{
			get
			{
				return (this.BillingAddress == null) ? string.Empty : this.BillingAddress.City;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.City = value;
			}
		}
		string IAddressBasic.County
		{
			get
			{
				return (this.BillingAddress == null) ? string.Empty : this.BillingAddress.County;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.County = value;
			}
		}
		string IAddressBasic.State
		{
			get
			{
				return (this.BillingAddress == null) ? string.Empty : this.BillingAddress.State;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.State = value;
			}
		}
		int? IAddress.StateProvinceID
		{
			get
			{
				return (this.BillingAddress == null) ? 0 : this.BillingAddress.StateProvinceID;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.StateProvinceID = value;
			}
		}
		string IAddressBasic.PostalCode
		{
			get
			{
				return (this.BillingAddress == null) ? string.Empty : this.BillingAddress.PostalCode;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.PostalCode = value;
			}
		}
		string IAddress.PhoneNumber
		{
			get
			{
				return (this.BillingAddress == null) ? string.Empty : this.BillingAddress.PhoneNumber;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.PhoneNumber = value;
			}
		}
		string IAddressBasic.Country
		{
			get
			{
				var country = SmallCollectionCache.Instance.Countries.GetById((this as IAddress).CountryID);
				if (country != null)
					return country.GetTerm();
				else
					return string.Empty;
			}
		}
		string IAddress.CountryCode
		{
			get
			{
				if ((this as IAddress).CountryID > 0)
				{
					var country = SmallCollectionCache.Instance.Countries.GetById((this as IAddress).CountryID);
					return country.CountryCode;
				}

				return string.Empty;
			}
		}
		int IAddress.CountryID
		{
			get
			{
				return (this.BillingAddress == null) ? 0 : this.BillingAddress.CountryID;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.CountryID = value;
			}
		}
		short IAddress.AddressTypeID
		{
			get
			{
				return (short)NetSteps.Data.Entities.Constants.AddressType.Billing;
			}
			set
			{

			}
		}
		bool IAddress.IsDefault
		{
			get
			{
				return (this.BillingAddress == null) ? false : this.BillingAddress.IsDefault;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.IsDefault = value;
			}
		}
		bool IAddress.IsWillCall
		{
			get
			{
				return false;
			}
			set
			{
			}
		}
		int IAddress.ProfileID
		{
			get
			{
				return (this.BillingAddress == null) ? 0 : this.BillingAddress.ProfileID;
			}
			set
			{
				if (this.BillingAddress != null)
					this.BillingAddress.ProfileID = value;
			}
		}
		string IAddress.StateProvinceAbbreviation
		{
			get
			{
				var thisAsIAddress = this as IAddress;
				if (thisAsIAddress.StateProvinceID.HasValue && thisAsIAddress.StateProvinceID > 0)
				{
					var stateProvince = SmallCollectionCache.Instance.StateProvinces.GetById(thisAsIAddress.StateProvinceID.Value);
					return stateProvince.StateAbbreviation;
				}

				return string.Empty;
			}
		}
		#endregion

		#region IPayment
		string IPayment.CVV
		{
			get
			{
				return _cvv;
			}
			set
			{
				_cvv = value;
			}
		}
		bool IPayment.IsDefault
		{
			get
			{
				return false;
			}
			set
			{
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
		string IPayment.NameOnCard
		{
			get
			{
				return this.NameOnCard;
			}
			set
			{
				NameOnCard = value;
			}
		}
		string IPayment.LastName
		{
			get
			{
				return this.LastName;
			}
			set
			{
				this.LastName = value;
			}
		}
		string IPayment.FirstName
		{
			get
			{
				return this.FirstName;
			}
			set
			{
				this.FirstName = value;
			}
		}
		DateTime? IPayment.ExpirationDate
		{
			get
			{
				return this.ExpirationDate;
			}
			set
			{
				this.ExpirationDate = value;
			}
		}
		int IPayment.PaymentTypeID
		{
			get
			{
				return this.PaymentTypeID;
			}
			set
			{
				this.PaymentTypeID = (int)value;
			}
		}
		IAddress IPayment.BillingAddress
		{
			get
			{
				return (this as IAddress);
			}
			set
			{
				if (this.BillingAddress == null)
					this.BillingAddress = new Address();

				Address.CopyPropertiesTo(value, this);
			}
		}

		short? IPayment.PaymentGatewayID
		{
			get
			{
				return null;
			}
			set
			{

			}
		}

		private bool? _canPayForTax;
		public bool CanPayForTax
		{
			get
			{
				if (!_canPayForTax.HasValue)
					_canPayForTax = SmallCollectionCache.Instance.PaymentTypes.GetById(PaymentTypeID).CanPayForTax;

				return (bool)_canPayForTax;
				}
			}

		private bool? _canPayForShippingAndHandling;
		public bool CanPayForShippingAndHandling
		{
			get
			{
				if (!_canPayForShippingAndHandling.HasValue)
					_canPayForShippingAndHandling = SmallCollectionCache.Instance.PaymentTypes.GetById(PaymentTypeID).CanPayForShippingAndHandling;

				return (bool)_canPayForShippingAndHandling;
				}
			}

		private bool? _isCommissionable;
		public bool IsCommissionable
		{
			get
			{
				if (!_isCommissionable.HasValue)
					_isCommissionable = SmallCollectionCache.Instance.PaymentTypes.GetById(PaymentTypeID).IsCommissionable;

				return (bool)_isCommissionable;
				}
			}
		#endregion

		#region Methods
		public static bool IsUsedByAnyActiveOrderTemplates(int accountPaymentMethodID)
		{
			return Repository.IsUsedByAnyActiveOrderTemplates(accountPaymentMethodID);
		}
		#endregion
	}
}
