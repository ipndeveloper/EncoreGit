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
    [ContainerRegister(typeof(IProductBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class ProductBusinessLogic : BusinessLogicBase<Product, Int32, IProductRepository, IProductBusinessLogic>, IProductBusinessLogic, IDefaultImplementation
    {
    	public override Func<Product, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.ProductID;
    		}
    	}
    	public override Action<Product, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (Product i, Int32 id) => i.ProductID = id;
    		}
    	}
    }
}
