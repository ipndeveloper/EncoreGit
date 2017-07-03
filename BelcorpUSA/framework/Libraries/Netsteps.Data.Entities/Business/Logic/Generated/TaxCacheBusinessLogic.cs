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
    [ContainerRegister(typeof(ITaxCacheBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class TaxCacheBusinessLogic : BusinessLogicBase<TaxCache, Int32, ITaxCacheRepository, ITaxCacheBusinessLogic>, ITaxCacheBusinessLogic, IDefaultImplementation
    {
    	public override Func<TaxCache, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.TaxCacheID;
    		}
    	}
    	public override Action<TaxCache, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (TaxCache i, Int32 id) => i.TaxCacheID = id;
    		}
    	}
    }
}