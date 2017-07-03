namespace NetSteps.Data.Entities.Business.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories.Interfaces;

    /// <summary>
    /// Metodos de Account Perfomance Data
    /// </summary>
    public class AccountPerformanceDataBusinessLogic
    {
        /// <summary>
        /// Obtiene cuentas asociadas
        /// </summary>
        /// <param name="titleId">Id Titulo</param>
        /// <param name="planId">Id Plan</param>
        /// <param name="countryId">Id Country</param>
        /// <returns>Lista de cuentas</returns>
        public IEnumerable<AccountPerformanceData> GetPickedAccounts(int titleId, int planId, int countryId)
        {
            return from r in repository.GetPickedAccounts(titleId, planId, countryId) select DtoToBO(r);
        }
        
        /// <summary>
        /// Obtiene una instancia singleton de la clase AccountPerformanceDataBusinessLogic
        /// </summary>
        public static AccountPerformanceDataBusinessLogic Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new AccountPerformanceDataBusinessLogic();
                    //IOC
                    repository = new NetSteps.Data.Entities.Repositories.AccountPerformanceDataRepository();
                }

                return instance;
            }
        }

        #region privates

        /// <summary>
        /// Mapper
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public AccountPerformanceData DtoToBO(NetSteps.Data.Entities.Dto.AccountPerformanceDataDto dto)
        {
            return new AccountPerformanceData()
            {
                AccountId = dto.AccountID,
                AccountNumber = dto.AccountNumber,
                AccountStatusId = dto.AccountStatusId,
                AccountTypeId = dto.AccountTypeId,
                AutoshipProcessDate = dto.AutoshipProcessDate,
                BirthdayUTC = dto.BirthdayUTC,
                //TODO
                BonusRequirementBonusPercent = dto.BonusPercent,
                BonusTypeId = dto.BonusTypeID,
                BTL = dto.BTL,
                CalculatedDateUTC = dto.CalculatedDateUTC,
                CQ = dto.CQ,
                CT = dto.CT,
                DQV = dto.DQV,
                EmailAddress = dto.EmailAddress,
                EnrollerId = dto.EnrollerId,
                EnrollmentDateUTC = dto.EnrollmentDateUTC,
                FirstName = dto.FirstName,
                FlatDownline = dto.FlatDownline,
                GQV = dto.GQV,
                HGen = dto.HGen,
                HLevel = dto.HLevel,
                Lastname = dto.Lastname,
                LastOrderCommissionDateUTC = dto.LastOrderCommissionDateUTC,
                LeftBower = dto.LeftBower,
                Location = dto.Location,
                PAT = dto.PAT,
                PCV = dto.PCV,
                PeriodId = dto.PeriodId,
                PQV = dto.PQV,
                RightBower = dto.RightBower,
                SorthPath = dto.SorthPath,
                SponsorId = dto.SponsorId,
                 CurrencyTypeID = dto.CurrencyTypeID
            };
        } 

        /// <summary>
        /// Convierte un objeto Bo a Dto
        /// </summary>
        /// <param name="bo">Account Performance Data</param>
        /// <returns>Account Performance Dto</returns>
        public NetSteps.Data.Entities.Dto.AccountPerformanceDataDto BOToDto(AccountPerformanceData bo)
        {
            return new NetSteps.Data.Entities.Dto.AccountPerformanceDataDto()
            {
                AccountID = bo.AccountId,
                AccountNumber = bo.AccountNumber,
                AccountStatusId = bo.AccountStatusId,
                AccountTypeId = bo.AccountTypeId,
                AutoshipProcessDate = bo.AutoshipProcessDate,
                BirthdayUTC = bo.BirthdayUTC,
                BonusPercent = bo.BonusRequirementBonusPercent,
                BonusTypeID = bo.BonusTypeId,
                BTL = bo.BTL,
                CalculatedDateUTC = bo.CalculatedDateUTC,
                CQ = bo.CQ,
                CT = bo.CT,
                DQV = bo.DQV,
                EmailAddress = bo.EmailAddress,
                EnrollerId = bo.EnrollerId,
                EnrollmentDateUTC = bo.EnrollmentDateUTC,
                FirstName = bo.FirstName,
                FlatDownline = bo.FlatDownline,
                GQV = bo.GQV,
                HGen = bo.HGen,
                HLevel = bo.HLevel,
                Lastname = bo.Lastname,
                LastOrderCommissionDateUTC = bo.LastOrderCommissionDateUTC,
                LeftBower = bo.LeftBower,
                Location = bo.Location,
                PAT = bo.PAT,
                PCV = bo.PCV,
                PeriodId = bo.PeriodId,
                PQV = bo.PQV,
                RightBower = bo.RightBower,
                SorthPath = bo.SorthPath,
                SponsorId = bo.SponsorId,
                 CurrencyTypeID = bo.CurrencyTypeID
            };
        }

        /// <summary>
        /// Previene una instancia por defecto de la clase AccountPerformanceDataBusinessLogic
        /// </summary>
        private AccountPerformanceDataBusinessLogic()
        { }
              
        /// <summary>
        /// Obtiene o establece una instancia de AccountPerformanceDataBusinessLogic
        /// </summary>
        private static AccountPerformanceDataBusinessLogic instance;

        /// <summary>
        /// Obtiene o establece una implementacion de IAccountPerformanceDataRepository
        /// </summary>
        private static IAccountPerformanceDataRepository repository;

        #endregion
    }
}