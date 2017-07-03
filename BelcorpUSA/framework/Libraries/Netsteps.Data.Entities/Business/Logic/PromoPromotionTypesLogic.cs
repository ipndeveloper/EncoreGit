using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class PromoPromotionTypesLogic
    {
        #region Private

        private static PromoPromotionTypesLogic instance;

        private static IPromoPromotionTypesRepository repositoryPromotionTypes;

        /// <summary>
        /// Transforms Dto object to business object
        /// </summary>
        /// <param name="dto">Promotion Types Dto</param>
        /// <returns>Promotion Types BO</returns>
        private PromoPromotionTypes DtoToBo(NetSteps.Data.Entities.Dto.PromoPromotionTypesDto dto)
        {
            if (dto == null)
                return null;

            return new PromoPromotionTypes()
            {
                PromotionTypeID = dto.PromotionTypeID,
                Name = dto.Name,
                TermName = dto.TermName,
                Active = dto.Active,
                SortIndex = dto.SortIndex
            };
        }

        /// <summary>
        /// Transforms business object to dto
        /// </summary>
        /// <param name="bo">Promotion Types BO</param>
        /// <returns>Promotion Types Dto</returns>
        private NetSteps.Data.Entities.Dto.PromoPromotionTypesDto BOtoDto(PromoPromotionTypes bo)
        {
            if (bo == null)
                return null;

            return new Dto.PromoPromotionTypesDto()
            {
                PromotionTypeID = bo.PromotionTypeID,
                Name = bo.Name,
                TermName = bo.TermName,
                Active = bo.Active,
                SortIndex = bo.SortIndex
            };
        }

        #endregion

        #region Singleton

        private PromoPromotionTypesLogic() { }

        public static PromoPromotionTypesLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PromoPromotionTypesLogic();
                    repositoryPromotionTypes = new PromoPromotionTypesRepository();
                }

                return instance;
            }
        }

        #endregion

        #region Methods

        public List<PromoPromotionTypes> ListPromotionTypes()
        {
            var data = repositoryPromotionTypes.ListPromotionTypes();

            return (from r in data select DtoToBo(r)).ToList();
        }

        public int GetActive()
        {
            return repositoryPromotionTypes.GetActive();
        }
        #endregion
    }
}
