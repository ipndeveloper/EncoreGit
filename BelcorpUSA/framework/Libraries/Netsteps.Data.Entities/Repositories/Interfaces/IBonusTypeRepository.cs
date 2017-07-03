namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Interface for BonusType
    /// </summary>
    public interface IBonusTypeRepository
    {
        /// <summary>
        /// Gets all Bonus Types joined with plans
        /// </summary>
        /// <returns>List of Bonus Types</returns>
        List<BonusTypeDto> GetAllByCommission();

        /// <summary>
        /// Gets all
        /// </summary>
        /// <returns>List of Bonus Types</returns>
        List<BonusTypeDto> GetAll();
    }
}
