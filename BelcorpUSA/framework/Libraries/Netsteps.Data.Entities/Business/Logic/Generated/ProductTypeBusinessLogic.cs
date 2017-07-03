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
    [ContainerRegister(typeof(IProductTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class ProductTypeBusinessLogic : BusinessLogicBase<ProductType, Int32, IProductTypeRepository, IProductTypeBusinessLogic>, IProductTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<ProductType, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.ProductTypeID;
    		}
    	}
    	public override Action<ProductType, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (ProductType i, Int32 id) => i.ProductTypeID = id;
    		}
    	}
    	public override Func<ProductType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<ProductType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (ProductType i, string title) => i.Name = title;
    		}
    	}
    }
}