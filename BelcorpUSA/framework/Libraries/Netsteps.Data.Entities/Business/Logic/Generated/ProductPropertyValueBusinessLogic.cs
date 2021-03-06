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
    [ContainerRegister(typeof(IProductPropertyValueBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class ProductPropertyValueBusinessLogic : BusinessLogicBase<ProductPropertyValue, Int32, IProductPropertyValueRepository, IProductPropertyValueBusinessLogic>, IProductPropertyValueBusinessLogic, IDefaultImplementation
    {
    	public override Func<ProductPropertyValue, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.ProductPropertyValueID;
    		}
    	}
    	public override Action<ProductPropertyValue, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (ProductPropertyValue i, Int32 id) => i.ProductPropertyValueID = id;
    		}
    	}
    	public override Func<ProductPropertyValue, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<ProductPropertyValue, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (ProductPropertyValue i, string title) => i.Name = title;
    		}
    	}
    }
}
