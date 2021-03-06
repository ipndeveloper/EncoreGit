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
    [ContainerRegister(typeof(ICorporateUserBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class CorporateUserBusinessLogic : BusinessLogicBase<CorporateUser, Int32, ICorporateUserRepository, ICorporateUserBusinessLogic>, ICorporateUserBusinessLogic, IDefaultImplementation
    {
    	public override Func<CorporateUser, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.CorporateUserID;
    		}
    	}
    	public override Action<CorporateUser, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (CorporateUser i, Int32 id) => i.CorporateUserID = id;
    		}
    	}
    }
}
