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
    [ContainerRegister(typeof(IHostessRewardRuleBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class HostessRewardRuleBusinessLogic : BusinessLogicBase<HostessRewardRule, Int32, IHostessRewardRuleRepository, IHostessRewardRuleBusinessLogic>, IHostessRewardRuleBusinessLogic, IDefaultImplementation
    {
    	public override Func<HostessRewardRule, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.HostessRewardRuleID;
    		}
    	}
    	public override Action<HostessRewardRule, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (HostessRewardRule i, Int32 id) => i.HostessRewardRuleID = id;
    		}
    	}
    }
}
