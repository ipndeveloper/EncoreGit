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
    [ContainerRegister(typeof(IMaterialBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class MaterialBusinessLogic : BusinessLogicBase<Material, Int32, IMaterialRepository, IMaterialBusinessLogic>, IMaterialBusinessLogic, IDefaultImplementation
    {
    	public override Func<Material, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.MaterialID;
    		}
    	}
    	public override Action<Material, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (Material i, Int32 id) => i.MaterialID = id;
    		}
    	}
    	public override Func<Material, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<Material, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (Material i, string title) => i.Name = title;
    		}
    	}
    }
}