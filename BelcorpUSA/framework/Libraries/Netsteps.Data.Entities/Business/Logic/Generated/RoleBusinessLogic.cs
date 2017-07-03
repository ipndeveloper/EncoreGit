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
    [ContainerRegister(typeof(IRoleBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class RoleBusinessLogic : BusinessLogicBase<Role, Int32, IRoleRepository, IRoleBusinessLogic>, IRoleBusinessLogic, IDefaultImplementation
    {
    	public override Func<Role, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.RoleID;
    		}
    	}
    	public override Action<Role, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (Role i, Int32 id) => i.RoleID = id;
    		}
    	}
    	public override Func<Role, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<Role, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (Role i, string title) => i.Name = title;
    		}
    	}
    }
}