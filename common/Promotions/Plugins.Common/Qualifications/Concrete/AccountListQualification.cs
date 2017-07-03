using System;
using System.Collections.Generic;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class AccountListQualification : IAccountListQualificationExtension
    {
        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.AccountListProviderKey; }
        }

        public const string propAccount = "AccountID";

        public AccountListQualification()
        {
            AccountNumbers = new List<int>();
        }

        public IList<int> AccountNumbers { get; private set; }

        public int PromotionQualificationID { get; set; }

        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propAccount:
                    var accountID = Convert.ToInt32(value);
                    return AccountNumbers.Contains(accountID);
                default:
                    return true;
            }
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propAccount }; }
        }
    }
}
