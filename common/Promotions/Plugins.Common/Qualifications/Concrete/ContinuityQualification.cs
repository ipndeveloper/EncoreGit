using System;
using System.Collections.Generic;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
    public class ContinuityQualification : IContinuityQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.ContinuityProviderKey; }
        }

        public const string propHasContinuity = "HasContinuity";

        public bool HasContinuity { get; set; }

        public int PromotionQualificationID { get; set; }


        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propHasContinuity:
                    var continuity = Convert.ToBoolean(value);
                    return HasContinuity.Equals(continuity);
                default:
                    return true;
            }
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propHasContinuity }; }
        }
    }
}
