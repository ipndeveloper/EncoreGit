namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Interface for Plan
    /// </summary>
    public interface IPlanRepository
    {
        /// <summary>
        /// Gets all plans
        /// </summary>
        /// <returns>List of Plan Dto</returns>
        List<PlanDto> GetAll();
    }
}
