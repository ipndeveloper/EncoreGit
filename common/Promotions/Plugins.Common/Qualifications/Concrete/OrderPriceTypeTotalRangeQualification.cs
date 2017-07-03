using System;
using System.Collections.Generic;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class OrderPriceTypeTotalRangeQualification : IOrderPriceTypeTotalRangeQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.OrderPriceTypeTotalRangeProviderKey; }
        }
        
        public const string propMarketID = "MarketID";

        public OrderPriceTypeTotalRangeQualification()
        {
            OrderPriceTypeTotalRangesByCurrencyID = new Dictionary<int, IOrderPriceTypeTotalRange>();
        }

        public IDictionary<int, IOrderPriceTypeTotalRange> OrderPriceTypeTotalRangesByCurrencyID { get; private set; }

        public int PromotionQualificationID { get; set; }

        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propMarketID:
                    var marketID = Convert.ToInt32(value);
                    return OrderPriceTypeTotalRangesByCurrencyID.ContainsKey(marketID);
                default:
                    return true;
            }
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propMarketID }; }
        }

        public int ProductPriceTypeID { get; set; }
    }
}
