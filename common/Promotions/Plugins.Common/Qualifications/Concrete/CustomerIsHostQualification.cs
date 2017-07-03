using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
    [Serializable]
    public class CustomerIsHostQualification : ICustomerIsHostQualificationExtension
    {
        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propOrderCustomerType }; }
        }

        public const string propOrderCustomerType = "OrderCustomerTypeID";

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.CustomerIsHostProviderKey; }
        }

        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propOrderCustomerType:
                    var orderCustomerTypeID = Convert.ToInt16(value);
                    return orderCustomerTypeID == 2; // This shouldn't be hardcoded but must be at present.
                default:
                    return true;
            }
        }

        public int PromotionQualificationID { get; set; }
    }
}
