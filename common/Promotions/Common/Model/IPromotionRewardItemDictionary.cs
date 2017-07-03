using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Context;
using NetSteps.OrderAdjustments.Common.Model;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Common.Model
{
    [ContractClass(typeof(Contracts.PromotionRewardItemDictionaryContract))]
    public interface IPromotionRewardItemDictionary
    {
        IList<IPromotionRewardItemSelection> Selections { get; }
        IPromotionRewardItemEffect[] GetOrderLineEffects(IPromotionRewardItemSelection selection);
        void AddItemSelection(IPromotionRewardItemSelection selection);
        void AddEffect(IPromotionRewardItemSelection selection, IPromotionRewardItemEffect modification);
        void AddToAdjustmentProfile(IOrderContext context, IOrderAdjustmentProfile adjustmentProfile);
    }
}

namespace NetSteps.Promotions.Common.Model.Contracts
{
    [ContractClassFor(typeof(IPromotionRewardItemDictionary))]
    public abstract class PromotionRewardItemDictionaryContract : IPromotionRewardItemDictionary
    {

        public IList<IPromotionRewardItemSelection> Selections
        {
            get
            {
                Contract.Ensures(Contract.Result<IList<IPromotionRewardItemSelection>>() != null);
                return null;
            }
        }

        public IPromotionRewardItemEffect[] GetOrderLineEffects(IPromotionRewardItemSelection selection)
        {
            Contract.Ensures(Contract.Result<IPromotionRewardItemEffect[]>() != null);
            return null;
        }

        public void AddItemSelection(IPromotionRewardItemSelection selection)
        {
            Contract.Requires<ArgumentNullException>(selection != null);
        }

        public void AddEffect(IPromotionRewardItemSelection selection, IPromotionRewardItemEffect modification)
        {
            Contract.Requires<ArgumentNullException>(selection != null);
            Contract.Requires<ArgumentNullException>(modification != null);
        }

        public void AddToAdjustmentProfile(IOrderContext context, IOrderAdjustmentProfile adjustmentProfile)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(adjustmentProfile != null);
        }
    }
}