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
    [ContainerRegister(typeof(IAutoshipScheduleTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AutoshipScheduleTypeBusinessLogic : BusinessLogicBase<AutoshipScheduleType, Int32, IAutoshipScheduleTypeRepository, IAutoshipScheduleTypeBusinessLogic>, IAutoshipScheduleTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<AutoshipScheduleType, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AutoshipScheduleTypeID;
    		}
    	}
    	public override Action<AutoshipScheduleType, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (AutoshipScheduleType i, Int32 id) => i.AutoshipScheduleTypeID = id;
    		}
    	}
    	public override Func<AutoshipScheduleType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<AutoshipScheduleType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (AutoshipScheduleType i, string title) => i.Name = title;
    		}
    	}
    }
}
