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
    [ContainerRegister(typeof(IOrderItemPropertyTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OrderItemPropertyTypeBusinessLogic : BusinessLogicBase<OrderItemPropertyType, Int32, IOrderItemPropertyTypeRepository, IOrderItemPropertyTypeBusinessLogic>, IOrderItemPropertyTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<OrderItemPropertyType, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.OrderItemPropertyTypeID;
    		}
    	}
    	public override Action<OrderItemPropertyType, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (OrderItemPropertyType i, Int32 id) => i.OrderItemPropertyTypeID = id;
    		}
    	}
    	public override Func<OrderItemPropertyType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<OrderItemPropertyType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (OrderItemPropertyType i, string title) => i.Name = title;
    		}
    	}
    }
}
