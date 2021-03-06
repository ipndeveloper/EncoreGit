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
    [ContainerRegister(typeof(IDomainEventTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class DomainEventTypeBusinessLogic : BusinessLogicBase<DomainEventType, Int16, IDomainEventTypeRepository, IDomainEventTypeBusinessLogic>, IDomainEventTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<DomainEventType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.DomainEventTypeID;
    		}
    	}
    	public override Action<DomainEventType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (DomainEventType i, Int16 id) => i.DomainEventTypeID = id;
    		}
    	}
    	public override Func<DomainEventType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<DomainEventType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (DomainEventType i, string title) => i.Name = title;
    		}
    	}
    }
}
