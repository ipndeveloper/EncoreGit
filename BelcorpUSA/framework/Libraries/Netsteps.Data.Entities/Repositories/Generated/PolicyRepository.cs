//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Repositories
{
    [ContainerRegister(typeof(IPolicyRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class PolicyRepository : BaseRepository<Policy, Int32, NetStepsEntities>, IPolicyRepository, IDefaultImplementation
    { 
    }
}
