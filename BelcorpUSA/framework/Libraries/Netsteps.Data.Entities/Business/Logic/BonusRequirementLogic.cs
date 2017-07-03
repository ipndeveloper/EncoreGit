namespace NetSteps.Data.Entities.Business.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Exceptions;
    using System;

    /// <summary>
    /// Methos for BonusRequirement business Object
    /// </summary>
    public class BonusRequirementLogic
    {
        
        #region constructor - singleton
        /// <summary>
        /// Prevents a default instance of the BonusRequirementLogic class.
        /// </summary>
        private BonusRequirementLogic()
        {   
        }

        /// <summary>
        /// Gets instance of the BonusRequirementLogic class using singleton pattern
        /// </summary>
        public static BonusRequirementLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BonusRequirementLogic();
                    //Injection TODO: use IOC
                    repositoryBonus = new BonusRequirementRepository();
                }

                return instance;
            }
        }
        #endregion

        #region privates

        /// <summary>
        /// Gets or sets BonusRequirementLogic class
        /// </summary>
        private static BonusRequirementLogic instance;

        /// <summary>
        /// gets or sets IBonusRequirementRepository implementation
        /// </summary>
        private static IBonusRequirementRepository repositoryBonus;

        /// <summary>
        /// Transform Dto Object in Business Object
        /// </summary>
        /// <param name="dto">Bonus Type Dto</param>
        /// <returns>Bonus Type BO</returns>
        private BonusRequirement DtoToBO(NetSteps.Data.Entities.Dto.BonusRequirementDto dto)
        {
            //mappper TODO: use something to mapper(ex: Automapper)
            return new BonusRequirement()
            {
                BonusAmount = dto.BonusAmount,
                BonusMaxAmount = dto.BonusMaxAmount,
                BonusMaxPercent = dto.BonusMaxPercent,
                BonusPercent = dto.BonusPercent,
                BonusTypeId = dto.BonusTypeId,
                CountryId = dto.CountryId,
                CurrencyTypeId = dto.CurrencyTypeId,
                EffectiveDate = dto.EffectiveDate,
                BonusRequirementId = dto.BonusRequirementId,
                RequirementTreeId = dto.RequirementsTreeId,
                MinTitleId = dto.MinTitleId,
                MaxTitleId = dto.MaxTitleId,
                BonusMinAmount = dto.BonusMinAmount,
                PayMonth = dto.PayMonth,
                PlanName = dto.PlanName,
                BonusTypeName = dto.BonusTypeName,
                RowsCount = dto.RowsCount
            };
        }

        /// <summary>
        /// Transform Business Object in Dto Object
        /// </summary>
        /// <param name="dto">Bonus Type BO</param>
        /// <returns>Bonus Type dto</returns>
        private NetSteps.Data.Entities.Dto.BonusRequirementDto BOtoDto(BonusRequirement bo)
        {
            return new Dto.BonusRequirementDto()
            {
                BonusAmount = bo.BonusAmount,
                BonusMaxAmount = bo.BonusMaxAmount,
                BonusMaxPercent = bo.BonusMaxPercent,
                BonusPercent = bo.BonusPercent,
                BonusTypeId = bo.BonusTypeId,
                CountryId = bo.CountryId,
                CurrencyTypeId = bo.CurrencyTypeId,
                EffectiveDate = bo.EffectiveDate,
                BonusRequirementId = bo.BonusRequirementId,
                RequirementsTreeId = bo.RequirementTreeId,
                MinTitleId = bo.MinTitleId,
                MaxTitleId = bo.MaxTitleId,
                BonusMinAmount = bo.BonusMinAmount,
                PayMonth = bo.PayMonth
            };
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets BonusRequirement by Id
        /// </summary>
        /// <param name="id">Bonus Requirement Id</param>
        /// <returns>Bonus Requirement Object</returns>
        public BonusRequirement GetById(int id)
        {

            var data = repositoryBonus.GetById(id);

            return DtoToBO(data);
        }

        /// <summary>
        /// Gets all by filters
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <returns>List of BonusRequirement</returns>
        public List<BonusRequirement> GetAllByFilters(int planId, int bonusTypeId)
        {
            var data = repositoryBonus.GetAllByFilters(planId, bonusTypeId);

            return (from r in data
                    select DtoToBO(r)).ToList();
        }

        /// <summary>
        /// Gets all by filters
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <param name="page">Page to take</param>
        /// <param name="pageSize">Rows count to take</param>
        /// <returns>List of BonusRequirement</returns>
        public List<BonusRequirement> GetAllByFilters(int planId, int bonusTypeId, int page, int pageSize)
        {
            var data = repositoryBonus.GetAllByFilters(planId, bonusTypeId, page, pageSize);

            return (from r in data
                    select DtoToBO(r)).ToList();
        }

        /// <summary>
        /// insert a new Bonus Requirement
        /// </summary>
        /// <param name="dto">Bonus Requirement Dto</param>
        public void Insert(BonusRequirement model)
        {
            repositoryBonus.Insert(BOtoDto(model));
        }

        /// <summary>
        /// Update Bonus Requirement
        /// </summary>
        /// <param name="dto">Bonus Requirement Dto</param>
        public void Update(BonusRequirement model)
        {
            repositoryBonus.Update(BOtoDto(model));
        }

        /// <summary>
        /// Delete a Bonus Requirement by Id
        /// </summary>
        /// <param name="id">Bonus Requirement Id</param>
        public void Delete(int id)
        {
            repositoryBonus.Delete(id);
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// REQ: BR- BR-BO-002
        /// Registra el bono de avance a pagar a las consultoras que alcanzaron el titulo de empresaria por primera vez
        /// </summary>
        /// <param name="PeriodID"></param>
        public void InsAdvanceBonus(int PeriodID)
        {
            try
            {
                repositoryBonus.InsAdvanceBonus(PeriodID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion        


    }
}
