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
    [ContainerRegister(typeof(IOrderAdjustmentOrderLineModificationBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OrderAdjustmentOrderLineModificationBusinessLogic : BusinessLogicBase<OrderAdjustmentOrderLineModification, Int32, IOrderAdjustmentOrderLineModificationRepository, IOrderAdjustmentOrderLineModificationBusinessLogic>, IOrderAdjustmentOrderLineModificationBusinessLogic, IDefaultImplementation
    {
    	public override Func<OrderAdjustmentOrderLineModification, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.OrderAdjustmentOrderLineModificationID;
    		}
    	}
    	public override Action<OrderAdjustmentOrderLineModification, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (OrderAdjustmentOrderLineModification i, Int32 id) => i.OrderAdjustmentOrderLineModificationID = id;
    		}
    	}
    	public override Func<OrderAdjustmentOrderLineModification, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.ModificationDescription;
    		}
    	}
    	public override Action<OrderAdjustmentOrderLineModification, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (OrderAdjustmentOrderLineModification i, string title) => i.ModificationDescription = title;
    		}
    	}
    }
}
