using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Extensibility.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class UseCountQualification : IUseCountQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get { return QualificationExtensionProviderKeys.UseCountProviderKey; }
        }

        public const string propAccountID = "AccountID";

        public int MaximumUseCount { get; set; }
        public bool FirstOrdersOnly { get; set; }

        public int PromotionQualificationID { get; set; }

        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            var registry = Create.New<IDataObjectExtensionProviderRegistry>();
            var handler = registry.RetrieveExtensionProviderForRegisteredProvidedType(typeof(UseCountQualification).ToString()) as IPromotionQualificationHandler;
            return handler.ValidFor<TProperty>(this, propertyName, value);
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get { return new string[] { propAccountID }; }
        }
    }
}
