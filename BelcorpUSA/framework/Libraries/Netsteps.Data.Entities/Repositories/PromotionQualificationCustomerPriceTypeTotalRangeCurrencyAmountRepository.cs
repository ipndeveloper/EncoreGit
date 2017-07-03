namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Utility;
using NetSteps.Data.Entities.EntityModels;

    /// <summary>
    /// Implementacion de la interface Inteface
    /// </summary>
    public partial class PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountRepository : IPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountRepository
    {
        public PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto GetByAmount(decimal amount)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var result = (from p in context.PromoPromotions
                              join pq in context.PromoPromotionQualifications on p.PromotionID equals pq.PromotionID
                              join pqcpttrca in context.PromoPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts on pq.PromotionQualificationID equals pqcpttrca.PromotionQualificationID
                              where amount >= pqcpttrca.MinimumAmount && amount < pqcpttrca.MaximumAmount
                              select new PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto()
                              {
                                  PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID = pqcpttrca.PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID,
                                  PromotionQualificationID = pqcpttrca.PromotionQualificationID,
                                  CurrencyID = pqcpttrca.CurrencyID,
                                  MinimumAmount = pqcpttrca.MinimumAmount,
                                  MaximumAmount = pqcpttrca.MaximumAmount,
                                  Cumulative = pqcpttrca.Cumulative,
                                  PromotionID = p.PromotionID,
                                  Description = p.Description
                                  //PromotionQualificationId = pq.PromotionQualificationID
                              }).FirstOrDefault();

                return result;
            }
        }

        PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto TableToDto(PromoPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountTable table)
        {
            if (table == null)
                return null;

            return new PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto()
            {
                PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID = table.PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID,
                PromotionQualificationID = table.PromotionQualificationID,
                CurrencyID = table.CurrencyID,
                MinimumAmount = table.MinimumAmount,
                MaximumAmount = table.MaximumAmount,
                Cumulative = table.Cumulative                
            };
        }


        public PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto GetById(int id)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var result = (from pqcpttrca in context.PromoPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts
                              where pqcpttrca.PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID == id
                              select new PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto()
                              {
                                  PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID = pqcpttrca.PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID,
                                  PromotionQualificationID = pqcpttrca.PromotionQualificationID,
                                  CurrencyID = pqcpttrca.CurrencyID,
                                  MinimumAmount = pqcpttrca.MinimumAmount,
                                  MaximumAmount = pqcpttrca.MaximumAmount,
                                  Cumulative = pqcpttrca.Cumulative                                
                              }).FirstOrDefault();

                return result;
            }
        }


        public PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto GetByPromotionID(int id)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var result = (from p in context.PromoPromotions
                              join pq in context.PromoPromotionQualifications on p.PromotionID equals pq.PromotionID
                              join pqcpttrca in context.PromoPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts on pq.PromotionQualificationID equals pqcpttrca.PromotionQualificationID
                              where p.PromotionID == id
                              select new PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto()
                              {
                                  PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID = pqcpttrca.PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID,
                                  PromotionQualificationID = pqcpttrca.PromotionQualificationID,
                                  CurrencyID = pqcpttrca.CurrencyID,
                                  MinimumAmount = pqcpttrca.MinimumAmount,
                                  MaximumAmount = pqcpttrca.MaximumAmount,
                                  Cumulative = pqcpttrca.Cumulative
                              }).FirstOrDefault();

                return result;
            }
        }
    }
}