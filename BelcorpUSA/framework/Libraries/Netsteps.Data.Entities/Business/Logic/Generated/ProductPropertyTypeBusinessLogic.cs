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
    [ContainerRegister(typeof(IProductPropertyTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class ProductPropertyTypeBusinessLogic : BusinessLogicBase<ProductPropertyType, Int32, IProductPropertyTypeRepository, IProductPropertyTypeBusinessLogic>, IProductPropertyTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<ProductPropertyType, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.ProductPropertyTypeID;
    		}
    	}
    	public override Action<ProductPropertyType, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (ProductPropertyType i, Int32 id) => i.ProductPropertyTypeID = id;
    		}
    	}
    	public override Func<ProductPropertyType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<ProductPropertyType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (ProductPropertyType i, string title) => i.Name = title;
    		}
    	}
    }
}
