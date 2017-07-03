using System;
using System.Collections.Generic;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class CustomerPriceTypeTotalRangeQualification : ICustomerPriceTypeTotalRangeQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.CustomerPriceTypeTotalRangeProviderKey; }
        }
        
        public const string propMarketID = "MarketID";

        public CustomerPriceTypeTotalRangeQualification()
        {
            CustomerPriceTypeTotalRangesByCurrencyID = new Dictionary<int, ICustomerPriceTypeTotalRange>();
        }

        public IDictionary<int, ICustomerPriceTypeTotalRange> CustomerPriceTypeTotalRangesByCurrencyID { get; private set; }

        public int PromotionQualificationID { get; set; }

        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propMarketID:
                    var marketID = Convert.ToInt32(value);
                    return CustomerPriceTypeTotalRangesByCurrencyID.ContainsKey(marketID);
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
