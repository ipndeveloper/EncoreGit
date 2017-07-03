using System.Collections.Generic;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class MockTokenValueProvider : ITokenValueProvider
    {
        private IEnumerable<string> _tokenPlaceholders { get; set; }
        private int _languageID { get; set; }

        public MockTokenValueProvider(IEnumerable<string> tokenPlaceholders, int languageID)
        {
            _tokenPlaceholders = tokenPlaceholders;
            _languageID = languageID;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return _tokenPlaceholders;
        }

        public string GetTokenValue(string token)
        {
            return Translation.GetTerm(_languageID, "PreviewTokenValue" + token);
        }
    }
}
