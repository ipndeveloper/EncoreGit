using System;
using System.Collections.Generic;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class MarketListQualification : IMarketListQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.MarketListProviderKey; }
        }

        public const string propMarketID = "MarketID";

        public MarketListQualification()
        {
            Markets = new List<int>();
        }

        public IList<int> Markets { get; private set; }

        public int PromotionQualificationID { get; set; }

        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propMarketID:
                    var marketID = Convert.ToInt32(value);
                    return Markets.Contains(marketID);
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
