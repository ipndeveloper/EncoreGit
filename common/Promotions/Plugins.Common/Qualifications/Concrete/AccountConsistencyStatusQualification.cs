using System;
using System.Collections.Generic;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class AccountConsistencyStatusQualification : IAccountConsistencyStatusQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.AccountConsistencyStatusProviderKey; }
        }

        public const string propAccountConsistencyStatus = "AccountConsistencyStatusID";

        public AccountConsistencyStatusQualification()
        {
            AccountConsistencyStatuses = new List<short>();
        }

        public IList<short> AccountConsistencyStatuses { get; private set; }

        public int PromotionQualificationID { get; set; }


        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propAccountConsistencyStatus:
                    var accountConsistencyStatusID = Convert.ToInt16(value);
                    return AccountConsistencyStatuses.Contains(accountConsistencyStatusID);
                default:
                    return true;
            }
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propAccountConsistencyStatus }; }
        }
    }
}
