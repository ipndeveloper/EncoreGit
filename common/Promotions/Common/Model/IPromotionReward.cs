using NetSteps.Encore.Core.Dto;
using NetSteps.Extensibility.Core;
using System.Collections.Generic;
using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Common.Model
{
    [ContractClass(typeof(Contracts.PromotionRewardContract))]
    public interface IPromotionReward
    {
        string PromotionRewardKind { get; }

        int PromotionRewardID { get; set; }

        IDictionary<string, IPromotionRewardEffect> Effects { get; }

        string[] OrderOfApplication { get; }
	}


}

namespace NetSteps.Promotions.Common.Model.Contracts
{
    [ContractClassFor(typeof(IPromotionReward))]
    public abstract class PromotionRewardContract : IPromotionReward
    {
        public string PromotionRewardKind
        {
            get
            {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
                return string.Empty;
            }
        }

        public int PromotionRewardID { get; set; }

        public IDictionary<string, IPromotionRewardEffect> Effects
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, IPromotionRewardEffect>>() != null);
                return new Dictionary<string, IPromotionRewardEffect>();
            }
        }

        public string[] OrderOfApplication
        {
            get
            {
                Contract.Ensures(Contract.Result<string[]>() != null);
                return new string[] { };
            }
        }

		public void CheckValidity(IPromotionState state)
		{
			Contract.Requires<ArgumentNullException>(state != null);
			throw new NotImplementedException();
		}
	}
}

