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
    [ContainerRegister(typeof(IAccountStatusChangeReasonBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AccountStatusChangeReasonBusinessLogic : BusinessLogicBase<AccountStatusChangeReason, Int16, IAccountStatusChangeReasonRepository, IAccountStatusChangeReasonBusinessLogic>, IAccountStatusChangeReasonBusinessLogic, IDefaultImplementation
    {
    	public override Func<AccountStatusChangeReason, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AccountStatusChangeReasonID;
    		}
    	}
    	public override Action<AccountStatusChangeReason, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (AccountStatusChangeReason i, Int16 id) => i.AccountStatusChangeReasonID = id;
    		}
    	}
    	public override Func<AccountStatusChangeReason, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<AccountStatusChangeReason, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (AccountStatusChangeReason i, string title) => i.Name = title;
    		}
    	}
    }
}
