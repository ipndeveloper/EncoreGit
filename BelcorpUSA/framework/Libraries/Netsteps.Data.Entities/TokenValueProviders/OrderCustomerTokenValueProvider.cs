using System.Collections.Generic;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class OrderCustomerTokenValueProvider : ITokenValueProvider
    {
        #region Tokens
        protected const string CUSTOMER_NAME = "CustomerName";
        protected const string CUSTOMER_SUBTOTAL = "CustomerSubtotal";
        protected const string CUSTOMER_TOTAL = "CustomerTotal";
        protected const string CUSTOMER_TAXTOTAL = "CustomerTaxTotal";
        protected const string CUSTOMER_SHIPPINGTOTAL = "CustomerShippingTotal";
        protected const string CUSTOMER_HANDLINGTOTAL = "CustomerHandlingTotal";
        protected const string CUSTOMER_SHIPPINGANDHANDLINGTOTAL = "CustomerShippingAndHandlingTotal";
        #endregion

        private OrderCustomer _customer;

        public OrderCustomerTokenValueProvider(OrderCustomer customer)
        {
            _customer = customer;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>()
            {
                CUSTOMER_NAME,
                CUSTOMER_SUBTOTAL,
                CUSTOMER_TOTAL,
                CUSTOMER_TAXTOTAL,
                CUSTOMER_SHIPPINGTOTAL,
                CUSTOMER_HANDLINGTOTAL,
                CUSTOMER_SHIPPINGANDHANDLINGTOTAL
            };
        }

        public virtual string GetTokenValue(string token)
        {
            if (_customer == null)
                return null;
            switch (token)
            {
                case CUSTOMER_NAME:
                    return _customer.FullName;
                case CUSTOMER_SUBTOTAL:
                    return _customer.Subtotal.ToString(_customer.Order.CurrencyID);
                case CUSTOMER_TOTAL:
                    return _customer.Total.ToString(_customer.Order.CurrencyID);
                case CUSTOMER_SHIPPINGTOTAL:
                    return _customer.ShippingTotal.ToString(_customer.Order.CurrencyID);
                case CUSTOMER_HANDLINGTOTAL:
                    return _customer.HandlingTotal.ToString(_customer.Order.CurrencyID);
                case CUSTOMER_SHIPPINGANDHANDLINGTOTAL:
                    return (_customer.ShippingTotal + _customer.HandlingTotal).ToString(_customer.Order.CurrencyID);
                default:
                    return null;
            }
        }
    }

    public class FakeOrderCustomerTokenValueProvider : OrderCustomerTokenValueProvider
    {
        private int _currencyId;

        public FakeOrderCustomerTokenValueProvider()
            : base(null)
        {
            _currencyId = (int)Constants.Currency.UsDollar;
        }

        public FakeOrderCustomerTokenValueProvider(int currencyId)
            : base(null)
        {
            _currencyId = currencyId;
        }

        public override string GetTokenValue(string token)
        {
            switch (token)
            {
                case CUSTOMER_NAME:
                    return "John Customer";
                case CUSTOMER_SUBTOTAL:
                    return 0M.ToString(_currencyId);
                case CUSTOMER_TOTAL:
                    return 0M.ToString(_currencyId);
                case CUSTOMER_SHIPPINGTOTAL:
                    return 0M.ToString(_currencyId);
                case CUSTOMER_HANDLINGTOTAL:
                    return 0M.ToString(_currencyId);
                case CUSTOMER_SHIPPINGANDHANDLINGTOTAL:
                    return 0M.ToString(_currencyId);
                default:
                    return null;
            }
        }
    }
}
