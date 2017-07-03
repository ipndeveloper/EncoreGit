using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Dto;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    /// <summary>
    /// Interface for PromoPromotionTypes
    /// </summary>
    public interface IPromoPromotionTypesRepository
    {
        List<PromoPromotionTypesDto> ListPromotionTypes();

        int GetActive();
    }
}
