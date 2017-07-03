using System;
using System.Collections.Generic;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class OrderSubtotalRangeQualification : IOrderSubtotalRangeQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.OrderSubtotalRangeProviderKey; }
        }
        
        public const string propMarketID = "MarketID";

        public OrderSubtotalRangeQualification()
        {
            OrderSubtotalRangesByCurrencyID = new Dictionary<int, IOrderSubtotalRange>();
        }

        public IDictionary<int, IOrderSubtotalRange> OrderSubtotalRangesByCurrencyID { get; private set; }

        public int PromotionQualificationID { get; set; }

        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propMarketID:
                    var marketID = Convert.ToInt32(value);
                    return OrderSubtotalRangesByCurrencyID.ContainsKey(marketID);
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
