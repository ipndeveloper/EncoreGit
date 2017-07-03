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
    [ContainerRegister(typeof(IStateProvinceBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class StateProvinceBusinessLogic : BusinessLogicBase<StateProvince, Int32, IStateProvinceRepository, IStateProvinceBusinessLogic>, IStateProvinceBusinessLogic, IDefaultImplementation
    {
    	public override Func<StateProvince, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.StateProvinceID;
    		}
    	}
    	public override Action<StateProvince, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (StateProvince i, Int32 id) => i.StateProvinceID = id;
    		}
    	}
    	public override Func<StateProvince, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<StateProvince, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (StateProvince i, string title) => i.Name = title;
    		}
    	}
    }
}
