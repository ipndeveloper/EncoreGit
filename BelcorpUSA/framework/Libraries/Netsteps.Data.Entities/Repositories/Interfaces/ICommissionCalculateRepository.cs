namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    public interface ICommissionCalculateRepository
    {
        /// <summary>
        /// Calculate Commission By Personal Volumen
        /// </summary>
        /// <param name="accountsPicked">Accounts picked</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <param name="pediodId">Period Id</param>
        void CommissionByPersonalVolumen(IEnumerable<AccountPerformanceDataDto> accountsPicked, int bonusTypeId, int periodId);

        /// <summary>
        /// Calculate Commission By Level
        /// </summary>
        /// <param name="accountsPicked">Accounts picked</param>
        /// <param name="level">Commision Level</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <param name="periodId">Period Id</param>
        void CommissionByLevels(IEnumerable<AccountPerformanceDataDto> accountsPicked, int level, int bonusTypeId, int periodId);

        /// <summary>
        /// Calculate commission by group sales
        /// </summary>
        /// <param name="accountsPicked">Accounts picked</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <param name="periodId">Period Id</param>
        void CommissionByGroupSales(IEnumerable<AccountPerformanceDataDto> accountsPicked, int bonusTypeId, int periodId);

        /// <summary>
        /// Calculate commission by Generation
        /// </summary>
        /// <param name="accountsPicked">Accounts picked</param>
        /// <param name="generation">Generation to calculate</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <param name="periodId">Period Id</param>
        void CommissionByGeneration(IEnumerable<AccountPerformanceDataDto> accountsPicked, int generation, int bonusTypeId, int periodId);

        /// <summary>
        /// Save Total
        /// </summary>
        /// <param name="periodId">Period Id</param>
        void SaveTotal(int periodId);
    }
}
