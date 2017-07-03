namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Implementation of the ILogCommissionRepository interface 
    /// </summary>
    public partial class LogCommissionRepository : ILogCommissionRepository
    {
        /// <summary>
        /// Gets all Log commission
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LogCommissionDto> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get by Id
        /// </summary>
        /// <returns>Log Commission DTO</returns>
        public LogCommissionDto GetById()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert a new Log Commission
        /// </summary>
        /// <param name="dto">Log Commission Dto</param>
        public void Insert(Dto.LogCommissionDto dto)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                context.LogCommissions.Add(new EntityModels.LogCommissionTable()
                {
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    Result = dto.Result,
                    Description = dto.Description
                });

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete log commission
        /// </summary>
        /// <param name="id">Log Commission Id</param>
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update Log commission
        /// </summary>
        /// <param name="dto">Log Commission Dto</param>
        public void Update(Dto.LogCommissionDto dto)
        {
            throw new NotImplementedException();
        }
    }
}