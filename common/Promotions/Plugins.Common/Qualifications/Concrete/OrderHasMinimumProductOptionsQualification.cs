using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class OrderHasMinimumProductSelectionsQualification : IOrderHasMinimumProductSelectionsQualificationExtension
    {
        public OrderHasMinimumProductSelectionsQualification()
        {
			ProductOptions = new List<IProductOption>();
        }

        public const string productIDProperty = "ProductID";

        public int NumberOfOptionsRequired { get; set; }

		public IList<IProductOption> ProductOptions { get; private set; }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { productIDProperty }; }
        }

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.OrderHasMinimumProductOptionsProviderKey; }
        }

        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            return true;
        }

        public int PromotionQualificationID { get; set; }
    }
}
