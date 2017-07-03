namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Data.Entities.EntityModels;

    /// <summary>
    /// Implementation of the IActivityStatusRepository Interface
    /// </summary>
    [ContainerRegister(typeof(IActivityStatusRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class ActivityStatusRepository : IActivityStatusRepository
    {
        /// <summary>
        /// Gets all
        /// </summary>
        /// <returns>List of activity status</returns>
        public List<ActivityStatus> GetAll()
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from r in context.ActivityStatuses
                            select r);

                if (data == null)
                    throw new Exception("Activity Status not found");

                return data.ToList();
            }
        }
    }
}