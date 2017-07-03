using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class ReferAMerchantTokenValueProvider : ITokenValueProvider
    {
        private const string BUSINESS_NAME = "BusinessName";
        private const string CONTACT_NAME = "ContactName";
        private const string MERCHANT_ADDRESS = "MerchantAddress";
        private const string CONTACT_PHONE = "ContactPhone";
        private const string BUSINESS_CATEGORY = "BusinessCategory";
        private const string MERCHANT_REFERRER_ID = "MerchantReferrerId";
        private const string MERCHANT_REFERRER_NAME = "MerchantReferrerName";

        private readonly string _businessName;
        private readonly string _contactName;
        private readonly string _merchantAddress;
        private readonly string _contactPhone;
        private readonly string _businessCategory;
        private readonly string _merchantReferrerId;
        private readonly string _merchantReferrerName;

        public ReferAMerchantTokenValueProvider(string businessName, string contactName, string merchantAddress, string contactPhone, string businessCategory, string merchantReferrerId, string merchantReferrerName)
        {
            _businessName = businessName;
            _contactName = contactName;
            _merchantAddress = merchantAddress;
            _contactPhone = contactPhone;
            _businessCategory = businessCategory;
            _merchantReferrerId = merchantReferrerId;
            _merchantReferrerName = merchantReferrerName;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>()
            {
                BUSINESS_NAME,
                CONTACT_NAME,
                MERCHANT_ADDRESS,
                CONTACT_PHONE,
                BUSINESS_CATEGORY,
                MERCHANT_REFERRER_ID,
                MERCHANT_REFERRER_NAME,
            };
        }

        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case BUSINESS_NAME:
                    return _businessName;
                case CONTACT_NAME:
                    return _contactName;
                case MERCHANT_ADDRESS:
                    return _merchantAddress;
                case CONTACT_PHONE:
                    return _contactPhone;
                case BUSINESS_CATEGORY:
                    return _businessCategory;
                case MERCHANT_REFERRER_ID:
                    return _merchantReferrerId;
                case MERCHANT_REFERRER_NAME:
                    return _merchantReferrerName;
                default:
                    return null;
            }
        }
    }
}
