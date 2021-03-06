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
    [ContainerRegister(typeof(IOrderShipmentPackageItemBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OrderShipmentPackageItemBusinessLogic : BusinessLogicBase<OrderShipmentPackageItem, Int32, IOrderShipmentPackageItemRepository, IOrderShipmentPackageItemBusinessLogic>, IOrderShipmentPackageItemBusinessLogic, IDefaultImplementation
    {
    	public override Func<OrderShipmentPackageItem, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.OrderShipmentPackageItemID;
    		}
    	}
    	public override Action<OrderShipmentPackageItem, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (OrderShipmentPackageItem i, Int32 id) => i.OrderShipmentPackageItemID = id;
    		}
    	}
    }
}
