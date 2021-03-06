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
    [ContainerRegister(typeof(IShippingRateTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class ShippingRateTypeBusinessLogic : BusinessLogicBase<ShippingRateType, Int16, IShippingRateTypeRepository, IShippingRateTypeBusinessLogic>, IShippingRateTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<ShippingRateType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.ShippingRateTypeID;
    		}
    	}
    	public override Action<ShippingRateType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (ShippingRateType i, Int16 id) => i.ShippingRateTypeID = id;
    		}
    	}
    	public override Func<ShippingRateType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<ShippingRateType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (ShippingRateType i, string title) => i.Name = title;
    		}
    	}
    }
}
