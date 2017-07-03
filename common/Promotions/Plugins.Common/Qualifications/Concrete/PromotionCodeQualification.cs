using System;
using System.Collections.Generic;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class PromotionCodeQualification : IPromotionCodeQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.PromotionCodeProviderKey; }
        }

        public const string propCouponCode = "CouponCode";
        public const string propPromotionCode = "PromotionCode";

        public string PromotionCode { get; set; }

        public int PromotionQualificationID { get; set; }


        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propPromotionCode:
                case propCouponCode:
                    var couponCode = Convert.ToString(value);
                    return PromotionCode.Equals(couponCode, StringComparison.InvariantCultureIgnoreCase);
                default:
                    return true;
            }
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propCouponCode, propPromotionCode }; }
        }
    }
}
