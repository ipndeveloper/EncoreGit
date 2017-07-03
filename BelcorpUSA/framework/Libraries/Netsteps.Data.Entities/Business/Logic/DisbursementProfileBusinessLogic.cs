namespace NetSteps.Data.Entities.Business.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Common.Base;
    using System;
    using NetSteps.Commissions.Common.Models;
    using NetSteps.Data.Entities.Business.HelperObjects;

    public partial class DisbursementProfileBusinessLogic
    {
        
        #region constructor - singleton
        /// <summary>
        /// Prevents a default instance of the AccountSponsorBusinessLogic class.
        /// </summary>
        private DisbursementProfileBusinessLogic()
        {   
        }

        /// <summary>
        /// Gets instance of the AccountSponsorBusinessLogic class using singleton pattern
        /// </summary>
        public static DisbursementProfileBusinessLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DisbursementProfileBusinessLogic();
                    //Injection TODO: use IOC
                    repository = new DisbursementProfileRepository();
                }

                return instance;
            }
        }
        #endregion

        #region privates

        /// <summary>
        /// Gets or sets AccountSponsorBusinessLogic class
        /// </summary>
        private static DisbursementProfileBusinessLogic instance;

        /// <summary>
        /// gets or sets IAccountSponsorRepository implementation
        /// </summary>
        private static IDisbusementProfileRepository repository;

        #endregion

        #region Methods

        public void SaveCheckDisbursementProfile(EFTAccount EFTAccount)
        {
            try{
                repository.SaveCheckDisbursementProfile(EFTAccount);                
            }
            catch(Exception ex){
                throw ex;
            }
        }

        #endregion        
    }
}
