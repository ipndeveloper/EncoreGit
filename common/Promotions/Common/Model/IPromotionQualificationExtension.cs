using NetSteps.Encore.Core.Dto;
using NetSteps.Extensibility.Core;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System;

namespace NetSteps.Promotions.Common.Model
{
    [ContractClass(typeof(Contracts.PromotionQualificationExtensionContract))]
    public interface IPromotionQualificationExtension : IPromotionQualificationSimpleExtension
    {
        string ExtensionProviderKey { get; }
        IEnumerable<string> AssociatedPropertyNames { get; }
        bool ValidFor<TProperty>(string propertyName, TProperty value);
	}

    
}

namespace NetSteps.Promotions.Common.Model.Contracts
{
    [ContractClassFor(typeof(IPromotionQualificationExtension))]
    public abstract class PromotionQualificationExtensionContract : IPromotionQualificationExtension
    {

        public string ExtensionProviderKey
        {
            get
            {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
                return string.Empty;
            }
        }

        public IEnumerable<string> AssociatedPropertyNames
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
                return new string[] { };
            }
        }

        public bool ValidFor<TProperty>(string propertyName, TProperty value)
        {
            Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(propertyName));
            return false;
        }

        public int PromotionQualificationID { get; set; }
	}
}