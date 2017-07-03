namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Interface for NetworkPermanenceRules
    /// </summary>
    public interface INetworkPermanenceRulesRepository
    {
        /// <summary>
        /// Applies Network Permanence Rules
        /// </summary>
        void ApplyNetworkPermanenceRules();
    }
}
