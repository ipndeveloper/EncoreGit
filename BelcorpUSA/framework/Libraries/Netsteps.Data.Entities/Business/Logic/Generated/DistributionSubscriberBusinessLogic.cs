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
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Business.Logic
{
    [ContainerRegister(typeof(IDistributionSubscriberBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class DistributionSubscriberBusinessLogic : BusinessLogicBase<DistributionSubscriber, Int32, IDistributionSubscriberRepository, IDistributionSubscriberBusinessLogic>, IDistributionSubscriberBusinessLogic, IDefaultImplementation
    {
    	public override Func<DistributionSubscriber, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.DistributionSubscriberID;
    		}
    	}
    	public override Action<DistributionSubscriber, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (DistributionSubscriber i, Int32 id) => i.DistributionSubscriberID = id;
    		}
    	}
    }
}
