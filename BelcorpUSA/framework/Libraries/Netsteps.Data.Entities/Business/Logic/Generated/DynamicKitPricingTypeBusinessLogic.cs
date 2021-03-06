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
    [ContainerRegister(typeof(IDynamicKitPricingTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class DynamicKitPricingTypeBusinessLogic : BusinessLogicBase<DynamicKitPricingType, Int32, IDynamicKitPricingTypeRepository, IDynamicKitPricingTypeBusinessLogic>, IDynamicKitPricingTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<DynamicKitPricingType, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.DynamicKitPricingTypeID;
    		}
    	}
    	public override Action<DynamicKitPricingType, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (DynamicKitPricingType i, Int32 id) => i.DynamicKitPricingTypeID = id;
    		}
    	}
    	public override Func<DynamicKitPricingType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<DynamicKitPricingType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (DynamicKitPricingType i, string title) => i.Name = title;
    		}
    	}
    }
}
