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
    [ContainerRegister(typeof(IShippingRateGroupBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class ShippingRateGroupBusinessLogic : BusinessLogicBase<ShippingRateGroup, Int32, IShippingRateGroupRepository, IShippingRateGroupBusinessLogic>, IShippingRateGroupBusinessLogic, IDefaultImplementation
    {
    	public override Func<ShippingRateGroup, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.ShippingRateGroupID;
    		}
    	}
    	public override Action<ShippingRateGroup, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (ShippingRateGroup i, Int32 id) => i.ShippingRateGroupID = id;
    		}
    	}
    	public override Func<ShippingRateGroup, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<ShippingRateGroup, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (ShippingRateGroup i, string title) => i.Name = title;
    		}
    	}
    }
}
