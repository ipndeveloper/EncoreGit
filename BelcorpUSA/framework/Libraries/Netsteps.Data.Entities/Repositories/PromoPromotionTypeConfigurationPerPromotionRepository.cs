namespace NetSteps.Data.Entities.Repositories
{
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.EntityModels;
    using System.Linq;

    public class PromoPromotionTypeConfigurationPerPromotionRepository : IPromoPromotionTypeConfigurationPerPromotionRepository
    {
        #region Private

        private PromoPromotionTypeConfigurationPerPromotionTable DtoToTable(PromoPromotionTypeConfigurationPerPromotionDto dto)
        {
            if (dto == null)
                return null;

            return new PromoPromotionTypeConfigurationPerPromotionTable()
            {
                PromotionTypeConfigurationPerPromotionID = dto.PromotionTypeConfigurationPerPromotionID,
                PromotionTypeConfigurationID = dto.PromotionTypeConfigurationID,
                PromotionID = dto.PromotionID
            };
        }

        #endregion

        /// <summary>
        /// Inserts a new record
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>        
        public bool Insert(PromoPromotionTypeConfigurationPerPromotionDto dto)
        {
            var result = false;
            try
            {
                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    var data = DbContext.PromoPromotionTypeConfigurationPerPromotions.Add(DtoToTable(dto));
                    DbContext.SaveChanges();
                    result = true;
                    return result;
                }
            }
            catch (System.Exception) { result = false; return result; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool Delete(PromoPromotionTypeConfigurationPerPromotionDto dto)
        {
            var result = false;
            try
            {
                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    var obj = (from r in DbContext.PromoPromotionTypeConfigurationPerPromotions
                               where r.PromotionTypeConfigurationPerPromotionID == dto.PromotionTypeConfigurationPerPromotionID
                               select r).FirstOrDefault();

                    var data = DbContext.PromoPromotionTypeConfigurationPerPromotions.Remove(obj);
                    DbContext.SaveChanges();
                    result = true;
                    return result;
                }
            }
            catch (System.Exception) { result = false; return result; }
        }

        public bool IsAssociated(int promotionId, int promotionTypeConfigurationId)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = from r in context.PromoPromotionTypeConfigurationPerPromotions
                           where r.PromotionID == promotionId
                           && r.PromotionTypeConfigurationID == promotionTypeConfigurationId
                           select r;

                return data.Any();
            }
        }
    }
}