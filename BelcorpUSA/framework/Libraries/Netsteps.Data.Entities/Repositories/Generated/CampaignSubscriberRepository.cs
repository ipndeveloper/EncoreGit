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
    [ContainerRegister(typeof(ICampaignSubscriberRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class CampaignSubscriberRepository : BaseRepository<CampaignSubscriber, Int32, NetStepsEntities>, ICampaignSubscriberRepository, IDefaultImplementation
    { 
    }
}
