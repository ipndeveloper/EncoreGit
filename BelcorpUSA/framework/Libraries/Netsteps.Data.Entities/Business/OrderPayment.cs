namespace NetSteps.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

	using NetSteps.Addresses.Common.Models;
	using NetSteps.Common.Base;
    using NetSteps.Common.Extensions;
    using NetSteps.Common.Interfaces;
    using NetSteps.Data.Entities.Business.Interfaces;
    using NetSteps.Data.Entities.Cache;
    using NetSteps.Data.Entities.Exceptions;
    using NetSteps.Data.Entities.Extensions;
    using NetSteps.Security;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Data.Entities.Business.HelperObjects;

	 public partial class OrderPayment : IAddress, IPayment, ITempGuid, IDateLastModified
    {
        #region Members
        private string _cvv = string.Empty;
        private string _message = string.Empty;
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

        public string Cvv
        {
            get { return _cvv; }
            set { _cvv = value; }
        }

		  [DataMember]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        // This is place to store additional (non-persisted) data to pass to the payment gateway - JHE
        private Dictionary<string, string> _processingAttributes = null;
        public Dictionary<string, string> ProcessingAttributes
        {
            get
            {
                if (_processingAttributes == null)
                    _processingAttributes = new Dictionary<string, string>();
                return _processingAttributes;
            }
            set
            {
                _processingAttributes = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return GetDisplayName();
            }
        }

        #endregion

        #region Constructors
        public OrderPayment(AccountPaymentMethod paymentMethod)
            : this((IPayment)paymentMethod)
        {
        }

        public OrderPayment(IPayment paymentMethod)
        {
            InitializeEntity();
            this.StartEntityTracking();

            IAddress billingAddress = paymentMethod.BillingAddress;

            this.OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Pending;
            this.DecryptedAccountNumber = paymentMethod.DecryptedAccountNumber;
            this.Cvv = paymentMethod.CVV;
            this.ExpirationDate = paymentMethod.ExpirationDate;
            this.NameOnCard = paymentMethod.NameOnCard;
            this.PaymentTypeID = paymentMethod.PaymentTypeID;
            this.OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Pending;

            if (billingAddress != null)
                Address.CopyPropertiesTo(billingAddress, this);
            this.BillingName = paymentMethod.FirstName + " " + paymentMethod.LastName;
            this.BillingFirstName = paymentMethod.FirstName;
            this.BillingLastName = paymentMethod.LastName;
            this.PaymentGatewayID = paymentMethod.PaymentGatewayID;
            this.BankName = paymentMethod.BankName;
            this.RoutingNumber = paymentMethod.RoutingNumber;
            this.BankAccountTypeID = paymentMethod.BankAccountTypeID;
        }
        #endregion

        #region IAddress
        int IAddress.AddressID
        {
            get
            {
				return OrderPaymentID;
            }
			set { }
        }
		string IAddress.FirstName
        {
            get
            {
				return BillingFirstName;
            }
            set
            {
				BillingFirstName = value;
            }
        }
		string IAddress.LastName
        {
            get
            {
				return BillingLastName;
            }
            set
            {
				BillingLastName = value;
            }
        }
		string IAddress.Name
        {
            get
            {
				return BillingName;
            }
            set
            {
				BillingName = value;
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
				return BillingAddress1;
            }
            set
            {
				BillingAddress1 = value;
            }
        }
		string IAddressBasic.Address2
        {
            get
            {
				return BillingAddress2;
            }
            set
            {
				BillingAddress2 = value;
            }
        }
		string IAddressBasic.Address3
        {
            get
            {
				return BillingAddress3;
            }
            set
            {
				BillingAddress3 = value;
            }
        }
		string IAddressBasic.City
        {
            get
            {
				return BillingCity;
            }
            set
            {
				BillingCity = value;
            }
        }
		string IAddressBasic.County
        {
            get
            {
				return BillingCounty;
            }
            set
            {
				BillingCounty = value;
            }
        }		 
		string IAddressBasic.State
        {
            get
            {
				return BillingState;
            }
            set
            {
				BillingState = value;
            }
        }
		int? IAddress.StateProvinceID
        {
            get
            {
				return BillingStateProvinceID;
            }
            set
            {
				BillingStateProvinceID = value;
            }
        }
		string IAddressBasic.PostalCode
        {
            get
            {
				return BillingPostalCode;
            }
            set
            {
				BillingPostalCode = value;
            }
        }
		string IAddress.PhoneNumber
        {
            get
            {
				return BillingPhoneNumber;
            }
            set
            {
				BillingPhoneNumber = value;
            }
        }
		public string CountryCode
		{
			get
			{
				var country = SmallCollectionCache.Instance.Countries.GetById((this as IAddress).CountryID);
				return country.CountryCode;
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
        int IAddress.CountryID
        {
            get
            {
				return BillingCountryID ?? 0;
            }
            set
            {
				BillingCountryID = value;
            }
        }
        short IAddress.AddressTypeID
        {
            get
            {
				return (short)Constants.AddressType.Billing;
            }
            set
            {
            }
        }
        bool IAddress.IsDefault
        {
            get
            {
                return false;
            }
            set
            {
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
                return 0;
            }
            set
            {
            }
        }
        string IAddress.ProfileName
        {
            get
            {
                return string.Empty;
            }
            set
            {
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
		#endregion

        #region IPayment
        string IPayment.CVV
        {
            get
            {
                return string.Empty;
            }
            set
            {
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
                return _nameOnCard;
            }
            set
            {
                _nameOnCard = value;
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
                Address.CopyPropertiesTo(value, this);
            }
        }
        #endregion

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
		
		public virtual string GetDisplayName()
        {
            try
            {
                return BusinessLogic.GetDisplayName(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<OrderPaymentResult> LoadOrderPaymentResultsByOrderPaymentID(int orderPaymentID)
        {
            try
            {
                return Repository.LoadOrderPaymentResultsByOrderPaymentID(orderPaymentID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        // BR-CC-007
        public static IOrderPayment LoadOrderPaymentByOrderPaymentId(int orderPaymentID)
        {
            try
            {
                return Repository.LoadOrderPaymentByOrderPaymentId(orderPaymentID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static IOrderPayment LoadOrderPaymentByOrderId(int orderID)
        {
            try
            {
                return Repository.LoadOrderPaymentByOrderId(orderID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        // BR-CC-007  Fin

        public static bool HasOrderPaymentResults(int orderPaymentID)
        {
            try
            {
                return Repository.HasOrderPaymentResults(orderPaymentID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #region ITempGuid Members

        private Guid? _guid = null;
        public Guid Guid
        {
            get
            {
                if (_guid == null)
                    _guid = Guid.NewGuid();
                return _guid.Value;
            }
            internal set
            {
                _guid = value;
            }
        }

        #endregion

        public BasicResponse IsPaymentValidForAuthorization()
        {
            try
            {
                return BusinessLogic.IsPaymentValidForAuthorization(Repository, this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

       
            public static int ApplyPayment()
            {
                var orderStatusID = 4;
                return orderStatusID;
            }

            public void Register(OrderPaymentNegotiationData oOrderPaymentSearchData)
            {
                try
                {
                    OrderPaymentsRepository Repository = new OrderPaymentsRepository();
                    Repository.RegisterOrderPayment(oOrderPaymentSearchData);
                    //return BusinessLogic.Search(Repository, orderSearchParameters);
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }

            public static List<OrderPaymentVirtualDesktop> SearchOrderPaymentVirtualDesktop(int accountid)
            {
                try
                {
                    OrderPaymentRepository Repository = new OrderPaymentRepository();
                    return Repository.TableOrderPaymentVirtualDesktop(accountid);
                        
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }

    }
}
