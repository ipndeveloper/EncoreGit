using System;
using System.Collections.Generic;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class CustomerSubtotalRangeQualification : ICustomerSubtotalRangeQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.CustomerSubtotalRangeProviderKey; }
        }
        
        public const string propMarketID = "MarketID";

        public CustomerSubtotalRangeQualification()
        {
            CustomerSubtotalRangesByCurrencyID = new Dictionary<int, ICustomerSubtotalRange>();
        }

        public IDictionary<int, ICustomerSubtotalRange> CustomerSubtotalRangesByCurrencyID { get; private set; }

        public int PromotionQualificationID { get; set; }

        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propMarketID:
                    var marketID = Convert.ToInt32(value);
                    return CustomerSubtotalRangesByCurrencyID.ContainsKey(marketID);
                default:
                    return true;
            }
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propMarketID }; }
        }
    }
}
