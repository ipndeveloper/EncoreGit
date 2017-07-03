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
    [ContainerRegister(typeof(IReturnTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class ReturnTypeBusinessLogic : BusinessLogicBase<ReturnType, Int32, IReturnTypeRepository, IReturnTypeBusinessLogic>, IReturnTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<ReturnType, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.ReturnTypeID;
    		}
    	}
    	public override Action<ReturnType, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (ReturnType i, Int32 id) => i.ReturnTypeID = id;
    		}
    	}
    	public override Func<ReturnType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<ReturnType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (ReturnType i, string title) => i.Name = title;
    		}
    	}
    }
}
