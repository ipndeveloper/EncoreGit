using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.TokenReplacement
{
    public class TokenValueProvider : ITokenValueProvider
    {
        protected Dictionary<string, string> _tokenLookup = new Dictionary<string, string>();

        public TokenValueProvider(Dictionary<string, string> tokensAndValues)
        {
            if (tokensAndValues == null)
                throw new ArgumentNullException("tokensAndValues");

            foreach (var keyValuePair in tokensAndValues)
                _tokenLookup[keyValuePair.Key] = keyValuePair.Value;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return _tokenLookup.Keys.ToList<string>();
        }

        public string GetTokenValue(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Parameter \"token\" cannot be null or empty.");

            if (!_tokenLookup.Keys.Contains(token))
                return null;

            return _tokenLookup[token];
        }
    }
}
