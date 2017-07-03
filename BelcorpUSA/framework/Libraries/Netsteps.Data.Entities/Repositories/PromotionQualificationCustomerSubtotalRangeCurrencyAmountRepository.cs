namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Utility;
    using System.Collections.Generic;

    /// <summary>
    /// Implementacion de la interface Inteface
    /// </summary>
    public partial class PromotionQualificationCustomerSubtotalRangeCurrencyAmountRepository : IPromotionQualificationCustomerSubtotalRangeCurrencyAmountRepository
    {
        public IEnumerable<PromotionQualificationCustomerSubtotalRangeCurrencyAmountDto> GetByPromotionTypeConfiguration(int promotionTypeConfigurationID)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = from ptcpp in context.PromoPromotionTypeConfigurationPerPromotions
                           join pq in context.PromoPromotionQualifications on ptcpp.PromotionID equals pq.PromotionID
                           join pqcsrca in context.PromotionQualificationCustomerSubtotalRangeCurrencyAmounts on pq.PromotionQualificationID equals pqcsrca.PromotionQualificationID
                           join pw in context.PromoPromotionRewards on ptcpp.PromotionID equals pw.PromotionID
                           join pre in context.PromoPromotionRewardEffects on pw.PromotionRewardID equals pre.PromotionRewardID
                           join preroipvmv in context.PromoPromotionRewardEffectReduceOrderItemPropertyValuesMarketValues on pre.PromotionRewardEffectID equals preroipvmv.PromotionRewardEffectID
                           where ptcpp.PromotionTypeConfigurationID == promotionTypeConfigurationID
                           && pq.PromotionPropertyKey == "Customer Subtotal Range"
                           && pre.RewardPropertyKey == "Retail"
                           select new PromotionQualificationCustomerSubtotalRangeCurrencyAmountDto()
                           {
                               PromotionQualificationCustomerSubtotalRangeCurrencyAmountID = pqcsrca.PromotionQualificationCustomerSubtotalRangeCurrencyAmountID,
                               CurrencyID = pqcsrca.CurrencyID,
                               PromotionQualificationID = pqcsrca.PromotionQualificationID,
                               MinimumAmount = pqcsrca.MinimumAmount,
                               MaximumAmount = pqcsrca.MaximumAmount,
                               PromotionID = pq.PromotionID,
                               Discount = preroipvmv.DecimalValue
                           };

                return data.ToList();
            }
        }
    }
}