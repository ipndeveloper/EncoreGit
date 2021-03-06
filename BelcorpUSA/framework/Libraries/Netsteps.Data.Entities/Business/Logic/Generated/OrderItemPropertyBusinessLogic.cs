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
    [ContainerRegister(typeof(IOrderItemPropertyBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OrderItemPropertyBusinessLogic : BusinessLogicBase<OrderItemProperty, Int32, IOrderItemPropertyRepository, IOrderItemPropertyBusinessLogic>, IOrderItemPropertyBusinessLogic, IDefaultImplementation
    {
    	public override Func<OrderItemProperty, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.OrderItemPropertyID;
    		}
    	}
    	public override Action<OrderItemProperty, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (OrderItemProperty i, Int32 id) => i.OrderItemPropertyID = id;
    		}
    	}
    }
}
