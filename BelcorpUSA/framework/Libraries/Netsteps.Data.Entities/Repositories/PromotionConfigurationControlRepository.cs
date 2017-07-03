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
    public partial class PromotionConfigurationControlRepository : IPromotionConfigurationControlRepository
    {
        public void UpdateAmount(int accountId, int periodId, decimal amount, int promotionId)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from r in context.PromoPromotionConfigurationControls
                           where r.AccountID == accountId
                           && r.PeriodID == periodId
                           select r).FirstOrDefault();

                if (data != null)
                {
                    data.Amount += amount;
                }
                else
                {
                    int promotionTypeConfigurationID =(context.PromoPromotionTypeConfigurations.Where(r => r.Active == true).Select(r => r.PromotionTypeConfigurationID)).FirstOrDefault();

                    Insert(new PromoPromotionConfigurationControlDto() 
                    {
                        PromotionTypeConfigurationID = promotionTypeConfigurationID,
                        PromotionID = promotionId,
                        AccountID = accountId,
                        PeriodID = periodId,
                        Amount = amount                     
                    });
                }
               
                context.SaveChanges();
            }
        }

        public void Insert(PromoPromotionConfigurationControlDto dto)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                context.PromoPromotionConfigurationControls.Add(new EntityModels.PromoPromotionConfigurationControlTable()
                {
                     PromotionTypeConfigurationID = dto.PromotionTypeConfigurationID,
                      PromotionID = dto.PromotionID,
                    AccountID = dto.AccountID,
                    PeriodID = dto.PeriodID,
                    Amount = dto.Amount
                });

                context.SaveChanges();
            }
        }

        public PromoPromotionConfigurationControlDto GetByAccount(int accountId)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from r in context.PromoPromotionConfigurationControls
                           where r.AccountID == accountId
                                select r).FirstOrDefault();

                return TableToDto(data);
            }
        }

        public PromoPromotionConfigurationControlDto GetByAccount(int accountId, int periodId)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from r in context.PromoPromotionConfigurationControls
                            where r.AccountID == accountId
                            && r.PeriodID == periodId
                            select r).FirstOrDefault();

                return TableToDto(data);
            }
        }

        public void UpdatePromotion(int promotionTypeConfigurationId, int newPromotionID)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from r in context.PromoPromotionConfigurationControls
                            where r.PromotionConfigurationControlID == promotionTypeConfigurationId
                            select r).FirstOrDefault();

                if (data != null)
                {
                    data.PromotionID = newPromotionID;
                }

                context.SaveChanges();
            }
        }

        private PromoPromotionConfigurationControlDto TableToDto(PromoPromotionConfigurationControlTable table)
        {
            if (table == null)
                return null;

            return new PromoPromotionConfigurationControlDto()
            {
                PromotionTypeConfigurationID = table.PromotionTypeConfigurationID,
                PromotionID = table.PromotionID,
                AccountID = table.AccountID,
                PeriodID = table.PeriodID,
                Amount = table.Amount,
                PromotionConfigurationControlID = table.PromotionConfigurationControlID
            };
        }




        public PromoPromotionConfigurationControlDto GetByPromotionID(int promotionID)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from r in context.PromoPromotionConfigurationControls
                            where r.PromotionID == promotionID
                            select r).FirstOrDefault();

                return TableToDto(data);
            }
        }
    }
}