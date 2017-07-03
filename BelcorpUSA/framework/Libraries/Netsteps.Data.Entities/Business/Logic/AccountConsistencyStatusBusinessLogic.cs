namespace NetSteps.Data.Entities.Business.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Data.Entities.Business.Logic.Interfaces;
    using NetSteps.Data.Entities.Repositories;
    using System;
    using NetSteps.Common.Interfaces;

    /// <summary>
    /// Method for Bonus Type Business Object
    /// </summary>
    [ContainerRegister(typeof(IAccountConsistencyStatusBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AccountConsistencyStatusBusinessLogic : IAccountConsistencyStatusBusinessLogic, IDefaultImplementation
    {
    }
}
