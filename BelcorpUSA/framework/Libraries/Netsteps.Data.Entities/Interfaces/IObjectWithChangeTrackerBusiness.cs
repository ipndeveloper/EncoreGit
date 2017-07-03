using System.Collections.Generic;

namespace NetSteps.Data.Entities.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Adds functionality to IObjectWithChangeTracker.
    /// Created: 06-01-2010
    /// </summary>
    public interface IObjectWithChangeTrackerBusiness
    {
        void StartEntityTracking();
        void StopEntityTracking();
        /// <summary>
        /// An AcceptChanges method that each Entity can override. - JHE
        /// </summary>
        void AcceptEntityChanges(List<IObjectWithChangeTracker> allTrackerItems = null);
    }
}
