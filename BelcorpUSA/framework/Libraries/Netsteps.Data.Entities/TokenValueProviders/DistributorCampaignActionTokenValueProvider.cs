using System;
using System.Collections.Generic;
using NetSteps.Common.Extensions;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class DistributorCampaignActionTokenValueProvider : TokenValueProvider
    {
        private Dictionary<string, string> _inputValues;
        private string _contentSubfolder;

        public DistributorCampaignActionTokenValueProvider(Dictionary<string, string> tokenValues, string contentSubfolder)
            : base(new Dictionary<string, string>())
        {
            _inputValues = tokenValues;
            _contentSubfolder = contentSubfolder;

            AddValueToBase(Constants.Token.DistributorImage, x => x.AddWebUploadPath(Constants.Token.DistributorImage.ToString(), _contentSubfolder));
            AddValueToBase(Constants.Token.DistributorContent);
        }

        private void AddValueToBase(Constants.Token token, Func<string, string> valueTransform = null)
        {
            string key = token.ToPlaceholder();

            if (!_inputValues.ContainsKey(key))
                return;

            string value = _inputValues[key];

            // Apply any custom formatting to the value.
            if (valueTransform != null)
                value = valueTransform(value);

            base._tokenLookup[key] = value;
        }
    }
}
