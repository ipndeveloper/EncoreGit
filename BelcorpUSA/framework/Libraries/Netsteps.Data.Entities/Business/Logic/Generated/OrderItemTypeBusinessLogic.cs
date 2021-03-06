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
    [ContainerRegister(typeof(IOrderItemTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OrderItemTypeBusinessLogic : BusinessLogicBase<OrderItemType, Int16, IOrderItemTypeRepository, IOrderItemTypeBusinessLogic>, IOrderItemTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<OrderItemType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.OrderItemTypeID;
    		}
    	}
    	public override Action<OrderItemType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (OrderItemType i, Int16 id) => i.OrderItemTypeID = id;
    		}
    	}
    	public override Func<OrderItemType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<OrderItemType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (OrderItemType i, string title) => i.Name = title;
    		}
    	}
    }
}
