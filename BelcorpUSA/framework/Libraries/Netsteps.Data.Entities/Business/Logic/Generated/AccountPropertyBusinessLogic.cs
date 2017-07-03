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
    [ContainerRegister(typeof(IAccountPropertyBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AccountPropertyBusinessLogic : BusinessLogicBase<AccountProperty, Int32, IAccountPropertyRepository, IAccountPropertyBusinessLogic>, IAccountPropertyBusinessLogic, IDefaultImplementation
    {
    	public override Func<AccountProperty, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AccountPropertyID;
    		}
    	}
    	public override Action<AccountProperty, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (AccountProperty i, Int32 id) => i.AccountPropertyID = id;
    		}
    	}
    }
}
