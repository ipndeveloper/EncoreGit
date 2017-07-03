using System;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities
{
    [Serializable]
    public class NonAccountPaymentMethod : CloneableBase<NonAccountPaymentMethod>, IPayment
    {
        #region IPayment Members

        public string NameOnCard { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string RoutingNumber { get; set; }

        public string BankName { get; set; }

        public short? BankAccountTypeID { get; set; }

        public string DecryptedAccountNumber { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string CVV { get; set; }

        public int PaymentTypeID { get; set; }

        public IAddress BillingAddress { get; set; }

        public bool IsDefault { get; set; }

        public short? PaymentGatewayID { get; set; }

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
    }
}
