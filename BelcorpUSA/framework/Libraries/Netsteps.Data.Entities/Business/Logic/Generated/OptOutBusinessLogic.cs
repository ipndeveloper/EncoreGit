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
    [ContainerRegister(typeof(IOptOutBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OptOutBusinessLogic : BusinessLogicBase<OptOut, Int32, IOptOutRepository, IOptOutBusinessLogic>, IOptOutBusinessLogic, IDefaultImplementation
    {
    	public override Func<OptOut, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.OptOutID;
    		}
    	}
    	public override Action<OptOut, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (OptOut i, Int32 id) => i.OptOutID = id;
    		}
    	}
    }
}
