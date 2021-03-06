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
    [ContainerRegister(typeof(ITimeUnitTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class TimeUnitTypeBusinessLogic : BusinessLogicBase<TimeUnitType, Int16, ITimeUnitTypeRepository, ITimeUnitTypeBusinessLogic>, ITimeUnitTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<TimeUnitType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.TimeUnitTypeID;
    		}
    	}
    	public override Action<TimeUnitType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (TimeUnitType i, Int16 id) => i.TimeUnitTypeID = id;
    		}
    	}
    	public override Func<TimeUnitType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<TimeUnitType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (TimeUnitType i, string title) => i.Name = title;
    		}
    	}
    }
}
