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
    [ContainerRegister(typeof(IPhoneTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class PhoneTypeBusinessLogic : BusinessLogicBase<PhoneType, Int32, IPhoneTypeRepository, IPhoneTypeBusinessLogic>, IPhoneTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<PhoneType, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.PhoneTypeID;
    		}
    	}
    	public override Action<PhoneType, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (PhoneType i, Int32 id) => i.PhoneTypeID = id;
    		}
    	}
    	public override Func<PhoneType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<PhoneType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (PhoneType i, string title) => i.Name = title;
    		}
    	}
    }
}
