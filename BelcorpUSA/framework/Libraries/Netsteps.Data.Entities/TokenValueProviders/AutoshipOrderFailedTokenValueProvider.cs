using System;
using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class AutoshipOrderFailedTokenValueProvider : ITokenValueProvider
    {
        private const string DISTRIBUTOR_WORKSTATION_URL = "DistributorWorkstationUrl";
        private const string ORDER_PAYMENT_CREDIT_CARD_LASTFOUR = "OrderPaymentCreditCardLastFour";

        private readonly string _distributorWorkstationUrl;
        private readonly OrderPayment _orderPayment;

        public AutoshipOrderFailedTokenValueProvider(OrderPayment orderPayment, string distributorWorkstationUrl)
        {
            _orderPayment = orderPayment;
            _distributorWorkstationUrl = distributorWorkstationUrl;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string> { DISTRIBUTOR_WORKSTATION_URL, ORDER_PAYMENT_CREDIT_CARD_LASTFOUR };
        }

        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case DISTRIBUTOR_WORKSTATION_URL:
                    return !String.IsNullOrEmpty(_distributorWorkstationUrl)
                               ? string.Format("{0}", _distributorWorkstationUrl)
                               : string.Empty;

                case ORDER_PAYMENT_CREDIT_CARD_LASTFOUR:
                    return _orderPayment != null ? _orderPayment.MaskedAccountNumber : string.Empty;

                default:
                    return null;
            }
        }
    }
}
