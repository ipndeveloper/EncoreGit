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
    [ContainerRegister(typeof(IOrderStatusBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OrderStatusBusinessLogic : BusinessLogicBase<OrderStatus, Int16, IOrderStatusRepository, IOrderStatusBusinessLogic>, IOrderStatusBusinessLogic, IDefaultImplementation
    {
    	public override Func<OrderStatus, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.OrderStatusID;
    		}
    	}
    	public override Action<OrderStatus, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (OrderStatus i, Int16 id) => i.OrderStatusID = id;
    		}
    	}
    	public override Func<OrderStatus, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<OrderStatus, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (OrderStatus i, string title) => i.Name = title;
    		}
    	}
    }
}
