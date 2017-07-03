namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Encore.Core.IoC;

    /// <summary>
    /// Implementation of the IAccountConsistencyStatusRepository Interface
    /// </summary>
    [ContainerRegister(typeof(IAccountConsistencyStatusRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AccountConsistencyStatusRepository : IAccountConsistencyStatusRepository
    {
        
        /// <summary>
        /// Gets all
        /// </summary>
        /// <returns>List of New BA</returns>
        public List<AccountConsistencyStatus> GetAll()
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from r in context.AccountConsistencyStatuses
                            select r);

                if (data == null)
                    throw new Exception("Account Consistency Status not found");

                return data.ToList();
            }
        }
    }
}