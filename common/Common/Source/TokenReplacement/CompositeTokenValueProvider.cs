using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Interfaces;

namespace NetSteps.Common.TokenReplacement
{
    public class CompositeTokenValueProvider : ITokenValueProvider
    {
        private readonly List<ITokenValueProvider> _tokenValueProviders;

        private Dictionary<ITokenValueProvider, IEnumerable<string>> _knownTokens;

        public CompositeTokenValueProvider(params ITokenValueProvider[] tokenValueProviders)
        {
            if (tokenValueProviders == null || tokenValueProviders.Count() == 0)
                throw new ArgumentNullException("tokenValueProviders");

            _tokenValueProviders = tokenValueProviders.ToList();
            _knownTokens = _tokenValueProviders.ToDictionary(tvp => tvp, tvp => tvp.GetKnownTokens());
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return _knownTokens.SelectMany(kt => kt.Value).Distinct();
        }

        public string GetTokenValue(string token)
        {
            foreach (ITokenValueProvider provider in _tokenValueProviders)
            {
                if (_knownTokens[provider].Contains(token))
                {
                    string tokenValue = provider.GetTokenValue(token);
                    if (tokenValue != null)
                        return tokenValue;
                }
            }
            return null;
        }
    }
}
