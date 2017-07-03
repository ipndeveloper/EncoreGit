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
    [ContainerRegister(typeof(IAccountDeviceBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AccountDeviceBusinessLogic : BusinessLogicBase<AccountDevice, Int32, IAccountDeviceRepository, IAccountDeviceBusinessLogic>, IAccountDeviceBusinessLogic, IDefaultImplementation
    {
    	public override Func<AccountDevice, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AccountDeviceID;
    		}
    	}
    	public override Action<AccountDevice, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (AccountDevice i, Int32 id) => i.AccountDeviceID = id;
    		}
    	}
    }
}