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
    [ContainerRegister(typeof(IAutoshipScheduleDayBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AutoshipScheduleDayBusinessLogic : BusinessLogicBase<AutoshipScheduleDay, Int32, IAutoshipScheduleDayRepository, IAutoshipScheduleDayBusinessLogic>, IAutoshipScheduleDayBusinessLogic, IDefaultImplementation
    {
    	public override Func<AutoshipScheduleDay, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AutoshipScheduleDayID;
    		}
    	}
    	public override Action<AutoshipScheduleDay, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (AutoshipScheduleDay i, Int32 id) => i.AutoshipScheduleDayID = id;
    		}
    	}
    }
}
