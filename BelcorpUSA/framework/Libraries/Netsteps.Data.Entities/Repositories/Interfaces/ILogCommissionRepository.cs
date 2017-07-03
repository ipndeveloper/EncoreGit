namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Interface for LogCommission
    /// </summary>
    public interface ILogCommissionRepository
    {
        /// <summary>
        /// Gets all Log commission
        /// </summary>
        /// <returns></returns>
        IEnumerable<LogCommissionDto> GetAll();

        /// <summary>
        /// Get by Id
        /// </summary>
        /// <returns>Log Commission DTO</returns>
        LogCommissionDto GetById();

        /// <summary>
        /// Insert a new Log Commission
        /// </summary>
        /// <param name="dto">Log Commission Dto</param>
        void Insert(LogCommissionDto dto);

        /// <summary>
        /// Delete log commission
        /// </summary>
        /// <param name="id">Log Commission Id</param>
        void Delete(int id);

        /// <summary>
        /// Update Log commission
        /// </summary>
        /// <param name="dto">Log Commission Dto</param>
        void Update(LogCommissionDto dto);
    }
}
