using System;
using System.Collections.Generic;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class ActivityStatusQualification : IActivityStatusQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.ActivityStatusProviderKey; }
        }

        public const string propActivityStatus = "ActivityStatusID";

        public ActivityStatusQualification()
        {
            ActivityStatuses = new List<short>();
        }

        public IList<short> ActivityStatuses { get; private set; }

        public int PromotionQualificationID { get; set; }


        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propActivityStatus:
                    var activityStatusID = Convert.ToInt16(value);
                    return ActivityStatuses.Contains(activityStatusID);
                default:
                    return true;
            }
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propActivityStatus }; }
        }
    }
}
