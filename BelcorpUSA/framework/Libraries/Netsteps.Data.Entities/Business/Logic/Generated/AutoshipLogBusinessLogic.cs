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
    [ContainerRegister(typeof(IAutoshipLogBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AutoshipLogBusinessLogic : BusinessLogicBase<AutoshipLog, Int32, IAutoshipLogRepository, IAutoshipLogBusinessLogic>, IAutoshipLogBusinessLogic, IDefaultImplementation
    {
    	public override Func<AutoshipLog, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AutoshipLogID;
    		}
    	}
    	public override Action<AutoshipLog, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (AutoshipLog i, Int32 id) => i.AutoshipLogID = id;
    		}
    	}
    }
}
