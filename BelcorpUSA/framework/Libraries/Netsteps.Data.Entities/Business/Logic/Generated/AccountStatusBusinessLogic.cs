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
    [ContainerRegister(typeof(IAccountStatusBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AccountStatusBusinessLogic : BusinessLogicBase<AccountStatus, Int16, IAccountStatusRepository, IAccountStatusBusinessLogic>, IAccountStatusBusinessLogic, IDefaultImplementation
    {
    	public override Func<AccountStatus, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AccountStatusID;
    		}
    	}
    	public override Action<AccountStatus, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (AccountStatus i, Int16 id) => i.AccountStatusID = id;
    		}
    	}
    	public override Func<AccountStatus, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<AccountStatus, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (AccountStatus i, string title) => i.Name = title;
    		}
    	}
    }
}
