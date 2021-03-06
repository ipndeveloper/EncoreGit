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
    [ContainerRegister(typeof(IOrderItemReturnBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OrderItemReturnBusinessLogic : BusinessLogicBase<OrderItemReturn, Int32, IOrderItemReturnRepository, IOrderItemReturnBusinessLogic>, IOrderItemReturnBusinessLogic, IDefaultImplementation
    {
    	public override Func<OrderItemReturn, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.OrderItemReturnID;
    		}
    	}
    	public override Action<OrderItemReturn, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (OrderItemReturn i, Int32 id) => i.OrderItemReturnID = id;
    		}
    	}
    }
}
