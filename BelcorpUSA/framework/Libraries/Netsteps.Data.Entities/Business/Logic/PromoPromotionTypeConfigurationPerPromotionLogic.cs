using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class PromoPromotionTypeConfigurationPerPromotionLogic
    {
        #region Private

        private static PromoPromotionTypeConfigurationPerPromotionLogic instance;

        private static IPromoPromotionTypeConfigurationPerPromotionRepository repository;

        private static NetSteps.Data.Entities.Dto.PromoPromotionTypeConfigurationPerPromotionDto BoToDto(PromoPromotionTypeConfigurationPerPromotion Bo)
        {
            if (Bo == null)
                return null;

            return new NetSteps.Data.Entities.Dto.PromoPromotionTypeConfigurationPerPromotionDto()
            {
                PromotionTypeConfigurationPerPromotionID = Bo.PromotionTypeConfigurationPerPromotionID,
                PromotionTypeConfigurationID = Bo.PromotionTypeConfigurationID,
                PromotionID = Bo.PromotionID
            };
        }

        #endregion

        #region Singleton

        private PromoPromotionTypeConfigurationPerPromotionLogic() { }

        public static PromoPromotionTypeConfigurationPerPromotionLogic Instance
        { 
            get
            {
                if (instance==null)
	            {
                    instance = new PromoPromotionTypeConfigurationPerPromotionLogic();
                    repository = new PromoPromotionTypeConfigurationPerPromotionRepository();
	            }
                return instance;
            }
        }

        #endregion

        #region Methods

        public bool Insert(PromoPromotionTypeConfigurationPerPromotion Bo)
        {
            return repository.Insert(BoToDto(Bo));
        }

        public bool Delete(PromoPromotionTypeConfigurationPerPromotion Bo)
        {
            return repository.Delete(BoToDto(Bo));
        }

        public bool IsAssociated(int promotionId, int promotionTypeConfigurationId)
        {
            return repository.IsAssociated(promotionId, promotionTypeConfigurationId);
        }

        #endregion        
    }
}
