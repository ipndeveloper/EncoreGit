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
    [ContainerRegister(typeof(IHostessRewardRuleTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class HostessRewardRuleTypeBusinessLogic : BusinessLogicBase<HostessRewardRuleType, Int32, IHostessRewardRuleTypeRepository, IHostessRewardRuleTypeBusinessLogic>, IHostessRewardRuleTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<HostessRewardRuleType, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.HostessRewardRuleTypeID;
    		}
    	}
    	public override Action<HostessRewardRuleType, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (HostessRewardRuleType i, Int32 id) => i.HostessRewardRuleTypeID = id;
    		}
    	}
    	public override Func<HostessRewardRuleType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<HostessRewardRuleType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (HostessRewardRuleType i, string title) => i.Name = title;
    		}
    	}
    }
}
