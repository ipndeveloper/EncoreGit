namespace NetSteps.Data.Entities.Business.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Data.Entities.Repositories.Interfaces;

    /// <summary>
    /// Logic Methods for Plan Object
    /// </summary>
    public class PlanLogic
    {
        
        #region Constructor - Singleton
        private PlanLogic()
        {
            
        }

        /// <summary>
        /// Gets Singleton instance of the PlanLogic class
        /// </summary>
        public static PlanLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlanLogic(); 
                    repository = new PlanRepository();
                }

                return instance;
            }
        }
        #endregion

        #region privates

        /// <summary>
        /// Gets or sets static instance of the PlanLogic
        /// </summary>
        private static PlanLogic instance;

        /// <summary>
        /// Gets or sets static implementation of the interface IPlanRepository
        /// </summary>
        private static IPlanRepository repository;

        /// <summary>
        /// Map from Data Trasfer Object to Business Objects
        /// </summary>
        /// <param name="dto">Plan dto</param>
        /// <returns>Plan Business Object</returns>
        private Plan DtoToBO(Dto.PlanDto dto)
        {
            return new Plan()
            {
                PlanId = dto.PlanId,
                PlanCode = dto.PlanCode,
                Name = dto.Name,
                Enabled = dto.Enabled,
                DefaultPlan = dto.DefaultPlan,
                TermName = dto.TermName
            };
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get all Plans
        /// </summary>
        /// <returns>List of Plans</returns>
        public List<Plan> GetAll() {
            var data = (from r in repository.GetAll()
                        select DtoToBO(r)).ToList();

            return data;
        }

        #endregion
    }
}
