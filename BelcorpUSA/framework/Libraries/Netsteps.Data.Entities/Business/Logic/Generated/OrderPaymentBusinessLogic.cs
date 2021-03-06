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
    [ContainerRegister(typeof(IOrderPaymentBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OrderPaymentBusinessLogic : BusinessLogicBase<OrderPayment, Int32, IOrderPaymentRepository, IOrderPaymentBusinessLogic>, IOrderPaymentBusinessLogic, IDefaultImplementation
    {
    	public override Func<OrderPayment, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.OrderPaymentID;
    		}
    	}
    	public override Action<OrderPayment, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (OrderPayment i, Int32 id) => i.OrderPaymentID = id;
    		}
    	}
    }
}
