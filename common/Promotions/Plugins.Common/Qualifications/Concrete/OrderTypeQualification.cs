using System;
using System.Collections.Generic;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class OrderTypeQualification : IOrderTypeQualificationExtension
    {
        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.OrderTypeProviderKey; }
        }

        public const string propOrderType = "OrderTypeID";

        public OrderTypeQualification()
        {
            OrderTypes = new List<int>();
        }
        public IList<int> OrderTypes { get; private set; }

        public int PromotionQualificationID { get; set; }

        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propOrderType:
                    var marketID = Convert.ToInt32(value);
                    return OrderTypes.Contains(marketID);
                default:
                    return true;
            }
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propOrderType }; }
        }

    }
}
