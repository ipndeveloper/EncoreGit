using System;

namespace NetSteps.Data.Entities.Business.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Interface for Entities with a ModifiedByUserID property.
    /// Used to set the UserID for use with the Audit Trigger.
    /// Created: 06-17-2010
    /// </summary>
    public interface IModifiedByUserID
    {
        Nullable<int> ModifiedByUserID { get; set; }
    }
}
