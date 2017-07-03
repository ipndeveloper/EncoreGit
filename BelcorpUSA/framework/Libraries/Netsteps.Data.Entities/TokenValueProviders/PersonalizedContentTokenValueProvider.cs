using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class PersonalizedContentTokenValueProvider : ITokenValueProvider
    {
        #region Tokens
        private const string DISTRIBUTOR_CONTENT = "DistributorContent";
        private const string DISTRIBUTOR_IMAGE = "DistributorImage";
        #endregion

        private string _distributorContent;
        private string _distributorImage;

        public PersonalizedContentTokenValueProvider(string distributorContent, string distributorImage)
        {
            _distributorContent = distributorContent;
            _distributorImage = distributorImage;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>()
            {
                DISTRIBUTOR_CONTENT,
                DISTRIBUTOR_IMAGE
            };
        }

        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case DISTRIBUTOR_CONTENT:
                    return _distributorContent;
                case DISTRIBUTOR_IMAGE:
                    return _distributorImage;
                default:
                    return null;
            }
        }
    }
}
