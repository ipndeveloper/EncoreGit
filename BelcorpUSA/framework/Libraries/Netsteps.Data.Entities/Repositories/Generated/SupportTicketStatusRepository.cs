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
    [ContainerRegister(typeof(ISupportTicketStatusRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class SupportTicketStatusRepository : BaseRepository<SupportTicketStatus, Int16, NetStepsEntities>, ISupportTicketStatusRepository, IDefaultImplementation
    { 
    }
}