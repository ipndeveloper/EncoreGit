namespace NetSteps.Data.Entities.Business.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Data.Entities.Repositories.Interfaces;

    /// <summary>
    /// Methos for NetworkPermanenceRules business Object
    /// </summary>
    public class NetworkPermanenceRulesBusinessLogic
    {
        
        #region constructor - singleton
        /// <summary>
        /// Prevents a default instance of the NetworkPermanenceRulesBusinessLogic class.
        /// </summary>
        private NetworkPermanenceRulesBusinessLogic()
        {   
        }

        /// <summary>
        /// Gets instance of the NetworkPermanenceRulesBusinessLogic class using singleton pattern
        /// </summary>
        public static NetworkPermanenceRulesBusinessLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NetworkPermanenceRulesBusinessLogic();
                    //Injection TODO: use IOC
                    repositoryNetworkPermanenceRules = new NetworkPermanenceRulesRepository();
                }

                return instance;
            }
        }
        #endregion

        #region privates

        /// <summary>
        /// Gets or sets NetworkPermanenceRulesBusinessLogic class
        /// </summary>
        private static NetworkPermanenceRulesBusinessLogic instance;

        /// <summary>
        /// gets or sets INetworkPermanenceRulesRepository implementation
        /// </summary>
        private static INetworkPermanenceRulesRepository repositoryNetworkPermanenceRules;

        #endregion

        #region Methods

        /// <summary>
        /// Applies Network Permanence Rules
        /// </summary>
        public void ApplyNetworkPermanenceRules()
        {
            repositoryNetworkPermanenceRules.ApplyNetworkPermanenceRules();
        }

        #endregion        
    }
}
