namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Interface for BonusRequirement
    /// </summary>
    public interface IBonusRequirementRepository
    {
        /// <summary>
        /// Get all bonus requirement
        /// </summary>
        /// <returns>List of bonus requirement dto</returns>
        List<BonusRequirementDto> GetAll();

        /// <summary>
        /// Get all bonus requirement by filters
        /// </summary>
        /// <param name="planId">plan id</param>
        /// <param name="bonusTypeId">bonus type id</param>
        /// <returns>List of bonus requirement dto</returns>
        List<BonusRequirementDto> GetAllByFilters(int planId, int bonusTypeId);

        /// <summary>
        /// Get all bonus requirement by filters
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <param name="page">Page to take</param>
        /// <param name="pageSize">Rows count to take</param>
        /// <returns>List of bonus requirement dto</returns>
        List<BonusRequirementDto> GetAllByFilters(int planId, int bonusTypeId, int page, int pageSize);

        /// <summary>
        /// Get Bonus Requirement by Id
        /// </summary>
        /// <param name="id">Bonus Requirement Id</param>
        /// <returns>Bonus Requirement Dto</returns>
        BonusRequirementDto GetById(int id);

        /// <summary>
        /// insert a new Bonus Requirement
        /// </summary>
        /// <param name="dto">Bonus Requirement Dto</param>
        void Insert(BonusRequirementDto dto);

        /// <summary>
        /// Update Bonus Requirement
        /// </summary>
        /// <param name="dto">Bonus Requirement Dto</param>
        void Update(BonusRequirementDto dto);

        /// <summary>
        /// Delete a Bonus Requirement by Id
        /// </summary>
        /// <param name="id">Bonus Requirement Id</param>
        void Delete(int id);

        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-BO-002 BONO DE AVANCE
        /// </summary>
        /// <param name="PeriodID"></param>
        void InsAdvanceBonus(int PeriodID);
    }
}
