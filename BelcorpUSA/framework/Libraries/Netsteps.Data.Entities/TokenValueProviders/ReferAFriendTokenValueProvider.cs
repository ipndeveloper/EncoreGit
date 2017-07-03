using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class ReferAFriendTokenValueProvider : ITokenValueProvider
    {
        private const string REFER_URL= "ReferAFriendUrl";
        private const string REFER_ID = "ReferrerID";
        private const string REFER_ACCOUNTTYPE_ID = "ReferralAccountTypeID";
        private const string CUSTOM_BODY = "CustomBody";

        private readonly string _url;
        private readonly string _referrerId;
        private readonly string _accountTypeId;
        private readonly string _customBody;

        public ReferAFriendTokenValueProvider(string url, string referrerId, string accountTypeId, string customBody)
        {
            _url = url;
            _referrerId = referrerId;
            _accountTypeId = accountTypeId;
            _customBody = customBody;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>()
            {
                REFER_URL,
                REFER_ID,
                REFER_ACCOUNTTYPE_ID,
                CUSTOM_BODY
            };  
        }       
                
        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case REFER_URL:
                    return _url;
                case REFER_ID:
                    return _referrerId;
                case REFER_ACCOUNTTYPE_ID:
                    return _accountTypeId;
                case CUSTOM_BODY:
                    return _customBody;
                default:
                    return null;
            }
        }
    }
}
