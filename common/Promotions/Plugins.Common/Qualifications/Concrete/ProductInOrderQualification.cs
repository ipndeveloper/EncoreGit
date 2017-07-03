using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class ProductInOrderQualification : IProductInOrderQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.ProductInOrderProviderKey; }
        }

        public const string propProductID = "ProductID";

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public int PromotionQualificationID { get; set; }


        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            return true;
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propProductID }; }
        }
    }
}
