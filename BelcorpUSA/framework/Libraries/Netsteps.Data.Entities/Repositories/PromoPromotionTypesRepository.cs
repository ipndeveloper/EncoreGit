using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Dto;

namespace NetSteps.Data.Entities.Repositories
{
    /// <summary>
    /// This class implements the IPromoPromotionTypesRepository interface.
    /// </summary>
    public class PromoPromotionTypesRepository : IPromoPromotionTypesRepository
    {
        /// <summary>
        /// Returns the active promotio types
        /// </summary>
        /// <returns>A generic list of PromoPromotionTypesDto class</returns>
        public List<PromoPromotionTypesDto> ListPromotionTypes()
        {
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from PromType in DbContext.PromoPromotionTypes
                            select new PromoPromotionTypesDto()
                            {
                                PromotionTypeID = PromType.PromotionTypeID,
                                TermName = PromType.TermName, 
                                 Name = PromType.Name,
                                  Active = PromType.Active,
                                   SortIndex     = PromType.SortIndex
                            });
                if (data == null)
                    throw new Exception("Promotion Types were not found");

                return data.ToList();
            }
        }


        public int GetActive()
        {
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from PromType in DbContext.PromoPromotionTypes
                            where PromType.Active
                            select PromType.PromotionTypeID);
                if (data == null)
                    throw new Exception("Promotion Types were not found");

                return data.FirstOrDefault();
            }
        }
    }
}
