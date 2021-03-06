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
    [ContainerRegister(typeof(IShippingOrderTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class ShippingOrderTypeBusinessLogic : BusinessLogicBase<ShippingOrderType, Int32, IShippingOrderTypeRepository, IShippingOrderTypeBusinessLogic>, IShippingOrderTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<ShippingOrderType, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.ShippingOrderTypeID;
    		}
    	}
    	public override Action<ShippingOrderType, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (ShippingOrderType i, Int32 id) => i.ShippingOrderTypeID = id;
    		}
    	}
    	public override Func<ShippingOrderType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<ShippingOrderType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (ShippingOrderType i, string title) => i.Name = title;
    		}
    	}
    }
}
