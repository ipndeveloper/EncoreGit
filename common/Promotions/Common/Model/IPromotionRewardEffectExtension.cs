using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Extensibility.Core;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Common.Model
{
    [ContractClass(typeof(Contracts.PromotionRewardEffectExtensionContract))]
    public interface IPromotionRewardEffectExtension : IPromotionRewardEffectSimpleExtension
    {
        string ExtensionProviderKey { get; }
    }
}

namespace NetSteps.Promotions.Common.Model.Contracts
{
    [ContractClassFor(typeof(IPromotionRewardEffectExtension))]
    public abstract class PromotionRewardEffectExtensionContract : IPromotionRewardEffectExtension
    {

        public string ExtensionProviderKey
        {
            get
            {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
                return string.Empty;
            }
        }

        public int PromotionRewardEffectID { get; set; }
    }
}