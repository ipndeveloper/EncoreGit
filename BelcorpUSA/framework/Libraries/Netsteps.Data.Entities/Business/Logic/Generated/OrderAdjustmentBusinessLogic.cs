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
    [ContainerRegister(typeof(IOrderAdjustmentBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OrderAdjustmentBusinessLogic : BusinessLogicBase<OrderAdjustment, Int32, IOrderAdjustmentRepository, IOrderAdjustmentBusinessLogic>, IOrderAdjustmentBusinessLogic, IDefaultImplementation
    {
    	public override Func<OrderAdjustment, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.OrderAdjustmentID;
    		}
    	}
    	public override Action<OrderAdjustment, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (OrderAdjustment i, Int32 id) => i.OrderAdjustmentID = id;
    		}
    	}
    	public override Func<OrderAdjustment, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Description;
    		}
    	}
    	public override Action<OrderAdjustment, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (OrderAdjustment i, string title) => i.Description = title;
    		}
    	}
    }
}
