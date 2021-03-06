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
    [ContainerRegister(typeof(IOrderItemPriceBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OrderItemPriceBusinessLogic : BusinessLogicBase<OrderItemPrice, Int32, IOrderItemPriceRepository, IOrderItemPriceBusinessLogic>, IOrderItemPriceBusinessLogic, IDefaultImplementation
    {
    	public override Func<OrderItemPrice, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.OrderItemPriceID;
    		}
    	}
    	public override Action<OrderItemPrice, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (OrderItemPrice i, Int32 id) => i.OrderItemPriceID = id;
    		}
    	}
    }
}
