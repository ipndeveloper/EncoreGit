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
    [ContainerRegister(typeof(IRoleTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class RoleTypeBusinessLogic : BusinessLogicBase<RoleType, Int16, IRoleTypeRepository, IRoleTypeBusinessLogic>, IRoleTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<RoleType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.RoleTypeID;
    		}
    	}
    	public override Action<RoleType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (RoleType i, Int16 id) => i.RoleTypeID = id;
    		}
    	}
    	public override Func<RoleType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<RoleType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (RoleType i, string title) => i.Name = title;
    		}
    	}
    }
}
