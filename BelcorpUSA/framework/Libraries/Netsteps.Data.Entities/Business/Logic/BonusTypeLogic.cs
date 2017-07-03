namespace NetSteps.Data.Entities.Business.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Data.Entities.Repositories.Interfaces;

    /// <summary>
    /// Method for Bonus Type Business Object
    /// </summary>
    public class BonusTypeLogic
    {        
        #region Constructor - Singleton
        /// <summary>
        /// Gets instance of the BonusTypeLogic class using singleton pattern
        /// </summary>
        public static BonusTypeLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BonusTypeLogic();
                    repository = new BonusTypeRepository();
                }

                return instance;
            }
        }
     
        #endregion
        
        #region privates
        /// <summary>
        /// prevents a default instance of the BonusTypeLogic class.
        /// </summary>
        private BonusTypeLogic()
        { }

        /// <summary>
        /// Gets or sets static instance of the BonusTypeLogic class.
        /// </summary>
        private static BonusTypeLogic instance;

        /// <summary>
        /// Gets or sets static repository
        /// </summary>
        private static IBonusTypeRepository repository;

        /// <summary>
        /// Transform Dto Object in Business Object
        /// </summary>
        /// <param name="dto">Bonus Type Dto</param>
        /// <returns>Bonus Type BO</returns>
        private BonusType DtoToBO(Dto.BonusTypeDto dto)
        {
            return new BonusType()
            {
                BonusCode = dto.BonusCode,
                BonusTypeId = dto.BonusTypeId,
                ClientCode = dto.ClientCode,
                ClientName = dto.ClientName,
                EarningsTypeId = dto.EarningsTypeId,
                Editable = dto.Editable,
                Enabled = dto.Enabled,
                IsCommission = dto.IsCommission,
                Name = dto.Name,
                PlanId = dto.PlanId,
                PlanName = dto.PlanName,
                TermName = dto.TermName
            };
        }
        #endregion 

        #region Public Methods

        /// <summary>
        /// Gets all Bonus Type by Commission marks
        /// </summary>
        /// <returns>List of Bonus Types</returns>       
        public List<BonusType> GetAllByCommission()
        {
            return (from r in repository.GetAllByCommission()
                       select DtoToBO(r)).ToList();   
        }

        /// <summary>
        /// Gets all Bonus Types
        /// </summary>
        /// <returns>List of Bonus Types</returns>       
        public List<BonusType> GetAll()
        {
            return (from r in repository.GetAll()
                    select DtoToBO(r)).ToList();
        }
        #endregion
    }
}
