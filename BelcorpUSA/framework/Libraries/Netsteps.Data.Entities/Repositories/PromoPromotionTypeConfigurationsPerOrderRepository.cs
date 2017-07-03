namespace NetSteps.Data.Entities.Repositories
{
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.EntityModels;
    using System.Linq;

    /// <summary>
    /// Implements the IPromoPromotionTypeConfigurationsPerOrderRepository interface.
    /// </summary>
    public class PromoPromotionTypeConfigurationsPerOrderRepository : IPromoPromotionTypeConfigurationsPerOrderRepository
    {
        #region Private

        private PromoPromotionTypeConfigurationsPerOrderTable DtoToTable(PromoPromotionTypeConfigurationsPerOrderDto dto)
        {
            if (dto == null)
                return null;

            return new PromoPromotionTypeConfigurationsPerOrderTable() 
            {
                PromotionTypeConfigurationsPerOrderID = dto.PromotionTypeConfigurationsPerOrderID, 
                PromotionTypeConfigurationID = dto.PromotionTypeConfigurationID,
                IncludeBAorders = dto.IncludeBAorders
            };
        }

        #endregion

        public bool Insert(PromoPromotionTypeConfigurationsPerOrderDto dto)
        {
            var result = false;
            try
            {
                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    DbContext.PromoPromotionTypeConfigurationsPerOrders.Add(DtoToTable(dto));
                    DbContext.SaveChanges();
                    result = true;
                    return result;
                }
            }
            catch (System.Exception) { result = false; return result; }
        }

        public bool GetBA()
        {
            var result = true;
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (DbContext.PromoPromotionTypeConfigurationsPerOrders.OrderByDescending( r=> r.PromotionTypeConfigurationsPerOrderID )).FirstOrDefault();

                if (data == null)
                    result = true;
                else
                    result = data.IncludeBAorders;

                return result;
            }
        }


        public bool GetBA(int promotionTypeConfigurationId)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = from r in context.PromoPromotionTypeConfigurationsPerOrders
                           where r.PromotionTypeConfigurationID == promotionTypeConfigurationId
                           select r.IncludeBAorders;

                return data.First();
            }
        }
    }
}
