using System;
using System.Collections.Generic;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class AccountTypeQualification : IAccountTypeQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.AccountTypeProviderKey; }
        }

        public const string propAccountType = "AccountTypeID";

        public AccountTypeQualification()
        {
            AccountTypes = new List<short>();
        }

        public IList<short> AccountTypes { get; private set; }

        public int PromotionQualificationID { get; set; }


        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            switch (propertyName)
            {
                case propAccountType:
                    var accountID = Convert.ToInt16(value);
                    return AccountTypes.Contains(accountID);
                default:
                    return true;
            }
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propAccountType }; }
        }
    }
}
