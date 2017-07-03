namespace NetSteps.Data.Entities.Business.Logic
{
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Repositories;    

    public class PromoPromotionTypeConfigurationsPerOrderLogic
    {
        #region Private

        private static PromoPromotionTypeConfigurationsPerOrderLogic instance;

        private static IPromoPromotionTypeConfigurationsPerOrderRepository repository;

        private NetSteps.Data.Entities.Dto.PromoPromotionTypeConfigurationsPerOrderDto BoToDto(PromoPromotionTypeConfigurationsPerOrder Bo)
        {
            if (Bo == null)
                return null;

            return new NetSteps.Data.Entities.Dto.PromoPromotionTypeConfigurationsPerOrderDto()
            {
                PromotionTypeConfigurationsPerOrderID = Bo.PromotionTypeConfigurationsPerOrderID,
                PromotionTypeConfigurationID = Bo.PromotionTypeConfigurationID,
                IncludeBAorders = Bo.IncludeBAorders
            };
        }

        #endregion

        #region Singleton

        private PromoPromotionTypeConfigurationsPerOrderLogic() { }

        public static PromoPromotionTypeConfigurationsPerOrderLogic Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new PromoPromotionTypeConfigurationsPerOrderLogic();
                    repository = new PromoPromotionTypeConfigurationsPerOrderRepository();
                }
                return instance;
            }
        }

        #endregion

        #region Methods

        public bool Insert(PromoPromotionTypeConfigurationsPerOrder Bo)
        {
            return repository.Insert(BoToDto(Bo));
        }

        public bool GetBA()
        {
            return repository.GetBA();
        }

        public bool GetBA(int promotionTypeConfigurationId)
        {
            return repository.GetBA(promotionTypeConfigurationId);
        }
        #endregion
    }
}
