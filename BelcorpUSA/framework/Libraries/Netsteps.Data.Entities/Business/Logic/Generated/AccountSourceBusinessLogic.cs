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
    [ContainerRegister(typeof(IAccountSourceBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AccountSourceBusinessLogic : BusinessLogicBase<AccountSource, Int16, IAccountSourceRepository, IAccountSourceBusinessLogic>, IAccountSourceBusinessLogic, IDefaultImplementation
    {
    	public override Func<AccountSource, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AccountSourceID;
    		}
    	}
    	public override Action<AccountSource, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (AccountSource i, Int16 id) => i.AccountSourceID = id;
    		}
    	}
    	public override Func<AccountSource, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<AccountSource, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (AccountSource i, string title) => i.Name = title;
    		}
    	}
    }
}